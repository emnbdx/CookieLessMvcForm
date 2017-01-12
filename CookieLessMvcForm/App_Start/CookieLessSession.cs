using System.Web;

namespace CookieLessMvcForm
{
    public class CookieLessSessionFilter
    {
        public HttpSessionStateBase GetCurrentSession(HttpContextBase context, string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return context.Session;
            }

            var session = MvcApplication.Sessions[sessionId];
            if (session == null)
            {
                return context.Session;
            }

            return new HttpSessionStateWrapper(session);
        }
    }
}