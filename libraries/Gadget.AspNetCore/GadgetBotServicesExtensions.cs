using Gadget.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gadget.AspNetCore
{
    public static class GadgetBotServicesExtensions
    {
        /// <summary>
        /// Allows configuration of gadget bot services
        /// </summary>
        public class GadgetBotBuilder
        {
            private IServiceCollection _services;

            /// <summary>
            /// Initializes a new instance of <see cref="GadgetBotBuilder"/>
            /// </summary>
            /// <param name="services"></param>
            public GadgetBotBuilder(IServiceCollection services)
            {
                _services = services;
            }

            /// <summary>
            /// Configure the bot to use a credentials provider
            /// </summary>
            /// <param name="credentialProvider">Credentials provider to use</param>
            /// <returns></returns>
            public GadgetBotBuilder UseCredentialProvider(ICredentialProvider credentialProvider)
            {
                _services.AddSingleton(_ => credentialProvider);

                return this;
            }

            /// <summary>
            /// Configure the gadget bot to use a set of static credentials
            /// </summary>
            /// <param name="appId">Application Identifier of the bot</param>
            /// <param name="secret">Secret for the bot</param>
            /// <returns></returns>
            public GadgetBotBuilder UseCredentials(string appId, string secret)
            {
                _services.AddSingleton(_ => new SimpleCredentialProvider(appId, secret));

                return this;
            }
        }

        /// <summary>
        /// Adds gadget bot dependencies to the service collection
        /// </summary>
        /// <typeparam name="TDialogFlowProvider">Type of dialog flow provider to use</typeparam>
        /// <param name="services">The service collection to configure</param>
        /// <returns></returns>
        public static GadgetBotBuilder AddGadgetBot<TDialogFlowProvider>(this IServiceCollection services) where TDialogFlowProvider : class, IDialogFlowProvider
        {
            services.AddSingleton<IDialogFlowProvider, TDialogFlowProvider>();
            services.AddSingleton<IBotAdapter, BotAdapter>();
            services.AddSingleton<IStreamAdapter>();

            return new GadgetBotBuilder(services);
        }
    }
}
