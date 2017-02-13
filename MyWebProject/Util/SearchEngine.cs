using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolrNet;

namespace MyWebProject.Util_Pro
{
	public class SearchEngine
	{
		public static void Init()
		{
			//SolrNet.Startup.Init
		}
		public static string searchGet(string key)
		{
			string url = "http://localhost:8983/solr/web_pro/select?indent=on&q=" + key + "&wt=json";
			return Util.CommonUtil.HttpGetResult(url);
		}
	}
}