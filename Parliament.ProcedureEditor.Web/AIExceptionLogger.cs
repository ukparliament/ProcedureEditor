using Microsoft.ApplicationInsights;
using System.Configuration;
using System.Web.Http.ExceptionHandling;

namespace Parliament.ProcedureEditor.Web
{
    public class AIExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                var ai = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration(ConfigurationManager.AppSettings["ApplicationInsightsInstrumentationKey"]));
                ai.TrackException(context.Exception);
            }

            base.Log(context);
        }
    }
}