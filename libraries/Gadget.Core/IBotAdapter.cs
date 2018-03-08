using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gadget.Core
{
    /// <summary>
    /// Adapter for the microsoft bot connector client
    /// </summary>
    public interface IBotAdapter
    {
        /// <summary>
        /// Sends a number of activities to the given service url.
        /// </summary>
        /// <param name="url">URL of the channel</param>
        /// <param name="credentials">Credentials to use for authentication</param>
        /// <param name="activities">Activities to send</param>
        void SendActivity(string url, ServiceClientCredentials credentials, IEnumerable<IActivity> activities);
    }
}
