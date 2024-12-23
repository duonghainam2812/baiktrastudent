using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test2.Models;

namespace test2.Controllers
{
    public class StudentController : Controller
    {
        private readonly Test1Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // Để lấy đường dẫn wwwroot

        public StudentController(Test1Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: Student
        public async Task<IActionResult> Index()
        {
            var students = await _context.SinhViens
                                .Include(s => s.MaNganhNavigation)
                                .ToListAsync();
            return View(students);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var student = await _context.SinhViens
                                .Include(s => s.MaNganhNavigation)
                                .FirstOrDefaultAsync(m => m.MaSv == id);

            if (student == null) return NotFound();

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            // Load danh sách mã ngành
            ViewBag.MaNganhList = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh");
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSv,HoTen,GioiTinh,NgaySinh,MaNganh")] SinhVien sinhVien, IFormFile HinhFile)
        {
            if (ModelState.IsValid)
            {
                // Xử lý file upload
                if (HinhFile != null)
                {
                    // Tạo tên file duy nhất và ngắn gọn
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    // Lấy phần mở rộng của file (.jpg, .png, ...)
                    string fileExtension = Path.GetExtension(HinhFile.FileName);

                    // Giới hạn tên file gốc và tạo tên mới
                    string originalFileName = Path.GetFileNameWithoutExtension(HinhFile.FileName);
                    string shortFileName = originalFileName.Length > 10 ? originalFileName.Substring(0, 10) : originalFileName;
                    string uniqueFileName = Guid.NewGuid().ToString().Substring(0, 8) + "_" + shortFileName + fileExtension;

                    // Đường dẫn lưu file
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Lưu file vào wwwroot/uploads
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhFile.CopyToAsync(fileStream);
                    }

                    // Lưu đường dẫn file vào database
                    sinhVien.Hinh = "/uploads/" + uniqueFileName;
                }

                // Thêm sinh viên vào database
                _context.Add(sinhVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            
            return View(sinhVien);
          
        
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            ViewBag.MaNganhList = new SelectList(_context.NganhHocs, "MaNganh", "TenNganh");
            var student = await _context.SinhViens.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaSv,HoTen,GioiTinh,NgaySinh,Hinh,MaNganh")] SinhVien sinhVien, IFormFile HinhFile)
        {
            if (id != sinhVien.MaSv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý file upload (nếu có)
                    if (HinhFile != null)
                    {
                        // Kiểm tra kích thước file (ví dụ: tối đa 5MB)
                        const long maxFileSize = 5 * 1024 * 1024; // 5MB
                        if (HinhFile.Length > maxFileSize)
                        {
                            ModelState.AddModelError("HinhFile", "File quá lớn. Kích thước tối đa là 5MB.");
                            return View(sinhVien);  // Quay lại trang với thông báo lỗi
                        }

                        // Xóa ảnh cũ (nếu có)
                        if (!string.IsNullOrEmpty(sinhVien.Hinh))
                        {
                            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, sinhVien.Hinh.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Tạo tên file duy nhất và ngắn gọn
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                        // Lấy phần mở rộng của file (.jpg, .png, ...)
                        string fileExtension = Path.GetExtension(HinhFile.FileName);

                        // Giới hạn tên file gốc và tạo tên mới
                        string originalFileName = Path.GetFileNameWithoutExtension(HinhFile.FileName);
                        string shortFileName = originalFileName.Length > 10 ? originalFileName.Substring(0, 10) : originalFileName;
                        string uniqueFileName = Guid.NewGuid().ToString().Substring(0, 8) + "_" + shortFileName + fileExtension;

                        // Đường dẫn lưu file
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Tạo thư mục nếu chưa có
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Lưu file vào wwwroot/uploads
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhFile.CopyToAsync(fileStream);
                        }

                        // Lưu đường dẫn file vào database
                        sinhVien.Hinh = "/uploads/" + uniqueFileName;
                    }

                    _context.Update(sinhVien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SinhVienExists(sinhVien.MaSv))
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

            
            return View(sinhVien);
        }


        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var student = await _context.SinhViens
                                .FirstOrDefaultAsync(m => m.MaSv == id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sinhVien = await _context.SinhViens.FindAsync(id);
            if (sinhVien != null)
            {
                _context.SinhViens.Remove(sinhVien); // Xóa trực tiếp khỏi DB
                await _context.SaveChangesAsync();  // Lưu thay đổi vào DB
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SinhVienExists(string id)
        {
            return _context.SinhViens.Any(e => e.MaSv == id);
        }
    }
}
