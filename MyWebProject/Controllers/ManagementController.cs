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
		/// <summary>
		/// 从数据库生成菜单列表
		/// </summary>
		/// <returns></returns>
		public ActionResult Menu()
		{
			List<MENU> list = new List<MENU>();
			using (Entity entity = new Entity())
			{
				list = entity.MENU.Where(m => m.MENU_SERIAL > 0).OrderBy(m => m.MENU_SERIAL).ToList();
			}
			return View(list);
		}
		/// <summary>
		/// 获取文章列表
		/// </summary>
		/// <returns></returns>
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
		/// <summary>
		/// 因为采用每次重新刷新子页面的方式,所以选择的页码什么的参数需要前台传给后台,再由后台重新生成页面时重新返回给前台,用到了DataView k=>v
		/// </summary>
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
		/// <summary>
		/// 删除文章
		/// </summary>
		/// <returns></returns>
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
		/// <summary>
		/// 添加文章
		/// </summary>
		/// <returns></returns>
		public string addPost()
		{
			using (Entity entity = new Entity())
			{
				List<TAG_INFO> list = entity.TAG_INFO.Where(t => t.TAG_ID > 0).ToList();
				return JsonConvert.SerializeObject(list);
			}
		}
		/// <summary>
		/// 获取文章明细信息,用于绑定到富文本编辑器
		/// </summary>
		/// <returns></returns>
		public string getPostDetial()
		{
			object obj = new object();
			using (Entity entity = new Entity())
			{
				List<TAG_INFO> list = entity.TAG_INFO.Where(t => t.TAG_ID != 0).ToList<TAG_INFO>();
				obj = new { tagInfo = list, detial = base.getDetial(Request).First() };
			}
			return JsonConvert.SerializeObject(obj);
		}
		/// <summary>
		/// 修改文章->保存
		/// </summary>
		/// <returns></returns>
		[ValidateInput(false)]
		public string saveModifyPost()
		{
			string postTitle = Request["postTitle"];
			string postSecondTitle = Request["postSecondTitle"];
			string tags = Request["tags"];
			bool isTop = Request["isTop"].Equals("0") ? false : true;
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
					info.IS_TOP = isTop;
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
						IS_TOP = isTop
					});
					entity.SaveChanges();
					List<POST_INFO> list = entity.POST_INFO.Where(p => p.MAIN_TITLE.Equals(postTitle) && p.SECOND_TITLE.Equals(postSecondTitle)).ToList();
					postid = entity.POST_INFO.Where(p => p.MAIN_TITLE.Equals(postTitle) && p.SECOND_TITLE.Equals(postSecondTitle)).OrderByDescending(p => p.DATE).First().POST_ID;
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