using System;
using System.IO;
using System.Web.Mvc;
using CookieLessMvcForm.Models;
using DevTrends.MvcDonutCaching;

namespace CookieLessMvcForm.Controllers
{
    /// <summary>
    /// In this controller I manage session id in hidden field and store model in session
    /// Then I just need to get session from id to get model
    /// 
    /// Mode management (form or widget) is managed by passing GET parameter in each request
    /// 
    /// I use custom RequestVerificationToken because MVC default use cookie
    /// </summary>
    public class FormController : Controller
    {
        /// <summary>
        /// Entry point to form in complete mode
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Default")]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// Entry point to form in widget view
        /// </summary>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "Default")]
        public ActionResult Widget()
        {
            return View();
        }

        public ActionResult RedirectToGoodAction(bool widget)
        {
            if (widget)
            {
                return View("Widget");
            }

            return View("Form");
        }

        /// <summary>
        /// This action need to be render excluded of cache
        /// Keep current session if exist, then render partiel view with technical data (token and session id)
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public ActionResult FormTechnicalData(string sessionId)
        {
            var session = new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId);

            var requestVerificationToken = Guid.NewGuid().ToString();
            this.ViewBag.RequestVerificationToken = requestVerificationToken;
            this.ViewBag.SessionId = session.SessionID;
            session["__RequestVerificationToken"] = requestVerificationToken;
            return PartialView("Shared/_FormTechnicalData");
        }

        /// <summary>
        /// Render partial view according to step in session 
        /// If new session generate empty model and define step to 1
        /// </summary>
        /// <param name="widget"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public ActionResult Formulaire(bool widget, string sessionId)
        {
            var session = new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId);

            var step = session["step"];
            var model = new FormModel();
            if (step == null)
            {
                step = 1;
            }
            else
            {
                this.ViewBag.SessionId = session.SessionID;
                model = (FormModel)session["model"];
            }

            this.ViewBag.Step = step;
            this.ViewBag.Widget = widget;

            return PartialView("Shared/_Form" + step, model);
        }

        public ActionResult GoBack(string step, string sessionId, bool widget)
        {
            return RedirectToAction("Step" + step, new { sessionId = sessionId, widget = widget });
        }

        /// <summary>
        /// Just define step in session then call RedirectToGoodAction to manage widget or form view
        /// 
        /// Same behaviour for all StepN
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="widget"></param>
        /// <returns></returns>
        public ActionResult Step1(string sessionId, bool widget)
        {
            new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId)["step"] = 1;
            this.ViewBag.SessionId = sessionId;

            return RedirectToGoodAction(widget);
        }

        /// <summary>
        /// This first step init model, save user input and redirect to next step
        /// 
        /// With the exception of model initialisation all StepNPost save input and redirect to StepN+1
        /// </summary>
        /// <param name="postModel"></param>
        /// <param name="__session"></param>
        /// <param name="widget"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAntiForgeryToken]
        public ActionResult Step1Post(FormModel postModel, string __session, bool widget)
        {
            var session = new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, __session);
            if (session["Form_Init_Ok"] != "1")
            {
                //Init model
                if (widget)
                {
                    session["step"] = null;
                    session["model"] = new FormModel
                    {
                        Id = 2,
                        Name = "widget model"
                    };
                }
                else
                {
                    session["step"] = null;
                    session["model"] = new FormModel
                    {
                        Id = 1,
                        Name = "form model"
                    };
                }

                session["Form_Init_Ok"] = "1";
            }

            var model = (FormModel)new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, __session)["model"];
            model.Amount = postModel.Amount;

            return RedirectToAction("Step2", new { sessionId = __session, widget = widget });
        }

        public ActionResult Step2(string sessionId, bool widget)
        {
            new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId)["step"] = 2;
            this.ViewBag.SessionId = sessionId;

            return RedirectToGoodAction(widget);
        }

        [HttpPost]
        [CustomAntiForgeryToken]
        public ActionResult Step2Post(FormModel postModel, string __session, bool widget)
        {
            if (new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, __session)["model"] == null)
            {
                throw new InvalidDataException("Session doesn't contain model");
            }

            var model = (FormModel)new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, __session)["model"];
            model.Email = postModel.Email;

            return RedirectToAction("Step3", new { sessionId = __session, widget = widget });
        }

        public ActionResult Step3(string sessionId, bool widget)
        {
            new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId)["step"] = 3;
            this.ViewBag.SessionId = sessionId;

            return RedirectToGoodAction(widget);
        }

        [HttpPost]
        [CustomAntiForgeryToken]
        public ActionResult Step3Post(FormModel postModel, string __session, bool widget)
        {
            if (new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, __session)["model"] == null)
            {
                throw new InvalidDataException("Session doesn't contain model");
            }

            var model = (FormModel)new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, __session)["model"];
            model.Tip = postModel.Tip;

            return RedirectToAction("Step4", new { sessionId = __session, widget = widget });
        }

        public ActionResult Step4(string sessionId, bool widget)
        {
            new CookieLessSessionFilter().GetCurrentSession(this.HttpContext, sessionId)["step"] = 4;
            this.ViewBag.SessionId = sessionId;

            return RedirectToGoodAction(widget);
        }
    }
}