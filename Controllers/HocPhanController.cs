using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test2.Models;

namespace test2.Controllers
{
    public class HocPhanController : Controller
    {
        private readonly Test1Context _context;

        public HocPhanController(Test1Context context)
        {
            _context = context;
        }

        // Hiển thị danh sách học phần
        public async Task<IActionResult> Index()
        {
            var hocPhans = await _context.HocPhans.ToListAsync();
            return View(hocPhans);
        }

        // Trang đăng ký học phần
        public async Task<IActionResult> Register(string maHp)
        {
            if (string.IsNullOrEmpty(maHp))
            {
                return NotFound();
            }

            var hocPhan = await _context.HocPhans
                                         .FirstOrDefaultAsync(hp => hp.MaHp == maHp);
            if (hocPhan == null)
            {
                return NotFound();
            }

            return View(hocPhan);
        }

        // Xử lý đăng ký học phần
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string maHp, string studentId)
        {
            if (string.IsNullOrEmpty(maHp) || string.IsNullOrEmpty(studentId))
            {
                return NotFound();
            }

            var hocPhan = await _context.HocPhans
                                         .FirstOrDefaultAsync(hp => hp.MaHp == maHp);
            if (hocPhan == null)
            {
                return NotFound();
            }

            // Kiểm tra xem sinh viên đã đăng ký học phần này chưa
            var existingRegistration = await _context.DangKies
                                                     .FirstOrDefaultAsync(dk => dk.MaSv == studentId && dk.MaHps.Any(hp => hp.MaHp == maHp));

            if (existingRegistration != null)
            {
                ViewBag.Message = "Bạn đã đăng ký học phần này rồi!";
                return View(hocPhan);
            }

            // Tạo mới DangKy và liên kết với học phần và sinh viên
            var newRegistration = new DangKy
            {
                MaSv = studentId,
                NgayDk = DateOnly.FromDateTime(DateTime.Now),
                MaHps = new List<HocPhan> { hocPhan }
            };

            _context.DangKies.Add(newRegistration);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Đăng ký học phần thành công!";
            return View(hocPhan);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ViewRegisteredCourses(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                TempData["Message"] = "MSSV không hợp lệ!";
                return RedirectToAction("Index");
            }

            // Lấy danh sách các học phần đã đăng ký của sinh viên
            var studentRegistration = _context.DangKies
                .Include(dk => dk.MaHps)  // Include để tải danh sách học phần liên quan
                .FirstOrDefault(dk => dk.MaSv == studentId);

            if (studentRegistration == null)
            {
                TempData["Message"] = "Sinh viên chưa đăng ký học phần nào!";
                return RedirectToAction("Index");
            }

            var registeredCourses = studentRegistration.MaHps.ToList();

            // Kiểm tra nếu danh sách học phần rỗng
            if (!registeredCourses.Any())
            {
                TempData["Message"] = "Sinh viên chưa đăng ký học phần nào!";
                return RedirectToAction("Index");
            }
            ViewData["StudentId"] = studentId;

            return View("MyCourse", registeredCourses);
        }


        // Hiển thị danh sách học phần đã đăng ký
        public IActionResult MyCourse(List<HocPhan> registeredCourses)
        {
            return View(registeredCourses);
        }

        // Hủy đăng ký học phần
        // Xóa học phần đã đăng ký
        [HttpPost]
        public IActionResult DeleteCourse(string studentId, string maHp)
        {
            var studentRegistration = _context.DangKies
                .Include(dk => dk.MaHps) // Include hoc phan (join them)
                .FirstOrDefault(dk => dk.MaSv == studentId);

            if (studentRegistration != null)
            {
                var hocPhan = studentRegistration.MaHps
                    .FirstOrDefault(hp => hp.MaHp == maHp);

                if (hocPhan != null)
                {
                    studentRegistration.MaHps.Remove(hocPhan); // Xóa học phần khỏi danh sách đã đăng ký
                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                }
            }

            return RedirectToAction("MyCourse", new { studentId = studentId }); // Quay lại danh sách học phần đã đăng ký
        }
    }
}
