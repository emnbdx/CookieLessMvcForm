using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CookieLessMvcForm
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class CustomAntiForgeryTokenAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Only apply on POST verb
        /// 
        /// Get token in posted form and session to compare
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.HttpMethod != "POST")
            {
                throw new HttpException(400, "AntiForgeryAttribute can only use on POST action");
            }

            if (!httpContext.Request.Form.AllKeys.Contains("__RequestVerificationToken"))
            {
                throw new HttpException(400, "RequestVerificationToken not found in posted form");
            }

            var formValue = httpContext.Request?.Form["__RequestVerificationToken"];
            var sessionValue = Convert.ToString(httpContext.Session?["__RequestVerificationToken"]);
            if (string.IsNullOrEmpty(sessionValue))
            {
                if (!httpContext.Request.Form.AllKeys.Contains("__Session"))
                {
                    throw new HttpException(400, "Session not managed in browser and not found in posted form");
                }

                var sessionId = httpContext.Request.Form["__Session"];

                sessionValue = Convert.ToString(MvcApplication.Sessions[sessionId]["__RequestVerificationToken"]);
            }

            if (formValue != sessionValue)
            {
                throw new HttpException(403, "RequestVerificationToken different from form and session");
            }

            return true;
        }
    }
}