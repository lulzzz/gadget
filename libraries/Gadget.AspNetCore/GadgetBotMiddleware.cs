using Gadget.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gadget.AspNetCore
{
    /// <summary>
    /// Middleware that connects the Gadget bot pipeline to the ASP.NET core webserver.
    /// Please note that this middleware should be the last in the pipeline.
    /// </summary>
    public class GadgetBotMiddleware
    {
        private GadgetBotMiddlewareOptions _options;
        private IStreamAdapter _streamAdapter;
        private RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of <see cref="GadgetBotMiddleware"/>
        /// </summary>
        /// <param name="options"></param>
        public GadgetBotMiddleware(GadgetBotMiddlewareOptions options, IStreamAdapter streamAdapter, RequestDelegate next)
        {
            _options = options;
            _next = next;
            _streamAdapter = streamAdapter;
        }

        /// <summary>
        /// Reads in the data to be processed by the chatbot and passes it on to the bot stream processor.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var activity = DeserializeBody(context.Request);

            var credentials = await JwtTokenValidation.AuthenticateRequest(
                activity, authorizationHeader, _options.CredentialProvider);

            await _streamAdapter.ProcessActivity(credentials, activity);

            context.Response.ContentLength = 0;
            context.Response.StatusCode = 202;

            await _next(context);
        }

        private Activity DeserializeBody(HttpRequest request)
        {
            using (var streamReader = new StreamReader(request.Body))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<Activity>(jsonReader);
            }
        }
    }
}
