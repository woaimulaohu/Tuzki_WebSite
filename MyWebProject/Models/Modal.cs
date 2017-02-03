using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;

namespace MyWebProject.Models
{
	public class Modal
	{
		public Modal()
		{
			this.listData = new List<dynamic>();
		}
		public string saveFuncTionName { get; set; }
		public string tips { get; set; }
		public List<dynamic> listData { get; set; }
	}
}