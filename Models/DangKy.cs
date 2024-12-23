using System;
using System.Collections.Generic;

namespace test2.Models;

public partial class DangKy
{
    public int MaDk { get; set; }

    public DateOnly? NgayDk { get; set; }

    public string? MaSv { get; set; }

    public virtual SinhVien? MaSvNavigation { get; set; }

    public virtual ICollection<HocPhan> MaHps { get; set; } = new List<HocPhan>();
}
