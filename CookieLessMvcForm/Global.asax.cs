using System;
using System.Collections.Concurrent;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace CookieLessMvcForm
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static readonly ConcurrentDictionary<string, HttpSessionState> _sessions = new ConcurrentDictionary<string, HttpSessionState>();

        public static ConcurrentDictionary<string, HttpSessionState> Sessions => _sessions;

        protected void Session_Start(object sender, EventArgs e)
        {
            _sessions.TryAdd(Session.SessionID, Session);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            HttpSessionState outSession;
            _sessions.TryRemove(Session.SessionID, out outSession);
        }
    }
}
