using CHETU_MVC_CRUD_APP.DB_context_EF;
using CHETU_MVC_CRUD_APP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CHETU_MVC_CRUD_APP.Controllers
{
  
    public class HomeController : Controller
    {
        Uri baseaddress = new Uri("http://localhost:57368/");
        HttpClient Client;
    

        public HomeController()
        {
            Client = new HttpClient();
            Client.BaseAddress = baseaddress;

        }
       
       
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
       
       
        public ActionResult Index(usermodel log)
        {
            Company_2Entities ent = new Company_2Entities();
            var user = ent.loginusers.Where(m => m.email == log.email).FirstOrDefault();
            if (user == null)
            {
                TempData["invalid"] = "Email.is not found invalid user ";
            }
            else
            {
                if(user.email==log.email && user.password==log.password)
                {
                    FormsAuthentication.SetAuthCookie(log.email, false);
                    Session["user"] = user.name;
                    return RedirectToAction("indexdashboard", "Home");
                }
                else
                {
                    TempData["not valid"] = "wrong password";
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        public ActionResult emplist()
        {
           // Company_2Entities obj = new Company_2Entities();
          
            List<empmodel> mod = new List<empmodel>();
            List<employee> tb = new List<employee>();

            HttpResponseMessage emp = Client.GetAsync(Client.BaseAddress + "get/employee").Result;

            if (emp.IsSuccessStatusCode)
            {

                string data = emp.Content.ReadAsStringAsync().Result;


                tb = JsonConvert.DeserializeObject<List<employee>>(data);

            }

            return View(tb);
        }

        [Authorize]
        public ActionResult indexdashboard()
        {
            return View();
        }
        [HttpGet]

        [Authorize]
        public ActionResult add_user()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult add_user(empmodel mod)
        {
            List<employee> dat=new List<employee>();
            
           
            string data = JsonConvert.SerializeObject(mod);

            StringContent content = new StringContent(data, Encoding.UTF8, mediaType: "application/json");

            HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "Emp/SaveEmployee", content).Result;

            return RedirectToAction("emplist");

        }
        [Authorize]
        public ActionResult edit(int id)
        {
            Company_2Entities ent = new Company_2Entities();
            var edit = ent.employees.Where(m => m.id == id).First();
            empmodel mod = new empmodel();
            mod.id = edit.id;
            mod.name = edit.name;
            mod.email = edit.email;
            mod.mobile = edit.mobile;
            mod.department = edit.department;
            mod.city = edit.city;

            return RedirectToAction("add_user", mod);

        }
      
        public ActionResult delete(int id)
        {
           // Company_2Entities ent = new Company_2Entities();
            //var delt = ent.employees.Where(m => m.id == id).First();
           // ent.employees.Remove(delt);
            //ent.SaveChanges();
            return RedirectToAction("emplist", "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult user_registration()
        {
           

            return View();
        }
      
        [HttpPost]
        [AllowAnonymous]
        public ActionResult user_registration(usermodel mod)
        {
            Company_2Entities db = new Company_2Entities();
            loginuser tb = new loginuser();
            tb.id = mod.id;
            tb.name = mod.name;
            tb.email = mod.email;
            tb.password = mod.password;
            db.loginusers.Add(tb);
            db.SaveChanges();
            return RedirectToAction("indexdashboard");
        }
        public ActionResult user_table()
        {
            Company_2Entities obj = new Company_2Entities();
            var dta = obj.loginusers.ToList();
            return View(dta);
            
        }
        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");



        }
    }
}