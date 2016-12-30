using MyWebProject.Models;
using MyWebProject.Models.QueryResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	public class HomeController : BaseController
	{
		public ActionResult Home()
		{
			return Redirect("~/Views/Main.cshtml");
		}
		public ActionResult Main()
		{
			return View("~/Views/Main.cshtml");
		}
		// GET: Home
		public ActionResult Index()
		{
			HomeResult homeResult = new HomeResult();
			
			return View();
		}

		public ActionResult Typo()
		{
			return View();
		}
		public ActionResult Tips(bool IsSuccess,string Msg)
		{
			return View("~/Views/ErrorPage.cshtml", new ResultObj {IsSuccess= IsSuccess,Obj="Error" ,Msg=Msg});
		}
	}
}