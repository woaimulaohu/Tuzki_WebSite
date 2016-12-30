using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebProject.Models.QueryResult
{
	public class HomeResult
	{
		public MsgBoard msgBoard { get; set; }
		HomeResult()
		{
		}
		private	class MsgBoard : MsgBoardResult
		{
		}
		class Snippet : SnippetResult
		{
		}

	}
}