using FakeItEasy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Bot.Connector.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Gadget.AspNetCore.Tests
{
    public class GadgetBotMiddlewareExtensionsTests
    {
        [Fact]
        public void AddGadgetBotMiddlewareDoesNotThrowExceptions()
        {
            var serviceProvider = A.Fake<IServiceProvider>();
            var appBuilder = new ApplicationBuilder(serviceProvider);

            appBuilder.UseGadgetBot();


        }
    }
}
