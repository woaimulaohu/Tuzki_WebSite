using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Main()
		{
			return View("~/Views/Main.cshtml");
		}
		// GET: Home
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Typo()
		{
			return View();
		}
	}
}