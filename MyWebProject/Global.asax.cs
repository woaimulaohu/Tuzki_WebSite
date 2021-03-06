﻿using MyWebProject.Models.Entity;
using MyWebProject.Util_Pro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			OnLoad();
		}
		/// <summary>
		/// 程序启动需要加载的数据
		/// </summary>
		private void OnLoad()
		{
#if !DEBUG
			Util.DBUtil.checkDB();
#endif
			PorgramInit.Cache.CacheInit();
		}
		protected void Application_Error(object sender, EventArgs e)
		{
			var ex = Server.GetLastError();
			logger.Error(ex); //记录日志信息  
			Server.ClearError();//处理后要清除本次异常记录,否则下次再取lastError会将本次再取出

		}
		protected void Session_Start(object sender, EventArgs e)
		{
			using (Entity entity = new Entity())
			{
				entity.VISIT_IP.Add(new VISIT_IP()
				{
					IP_ADDRESS = GetIPAddress(),
					SESSION_START_TIME = DateTime.Now
				});
				entity.SaveChanges();
			}
		}
		public static string GetIPAddress()
		{
			string ipv4 = String.Empty;
			foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
			{
				if (IPA.AddressFamily.ToString() == "InterNetwork")
				{
					ipv4 = IPA.ToString();
					break;
				}
			}
			if (ipv4 != String.Empty)
			{
				return ipv4;
			}
			foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
			{
				if (IPA.AddressFamily.ToString() == "InterNetwork")
				{
					ipv4 = IPA.ToString();
					break;
				}
			}
			return ipv4;
		}
		protected void Session_End(object sender, EventArgs e)
		{
			// 在会话结束时运行的代码。 
			// 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
			// InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
			// 或 SQLServer，则不会引发该事件。

		}
	}
}
