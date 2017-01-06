using MyWebProject.Models;
using MyWebProject.Models.Entity;
using MyWebProject.Models.QueryResult;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
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
			return View("~/Views/Main.cshtml", Index());
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
				foreach (SnippetResult p in queryResult)
				{
					string content = HttpUtility.HtmlDecode(p.POST_CONTENT);
					if (p.POST_CONTENT.Length > 200)
					{
						p.POST_CONTENT = p.POST_CONTENT.Substring(0, 180) + "……";
					}
					//把摘要中标签属性获取出来
					homeResult.top3PostResults.Add(p);
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
					if (p.POST_CONTENT.Length > 200)
					{
						p.POST_CONTENT = p.POST_CONTENT.Substring(0, 180) + "……";
					}
					//把摘要中标签属性获取出来
					homeResult.top2PostResults.Add(p);
				}
			}
			return homeResult;
		}
		public ActionResult ConversationMsgBoard()
		{
			return View();
		}
		public ActionResult Typo()
		{
			return View();
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