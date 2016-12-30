using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebProject.Models.QueryResult
{
	public class HomeResult
	{
		public List<MsgBoardResult> msgBoards { get; set; }
		public List<SnippetResult> snipeetResults { get; set; }
		public HomeResult()
		{
			this.msgBoards = new List<MsgBoardResult>();
			this.snipeetResults = new List<SnippetResult>();
		}
	}
}