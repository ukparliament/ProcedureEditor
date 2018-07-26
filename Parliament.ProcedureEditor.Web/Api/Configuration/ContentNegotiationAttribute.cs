using System.Collections.Generic;
using System.Web.Http.Routing;

namespace Parliament.ProcedureEditor.Web.Api.Configuration
{
    /// <summary>
    /// Route constraint on Accept header of the request
    /// See <see cref="Parliament.ProcedureEditor.Web.Api.Configuration.ContentNegotiationConstraint"/>
    /// </summary>
    public class ContentNegotiationAttribute: RouteFactoryAttribute
    {
        private readonly ContentType mediaType;

        public ContentNegotiationAttribute(string template, ContentType mediaTypeValue)
            :base(template)
        {
            mediaType = mediaTypeValue;
        }


        public override IDictionary<string, object> Constraints
        {
            get
            {
                return new HttpRouteValueDictionary()
                {
                    {"mediaType",new ContentNegotiationConstraint(mediaType) }
                };
            }
        }
        

    }
}