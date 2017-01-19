using MyWebProject.Models;
using MyWebProject.Models.Entity;
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
			return View();
		}
		public ActionResult Menu()
		{
			List<MENU> list = new List<MENU>();
			using (Entity entity = new Entity())
			{
				list = entity.MENU.Where(m => m.MENU_SERIAL > 0).OrderBy(m => m.MENU_SERIAL).ToList();
			}
			return View(list);
		}
		public ActionResult Grid()
		{
			int menuId;
			int.TryParse(Request["menuId"], out menuId);
			string viewName = string.Empty;
			using (Entity entity = new Entity())
			{
				viewName = entity.MENU.Where(m => m.MENU_ID == menuId).First().GRID_PAGE_NAME;
			}
			switch (viewName)
			{
				case "PostManage": return View("~/Views/Management/" + viewName + ".cshtml", base.getSnippet(Request));
			}
			return null;
		}
	}
}