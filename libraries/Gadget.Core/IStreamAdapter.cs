using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Gadget.Core
{
    /// <summary>
    /// Links incoming chatbot traffic to an Akka stream
    /// </summary>
    public interface IStreamAdapter
    {
        /// <summary>
        /// Processes an activity by letting it flow through a stream
        /// </summary>
        /// <param name="identity">Identity for the request</param>
        /// <param name="activity">Incoming activity</param>
        Task ProcessActivity(ClaimsIdentity identity, Activity activity);
    }
}
