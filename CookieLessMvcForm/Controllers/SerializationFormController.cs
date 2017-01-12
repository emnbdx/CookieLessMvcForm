using System.Web.Mvc;
using CookieLessMvcForm.Models;
using DevTrends.MvcDonutCaching;
using Microsoft.Web.Mvc;

namespace CookieLessMvcForm.Controllers
{
    /// <summary>
    /// Try with model serialization in view
    /// 
    /// This is worse than other method beacause I need to store model in session too
    /// </summary>
    public class SerializationFormController : Controller
    {
        [DonutOutputCache(CacheProfile = "Default")]
        public ActionResult Form()
        {
            return View("Step1");
        }

        [DonutOutputCache(CacheProfile = "Default")]
        public ActionResult Widget()
        {
            return View("Step1");
        }

        public ActionResult Step1(string sessionId)
        {
            var model = (SerializationFormViewModel)new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId)["model"];
            this.ViewBag.SessionId = sessionId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Step1(SerializationFormStep1ViewModel step1, [Deserialize] SerializationFormStep2ViewModel step2,
            [Deserialize] SerializationFormStep3ViewModel step3, string sessionId)
        {
            var model = new SerializationFormViewModel
            {
                Step1 = step1,
                Step2 = step2,
                Step3 = step3
            };

            var session = new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId);
            session["model"] = model;
            return this.RedirectToAction("Step2", new { sessionId = session.SessionID });
        }

        public ActionResult Step2(string sessionId)
        {
            var session = new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId);
            var model = session["model"];
            this.ViewBag.SessionId = session.SessionID;
            return View(model);
        }

        [HttpPost]
        public ActionResult Step2([Deserialize] SerializationFormStep1ViewModel step1, SerializationFormStep2ViewModel step2,
            [Deserialize] SerializationFormStep3ViewModel step3, string previous, string sessionId)
        {
            var model = new SerializationFormViewModel
            {
                Step1 = step1,
                Step2 = step2,
                Step3 = step3
            };

            new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId)["model"] = model;
            return this.RedirectToAction(!string.IsNullOrEmpty(previous) ? "Step1" : "Step3", new { sessionId = sessionId });
        }

        public ActionResult Step3(string sessionId)
        {
            var session = new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId);
            var model = session["model"];
            this.ViewBag.SessionId = session.SessionID;
            return View(model);
        }

        [HttpPost]
        public ActionResult Step3([Deserialize] SerializationFormStep1ViewModel step1, [Deserialize] SerializationFormStep2ViewModel step2,
            SerializationFormStep3ViewModel step3, string previous, string sessionId)
        {
            var model = new SerializationFormViewModel
            {
                Step1 = step1,
                Step2 = step2,
                Step3 = step3
            };

            new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId)["model"] = model;
            return this.RedirectToAction(!string.IsNullOrEmpty(previous) ? "Step2" : "Step4",
                new {sessionId = sessionId});
        }

        public ActionResult Step4(string sessionId)
        {
            var session = new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId);
            var model = session["model"];
            this.ViewBag.SessionId = session.SessionID;
            return View(model);
        }
    }
}