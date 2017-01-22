using MyWebProject.Models;
using MyWebProject.Models.Entity;
using Newtonsoft.Json;
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
		public string addPost()
		{
			using (Entity entity = new Entity())
			{
				List<TAG_INFO> list = entity.TAG_INFO.Where(t => t.TAG_ID > 0).ToList();
				return JsonConvert.SerializeObject(list);
			}
		}
		public string getPostList()
		{
			object obj = new object();
			using (Entity entity = new Entity())
			{
				List<TAG_INFO> list = entity.TAG_INFO.Where(t => t.TAG_ID != 0).ToList<TAG_INFO>();
				obj = new { tagInfo = list, detial = base.getDetial(Request).First() };
			}
			return JsonConvert.SerializeObject(obj);
		}
		[ValidateInput(false)]
		public string saveModifyPost()
		{
			string postTitle = Request["postTitle"];
			string postSecondTitle = Request["postSecondTitle"];
			string tags = Request["tags"];
			string isTop = Request["isTop"];
			int selectPostId;
			int.TryParse(Request["selectPostId"], out selectPostId);
			string postContent = HttpUtility.UrlDecode(Request["postContent"]);
			int successCount = 0;
			using (Entity entity = new Entity())
			{
				if (entity.POST_INFO.Where(p => p.POST_ID == selectPostId).Count() > 0)
				{
					POST_INFO info = entity.POST_INFO.Where(p => p.POST_ID == selectPostId).First();
					info.MAIN_TITLE = postTitle;
					info.SECOND_TITLE = postSecondTitle;
					info.TAG_ID = tags;
					entity.POST_CONTENT.Where(p => p.POST_ID == selectPostId).First().POST_CONTENT1 = postContent;
				}
				else
				{
					int postid;
					entity.POST_INFO.Add(new POST_INFO
					{
						DATE = DateTime.Now,
						MAIN_TITLE = postTitle,
						SECOND_TITLE = postSecondTitle,
						PRAISE_COUNT = 0,
						REPRODUCED_COUNT = 0,
						TAG_ID = tags,
						VIEW_COUNT = 0,
						IS_TOP = isTop.Equals("0") ? false : true
					});
					entity.SaveChanges();
					List<POST_INFO> list = entity.POST_INFO.Where(p => p.MAIN_TITLE.Equals(postTitle) && p.SECOND_TITLE.Equals(postSecondTitle)).ToList();
					postid = entity.POST_INFO.Where(p => p.MAIN_TITLE.Equals(postTitle) && p.SECOND_TITLE.Equals(postSecondTitle)).First().POST_ID;
					entity.POST_CONTENT.Add(new POST_CONTENT
					{
						POST_CONTENT1 = postContent,
						POST_ID = postid
					});
				}
				successCount = entity.SaveChanges();
			}
			return successCount > 0 ? "success" : "fail";
		}
	}
}