using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Threading;

namespace Parliament.ProcedureEditor.Web
{
    public class AIInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if ((Thread.CurrentPrincipal != null) && (Thread.CurrentPrincipal.Identity != null))
                telemetry.Context.User.Id = Thread.CurrentPrincipal.Identity.Name;
        }
        
    }
}