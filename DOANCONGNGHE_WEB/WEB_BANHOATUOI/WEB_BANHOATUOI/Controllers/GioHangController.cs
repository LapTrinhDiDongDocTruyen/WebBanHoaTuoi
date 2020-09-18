using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_BANHOATUOI.Models;

namespace WEB_BANHOATUOI.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        SQLShopHoaDataContext db = new SQLShopHoaDataContext();
        public List<GIOHANG> LayGioHang()
        {
            List<GIOHANG> lstGioHang = Session["GioHang"] as List<GIOHANG>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GIOHANG>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int ma, string strURL)
        {
            List<GIOHANG> lstGioHang = LayGioHang();

            GIOHANG sp = lstGioHang.Find(n => n.mahoa == ma);
            if (sp == null)
            {
                sp = new GIOHANG(ma);
                lstGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.soluong++;
                return Redirect(strURL);
            }
        }
        public ActionResult XoaGioHang(int ma)
        {
            List<GIOHANG> lstGioHang = LayGioHang();

            GIOHANG sp = lstGioHang.SingleOrDefault(n => n.mahoa == ma);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.mahoa == ma);
                return RedirectToAction("GioHang", "GioHang");
            }
            if(lstGioHang.Count == 0)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("GioHang", "GioHang");
        }
        public ActionResult CapNhatGioHang(int ma, FormCollection f)
        {
            List<GIOHANG> lstGiohang = LayGioHang();
            GIOHANG sanpham = lstGiohang.FirstOrDefault(n => n.mahoa == ma);
            if(sanpham != null)
            {
                sanpham.soluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang","GioHang");
        }
        public ActionResult XoaTaCaGioHang()
        {
            List<GIOHANG> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Home");
        }
        private int TongSoLuong()
        {
            int iTongSL = 0;
            List<GIOHANG> lstGioHang = Session["GioHang"] as List<GIOHANG>;
            if (lstGioHang != null)
            {
                iTongSL = lstGioHang.Sum(n => n.soluong);
            }
            return iTongSL;
        }

        private double tinhTongTien()
        {
            double dTongTien = 0;
            List<GIOHANG> lstGioHang = Session["GioHang"] as List<GIOHANG>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }

        public ActionResult GioHang()
        {
            List<GIOHANG> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = tinhTongTien();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = tinhTongTien();
            return PartialView();
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TK"] == null || Session["TK"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Home");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GIOHANG> lstGH = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = tinhTongTien();
            return View(lstGH);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection col)
        {
            DONHANG dh = new DONHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GIOHANG> gh = LayGioHang();
            dh.MAKH = kh.MAKH;
            dh.NGAYDAT = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", col["NgayGiao"]);
            var diachigiao = col["diachigiao"];
            if (diachigiao == "" || diachigiao == null)
            {

            }
            dh.NGAYGIAO = DateTime.Parse(ngaygiao);
            dh.TINHTRANGGIAOHANG = "Chưa giao hàng";
            dh.TINHTRANGTHANHTOAN = "Chưa thanh toán";
            dh.DIACHIGIAOHANG = diachigiao;
            dh.TONGTIEN = tinhTongTien();
            db.DONHANGs.InsertOnSubmit(dh);
            db.SubmitChanges();
            foreach (var item in gh)
            {
                CHITIETDONHANG ctdh = new CHITIETDONHANG();
                ctdh.MADH = dh.MADH;
                ctdh.MAHOA = item.mahoa;
                ctdh.SOLUONG = item.soluong;
                ctdh.DONGIA = (int)item.dongia;
                db.CHITIETDONHANGs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhan", "GioHang");
        }
        public ActionResult XacNhan()
        {
            return View();
        }
    }
}
