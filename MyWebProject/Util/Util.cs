using MyWebProject.Models.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace MyWebProject.Util
{
	public static class Util
	{
		private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// 缓存操作
		/// </summary>
		public class CacheUtil
		{
			#region

			/// <summary>
			/// 添加缓存,设置超时后如果timeSpan时间内未使用此缓存则移除
			/// </summary>
			/// <param name="key"></param>
			/// <param name="value"></param>
			/// <param name="overTime">入参null默认1hour</param>
			/// <param name="timeSpan">入参null默认30min</param>
			public static void InsertCache(string key, object value, DateTime overTime, TimeSpan timeSpan)
			{
				HttpRuntime.Cache.Insert(key, value, null, overTime == null ? DateTime.UtcNow.AddHours(1) : overTime, timeSpan == null ? new TimeSpan(0, 0, 0, 60 * 30) : timeSpan);
			}
			public static void RemoveCashe(string key)
			{
				HttpRuntime.Cache.Remove(key);
			}
			public static object GetCashe(string key)
			{
				return HttpRuntime.Cache.Get(key);
			}

			#endregion
		}
		/// <summary>
		/// 全局操作
		/// </summary>
		public class CommonUtil
		{
			#region
			/// <summary>
			/// 获取配置文件参数
			/// </summary>
			/// <param name="key"></param>
			/// <returns></returns>
			public static string GetAppConfig(string key)
			{
				return ConfigurationManager.AppSettings[key];
			}
			#endregion
			/// <summary>
			/// 读取文件
			/// </summary>
			/// <param name="filePath"></param>
			/// <returns></returns>
			public static string ReadFile(string filePath)
			{
				try
				{
					using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
					{
						StringBuilder sb = new StringBuilder();
						byte[] buff = new byte[1024];
						while (sr.Peek() > -1)
						{
							sb.Append(sr.ReadLine());
						}
						logger.Debug(sb.ToString());
						return sb.ToString();
					}
				}
				catch (Exception ex)
				{
					logger.Error(ex);
					return null;
				}
			}
		}

		public class DBUtil
		{
			public static void checkDB()
			{
				using (Entity entityDB = new Entity())
				{
					try
					{
						if (!entityDB.Database.Exists())
						{
							//int count = entityDB.Database.ExecuteSqlCommand(CommonUtil.ReadFile(HttpRuntime.AppDomainAppPath + "/Models/script.sql"));
							excutesqlfile();
						}
					}
					catch (Exception ex)
					{
						logger.Error(ex);
					}
				}
			}

			///<summary>
			///利用osql实现执行sql脚本文件
			/// </summary>
			//利用osql实现执行sql脚本文件
			public static void excutesqlfile()
			{
				//data source=127.0.0.1\SQLEXPRESS;initial catalog=MyWebSite;persist security info=True;user id=WebManager;password=123456;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient
				System.Diagnostics.Process sqlProcess = new System.Diagnostics.Process();
				sqlProcess.StartInfo.FileName = "osql.exe ";
				sqlProcess.StartInfo.Arguments = "-S 127.0.0.1\\SQLEXPRESS -U " + "WebManager" + " -P " + "123456" + " -i " + HttpRuntime.AppDomainAppPath + "Models\\script.sql";
				sqlProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
				sqlProcess.Start();
				sqlProcess.WaitForExit();//程序安装过程中执行
				sqlProcess.Close();
			}

			public static void Insert()
			{
				using (Entity entityDB = new Entity())
				{

				}
			}
		}
	}
}