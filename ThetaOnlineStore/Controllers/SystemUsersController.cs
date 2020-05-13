using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register (SystemUser user)


        {
           
                ORM.SystemUser.Add(user);
                await ORM.SaveChangesAsync();
                // ORM.SaveChanges();
                ViewBag.Message = user.UserName + "User is Successfully Registered";

           

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
        public IActionResult Delete(int id)
        {
            SystemUser p = ORM.SystemUser.Find(id);
            if (p != null)
            {
                ORM.SystemUser.Remove(p);
                ORM.SaveChanges();
                TempData["Message"] = p.UserName + "Deleted Successfully";
                return RedirectToAction("Alluser");
            }
            return View();
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
        public IActionResult Edit(SystemUser p)
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

            ORM.SystemUser.Update(p);
            ORM.SaveChanges();
            return RedirectToAction("Alluser");


        }
    }
}