using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebProject.Models
{
	public class ResultObj
	{
		public string Msg { get; set; }
		public object Obj { get; set; }
		public bool IsSuccess { get; set; }
	}
}