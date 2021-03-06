﻿using MyWebProject.Models;
using MyWebProject.Models.Entity;
using MyWebProject.Util_Pro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MyWebProject.Controllers
{
	public class OtherController : BaseController
	{
		log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		// GET: Other
		public ActionResult Index()
		{
			return View();
		}
		
		public void TencentAccountLogin()
		{

		}
		/// <summary>
		/// 第三方登录方式github
		/// </summary>
		public void GitHubAccountLogin()
		{
			string code = Request["code"];
			string step2Url = "https://github.com/login/oauth/access_token?client_id=1e8991731cc506dd7aa3&client_secret=b5e72132d8c5eadda8100e368ae2718907c5ecda&code=" + code;
			string access_token = Util.CommonUtil.HttpGetResult(step2Url).Split('&')[0].Split('=')[1];
			string step3Url = "https://api.github.com/user?access_token=" + access_token;
			string json = Util.CommonUtil.HttpGetResult(step3Url);
			GitHubUserInfo gitInfo = JsonConvert.DeserializeObject<GitHubUserInfo>(json);

			using (Entity entity = new Entity())
			{
				List<USER_INFO> list = entity.USER_INFO.Where<USER_INFO>(u => u.GITHUB_ID == gitInfo.id).ToList();
				if (list.Count > 0)
				{
					USER_INFO user = list.First();
					user.AVATAR_URL = gitInfo.avatar_url;
					user.NICK_NAME = gitInfo.name;
					entity.SaveChanges();
					HttpCookie cookie = new HttpCookie("token");
					cookie.Value = user.TOKEN;
					cookie.Expires = user.EXPIRE_TIME.AddDays(7);
					Response.SetCookie(cookie);
				}
				else
				{
					DateTime expireTime = DateTime.Now.AddDays(7);
					string token = Util.CommonUtil.MD5_Encode(gitInfo.id + "" + gitInfo.name+""+ expireTime);
					entity.USER_INFO.Add(new USER_INFO
					{
						NICK_NAME = gitInfo.name,
						AVATAR_URL = gitInfo.avatar_url,
						GITHUB_ID = gitInfo.id,
						USER_AUTH = "16289008".Equals(gitInfo.id) ? 2 : 0,//用户权限0一般,1特别访客,2管理员,3黑名单
						EXPIRE_TIME = expireTime,
						GITHUB_LOG_IN_ACCOUNT = gitInfo.login,
						TOKEN = token
					});
					entity.SaveChanges();
					HttpCookie cookie = new HttpCookie("token");
					cookie.Value = token;
					cookie.Expires = expireTime;
					Response.SetCookie(cookie);
				}
			}
#if DEBUG
			Response.Redirect("http://localhost:7592/");
#else
			Response.Redirect("http://woaimulaohu.imwork.net/");
#endif
		}
		public string EmailLogin()
		{
			string nickName = Request["nickName"];
			string email = Request["email"];
			string token = Util.CommonUtil.MD5_Encode(nickName + "" + email);
			using (Entity entity = new Entity())
			{
				entity.USER_INFO.Add(new USER_INFO
				{
					NICK_NAME = nickName,
					AVATAR_URL = "https://s.gravatar.com/avatar/" + Util.CommonUtil.MD5_Encode(email) + "?s=80&d=retro",
					USER_AUTH = 0,
					EXPIRE_TIME = DateTime.Now.AddDays(7),
					TOKEN = token
				});
				entity.SaveChanges();
				HttpCookie cookie = new HttpCookie("token");
				cookie.Value = token;
				cookie.Expires = DateTime.Now.AddDays(7);
				Response.SetCookie(cookie);
			}
			return "success";
		}
	}
}