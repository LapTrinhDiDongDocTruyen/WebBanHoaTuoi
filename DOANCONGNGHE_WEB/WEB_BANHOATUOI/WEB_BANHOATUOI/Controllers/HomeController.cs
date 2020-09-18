using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_BANHOATUOI.Models;
using PagedList;
using PagedList.Mvc;

namespace WEB_BANHOATUOI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        SQLShopHoaDataContext db = new SQLShopHoaDataContext();

        public ActionResult Index(int? page)
        {
            if (Session["DN"] == null)
            {
                Session["DN"] = @Url.Action("DangNhap");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 8;
            return View(db.HOAs.ToList().OrderBy(n => n.MAHOA).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChiTiet(int id)
        {
            var hoa = db.HOAs.Where(m => m.MAHOA == id).First();
            return View(hoa);
        }
        public ActionResult DangXuat()
        {
            string s = @Url.Action("DangNhap");
            @Session["DX"] = "";
            @Session["TK"] = "";
            @Session["DN"] = s;
            return RedirectToAction("Index","Home");
        }
        public ActionResult SanPhamLQ(int m)
        {
            List<HOA> lst = db.HOAs.Where(t => t.MACHUDE == m).ToList();
            return PartialView(lst);
        }
        public ActionResult HinhAnhDong()
        {
            return PartialView();
        }
        public ActionResult ChuDe()
        {
            List<CHUDE> lst = db.CHUDEs.ToList();
            return PartialView(lst);
        }
        public ActionResult Hoa(int m)
        {
            List<HOA> lst = db.HOAs.Where(t => t.MACHUDE == m).ToList();
            return View(lst);
        }
        public ActionResult HoaTheoLoai(int m)
        {
            List<HOA> lst = db.HOAs.Where(t => t.LOAI == m).ToList();
            return View(lst);
        }
        public ActionResult LoaiHoa()
        {
            List<LOAI> lst = db.LOAIs.ToList();
            return PartialView(lst);
        }
        public ActionResult TrangKhac()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.DangKy = new SelectList(new object[] {"Nam","Nữ"});
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection col, KHACHHANG kh)
        {
            var hoten = col["hoten"];
            var tentk = col["tentk"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", col["ngaysinh"]);

            var mk = col["matkhau"];
            var gioitinh = col["gioitinh"];
            var nlmk = col["nhaplaimk"];
            var sdt = col["txtsdt"];
            var email = col["email"];
            var diachi = col["diachi"];
            if (mk != nlmk)
            {
                ViewBag.ThongbaoDK = "Mật khẩu nhập lại không đúng";
                return RedirectToAction("DangKy", "Home");
            }
            kh.HOTEN = hoten;
            kh.TAIKHOAN = tentk;
            kh.MATKHAU = mk;
            kh.NGAYSINH = DateTime.Parse(ngaysinh);
            kh.GIOITINH = gioitinh;
            kh.DIENTHOAI = sdt;
            kh.EMAIL = email;
            kh.DIACHI = diachi;
            db.KHACHHANGs.InsertOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("DangNhap","Home");
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection col)
        {
            var tendn = col["uname"];
            var mk = col["pswd"];
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TAIKHOAN == tendn && n.MATKHAU == mk);
            if (kh != null)
            {
                    @Session["TK"] = kh.HOTEN;
                    @Session["Taikhoan"] = kh;
                    @Session["DX"] = "Đăng xuất";
                    @Session["DN"] = "";
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Thongbao = "Tên tài khoản hoặc mật khẩu không đúng";
            }
            return View("DangNhap");
        }
        public ActionResult TimKiem(FormCollection col)
        {
            var tim = col["tim"];
            if (tim == "" || tim == null)
            {
                return RedirectToAction("Index","Home");
            }
            var bg = db.HOAs.Where(n => n.TENHOA.Contains(tim) == true).ToList();
            return View(bg);
        }
        
    }
}
