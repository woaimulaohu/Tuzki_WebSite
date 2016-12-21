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
	}
}
