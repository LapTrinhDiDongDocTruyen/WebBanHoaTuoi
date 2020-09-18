using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_BANHOATUOI.Models
{
    public class GIOHANG
    {
        SQLShopHoaDataContext db = new SQLShopHoaDataContext();
        public int mahoa { get; set; }

        public string tenhoa { get; set; }

        public string anhbia { get; set; }

        public double dongia { get; set; }

        public int soluong { get; set; }

        public double ThanhTien
        {
            get { return soluong * dongia; }
        }

        public GIOHANG(int ma)
        {
            mahoa = ma;
            HOA h = db.HOAs.Single(m => m.MAHOA == mahoa);
            tenhoa = h.TENHOA;
            anhbia = h.ANHBIA;
            dongia = double.Parse(h.GIABAN.ToString());
            soluong = 1;
        }
    }
}