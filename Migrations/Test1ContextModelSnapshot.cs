﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using test2.Models;

#nullable disable

namespace test2.Migrations
{
    [DbContext(typeof(Test1Context))]
    partial class Test1ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChiTietDangKy", b =>
                {
                    b.Property<int>("MaDk")
                        .HasColumnType("int")
                        .HasColumnName("MaDK");

                    b.Property<string>("MaHp")
                        .HasMaxLength(6)
                        .IsUnicode(false)
                        .HasColumnType("char(6)")
                        .HasColumnName("MaHP")
                        .IsFixedLength();

                    b.HasKey("MaDk", "MaHp")
                        .HasName("PK__ChiTietD__F557DC029BC25332");

                    b.HasIndex("MaHp");

                    b.ToTable("ChiTietDangKy", (string)null);
                });

            modelBuilder.Entity("test2.Models.DangKy", b =>
                {
                    b.Property<int>("MaDk")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MaDK");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaDk"));

                    b.Property<string>("MaSv")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .HasColumnName("MaSV")
                        .IsFixedLength();

                    b.Property<DateOnly?>("NgayDk")
                        .HasColumnType("date")
                        .HasColumnName("NgayDK");

                    b.HasKey("MaDk")
                        .HasName("PK__DangKy__2725866CCDF4043C");

                    b.HasIndex("MaSv");

                    b.ToTable("DangKy", (string)null);
                });

            modelBuilder.Entity("test2.Models.HocPhan", b =>
                {
                    b.Property<string>("MaHp")
                        .HasMaxLength(6)
                        .IsUnicode(false)
                        .HasColumnType("char(6)")
                        .HasColumnName("MaHP")
                        .IsFixedLength();

                    b.Property<int?>("SoTinChi")
                        .HasColumnType("int");

                    b.Property<string>("TenHp")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("TenHP");

                    b.HasKey("MaHp")
                        .HasName("PK__HocPhan__2725A6EC01C769F4");

                    b.ToTable("HocPhan", (string)null);
                });

            modelBuilder.Entity("test2.Models.NganhHoc", b =>
                {
                    b.Property<string>("MaNganh")
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("char(4)")
                        .IsFixedLength();

                    b.Property<string>("TenNganh")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("MaNganh")
                        .HasName("PK__NganhHoc__A2CEF50D2D5BD7A8");

                    b.ToTable("NganhHoc", (string)null);
                });

            modelBuilder.Entity("test2.Models.SinhVien", b =>
                {
                    b.Property<string>("MaSv")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("char(10)")
                        .HasColumnName("MaSV")
                        .IsFixedLength();

                    b.Property<string>("GioiTinh")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Hinh")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MaNganh")
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("char(4)")
                        .IsFixedLength();

                    b.Property<DateOnly?>("NgaySinh")
                        .HasColumnType("date");

                    b.HasKey("MaSv")
                        .HasName("PK__SinhVien__2725081AA18E85A9");

                    b.HasIndex("MaNganh");

                    b.ToTable("SinhVien", (string)null);
                });

            modelBuilder.Entity("ChiTietDangKy", b =>
                {
                    b.HasOne("test2.Models.DangKy", null)
                        .WithMany()
                        .HasForeignKey("MaDk")
                        .IsRequired()
                        .HasConstraintName("FK__ChiTietDan__MaDK__412EB0B6");

                    b.HasOne("test2.Models.HocPhan", null)
                        .WithMany()
                        .HasForeignKey("MaHp")
                        .IsRequired()
                        .HasConstraintName("FK__ChiTietDan__MaHP__4222D4EF");
                });

            modelBuilder.Entity("test2.Models.DangKy", b =>
                {
                    b.HasOne("test2.Models.SinhVien", "MaSvNavigation")
                        .WithMany("DangKies")
                        .HasForeignKey("MaSv")
                        .HasConstraintName("FK__DangKy__MaSV__3E52440B");

                    b.Navigation("MaSvNavigation");
                });

            modelBuilder.Entity("test2.Models.SinhVien", b =>
                {
                    b.HasOne("test2.Models.NganhHoc", "MaNganhNavigation")
                        .WithMany("SinhViens")
                        .HasForeignKey("MaNganh")
                        .HasConstraintName("FK__SinhVien__MaNgan__398D8EEE");

                    b.Navigation("MaNganhNavigation");
                });

            modelBuilder.Entity("test2.Models.NganhHoc", b =>
                {
                    b.Navigation("SinhViens");
                });

            modelBuilder.Entity("test2.Models.SinhVien", b =>
                {
                    b.Navigation("DangKies");
                });
#pragma warning restore 612, 618
        }
    }
}
