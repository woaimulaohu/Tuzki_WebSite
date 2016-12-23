using MyWebProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebProject.Models.QueryResult
{
	public class SnippetResult
	{
		public string POST_CONTENT { get; set;}
		public int POST_ID { get; set; }

		public DateTime DATE { get; set; }

		public string TITLE { get; set; }

		public int? VIEW_COUNT { get; set; }

		public int? REPRODUCED_COUNT { get; set; }

		public int? PRAISE_COUNT { get; set; }

		public string TAG_ID { get; set; }
	}
}