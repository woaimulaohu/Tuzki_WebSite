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
	public class BlogController : Controller
	{
		log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		// GET: Blog
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 获取摘要
		/// </summary>
		/// <returns></returns>
		[ValidateInput(false)]
		public ActionResult Snippet()
		{
			int pageStartNum = 0;
			int pageSize = 5;
			var aaa=Request["content"].ToString();
			string ccc=HttpUtility.UrlDecode(aaa);
		
			if (Request["pageStartNum"] != null && Request["pageSize"] != null)
			{
				int.TryParse(Request["pageStartNum"].ToString(), out pageStartNum);
				int.TryParse(Request["pageSize"].ToString(), out pageStartNum);
			}

			List<SnippetResult> list = new List<SnippetResult>();
			try
			{
				using (Entity entity = new Entity())
				{
					//conn.ConnectionString = "Server= 127.0.0.1\\SQLEXPRESS;DataDase= MyWebSite;user id=WebManager ;password= ;";
					string sql = "SELECT " +
				   "POST_CONTENT.POST_CONTENT," +
				   "POST_INFO.DATE," +
				   "POST_INFO.PRAISE_COUNT," +
				   "POST_INFO.REPRODUCED_COUNT," +
				   "POST_INFO.TITLE," +
				   "POST_INFO.VIEW_COUNT " +
				   "FROM " +
				   "POST_INFO " +
				   "JOIN POST_CONTENT ON POST_CONTENT.POST_ID = POST_INFO.POST_ID " +
				   "AND POST_CONTENT.POST_ID BETWEEN {0} AND {1}";
					var queryResult = entity.Database.SqlQuery<SnippetResult>(string.Format(sql, pageStartNum * pageSize, pageStartNum * pageSize + pageSize)).ToList();
					foreach (SnippetResult p in queryResult)
					{
						list.Add(p);
					}
					string a = JsonConvert.SerializeObject(queryResult);
					return View(list);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex);
			}
			return View();
		}
		public ActionResult Detial()
		{
			return View();
		}
		public ActionResult Search()
		{
			return View();
		}
		public ActionResult MsgBoard()
		{
			return View();
		}
		public ActionResult SnippetPaging()
		{
			int pageStartNum = 0;
			int pageSize = 0;
			int.TryParse(Request["pageStartNum"].ToString(), out pageStartNum);
			int.TryParse(Request["pageSize"].ToString(), out pageStartNum);

			List<POST_INFO> list = new List<POST_INFO>();
			try
			{
				using (EntityConnection conn = new EntityConnection())
				{
					conn.Open();
					EntityCommand comm = new EntityCommand();
					string sql = "SELECT" +
				   "POST_CONTENT.POST_CONTENT," +
				   "POST_INFO.DATE," +
				   "POST_INFO.PRAISE_COUNT," +
				   "POST_INFO.REPRODUCED_COUNT," +
				   "POST_INFO.TITLE," +
				   "POST_INFO.VIEW_COUNT" +
				   "FROM" +
				   "POST_INFO" +
				   "JOIN POST_CONTENT ON POST_CONTENT.POST_ID = POST_INFO.POST_ID" +
				   "AND POST_CONTENT.POST_ID IN({0},{1})";
					comm.CommandText = string.Format(sql, pageStartNum, pageStartNum * pageSize);
					DbDataReader reader = comm.ExecuteReader();
					//foreach (POST_INFO p in result)
					//{
					//	logger.Info(p.TITLE);
					//	list.Add(p);
					//}
					string a = JsonConvert.SerializeObject(reader);
					return View(list);
				}
			}
			catch (Exception ex)
			{
				logger.Error(ex);
			}
			return View();
		}
	}
}