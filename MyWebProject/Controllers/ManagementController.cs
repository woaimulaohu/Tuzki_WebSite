using MyWebProject.Models;
using MyWebProject.Models.Entity;
using MyWebProject.Util_Pro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	public class ManagementController : BaseController
	{
		// GET: Management
		public ActionResult Index()
		{
			//using (Entity entity = new Entity())
			//{
			//	string token = Request.Cookies.Get("token").Value;
			//	if (entity.USER_INFO.Where(u => u.TOKEN == token).First().USER_AUTH == 2)
			//	{
			//		return View();
			//	}
			//	else
			//	{
			//		return View("~/Views/ErrorPage.cshtml", new ResultObj { IsSuccess = false, Obj = "Error", Msg = "非管理员禁止访问!" });
			//	}
			//}
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
		/// 根据菜单ID获取右侧表格
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
				case "ConfigManage": return View("~/Views/Management/" + viewName + ".cshtml", ConfigManage());
				case "UserManage": return View("~/Views/Management/" + viewName + ".cshtml", UserManage());
			}
			return null;
		}
		//-----------------------------用户管理相关--start----------------------------------------------------
		private List<USER_INFO> UserManage()
		{
			using (Entity entity = new Entity())
			{
				return entity.USER_INFO.Where(u => u.USER_ID > 0).ToList();
			}
		}

		public ActionResult ModUserInfo()
		{
			int userId = int.Parse(Request["userId"]);
			List<USER_INFO> list = new List<USER_INFO>();
			List<dynamic> dynamicList = new List<dynamic>();
			using (Entity entity = new Entity())
			{
				list = entity.USER_INFO.Where(u => u.USER_ID == userId).ToList();
			}
			foreach (USER_INFO u in list)
			{
				PropertyInfo[] peroperties = u.GetType().GetProperties();
				foreach (PropertyInfo pi in peroperties)
				{
					if (pi.Name.Equals("TOKEN"))
					{
						continue;
					}
					string val = pi.GetValue(u) + "";//规避为null的情况,所以不用tostring
					dynamicList.Add(new { pi.Name, val });
				}
			}
			return base.modal(dynamicList, "修改用户信息", "Management/SaveUserInfo");
		}
		public ActionResult AddUserInfo()
		{
			List<dynamic> dynamicList = new List<dynamic>();
			USER_INFO u = new USER_INFO();
			PropertyInfo[] peroperties = u.GetType().GetProperties();
			foreach (PropertyInfo pi in peroperties)
			{
				dynamicList.Add(new { pi.Name, string.Empty });
			}
			return base.modal(dynamicList, "新增用户信息", "Management/AddUser");
		}
		public string AddUser()
		{
			if (!Util.CommonUtil.checkManagementAuthority(Request["token"]))
			{
				return "fail";
			}
			string json = Request["json"];
			JArray ja = (JArray)JsonConvert.DeserializeObject(json);
			Dictionary<string, string> dic = new Dictionary<string, string>();
			foreach (var obj in ja)
			{
				string key = obj["key"].ToString();
				string val = obj["val"].ToString();
				dic.Add(key, val);
			}

			using (Entity entity = new Entity())
			{
				DateTime expire = DateTime.Now.AddDays(7);
				DateTime.TryParse(string.IsNullOrEmpty(dic["EXPIRE_TIME"]) ? expire.ToString() : dic["EXPIRE_TIME"], out expire);
				int auth = 0;
				int.TryParse(dic["USER_AUTH"], out auth);
				entity.USER_INFO.Add(new USER_INFO
				{
					AVATAR_URL = string.IsNullOrEmpty(dic["AVATAR_URL"]) ? "https://s.gravatar.com/avatar/" + Util.CommonUtil.MD5_Encode(expire.ToString()) + "?s=80&d=retro" : dic["AVATAR_URL"],
					EXPIRE_TIME = expire,
					NICK_NAME = dic.ContainsKey("NICK_NAME") ? dic["NICK_NAME"] : expire.ToString(),
					TOKEN = Util.CommonUtil.MD5_Encode(expire.ToString()),
					USER_AUTH = auth
				});
				entity.SaveChanges();
			}
			return "success";
		}
		public string SaveUserInfo()
		{
			if (!Util.CommonUtil.checkManagementAuthority(Request["token"]))
			{
				return "fail";
			}
			string json = Request["json"];
			USER_INFO info = new USER_INFO();
			PropertyInfo[] peroperties = info.GetType().GetProperties();
			JArray ja = (JArray)JsonConvert.DeserializeObject(json);
			foreach (var obj in ja)
			{
				string key = obj["key"].ToString();
				string val = obj["val"].ToString();
				foreach (PropertyInfo pi in peroperties)
				{
					if (pi.Name.Equals(key))
					{
						pi.SetValue(info, Convert.ChangeType(val, pi.PropertyType));
						break;
					}
				}
			}
			using (Entity entity = new Entity())
			{
				USER_INFO user = entity.USER_INFO.Where(u => u.USER_ID == info.USER_ID).First();
				user.EXPIRE_TIME = info.EXPIRE_TIME;
				user.NICK_NAME = info.NICK_NAME;
				user.USER_AUTH = info.USER_AUTH;
				entity.SaveChanges();
			}
			return "success";
		}
		public string UserDel()
		{
			if (!Util.CommonUtil.checkManagementAuthority(Request["token"]))
			{
				return "fail";
			}
			int userId = int.Parse(Request["userId"]);
			using (Entity entity = new Entity())
			{
				entity.USER_INFO.Remove(entity.USER_INFO.Where(u => u.USER_ID == userId).First());
				entity.SaveChanges();
			}
			return "success";
		}
		//-----------------------------用户管理相关--end----------------------------------------------------


		//-----------------------------配置管理相关--start----------------------------------------------------
		/// <summary>
		/// 初始化配置管理菜单返回的数据
		/// </summary>
		/// <returns></returns>
		#region
		private List<CONFIG> ConfigManage()
		{
			List<CONFIG> list = new List<CONFIG>();
			using (Entity entity = new Entity())
			{
				list = entity.CONFIG.Where(c => c.ID > 0).ToList();
			}
			return list;
		}
		#endregion

		/// <summary>
		/// 配置管理中删除配置数据
		/// </summary>
		#region
		public void configDel()
		{
			int id = int.Parse(Request["id"]);
			using (Entity entity = new Entity())
			{
				entity.CONFIG.Remove(entity.CONFIG.Where(c => c.ID == id).First());
				entity.SaveChanges();
			}
		}
		#endregion
		/// <summary>
		/// 配置管理保存数据
		/// </summary>
		/// <returns></returns>
		#region
		public string configSaveAll()
		{
			if (!Util.CommonUtil.checkManagementAuthority(Request["token"]))
			{
				return "fail";
			}
			string json = Request["json"];
			List<CONFIG> configs = JsonConvert.DeserializeObject<List<CONFIG>>(json);
			using (Entity entity = new Entity())
			{
				foreach (CONFIG cfg in configs)
				{
					List<CONFIG> ef = entity.CONFIG.Where(c => c.ID == cfg.ID).ToList();
					if (ef.Count() > 0)
					{
						entity.CONFIG.Remove(entity.CONFIG.Where(c => c.ID == cfg.ID).First());
						entity.SaveChanges();
					}
					entity.CONFIG.Add(new CONFIG
					{
						KEY_NAME = cfg.KEY_NAME,
						VALUE = cfg.VALUE
					});
				}
				if (entity.SaveChanges() < configs.Count)
				{
					return "fail";
				}
			}
			return "success";
		}
		#endregion
		//-----------------------------配置管理相关--end----------------------------------------------------


		///---------------------------文章管理相关--start------------------------------------------------------
		/// <summary>
		/// 因为采用每次重新刷新子页面的方式,所以选择的页码什么的参数需要前台传给后台,再由后台重新生成页面时重新返回给前台,用到了DataView k=>v
		/// </summary>
		#region
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
		#endregion
		/// <summary>
		/// 删除文章
		/// </summary>
		/// <returns></returns>
		#region
		public string delPost()
		{
			if (!Util.CommonUtil.checkManagementAuthority(Request["token"]))
			{
				return "fail";
			}
			int postId = int.Parse(Request["postId"]);
			using (Entity entity = new Entity())
			{
				entity.POST_INFO.Remove(entity.POST_INFO.Where(p => p.POST_ID == postId).First());
				entity.POST_CONTENT.Remove(entity.POST_CONTENT.Where(p => p.POST_ID == postId).First());
				entity.SaveChanges();
			}
			return "success";
		}
		#endregion
		/// <summary>
		/// 添加文章
		/// </summary>
		/// <returns></returns>
		#region
		public string addPost()
		{
			if (!Util.CommonUtil.checkManagementAuthority(Request["token"]))
			{
				return "fail";
			}
			using (Entity entity = new Entity())
			{
				List<TAG_INFO> list = entity.TAG_INFO.Where(t => t.TAG_ID > 0).ToList();
				return JsonConvert.SerializeObject(list);
			}
		}
		#endregion

		/// <summary>
		/// 获取文章明细信息,用于绑定到富文本编辑器
		/// </summary>
		/// <returns></returns>
		#region
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
		#endregion
		/// <summary>
		/// 修改文章->保存
		/// </summary>
		/// <returns></returns>
		[ValidateInput(false)]
		#region
		public string saveModifyPost()
		{
			if (!Util.CommonUtil.checkManagementAuthority(Request["token"]))
			{
				return "fail";
			}
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
		#endregion
		///---------------------------文章管理相关--end------------------------------------------------------
	}
}