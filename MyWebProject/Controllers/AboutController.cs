using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MyWebProject.Controllers
{
	public class AboutController : BaseController
	{
		// GET: About
		public ActionResult Index()
		{
			return View();
		}
		public string contactMe()
		{
			try
			{
				MailMessage mail = new MailMessage();
				mail.From = new MailAddress("tuzkiwebmanager@163.com"); //发送人邮箱地址
				mail.To.Add("crazytuzki@vip.qq.com");   //收件人邮箱地址
				mail.Subject = Request["WebSite联系留言"];    //主题
				mail.Body = "发件人名称 : " + Request["name"] + "\n 发件人邮箱 : " + Request["email"] + "\n 联系方式 : " + Request["customContact"] + "\n 正文 : " + Request["Message"] + "\n";    //正文
				SmtpClient smtp = new SmtpClient();
				smtp.Host = "smtp.163.com";         //smtp服务器名称
				smtp.UseDefaultCredentials = true;
				smtp.Credentials = new NetworkCredential("tuzkiwebmanager@163.com", "zz123456");  //发送人的登录名和密码manager123(网易要求为授权码zz123456)
				smtp.Send(mail);
				return "success";
			}
			catch (Exception ex)
			{
				return "fail";
			}

		}

	}
}