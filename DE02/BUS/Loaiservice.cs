using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class Loaiservice
    {
        public List<LoaiSP> GetAll()
        {
            Model1 context = new Model1();
            return context.LoaiSPs.ToList();
        }

        public LoaiSP FindByMaId(string loaiSpId)
        {
            Model1 context = new Model1();
            return context.LoaiSPs.FirstOrDefault(p => p.MaLoai == loaiSpId);
        }
        public void InsertUpdate(LoaiSP l)
        {
            Model1 context = new Model1();
            context.LoaiSPs.AddOrUpdate(l);
            context.SaveChanges();
        }
    }
}
