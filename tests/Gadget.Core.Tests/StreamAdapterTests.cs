using Akka.Streams.Dsl;
using FakeItEasy;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Gadget.Core.Tests
{
    public class StreamAdapterTests
    {
        [Fact]
        public async Task ProcessActivityPassesActivityThroughStream()
        {
            var credentialProvider = A.Fake<ICredentialProvider>();
            var botAdapter = A.Fake<IBotAdapter>();
            var dialogFlowProvider = A.Fake<IDialogFlowProvider>();

            ClaimsIdentity identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(AuthenticationConstants.AppIdClaim, "TestApp"));

            A.CallTo(() => credentialProvider.GetAppPasswordAsync(A<string>._)).Returns("HelloWorld");
            A.CallTo(() => dialogFlowProvider.GetDialogFlow()).Returns(Flow.FromFunction((IActivity input) => (IEnumerable<IActivity>)new IActivity[] {  }));

            var streamAdapter = new StreamAdapter(credentialProvider, botAdapter, dialogFlowProvider);
            
            await streamAdapter.ProcessActivity(identity, (Activity)Activity.CreateMessageActivity()).ConfigureAwait(false);

            // Give this a short delay, since we are working on a background thread that doesn't wait.
            await Task.Delay(100);

            A.CallTo(() => botAdapter.SendActivity(A<string>._, A<ServiceClientCredentials>._, A<IEnumerable<IActivity>>._)).MustHaveHappened();
        }
    }
}
