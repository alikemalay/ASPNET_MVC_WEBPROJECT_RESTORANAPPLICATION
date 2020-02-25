using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _RestoranWeb.Models
{
    public class BigViewModel
    {
        public Register RegisterModel { get; set; }
        public Login LoginModel { get; set; }
        public Semt SemtModel { get; set; }
        public Restoran RestoranModel { get; set; }
        public IEnumerable<Restoran> IERestoran { get; set; }
        public Urun UrunModel { get; set; }
        public Siparis SiparisModel { get; set; }
        public IEnumerable<Siparis> IESiparisModel { get; set; }
        public SiparisDetay SiparisDetayModel { get; set; }

        public IEnumerable<Urun> IEUrun { get; set; }
        public AspNetUsers AspNetUsersModel { get; set; }
        public int SelectedSemtID { get; set; }
        public IEnumerable<SelectListItem> IESemt { get; set; }


    }
}