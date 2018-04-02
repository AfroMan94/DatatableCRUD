using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatatableCRUD.Models;

namespace DatatableCRUD.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (MyDBEntities dc = new MyDBEntities())
            {
                var employees = dc.Employees.OrderBy(a => a.FirstName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (MyDBEntities dc = new MyDBEntities())
            {
                var v = dc.Employees.Where(a => a.Id == id).FirstOrDefault();
                return View(v);
            }
        }

        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                using (MyDBEntities dc = new MyDBEntities())
                {
                    if (emp.Id > 0)
                    {
                        //Save
                        var v = dc.Employees.Where(a => a.Id == emp.Id).FirstOrDefault();
                        if (v != null) {
                            v.FirstName = emp.FirstName;
                            v.LastName = emp.LastName;
                            v.Email = emp.Email;
                            v.City = emp.City;
                            v.Country = emp.Country;
                        }
                    }
                    else {
                        //Edit
                        dc.Employees.Add(emp);
                    }

                    dc.SaveChanges();
                    status = true;
                }
            }

            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MyDBEntities dc = new MyDBEntities())
            {
                var v = dc.Employees.Where(a => a.Id == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);

                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id) {

            bool status = false;

            using (MyDBEntities dc = new MyDBEntities()) {
                var v = dc.Employees.Where(a => a.Id == id).FirstOrDefault();
                if (v != null)
                {
                    dc.Employees.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }


            return new JsonResult { Data = new { status = status } };

        }


    }
}