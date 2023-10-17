using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SanPhamService
    {
        public List<SanPham> GetAll()
        {
            Model1 context = new Model1();
            return context.SanPhams.ToList();
        }
        public SanPham FindByMaId(string sanphamId)
        {
            Model1 context = new Model1();
            return context.SanPhams.FirstOrDefault(p => p.MaSP == sanphamId);
        }

        public void InsertUpdate(List<SanPham> list)
        {
            Model1 context = new Model1();
            foreach (var item in list)
            {
                var ExistingStudent = context.SanPhams.Find(item.MaSP);
                if (ExistingStudent != null)
                {
                    ExistingStudent.TenSP = item.TenSP;
                    ExistingStudent.Ngaynhap = item.Ngaynhap;
                    ExistingStudent.MaLoai = item.MaLoai;
                    context.SanPhams.AddOrUpdate(ExistingStudent);
                }
                else
                {
                    context.SanPhams.Add(item);
                }
            }
            context.SaveChanges();
        }
    }
}
