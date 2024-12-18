using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication20.Models;

namespace WebApplication20.Controllers
{
    public class HocPhanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HocPhanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách học phần
        public async Task<IActionResult> ListHP()
        {
            var hocPhans = await _context.HocPhan.ToListAsync();
            return View(hocPhans);
        }

        // Xử lý đăng ký học phần
        [HttpPost]
        public async Task<IActionResult> DangKy(string maHP)
        {
            // Mã sinh viên giả định đang đăng nhập
            string maSV = "0123456789";

            // Kiểm tra bản ghi đăng ký đã tồn tại chưa
            var dangKy = await _context.DangKy
                .FirstOrDefaultAsync(d => d.MaSV == maSV && d.NgayDK == DateTime.Today);

            if (dangKy == null)
            {
                // Tạo mới đăng ký
                dangKy = new DangKy
                {
                    MaSV = maSV,
                    NgayDK = DateTime.Today
                };
                _context.DangKy.Add(dangKy);
                await _context.SaveChangesAsync();
            }

            // Thêm vào bảng ChiTietDangKy nếu chưa có
            bool daDangKy = await _context.ChiTietDangKy
                .AnyAsync(c => c.MaDK == dangKy.MaDK && c.MaHP == maHP);

            if (!daDangKy)
            {
                var chiTiet = new ChiTietDangKy
                {
                    MaDK = dangKy.MaDK,
                    MaHP = maHP
                };
                _context.ChiTietDangKy.Add(chiTiet);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đăng ký học phần thành công!";
            }
            else
            {
                TempData["Error"] = "Bạn đã đăng ký học phần này!";
            }

            return RedirectToAction(nameof(ListHP));
        }
    }
}
