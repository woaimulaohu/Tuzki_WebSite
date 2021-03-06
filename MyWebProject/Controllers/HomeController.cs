﻿using HtmlAgilityPack;
using MyWebProject.Models;
using MyWebProject.Models.Entity;
using MyWebProject.Models.QueryResult;
using MyWebProject.Util_Pro;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	public class HomeController : BaseController
	{
		log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public ActionResult Home()
		{
			return Redirect("~/Views/Main.cshtml");
		}
		public ActionResult Main()
		{
			return View("~/Views/Main.cshtml", Index());
		}

		public string CheckLogin()
		{
			string token = Request["token"];
			using (Entity entity = new Entity())
			{
				List<USER_INFO> list = entity.USER_INFO.Where(u => u.TOKEN == token).ToList();
				if (list.Count > 0)
				{
					HttpCookie cookie = new HttpCookie("userInfo");
					//有中文,cookie需要转utf8,不然IE中cookie显示为乱码
					cookie.Values.Add("name", HttpUtility.UrlEncode(list.First().NICK_NAME, Encoding.UTF8));
					cookie.Values.Add("avatar_url", list.First().AVATAR_URL);
					Response.SetCookie(cookie);
					return "success";
				}
				else
				{
					return "fail";
				}
			}
		}

		// GET: Home
		public HomeResult Index()
		{
			HomeResult homeResult = new HomeResult();
			using (Entity entity = new Entity())
			{
				//首页第一行区域中获取要显示的TOP3 POST
				string sql_top_post = "SELECT top 3 " +
							"POST_CONTENT.POST_CONTENT, " +
							"POST_INFO.DATE, " +
							"POST_INFO.POST_ID, " +
							"POST_INFO.PRAISE_COUNT, " +
							"POST_INFO.REPRODUCED_COUNT, " +
							"POST_INFO.MAIN_TITLE, " +
							"POST_INFO.SECOND_TITLE, " +
							"POST_INFO.VIEW_COUNT, " +
							"POST_INFO.TAG_ID " +
							"FROM " +
							"POST_INFO " +
							"JOIN POST_CONTENT ON POST_CONTENT.POST_ID = POST_INFO.POST_ID " +
							"ORDER BY POST_INFO.DATE DESC";
				var queryResult = entity.Database.SqlQuery<SnippetResult>(sql_top_post).ToList();
				HtmlDocument html = new HtmlDocument();
				StringBuilder sb = new StringBuilder();
				foreach (SnippetResult p in queryResult)
				{
					string content = HttpUtility.HtmlDecode(p.POST_CONTENT);
					if (content.Length > 200)
					{
						html.LoadHtml(p.POST_CONTENT);
						HtmlNodeCollection coll = html.DocumentNode.SelectNodes("//p");
						foreach (HtmlNode node in coll)
						{
							sb.Append(node.InnerText);
						}
						p.POST_CONTENT = sb.ToString().Substring(0, sb.Length > 180 ? 180 - 1 : sb.Length - 1) + "……";
					}
					//把摘要中标签属性获取出来
					homeResult.top3PostResults.Add(p);
					sb.Clear();
				}

				//首页第二行区域获取TOP4 COMMENTS
				string sql_top_comment = "SELECT " +
											"* " +
											"FROM " +
											"COMMENTS AS T1 " +
											"JOIN( " +
											"SELECT " +
											"TOP 4 BEFOR_COMMENTS_ID, " +
											"COUNT(BEFOR_COMMENTS_ID) AS replyCount " +
											"FROM " +
											"COMMENTS " +
											"WHERE " +
											"BEFOR_COMMENTS_ID != 0 " +
											"GROUP BY " +
											"BEFOR_COMMENTS_ID " +
											"ORDER BY " +
											"replyCount DESC " +
											") AS T2 ON t1.COMMENTS_ID = T2.BEFOR_COMMENTS_ID";
				homeResult.msgBoards = entity.Database.SqlQuery<MsgBoardResult>(sql_top_comment).ToList();
				foreach (MsgBoardResult r in homeResult.msgBoards.Where(m => m.POST_ID != 0))
				{
					string title = entity.POST_INFO.Where(p => p.POST_ID == 1).First().MAIN_TITLE;
					r.MAIN_TITLE = title;
				}
				//首页第三行区域获取置顶帖 TOP2 POST依据IS TOP判断
				string sql_top2_post = "SELECT top 2 " +
							"POST_CONTENT.POST_CONTENT, " +
							"POST_INFO.DATE, " +
							"POST_INFO.POST_ID, " +
							"POST_INFO.PRAISE_COUNT, " +
							"POST_INFO.REPRODUCED_COUNT, " +
							"POST_INFO.MAIN_TITLE, " +
							"POST_INFO.SECOND_TITLE, " +
							"POST_INFO.VIEW_COUNT, " +
							"POST_INFO.TAG_ID " +
							"FROM " +
							"POST_INFO " +
							"JOIN POST_CONTENT ON POST_CONTENT.POST_ID = POST_INFO.POST_ID  AND POST_INFO.IS_TOP=1" +
							"ORDER BY POST_INFO.DATE DESC";
				var top2PostResults = entity.Database.SqlQuery<SnippetResult>(sql_top2_post).ToList();
				foreach (SnippetResult p in top2PostResults)
				{
					string content = HttpUtility.HtmlDecode(p.POST_CONTENT);
					if (content.Length > 200)
					{
						html.LoadHtml(p.POST_CONTENT);
						HtmlNodeCollection coll = html.DocumentNode.SelectNodes("//p");
						foreach (HtmlNode node in coll)
						{
							sb.Append(node.InnerText);
						}
						p.POST_CONTENT = sb.ToString().Substring(0, sb.Length > 180 ? 180 - 1 : sb.Length - 1) + "……";
					}
					//把摘要中标签属性获取出来
					homeResult.top2PostResults.Add(p);
				}
			}
			return homeResult;
		}
		public ActionResult ConversationMsgBoard()
		{
			int pageStartNum = 1;
			int pageSize = 5;
			if (Request["pageStartNum"] != null && Request["pageSize"] != null)
			{
				int.TryParse(Request["pageStartNum"].ToString(), out pageStartNum);
				int.TryParse(Request["pageSize"].ToString(), out pageSize);
			}
			List<COMMENTS> commentsNotReplies = new List<COMMENTS>();
			List<MsgBoardResult> listResult = new List<MsgBoardResult>();
			using (Entity entity = new Entity())
			{
				//TOP10最近留言,获取文章明细中显示当前文章相关的留言的集合(不包含回复的)最近pageSize条(文章ID为0的表示大厅留言)
				commentsNotReplies = entity.COMMENTS.Where(c => c.BEFOR_COMMENTS_ID == 0 && c.POST_ID == 0).OrderByDescending(c => c.DATE).ToList();
				if ((pageStartNum - 1) * pageSize > 0)
				{
					//本页前的跳过
					commentsNotReplies = commentsNotReplies.Skip((pageStartNum - 1) * pageSize).ToList();
				}
				commentsNotReplies = commentsNotReplies.Take(pageSize).ToList();
				foreach (COMMENTS commentsNotReply in commentsNotReplies)
				{
					List<COMMENTS> replyInComment = new List<COMMENTS>();
					//检查是否有对应的回复记录
					replyInComment = entity.COMMENTS.Where(c => c.BEFOR_COMMENTS_ID == commentsNotReply.COMMENTS_ID && c.COMMENTS_ID != commentsNotReply.COMMENTS_ID && c.BEFOR_COMMENTS_ID != 0).OrderByDescending(c => c.DATE).ToList();
					listResult.Add(new MsgBoardResult
					{
						COMMENTS_ID = commentsNotReply.COMMENTS_ID,
						DATE = commentsNotReply.DATE,
						NICK_NAME = commentsNotReply.NICK_NAME,
						TEXT = commentsNotReply.TEXT,
						EMAIL = commentsNotReply.EMAIL,
						POST_ID = commentsNotReply.POST_ID,
						BEFOR_COMMENTS_ID = commentsNotReply.BEFOR_COMMENTS_ID,
						REPLY = replyInComment.Count > 0 ? replyInComment : null,
						AVATAR_URL = commentsNotReply.AVATAR_URL
					});
				}
			}
			return View(listResult);
		}

		public string loadGitHistory()
		{
			string after = string.IsNullOrEmpty(Request["after"]) ? string.Empty : "?after=" + Request["after"];
			int page = string.IsNullOrEmpty(Request["page"]) ? 0 : int.Parse(Request["page"]);
			string url = "https://github.com/woaimulaohu/Tuzki_WebSite/commits/master/MyWebProject";
			url += after;
			string gitHtml = Util.CommonUtil.HttpGetResult(url);
			HtmlDocument html = new HtmlDocument();
			StringBuilder sb = new StringBuilder();
			html.LoadHtml(HttpUtility.HtmlDecode(gitHtml));
			HtmlNodeCollection coll = html.DocumentNode.SelectNodes("//div");
			bool flag = false;
			string nextPageTag = string.Empty;
			int i = 1;
			string[] style = { "success", "info", "warning" };
			foreach (HtmlNode node in coll)
			{
				if (node.Attributes.Where(a => a.Name.Equals("class") && a.Value.Equals("commit-group-title")).Count() > 0)
				{
					if (flag)
					{
						sb.Append("</div></div></br>");
					}
					sb.Append("<div class=\"row\" style=\"padding-left: 15px; padding-right: 15px;margin-left:5px;\">" +
							  "<div class=\"alert alert-" + style[page % 3] + "\" style=\"margin-bottom:0px;padding-bottom:20px\">" +
							  "<p style=\"word-break:break-all;font-size:small; word-wrap:break-word;float:left \">");
					sb.Append(node.InnerText);
					sb.Append("</p></br>");
					flag = true;
					i = 1;
				}
				if (node.Attributes.Where(a => a.Name.Equals("class") && a.Value.Equals("table-list-cell")).Count() > 0)
				{
					sb.Append("<p style =\"word-break:break-all;font-size:small; word-wrap:break-word;float:left \">");
					sb.Append("&nbsp;&nbsp;&nbsp;" + i + ")&nbsp;" + node.ChildNodes[1].ChildNodes[1].Attributes.Where(a => a.Name.Equals("title")).First().Value);
					sb.Append("</p></br>");
					i++;
				}
				//pagination
				if (node.Attributes.Where(a => a.Name.Equals("class") && a.Value.Equals("pagination")).Count() > 0)
				{
					nextPageTag = node.ChildNodes[1].Attributes.Where(a => a.Name.Equals("href")).First().Value.Split('=')[1];
					nextPageTag = "<a  style=\"padding-left: 15px; padding-right: 15px;margin-left:5px;\" id='historyMore' href='javascript:void(0)' onclick=\"loadGitHistory('" + nextPageTag + "','" + (page + 1) + "')\">更多</a>";
				}
			}
			sb.Append("</div></div></br>" + nextPageTag + "</br>");
			return sb.ToString();
		}
		public ActionResult Tips(bool IsSuccess, string Msg)
		{
			return View("~/Views/ErrorPage.cshtml", new ResultObj { IsSuccess = IsSuccess, Obj = "Error", Msg = Msg });
		}
		public string AudioNext()
		{
			string index = Request["index"];
			return index + "," + "http://mp3.haoduoge.com/s/2017-01-04/1483513792.mp3";
		}
	}
}