using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThetaOnlineStore.Models;

namespace ThetaOnlineStore.Controllers
{
    public class SystemUsersController : Controller
    {
        RAMADAN20Context ORM = null;
        public SystemUsersController ( RAMADAN20Context _ORM)
        {
            ORM = _ORM;
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("UnAuthroizedAccess");
            }
            return View();
        }

        public IActionResult UnAuthroizedAccess()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register (SystemUser user)


        {
           

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            ORM.SystemUser.Add(user);
                await ORM.SaveChangesAsync();
            // ORM.SaveChanges();
            TempData["Message"] = user.UserName + "Successfully Registered";



            return View();
        }
        public IActionResult Alluser(string SearchQuery)
        {
            if (TempData["Message"]!=null){
                ViewBag.Message = TempData["Message"].ToString();
             }
            if (string.IsNullOrEmpty(SearchQuery))
            {
                //IList<SystemUser> allp = ORM.SystemUser.ToList<SystemUser>();
                return View(ORM.SystemUser.ToList<SystemUser>());
            } else
            {
                return View(ORM.SystemUser.Where(a=>a.UserName.Contains(SearchQuery)).ToList<SystemUser>());

            }

        }
        public async Task<IActionResult> Detail(int id)
        {
            //

            // SystemUsers p= ORM.SystemUsers.FirstOrDefault(abc => abc.Id==id)
            // SystemUsers p1 = ORM.SystemUsers.Find(id
            //SystemUsers p =await ORM.SystemUsers.FindAsync(id);///
            bool isExixts = ORM.SystemUser.Any(abc => abc.Id == id);
            if (isExixts)
            {
                return View(await ORM.SystemUser.FindAsync(id));
            }
            else

            {
                ViewBag.Message = "not found";
                return View();

            }


        }
        //public IActionResult Delete(int id)
        //{
        //    SystemUser p = ORM.SystemUser.Find(id);
        //    if (p != null)
        //    {
        //        ORM.SystemUser.Remove(p);
        //        ORM.SaveChanges();
        //        TempData["Message"] = p.UserName + "Deleted Successfully";
        //        return RedirectToAction("Alluser");
        //    }
        //    return View();
        //}

        public string Delete(int id)
        {
            try
            {   
                SystemUser u = ORM.SystemUser.Find(id);

                if (u != null)
                {
                    ORM.SystemUser.Remove(u);
                    ORM.SaveChanges();
                    TempData["Message"] = u.UserName + "Deleted Successfully";
                    return "1";
                }
            }
            catch
            {
                return "0";
            }
            finally
            {

            }


            return "0";
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //SystemUsers p= ORM.SystemUsers.FirstOrDefault(abc => abc.Id==id)
            // SystemUsers p1 = ORM.SystemUsers.Find(id
            //SystemUsers p =await ORM.SystemUsers.FindAsync(id);
            return View(await ORM.SystemUser.FindAsync(id));

        }
        [HttpPost]
        public IActionResult Edit(SystemUser u)
        {
            //SystemUsers p= ORM.SystemUsers.FirstOrDefault(abc => abc.Id==id)
            // SystemUsers p1 = ORM.SystemUsers.Find(id
            //SystemUsers p =await ORM.SystemUsers.FindAsync(id);
            //  bool isExixts = ORM.SystemUsers.Any(abc => abc.Id== p.Id);
            //SystemUsers found = ORM.SystemUsers.Find(p.Id);
            // if (found == null)
            // {
            //     return NotFound();
            // }
            // else
            // {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ORM.SystemUser.Update(u);
            ORM.SaveChanges();

            TempData["Message"] = u.UserName + "Updated Successfully";
            return RedirectToAction("Alluser");


        }
        // GET: SystemUsers/Login
        public IActionResult Login()
        {
            return View();
        }


        // POST: SystemUsers/Login
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {

            SystemUser LoggedInUser = ORM.SystemUser.Where(a => a.UserName == Username && a.Password== Password).FirstOrDefault();


            if (LoggedInUser == null)
            {
                //for custom error messaging
                //ViewBag.Message = "Invalid Details";


                ModelState.AddModelError("", "Invalid Details, Please try again.");
                return View();
            }

            HttpContext.Session.SetString("Role", LoggedInUser.Role);
            HttpContext.Session.SetString("DisplayName", LoggedInUser.DisplayName);


            return RedirectToAction("Alluser");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}