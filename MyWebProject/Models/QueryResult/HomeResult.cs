using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebProject.Models.QueryResult
{
	public class HomeResult
	{
		/// <summary>
		/// TOP4留言
		/// </summary>
		public List<MsgBoardResult> msgBoards { get; set; }
		/// <summary>
		/// TOP3文章
		/// </summary>
		public List<SnippetResult> top3PostResults { get; set; }
		/// <summary>
		/// TOP2置顶帖
		/// </summary>
		public List<SnippetResult> top2PostResults { get; set; }
		public HomeResult()
		{
			this.msgBoards = new List<MsgBoardResult>();
			this.top3PostResults = new List<SnippetResult>();
			this.top2PostResults = new List<SnippetResult>();
		}
	}
}