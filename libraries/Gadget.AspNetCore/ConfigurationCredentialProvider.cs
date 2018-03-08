using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gadget.AspNetCore
{
    /// <summary>
    /// Credential provider which uses <see cref="Microsoft.Extensions.Configuration.IConfiguration"/> to lookup appId and password.
    /// </summary>
    public sealed class ConfigurationCredentialProvider : SimpleCredentialProvider
    {
        public ConfigurationCredentialProvider(IConfiguration configuration)
        {
            this.AppId = configuration.GetSection(MicrosoftAppCredentials.MicrosoftAppIdKey)?.Value;
            this.Password = configuration.GetSection(MicrosoftAppCredentials.MicrosoftAppPasswordKey)?.Value;
        }
    }
}
