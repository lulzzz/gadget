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
            return Flow.FromFunction<IActivity, IEnumerable<IActivity>>(activity =>
            {
                if (activity.Type == ActivityTypes.Message)
                {
                    return new[]
                    {
                        ((Activity)activity).CreateReply(activity.AsMessageActivity().Text)
                    };
                }

                return new Activity[] { };
            });
        }
    }
}
