using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThetaOnlineStore.Models;

namespace ThetaOnlineStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly RAMADAN20Context ORM;
  
        private readonly IWebHostEnvironment ENV;

        public CategoriesController(RAMADAN20Context context, IWebHostEnvironment _ENV)
        {
            ORM = context;
            ENV = _ENV;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string SearchQuery)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (string.IsNullOrEmpty(SearchQuery))
            {
                //IList<SystemUser> allp = ORM.SystemUser.ToList<SystemUser>();
                return View(await ORM.Category.ToListAsync());

            }
            else
            {
                return View(ORM.Category.Where(a => a.Name.Contains(SearchQuery)).ToList<Category>());

            }
          //  return View(await ORM.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await ORM.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
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
        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Picture,Status,ShortDescription,LongDescription,CreateDate,CreatedBy,ModifiedDate,ModifiedBy")] Category category, IFormFile CImage)
        {
            string FileName = "";

            if (CImage != null)
            {
                string FTPFolderPath = ENV.ContentRootPath + "\\wwwroot\\Images\\CategoryImages";

                string FileExt = Path.GetExtension(CImage.FileName);

                FileName = Guid.NewGuid() + FileExt;
                string FinalFilePath = FTPFolderPath + "\\" + FileName;





                FileStream FS = new FileStream(FinalFilePath, FileMode.Create);

                CImage.CopyTo(FS);

            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (ModelState.IsValid)
            {
                category.Picture = FileName;
                ORM.Add(category);
                await ORM.SaveChangesAsync();
                TempData["Message"] = category.Name + " Successfully added";
                HttpContext.Session.SetString("CNAME", category.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("UnAuthroizedAccess");
            }
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (id == null)
            {
                return NotFound();
            }

            var category = await ORM.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Picture,Status,ShortDescription,LongDescription,CreateDate,CreatedBy,ModifiedDate,ModifiedBy")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ORM.Update(category);
                    await ORM.SaveChangesAsync();
                    TempData["Message"] = category.Name + " Successfully added";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //GET: Categories/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var category = await ORM.Category
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}
        public string Delete(int id)
        {
            try
            {
                Category c = ORM.Category.Find(id);

                if (c != null)
                {
                    ORM.Category.Remove(c);
                    ORM.SaveChanges();
                    TempData["Message"] = c.Name + "Deleted Successfully";
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


        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await ORM.Category.FindAsync(id);
            ORM.Category.Remove(category);
            await ORM.SaveChangesAsync();
            TempData["Message"] = category.Name + "Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return ORM.Category.Any(e => e.Id == id);
        }
        public int LoadAd1()
        {
            IList<Category> p = ORM.Category.ToList();



            return p.Count();
        }

    }
}
