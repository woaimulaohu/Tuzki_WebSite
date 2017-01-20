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
				case "PostManage": postManage(); return View("~/Views/Management/" + viewName + ".cshtml", base.getSnippet(Request));
			}
			return null;
		}
		private void postManage()
		{
			int pageSize, pageStart;
			int.TryParse(Request["pageStartNum"], out pageStart);
			int.TryParse(Request["pageSize"], out pageSize);
			pageSize = pageSize == 0 ? 10 : pageSize;
			pageStart = pageStart == 0 || pageStart < 0 ? 1 : pageStart;
			ViewData.Add("saveStartPage", pageStart);
			ViewData.Add("pageSize", pageSize);
		}
		public string delPost()
		{
			int postId = int.Parse(Request["postId"]);
			using (Entity entity = new Entity())
			{
				entity.POST_INFO.Remove(entity.POST_INFO.Where(p => p.POST_ID == postId).First());
				entity.POST_CONTENT.Remove(entity.POST_CONTENT.Where(p => p.POST_ID == postId).First());
				entity.SaveChanges();
			}
			return "success";
		}
	}
}