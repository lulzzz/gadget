using Microsoft.Bot.Connector.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gadget.AspNetCore
{
    /// <summary>
    /// Used to provide settings for the bot middleware
    /// </summary>
    public class GadgetBotMiddlewareOptions
    {
        private ICredentialProvider _credentialProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="GadgetBotMiddlewareOptions"/>
        /// </summary>
        /// <param name="credentialProvider"></param>
        public GadgetBotMiddlewareOptions(ICredentialProvider credentialProvider)
        {
            _credentialProvider = credentialProvider;
        }

        public ICredentialProvider CredentialProvider => _credentialProvider;
    }
}
