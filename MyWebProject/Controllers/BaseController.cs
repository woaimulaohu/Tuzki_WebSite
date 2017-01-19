using HtmlAgilityPack;
using MyWebProject.Models;
using MyWebProject.Models.Entity;
using MyWebProject.Models.QueryResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	/// <summary>
	/// 对所有Controller进行统一拦截和异常处理
	/// </summary>
	public class BaseController : Controller
	{
		log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// 当请求与此控制器匹配但在此控制器中找不到任何具有指定操作名称的方法时调用。
		/// </summary>
		/// <param name="actionName"></param>
		protected override void HandleUnknownAction(string actionName)
		{
			logger.Error(string.Format("Action_Name :{0} 出现异常", actionName));
			Response.RedirectToRoute(new { action = "Tips", controller = "Home", IsSuccess = false, Msg = "请求访问路径非法" });
		}
		/// <summary>
		/// 在调用action前调用。
		/// </summary>
		/// <param name="filterContext"></param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{

		}
		/// <summary>
		/// 当操作中发生未经处理的异常时调用。
		/// </summary>
		/// <param name="filterContext"> 有关当前请求和操作的信息。</param>
		protected override void OnException(ExceptionContext filterContext)
		{
			logger.Error(string.Format("执行Action出现异常 :{0} ", filterContext.Exception));
			Response.RedirectToRoute(new { action = "Tips", controller = "Home", IsSuccess = false, Msg = "程序出现异常" });
		}
		/// <summary>
		/// 获取博客文章的摘要
		/// </summary>
		/// <param name="Request"></param>
		/// <returns></returns>
		#region
		public List<SnippetResult> getSnippet(HttpRequestBase Request)
		{
			int pageStartNum = 1;
			int pageSize = 5;
			if (Request["pageStartNum"] != null && Request["pageSize"] != null)
			{
				int.TryParse(Request["pageStartNum"].ToString(), out pageStartNum);
				int.TryParse(Request["pageSize"].ToString(), out pageSize);
			}

			List<SnippetResult> list = new List<SnippetResult>();
			using (Entity entity = new Entity())
			{
				//conn.ConnectionString = "Server= 127.0.0.1\\SQLEXPRESS;DataDase= MyWebSite;user id=WebManager ;password= ;";
				string sql = "SELECT " +
			   "POST_CONTENT.POST_CONTENT," +
			   "POST_INFO.DATE," +
			   "POST_INFO.POST_ID," +
			   "POST_INFO.PRAISE_COUNT," +
			   "POST_INFO.REPRODUCED_COUNT," +
			   "POST_INFO.MAIN_TITLE," +
			   "POST_INFO.SECOND_TITLE," +
			   "POST_INFO.VIEW_COUNT, " +
			   "POST_INFO.TAG_ID " +
			   "FROM " +
			   "POST_INFO " +
			   "JOIN POST_CONTENT ON POST_CONTENT.POST_ID = POST_INFO.POST_ID " +
			   "AND POST_CONTENT.POST_ID BETWEEN {0} AND {1}";
				var queryResult = entity.Database.SqlQuery<SnippetResult>(string.Format(sql, (pageStartNum - 1) * pageSize, (pageStartNum - 1) * pageSize + pageSize)).ToList();
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
						p.POST_CONTENT = sb.ToString().Substring(0, 180) + "……";
					}
					//把摘要中标签属性获取出来
					p.tag_info = entity.Database.SqlQuery<TAG_INFO>("select * from TAG_INFO where TAG_ID in (" + p.TAG_ID + " )").ToList();
					list.Add(p);
				}
				return list;
			}
		}
		#endregion
		/// <summary>
		/// 获取博客文章正文
		/// </summary>
		/// <param name="Request"></param>
		/// <returns></returns>
		#region
		public List<SnippetResult> getDetial(HttpRequestBase Request)
		{
			int postId = int.Parse(Request["postId"].ToString());
			List<SnippetResult> list = new List<SnippetResult>();
			using (Entity entity = new Entity())
			{
				entity.POST_INFO.Where(p => p.POST_ID == postId).First().VIEW_COUNT += 1;
				entity.SaveChanges();
				//conn.ConnectionString = "Server= 127.0.0.1\\SQLEXPRESS;DataDase= MyWebSite;user id=WebManager ;password= ;";
				string sql = "SELECT " +
			   "POST_CONTENT.POST_CONTENT," +
			   "POST_INFO.DATE," +
			   "POST_INFO.PRAISE_COUNT," +
			   "POST_INFO.REPRODUCED_COUNT," +
			   "POST_INFO.MAIN_TITLE," +
			   "POST_INFO.SECOND_TITLE," +
			   "POST_INFO.VIEW_COUNT, " +
			   "POST_INFO.TAG_ID, " +
			   "POST_INFO.POST_ID " +
			   "FROM " +
			   "POST_INFO " +
			   "JOIN POST_CONTENT ON POST_CONTENT.POST_ID = POST_INFO.POST_ID " +
			   "AND POST_CONTENT.POST_ID ={0}";
				var queryResult = entity.Database.SqlQuery<SnippetResult>(string.Format(sql, postId)).ToList();
				foreach (SnippetResult p in queryResult)
				{
					string content = HttpUtility.HtmlDecode(p.POST_CONTENT);
					p.POST_CONTENT = content;
					//把摘要中标签属性获取出来
					p.tag_info = entity.Database.SqlQuery<TAG_INFO>("select * from TAG_INFO where TAG_ID in (" + p.TAG_ID + " )").ToList();
					list.Add(p);
				}
				return list;
			}
		}
		#endregion
		/// <summary>
		/// 获取留言板内容
		/// </summary>
		/// <param name="Request"></param>
		/// <returns></returns>
		#region
		public List<MsgBoardResult> getMsgBoardContent(HttpRequestBase Request)
		{
			int postId = 0;//没有正确的文章ID情况下,返回全部留言内容
			int pageStartNum = 1;
			int pageSize = 5;
			if (Request["pageStartNum"] != null && Request["pageSize"] != null)
			{
				int.TryParse(Request["postId"], out postId);
				int.TryParse(Request["pageStartNum"].ToString(), out pageStartNum);
				int.TryParse(Request["pageSize"].ToString(), out pageSize);
			}
			List<COMMENTS> commentsNotReplies = new List<COMMENTS>();
			List<MsgBoardResult> listResult = new List<MsgBoardResult>();
			using (Entity entity = new Entity())
			{
				if (postId > 0)//目前postId>0的情况为查看文章详情
				{
					//获取文章明细中显示当前文章相关的留言的集合(BEFOR_COMMENTS_ID == 0不包含回复的)最近pageSize条
					commentsNotReplies = entity.COMMENTS.Where(c => c.POST_ID == postId && c.BEFOR_COMMENTS_ID == 0).OrderByDescending(c => c.DATE).ToList();
				}
				else
				{
					//TOP10最近留言,获取文章明细中显示当前文章相关的留言的集合(不包含回复的)最近pageSize条(文章ID为0的表示大厅留言)
					commentsNotReplies = entity.COMMENTS.Where(c => c.BEFOR_COMMENTS_ID == 0 && c.POST_ID == 0).OrderByDescending(c => c.DATE).ToList();
				}
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
				return listResult;
			}
		}
		#endregion
	}
}