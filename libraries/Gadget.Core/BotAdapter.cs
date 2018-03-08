using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Rest;

namespace Gadget.Core
{
    /// <summary>
    /// Implementation of the bot adapter that talks to MS Bot framework channel
    /// </summary>
    public class BotAdapter : IBotAdapter
    {
        /// <summary>
        /// Sends a number of activities to the given service url.
        /// </summary>
        /// <param name="url">URL of the channel</param>
        /// <param name="credentials">Credentials to use for authentication</param>
        /// <param name="activities">Activities to send</param>
        public void SendActivity(string url, ServiceClientCredentials credentials, IEnumerable<IActivity> activities)
        {
            var connectorClient = new ConnectorClient(new Uri(url, UriKind.RelativeOrAbsolute), credentials);
            
            foreach(var activity in activities)
            {
                connectorClient.Conversations.SendToConversation((Activity)activity);
            }
        }
    }
}
