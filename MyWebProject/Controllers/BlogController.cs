﻿using MyWebProject.Models;
using MyWebProject.Models.Entity;
using MyWebProject.Models.QueryResult;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
		/// <summary>
		/// 获取摘要内容
		/// </summary>
		/// <returns></returns>
		[ValidateInput(false)]
		public ActionResult SnippetContent()
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
			   "POST_INFO.TITLE," +
			   "POST_INFO.VIEW_COUNT, " +
			   "POST_INFO.TAG_ID " +
			   "FROM " +
			   "POST_INFO " +
			   "JOIN POST_CONTENT ON POST_CONTENT.POST_ID = POST_INFO.POST_ID " +
			   "AND POST_CONTENT.POST_ID BETWEEN {0} AND {1}";
				var queryResult = entity.Database.SqlQuery<SnippetResult>(string.Format(sql, (pageStartNum - 1) * pageSize, (pageStartNum - 1) * pageSize + pageSize)).ToList();
				foreach (SnippetResult p in queryResult)
				{
					string content = HttpUtility.HtmlDecode(p.POST_CONTENT);
					p.POST_CONTENT = content.Substring(content.IndexOf("<p>"), content.IndexOf("</p>", content.IndexOf("<p>") + 1) - content.IndexOf("<p>") + 4);
					p.POST_CONTENT = p.POST_CONTENT.Length > 1000 ? p.POST_CONTENT.Substring(1000) + "</p>" : p.POST_CONTENT;
					//把摘要中标签属性获取出来
					p.tag_info = entity.Database.SqlQuery<TAG_INFO>("select * from TAG_INFO where TAG_ID in (" + p.TAG_ID + " )").ToList();
					list.Add(p);
				}
				return View(list);
			}
		}
		public ActionResult Detial()
		{
			int postId = int.Parse(Request["postId"].ToString());
			List<SnippetResult> list = new List<SnippetResult>();
			using (Entity entity = new Entity())
			{
				//conn.ConnectionString = "Server= 127.0.0.1\\SQLEXPRESS;DataDase= MyWebSite;user id=WebManager ;password= ;";
				string sql = "SELECT " +
			   "POST_CONTENT.POST_CONTENT," +
			   "POST_INFO.DATE," +
			   "POST_INFO.PRAISE_COUNT," +
			   "POST_INFO.REPRODUCED_COUNT," +
			   "POST_INFO.TITLE," +
			   "POST_INFO.VIEW_COUNT, " +
			   "POST_INFO.TAG_ID " +
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
				return View(list);
			}
		}
		public ActionResult Search()
		{
			return View();
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
					//获取文章明细中显示当前文章相关的留言的集合(不包含回复的)最近pageSize条
					commentsNotReplies = entity.COMMENTS.Where(c => c.POST_ID == postId && c.BEFOR_COMMENTS_ID == 0 && c.POST_ID >= (pageStartNum - 1) * pageSize).Take(pageSize).OrderByDescending(c => c.DATE).ToList();
				}
				else
				{
					//TOP10最近留言,获取文章明细中显示当前文章相关的留言的集合(不包含回复的)最近pageSize条(不考虑文章ID)
					commentsNotReplies = entity.COMMENTS.Where(c => c.BEFOR_COMMENTS_ID == 0 && c.POST_ID >= (pageStartNum - 1) * pageSize).Take(pageSize).OrderByDescending(c => c.DATE).ToList();
				}
				foreach (COMMENTS commentsNotReply in commentsNotReplies)
				{
					List<COMMENTS> replyInComment = new List<COMMENTS>();
					//检查是否有对应的回复记录
					replyInComment = entity.COMMENTS.Where(c => c.POST_ID == commentsNotReply.POST_ID).OrderBy(c => c.DATE).ToList();
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
	}
}