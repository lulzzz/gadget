using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Streams;
using Akka.Streams.Dsl;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;

namespace Gadget.Core
{
    /// <summary>
    /// Connects incoming requests for the bot to a stream for processing
    /// </summary>
    public class StreamAdapter : IStreamAdapter
    {
        private readonly ICredentialProvider _credentialProvider;
        private readonly IDialogFlowProvider _dialogFlowProvider;
        private readonly IBotAdapter _botAdapter;
        private readonly ActorSystem _actorSystem;

        /// <summary>
        /// Initializes a new instance of <see cref="StreamAdapter"/>
        /// </summary>
        /// <param name="credentialProvider">Credential provider to use</param>
        public StreamAdapter(ICredentialProvider credentialProvider, IBotAdapter botAdapter, IDialogFlowProvider dialogFlowProvider)
        {
            _credentialProvider = credentialProvider;
            _dialogFlowProvider = dialogFlowProvider;
            _botAdapter = botAdapter;

            _actorSystem = ActorSystem.Create($"gadgetbot-{Guid.NewGuid()}");
        }
        
        /// <summary>
        /// Processing incoming user activities
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task ProcessActivity(ClaimsIdentity identity, Activity activity)
        {
            var credentials = await GetAppCredentials(identity);
            
            // The sink for our stream is the bot connector client sink.
            var activitySink = Sink.ForEach((IEnumerable<IActivity> replies) =>
            {
                _botAdapter.SendActivity(activity.ServiceUrl,credentials, replies);
            });

            // The dialog flow is provided through an external provider.
            // We don't know what this part of the stream looks like, just that we want a single
            // activity as input and a series of activities as output.
            var dialogFlow = _dialogFlowProvider.GetDialogFlow();

            var activityStream = Source.Single((IActivity)activity).Via(dialogFlow).To(activitySink);

            using(var materializer = _actorSystem.Materializer())
            {
                activityStream.Run(materializer);
            }
        }

        private async Task<MicrosoftAppCredentials> GetAppCredentials(ClaimsIdentity identity)
        {
            var appId = GetBotIdentifierClaim(identity)?.Value;
            var appPassword = await _credentialProvider.GetAppPasswordAsync(appId);

            var appCredentials = new MicrosoftAppCredentials(appId, appPassword);

            return appCredentials;
        }

        private Claim GetBotIdentifierClaim(ClaimsIdentity identity)
        {
            // Our first attempt is to use the audience claim.
            // This is used by traffic coming from channels. It contains the identifier for the bot in Azure Active Directory.
            var identifierClaim = identity.Claims?.SingleOrDefault(claim => claim.Type == AuthenticationConstants.AudienceClaim);

            // The audience claim is not available on requests coming from the bot emulator. So instead we use the AppId claim.
            if (identifierClaim == null)
            {
                identifierClaim = identity.Claims?.SingleOrDefault(claim => claim.Type == AuthenticationConstants.AppIdClaim);
            }

            return identifierClaim;
        }
    }
}
