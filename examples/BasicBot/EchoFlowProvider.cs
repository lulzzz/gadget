using Akka;
using Akka.Streams.Dsl;
using Gadget.Core;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicBot
{
    public class EchoFlowProvider : IDialogFlowProvider
    {
        public Flow<IActivity, IEnumerable<IActivity>, NotUsed> GetDialogFlow()
        {
            return Flow.Create<IActivity>()
                .Where(activity => activity.Type == ActivityTypes.Message)
                .Select(activity => (Activity)activity)
                .Select(activity => new IActivity[] { activity.CreateReply(activity.Text) })
                .Select(activities => (IEnumerable<IActivity>)activities);
        }
    }
}
