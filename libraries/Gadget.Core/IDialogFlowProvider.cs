using Akka.Streams;
using Akka.Streams.Dsl;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gadget.Core
{
    /// <summary>
    /// Implement this interface to produce dialog flows for the gadget bot.
    /// </summary>
    public interface IDialogFlowProvider
    {
        /// <summary>
        /// When invoked this method should return a flow that takes a single activity
        /// and produces zero, one or more replies to send back to the user.
        /// </summary>
        /// <returns>Returns a flow element for the stream that processes incoming activities and 
        /// produces zero, one or more activities as a reply to the incoming activity.</returns>
        Flow<IActivity,IEnumerable<IActivity>, Akka.NotUsed> GetDialogFlow();
    }
}
