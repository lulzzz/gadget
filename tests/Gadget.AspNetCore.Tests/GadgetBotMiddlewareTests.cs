using FakeItEasy;
using Gadget.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Gadget.AspNetCore.Tests
{
    public class GadgetBotMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsyncInvokesStreamAdapter()
        {
            var context = new DefaultHttpContext();
            var streamAdapter = A.Fake<IStreamAdapter>();
            var credentialProvider = A.Fake<ICredentialProvider>();

            A.CallTo(() => credentialProvider.IsAuthenticationDisabledAsync()).Returns(true);

            context.Request.Path = "/api/messages";
            context.Request.Method = "POST";

            var middleware = new GadgetBotMiddleware(new GadgetBotMiddlewareOptions(credentialProvider), streamAdapter, (httpContext) =>
            {
                return Task.CompletedTask;
            });

            await middleware.InvokeAsync(context);

            A.CallTo(() => streamAdapter.ProcessActivity(A<ClaimsIdentity>._, A<Activity>._)).MustHaveHappened();
        }
    }
}
