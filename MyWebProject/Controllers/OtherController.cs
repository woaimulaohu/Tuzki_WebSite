using MyWebProject.Models;
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
		private string HttpGetResult(string reqUrl)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqUrl);
			req.Method = "GET";
			req.Timeout = 40 * 1000;
			//必须加上UserAgent信息,值为Developer Settings-->OAuth Application-->Application name
			req.UserAgent = "Personal web page";
			req.ContentType = "application/x-www-form-urlencoded";
			HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
			// Get the stream associated with the response.
			Stream receiveStream = rsp.GetResponseStream();
			// Pipes the stream to a higher level stream reader with the required encoding format. 
			return new StreamReader(receiveStream, Encoding.UTF8).ReadToEnd();
		}

		/// <summary>
		/// 第三方登录方式github
		/// </summary>
		public void GitHubAccountLogin()
		{
			string code = Request["code"];
			string step2Url = "https://github.com/login/oauth/access_token?client_id=1e8991731cc506dd7aa3&client_secret=b5e72132d8c5eadda8100e368ae2718907c5ecda&code=" + code;
			string access_token = HttpGetResult(step2Url).Split('&')[0].Split('=')[1];
			string step3Url = "https://api.github.com/user?access_token=" + access_token;
			string json = HttpGetResult(step3Url);
			GitHubUserInfo userInfo = JsonConvert.DeserializeObject<GitHubUserInfo>(json);
			HttpCookie cookie = new HttpCookie("userInfo");
			cookie.Values.Add("name", userInfo.name);
			cookie.Values.Add("avatar_url", userInfo.avatar_url);
			Response.SetCookie(cookie);
#if DEBUG
			Response.Redirect("http://localhost:7592/");
#else
			Response.Redirect("http://woaimulaohu.imwork.net/");
#endif
		}
	}
}