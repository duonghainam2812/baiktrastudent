using System;
using System.Collections.Generic;

namespace test2.Models;

public partial class SinhVien
{
    public string MaSv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? Hinh { get; set; }

    public string? MaNganh { get; set; }

    public virtual ICollection<DangKy> DangKies { get; set; } = new List<DangKy>();

    public virtual NganhHoc? MaNganhNavigation { get; set; }
}
