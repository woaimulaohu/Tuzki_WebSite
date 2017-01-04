using MyWebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
    public class ManagementController : BaseController
	{
        // GET: Management
        public ActionResult Index()
        {
			Response.RedirectToRoute(new { action = "Tips", controller = "Home", IsSuccess = false, Msg = "网站建设中,暂不支持此操作" });
			return View();
        }
    }
}