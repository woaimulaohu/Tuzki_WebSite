﻿using MyWebProject.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyWebProject
{
	public class MvcApplication : System.Web.HttpApplication
	{
		[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
		log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		protected void Application_Start()
		{
			log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
#if DEBUG
#else
			Util.Util.DBUtil.checkDB();
#endif
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}
		/// <summary>
		/// 程序启动需要加载的数据
		/// </summary>
		private void OnLoad()
		{
			PorgramInit.Cache.CacheInit();
		}
		//protected void Application_Error(object sender, EventArgs e)
		//{
		//	var ex = Server.GetLastError();
		//	logger.Error(ex); //记录日志信息  
		
		//}
	}
}
