using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gadget.AspNetCore
{
    public static class GadgetBotMiddlewareExtensions
    {
        public static IApplicationBuilder UseGadgetBot(this IApplicationBuilder app)
        { 
            app.UseMiddleware<GadgetBotMiddleware>();
            return app;    
        }
    }
}
