using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SQLite;
using MvcDemo.Models;

namespace MvcDemo.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult QueryList(int pageIndex)
        {         
            List<Table1> lst = new List<Table1>();
            using (SQLiteConnection conn = new SQLiteConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sqliteConnection"].ConnectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("select  * from table1 " + (pageIndex > 0 ? "limit 10 Offset " + ((pageIndex-1)* 10)  : ""), conn);
                //SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                //System.Data.DataTable dt = new System.Data.DataTable();
                //da.Fill(dt);
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Table1 t1 = new Table1() { ID = dr.GetInt32(0), Title = dr.GetString(1) };
                    lst.Add(t1);
                }
                conn.Close();
            }            
            return Json(lst,JsonRequestBehavior.AllowGet);            
        }
	}
}