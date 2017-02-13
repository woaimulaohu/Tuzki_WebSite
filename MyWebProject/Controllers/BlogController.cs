using HtmlAgilityPack;
using MyWebProject.Models;
using MyWebProject.Models.Entity;
using MyWebProject.Models.QueryResult;
using MyWebProject.Util_Pro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	public class BlogController : BaseController
	{
		log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		// GET: Blog
		public ActionResult Index()
		{
			return View();
		}
		/// <summary>
		/// 获取留言板框架
		/// </summary>
		/// <returns></returns>
		public ActionResult Snippet()
		{
			return View();
		}
		public ActionResult DetialDirect()
		{
			object postId = Request["postId"].ToString();
			if (postId.Equals("0"))
			{
				return SnippetContent();
			}
			return View(postId);
		}
		/// <summary>
		/// 获取摘要内容
		/// </summary>
		/// <returns></returns>
		[ValidateInput(false)]
		public ActionResult SnippetContent()
		{
			return View(base.getSnippet(Request));
		}

		public ActionResult Detial()
		{
			return View(base.getDetial(Request));
		}
		/// <summary>
		/// 获取留言板框架页面
		/// </summary>
		/// <returns></returns>
		public ActionResult MsgBoard()
		{
			int postId = 0;
			if (Request["postId"] != null)
			{
				int.TryParse(Request["postId"].ToString(), out postId);
			}
			return View(postId);
		}
		/// <summary>
		/// 获取留言板内容
		/// </summary>
		/// <returns></returns>
		public ActionResult MsgBoardContent()
		{
			return View(base.getMsgBoardContent(Request));
		}
		/// <summary>
		/// 将留言信息插入数据库
		/// </summary>
		/// <returns></returns>
		[ValidateInput(false)]
		public string leaveComment()
		{
			string nickName = Request["nickName"];
			int postId = 0;
			int.TryParse(Request["postId"], out postId);
			string avatarUrl = Request["avatarUrl"];
			int beforCommentsId = 0;
			int.TryParse(Request["beforCommentsId"], out beforCommentsId);
			string comment = Request["comment"];
			using (Entity entity = new Entity())
			{
				entity.COMMENTS.Add(new COMMENTS
				{
					COMMENTS_ID = -1,
					POST_ID = postId,
					NICK_NAME = nickName,
					DATE = DateTime.Now,
					TEXT = comment,
					BEFOR_COMMENTS_ID = beforCommentsId,
					AVATAR_URL = avatarUrl
				});
				entity.SaveChanges();
			}
			return "success";
		}
		public ActionResult Search()
		{
			string keyWords = string.IsNullOrEmpty(Request["keyWords"]) ? "*" : Request["keyWords"];
			string jsonStr = SearchEngine.searchGet(keyWords);
			JObject ja = (JObject)JsonConvert.DeserializeObject(jsonStr);
			JObject response = (JObject)JsonConvert.DeserializeObject(ja["response"].ToString());
			if (int.Parse(response["numFound"].ToString()) < 1)
			{
				return View(new ResultObj() { IsSuccess = false, Msg = "未找到对应结果" });
			}
			JArray docs = (JArray)JsonConvert.DeserializeObject(response["docs"].ToString());
			HtmlDocument html = new HtmlDocument();
			StringBuilder sb = new StringBuilder();
			foreach (JObject obj in docs)
			{
				string content = HttpUtility.HtmlDecode(obj["postContent"].ToString());
				if (content.Length > 200)
				{
					html.LoadHtml(content);
					HtmlNodeCollection coll = html.DocumentNode.SelectNodes("//p");
					foreach (HtmlNode node in coll)
					{
						sb.Append(node.InnerText);
					}
					obj["postContent"] = sb.ToString().Substring(0, sb.Length > 180 ? 180 - 1 : sb.Length - 1) + "……";
				}
			}
			return View(new ResultObj() { Obj = docs, IsSuccess = true });
		}
	}
}