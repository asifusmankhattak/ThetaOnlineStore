using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ThetaOnlineStore.Models;

namespace ThetaOnlineStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly RAMADAN20Context ORM;
        private readonly IHostEnvironment ENV;

        public ProductsController(RAMADAN20Context context,IHostEnvironment _ENV)
        {
            ORM = context;
            ENV = _ENV;
        }

        // GET: Products
        public async Task<IActionResult> Index(string SearchQuery)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (string.IsNullOrEmpty(SearchQuery))
            {
                //IList<SystemUser> allp = ORM.SystemUser.ToList<SystemUser>();
                return View(await ORM.Product.ToListAsync());

            }
            else
            {
                return View(ORM.Product.Where(a => a.Name.Contains(SearchQuery)).ToList<Product>());

            }
            //  return View(await ORM.Category.ToListAsync());
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await ORM.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ShortDescription,LongDescription,CurrentStock,CostPrice,SalePrice,ProductCode,Status,OpeningStock,OpeningDate,ProductFeatures,CreatedBy")] Product product,IList<IFormFile> PImages)
        {
            string AllFileNames = "";
            if (PImages != null && PImages.Count > 0)
            {

                foreach (IFormFile PImage in PImages)
                {
                    string FolderPath = ENV.ContentRootPath + "\\wwwroot\\Images\\ProductImages\\";
                    string FileName = Guid.NewGuid() + Path.GetExtension(PImage.FileName);
                    PImage.CopyTo(new FileStream(FolderPath + FileName, FileMode.Create));
                    AllFileNames += (FileName + ",");
                }
            }

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (ModelState.IsValid)
            {  if (AllFileNames.Contains(','))
                {
                    AllFileNames = AllFileNames.Remove(AllFileNames.LastIndexOf(','));
                }
                product.Images = AllFileNames;
                ORM.Add(product);
                await ORM.SaveChangesAsync();
                TempData["Message"] =product.Name + " Successfully added";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await ORM.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ShortDescription,LongDescription,CurrentStock,CostPrice,SalePrice,Images,ProductCode,Status,OpeningStock,OpeningDate,ProductFeatures,CreatedBy")] Product product)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ORM.Update(product);
                    await ORM.SaveChangesAsync();
                    TempData["Message"] = product.Name + "updated Successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await ORM.Product
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}
        //GET: Products/Delete/5
        public string Delete(int id)
        {
            try
            {
                Product p = ORM.Product.Find(id);

                if (p != null)
                {
                    ORM.Product.Remove(p);
                    ORM.SaveChanges();
                    TempData["Message"] = p.Name + "Deleted Successfully";
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await ORM.Product.FindAsync(id);
            ORM.Product.Remove(product);
            await ORM.SaveChangesAsync();
            TempData["Message"] = product.Name + "Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return ORM.Product.Any(e => e.Id == id);
        }
        public int LoadAd1()
        {
            IList<Product> p = ORM.Product.ToList();
               


            return p.Count(); 
        }


        public string LoadAd2()
        {

            System.Threading.Thread.Sleep(10000);

            string Ad1 = "<iframe width = '560' height = '315' src = 'https://www.youtube.com/embed/KW-bqPDTY2k' frameborder = '0' allow = 'accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen ></iframe>";

            return Ad1;
        }

    }
}
