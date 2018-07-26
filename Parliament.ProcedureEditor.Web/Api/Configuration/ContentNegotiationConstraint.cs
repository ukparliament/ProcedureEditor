using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Parliament.ProcedureEditor.Web.Api.Configuration
{
    /// <summary>
    /// Logic for evaluating route constraint on Accept header of the request
    /// See <see cref="Parliament.ProcedureEditor.Web.Api.Configuration.ContentNegotiationAttribute"/>
    /// </summary>
    public class ContentNegotiationConstraint : IHttpRouteConstraint
    {
        private readonly ContentType mediaType;

        public ContentNegotiationConstraint(ContentType mediaTypeValue)
        {
            mediaType = mediaTypeValue;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            string contentType = null;
            switch (mediaType)
            {
                case ContentType.HTML:
                    contentType = "text/html";
                    break;
                case ContentType.JSON:
                    contentType = "application/json";
                    break;
            }
            if (routeDirection == HttpRouteDirection.UriResolution)
                return request.Headers.Accept.Any(h => h.MediaType.Equals(contentType, StringComparison.InvariantCultureIgnoreCase));
            else
                return true;
        }
    }
}