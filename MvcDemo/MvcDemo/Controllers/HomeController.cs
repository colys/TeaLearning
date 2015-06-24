using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDemo.Models;

namespace MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index(String message)
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult Login(User user,string message)
        {
            ViewBag.Message = message;
            return View(user);
        }

        /// <summary>
        /// 执行登录操作
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult GoLogin(User user)
        {
            //TODO:开始之前，请修改你web.config中sqlite的路径
            User findUser = null;
            //用参数的方式，可以避免非法字符，以及sql注入的问题,sqlserver 请用@符号
            string sql = "select * from users where account =:account ";
            //使用using方式，如果发生异常，将会执行Displose方法，conn的Dispose方法会关闭连接
            using (SQLiteConnection conn = new SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sqliteConnection"].ConnectionString))
            {
                 conn.Open();
                //执行查询
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.Add(new SQLiteParameter(":account", user.Account));               
                SQLiteDataReader dr = cmd.ExecuteReader();
                //将查询的数据填充到User实体
                if (dr.Read())
                {
                    findUser = new User() { Name=dr["name"].ToString(), Account = dr["account"].ToString(), Password = dr["password"].ToString() };
                }
                conn.Close();
            }
            string Msg = null;
            if (findUser == null) Msg = "账号不存在!";
            else if (findUser.Password != user.Password) Msg = "密码错误！";           


            if(Msg ==null)
            {
                Msg = "登录成功，欢迎回来 , " + findUser.Name;
                return RedirectToAction("Index", new { message = Msg });
            }
            else
            {
                return RedirectToAction("Login", new { user = user, message = Msg });
            }
            
        }

        

    }
}
