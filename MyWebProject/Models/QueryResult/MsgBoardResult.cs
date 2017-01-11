using MyWebProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebProject.Models.QueryResult
{
	public class MsgBoardResult
	{
		public int COMMENTS_ID { get; set; }

		public DateTime DATE { get; set; }

		public string NICK_NAME { get; set; }

		public string TEXT { get; set; }

		public string EMAIL { get; set; }

		public int POST_ID { get; set; }

		public int? BEFOR_COMMENTS_ID { get; set; }
		public List<COMMENTS> REPLY { get; set; }
		public string AVATAR_URL { get; set; }
		public int replyCount { get; set; }
		private string main_title;
		public string MAIN_TITLE
		{
			get
			{
				if (main_title == null)
				{
					return "留言板留言";
				}
				return main_title;
			}
			set
			{
				this.main_title = value;
			}
		}
	}
}