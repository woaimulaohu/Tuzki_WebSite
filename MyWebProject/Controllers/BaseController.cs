using MyWebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	/// <summary>
	/// 对所有Controller进行统一拦截和异常处理
	/// </summary>
	public class BaseController : Controller
	{
		log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// 当请求与此控制器匹配但在此控制器中找不到任何具有指定操作名称的方法时调用。
		/// </summary>
		/// <param name="actionName"></param>
		protected override void HandleUnknownAction(string actionName)
		{
			logger.Error(string.Format("Action_Name :{0} 出现异常", actionName));
			Response.RedirectToRoute(new { action = "Tips", controller = "Home", IsSuccess = false, Msg = "请求访问路径非法" });
		}
		/// <summary>
		/// 在调用操作方法前调用。
		/// </summary>
		/// <param name="filterContext"></param>
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			
		}
		/// <summary>
		/// 当操作中发生未经处理的异常时调用。
		/// </summary>
		/// <param name="filterContext"> 有关当前请求和操作的信息。</param>
		protected override void OnException(ExceptionContext filterContext)
		{
			logger.Error(string.Format("执行Action出现异常 :{0} ", filterContext.Exception));
			Response.RedirectToRoute(new { action = "Tips", controller = "Home", IsSuccess = false, Msg = "程序出现异常" });
		}
	}
}