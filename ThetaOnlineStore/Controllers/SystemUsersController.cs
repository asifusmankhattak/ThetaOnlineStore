using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ThetaOnlineStore.Models;

namespace ThetaOnlineStore.Controllers
{
    public class SystemUsersController : Controller
    {
        RAMADAN20Context ORM = null;
        private readonly IHostEnvironment ENV;
        public SystemUsersController ( RAMADAN20Context _ORM, IHostEnvironment _ENV)
        {
            ORM = _ORM;
            ENV = _ENV;
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
        public FileResult DownloadCV(string FN)
        {
            string FilePath = ENV.ContentRootPath + "\\wwwroot\\Docs\\CVs\\" + FN;
            string MimeType = MimeGuesser.GuessMimeType(FilePath);
            //MIME Type

            //FileStream FS = new FileStream();
            return File("/Docs/CVs/" + FN, MimeType, Guid.NewGuid() + Path.GetExtension(FN));
        }

        public IActionResult UnAuthroizedAccess()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register (SystemUser user, IFormFile UCV)
        {
            if (UCV != null)
            {

                //in bytes
                if (UCV.Length > 1000)
                {

                }
                string FolderPath = ENV.ContentRootPath + "\\wwwroot\\Docs\\CVs\\";
                string FileName = Guid.NewGuid() + Path.GetExtension(UCV.FileName);
                FileStream FS = new FileStream(FolderPath + FileName, FileMode.Create); ;
                UCV.CopyTo(FS);
                user.CV = FileName;
            }



            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            ORM.SystemUser.Add(user);
                await ORM.SaveChangesAsync();

            // ORM.SaveChanges();
            TempData["Message"] = user.UserName + "Successfully Registered";
            //send welcome email to user
            MailMessage oEmail = new MailMessage();

            oEmail.From = new MailAddress("asifusmankhattak@gmail.com", "The Students");

            oEmail.To.Add(user.Email);
            //oEmail.CC.Add("usman@thetasolutions.co.uk");
            //oEmail.Bcc.Add("");

            oEmail.Subject = "Welcome to Theta Solutions";

            oEmail.Body = "<p><b>Welcome, " +user.DisplayName + ",</b></p><br>" +

                "Thank you for registering your account with Theta.<br>Please find below your account details and keep it safe<br><br>" +


                "Username: " + user.UserName + "<br>" +
                "Password: " + user.Password +

                "<br style='color:red;'>Regards,<br>" +
                "Support Team";

            ;

            oEmail.IsBodyHtml = true;

            if (System.IO.File.Exists("/Docs/CVs/" + user.CV))
            {
                oEmail.Attachments.Add(new Attachment("/Docs/CVs/" + user.CV));
            }


            SmtpClient oSMTP = new SmtpClient();
            oSMTP.Host = "smtp.gmail.com";
            oSMTP.Credentials = new System.Net.NetworkCredential("asifusmankhattak@gmail.com", "drinayatasad1234");
            oSMTP.Port = 587; //25 465
            oSMTP.EnableSsl = true;

            try
            {
                oSMTP.Send(oEmail);
            }
            catch (Exception Ex)
            {

            }


            //send sms

            string SMSAPIURL = "https://sendpk.com/api/sms.php?username=923479389419&password=123456&sender=Masking&mobile=" + user.Mobile + "&message=asif khattak";


            WebRequest request = HttpWebRequest.Create(SMSAPIURL);
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string urlText = reader.ReadToEnd(); // it takes the response from your url. now you can use as your need  

            if (urlText.Contains("OK"))
            {

            }


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


                ModelState.AddModelError("", "UserName or Password is incorrect.");
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
        public int LoadAd1()
        {
            IList<SystemUser> p = ORM.SystemUser.ToList();



            return p.Count();
        }
    }
}