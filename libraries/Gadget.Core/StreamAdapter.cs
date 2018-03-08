using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        private readonly ICredentialProvider credentialProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="StreamAdapter"/>
        /// </summary>
        /// <param name="credentialProvider">Credential provider to use</param>
        public StreamAdapter(ICredentialProvider credentialProvider)
        {
            this.credentialProvider = credentialProvider;
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
            var botConnectorClient = new ConnectorClient(new Uri(activity.ServiceUrl, UriKind.RelativeOrAbsolute), credentials);

            //TODO: Pass the activity through the stream and send the results to the bot connector client
        }

        private async Task<MicrosoftAppCredentials> GetAppCredentials(ClaimsIdentity identity)
        {
            var appId = GetBotIdentifierClaim(identity)?.Value;
            var appPassword = await credentialProvider.GetAppPasswordAsync(appId);

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
