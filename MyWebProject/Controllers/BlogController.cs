using MyWebProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	public class BlogController : Controller
	{
		// GET: Blog
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Snippet()
		{
			//using (EntityConnection conn = new EntityConnection())
			//{
			//	conn.Open();
			//	EntityCommand comm = new EntityCommand();
			//	comm.CommandText = "";
			//	comm.ExecuteReader();
			//}
			List<POST_INFO> list = new List<POST_INFO>();

			using (Entity entity = new Entity())
			{
				var result = entity.POST_INFO.Where(p => p.POST_ID >= 2 && p.POST_ID <= 4);
				list = result as List<POST_INFO>;
			}
			return View(list);
		}
		public ActionResult Detial()
		{
			return View();
		}
		public ActionResult Search()
		{
			return View();
		}
		public ActionResult MsgBoard()
		{
			return View();
		}
		public string SnippetPaging()
		{
			return "<h3>CSS 派生选择器</h3>< h4 > 通过依据元素在其位置的上下文关系来定义样式，你可以使标记更加简洁。</ h4 >< p >" +
					   "在 CSS1 中，通过这种方式来应用规则的选择器被称为上下文选择器(contextual selectors)，这是由于它们依赖于上下文关系来应用或者避免某项规则。" +
					   "在 CSS2 中，它们称为派生选择器，但是无论你如何称呼它们，它们的作用都是相同的。" +
					"派生选择器允许你根据文档的上下文关系来确定某个标签的样式。通过合理地使用派生选择器，我们可以使 HTML 代码变得更加整洁。......" +

				"</ p >				< ul class=\"list-inline\">                   <li>                        <span class=\"label label-primary\">CSS</span>" +
					 "   <span class=\"label label-primary\">派生选择器</span>                    </li>                    <li>" +
						"<p>2016-01-01 12:01:59</p>                    </li>                    <li>                        <p>阅读次数:100</p>                    </li> " +
			"	</ul>                <div class=\"w-btn\">                    <a href = \"#\" class=\"hvr-shutter-out-horizontal\" onclick=\"getDetial($(this).next().attr('postId'))\">查看</a>" +
					"<input style = \"visibility:collapse\" postId=\"3\" />                </div>";
		}
	}
}