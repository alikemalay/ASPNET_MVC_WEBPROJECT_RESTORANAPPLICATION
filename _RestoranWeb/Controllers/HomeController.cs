using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using _RestoranWeb.Identity;
using _RestoranWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace _RestoranWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        private Entities dm = new Entities();


        // GET: Account
        public ActionResult Index()
        {
            ViewBag.Semtara = new SelectList(dm.Semt, "SemtId", "SemtAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BigViewModel model, string searchstring, int? semtara)
        {
            var Semtara = new SelectList(dm.Semt, "SemtId", "SemtAdi");
            model.IESemt = Semtara;
            var restoranlar = model.IERestoran;

            if (!string.IsNullOrEmpty(searchstring))
            {
                restoranlar = dm.Semt.Where(p => p.SemtId == semtara).SelectMany(p => p.Restoran).Where(p => p.RestoranAdi.Contains(searchstring));
                return View(model);
            }
            else
            {
                return View("SearchRest");
            }
        }

        public ActionResult SearchRest()
        {
            // Semt tablosundan veri çekme
            ViewBag.Semtara = new SelectList(dm.Semt, "SemtId", "SemtAdi");
            return View();

        }

        [HttpPost]
        //[Authorize(Roles = "Admin, User")]
        public ActionResult SearchRest(BigViewModel model, string searchstring, int? semtara)
        {


            //if (HttpContext.User.IsInRole("Admin"))
            //{

            //    return View("");
            //}
            // Semt tablosundan veri çekme
            ViewBag.Semtara = new SelectList(dm.Semt, "SemtId", "SemtAdi");
            // Restoran Arama Bölümü
            var restoranlar = from m in dm.Restoran
                              select m;


            if (!string.IsNullOrEmpty(searchstring))
            {
                restoranlar = dm.Semt.Where(p => p.SemtId == semtara).SelectMany(p => p.Restoran).Where(p => p.RestoranAdi.Contains(searchstring));
                model.IERestoran = restoranlar;
                return View(model);
            }
            else
            {
                restoranlar = dm.Semt.Where(p => p.SemtId == semtara).SelectMany(p => p.Restoran).Take(10);
                model.IERestoran = restoranlar;
                return View(model);
            }


        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult ListRest(BigViewModel model)
        {
            var listRest = dm.Restoran.OrderByDescending(p => p.EklenmeTarihi).Take(10);
            model.IERestoran = listRest;
            return View(model);

        }

        [Authorize(Roles = "Admin, User")]
        public ActionResult AddRest()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public ActionResult AddRest([Bind(Include = "RestoranId,RestoranAdi,Durum,OrtSipSure,RezervasyonUcreti,EklenmeTarihi")]  Restoran restoran, BigViewModel model)
        {
            DateTime zaman = DateTime.Now;
            model.RestoranModel.EklenmeTarihi = zaman;
            restoran = model.RestoranModel;
           
            if (ModelState.IsValid)
            {
              
            dm.Restoran.Add(restoran);
            dm.SaveChanges();
            return View(model);
            //"DBCC CHECKIDENT ('Restoran', RESEED,0)" RestoranId Identity özelliğini sıfırlar.
            }

            return View();


        }



        //[Authorize(Roles="Admin, User")]
        //public ActionResult SelectProduct(BigViewModel model)
        //{
        //    var RestoranId = Request.QueryString["restoranid"];
        //    ViewBag.RestId = RestoranId;




        //    int restid = Convert.ToInt32(RestoranId);
        //    var selectedRest = dm.Restoran.Where(p => p.RestoranId == restid).FirstOrDefault();
        //    model.RestoranModel = selectedRest;
        //    var products = dm.Restoran.Where(p => p.RestoranId == restid).SelectMany(p => p.Urun).Take(10);
        //    model.IEUrun = products;
        //    var semt = dm.Restoran.Where(p => p.RestoranId == restid).SelectMany(p => p.Semt).FirstOrDefault();
        //    model.SemtModel = semt;
        //    // == ).SelectMany(p => p.Restoran).Where(p => p.RestoranAdi.Contains(searchstring));

        //    return View(model);

        //}


        [Authorize(Roles = "Admin, User")]
        public ActionResult SelectProduct([Bind(Include = "SiparisID,SiparisTarih,Id,RestoranId,UrunId,UrunMiktari")] BigViewModel model, Siparis siparis)
        {
            var RestoranId = Request.QueryString["restoranid"];
            ViewBag.RestId = RestoranId;

            int restid = Convert.ToInt32(RestoranId);
            var selectedRest = dm.Restoran.Where(p => p.RestoranId == restid).FirstOrDefault();
            model.RestoranModel = selectedRest;
            var products = dm.Restoran.Where(p => p.RestoranId == restid).SelectMany(p => p.Urun).Take(10);
            model.IEUrun = products;
            var semt = dm.Restoran.Where(p => p.RestoranId == restid).SelectMany(p => p.Semt).FirstOrDefault();
            model.SemtModel = semt;
            // == ).SelectMany(p => p.Restoran).Where(p => p.RestoranAdi.Contains(searchstring));

            string adi = User.Identity.Name;
            var kullaniciadi = dm.AspNetUsers.Where(a => a.UserName == adi).FirstOrDefault();

            var UrunId = Request.QueryString["urunid"];
            ViewBag.UrunId = UrunId;

            DateTime zaman = DateTime.Now;



            var kullaniciSiparisleri = dm.Siparis.Where(p => p.Id == kullaniciadi.Id).Where(p => (p.SiparisTarih.Month) - zaman.Month == 0).OrderByDescending(p => p.SiparisTarih).Take(3);
            model.IESiparisModel = kullaniciSiparisleri;
            //Son bir ay dm.Siparis.Where(p => p.Id == kullaniciadi.Id).Where(p => (p.SiparisTarih.Month) - zaman.Month == 0).OrderByDescending(p => p.SiparisTarih).Take(3);
            //Son bir saat  dm.Siparis.Where(p => p.Id == kullaniciadi.Id).Where(p => ((p.SiparisTarih.Hour) + 1 - zaman.Hour == 0) || ((p.SiparisTarih.Hour) + 1 - zaman.Hour == 1)).OrderByDescending(p => p.SiparisTarih).Take(3);


            if (UrunId != null)
            {
                if (ModelState.IsValid)
                {

                    siparis.Id = kullaniciadi.Id;
                    siparis.SiparisTarih = zaman;
                    siparis.RestoranId = Convert.ToInt32(RestoranId);
                    siparis.UrunId = Convert.ToInt32(UrunId);

                    model.SiparisModel = siparis;
                    dm.Siparis.Add(model.SiparisModel);
                    dm.SaveChanges();

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Register", "Account");
                }
            }


            return View(model);


        }

    }
}