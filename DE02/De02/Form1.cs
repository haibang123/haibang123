using BUS;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;

namespace De02
{
    public partial class frmSanpham : Form
    {
        private readonly SanPhamService sanPhamService = new SanPhamService();
        private readonly Loaiservice loaiservice = new Loaiservice();
        int CountRow;
        int OriginalRow;
        Model1 db = new Model1();
        public frmSanpham()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSanpham.SelectedItems.Count > 0)
            {
                ListViewItem item = lvSanpham.SelectedItems[0];

                string maSP = item.Text;
                string tenSP = item.SubItems[1].Text;
                DateTime NgayNhap;
                string loaiSP = item.SubItems[3].Text;

                txtMaSP.Text = maSP;
                txtTenSP.Text = tenSP;
                if (DateTime.TryParse(item.SubItems[2].Text.ToString(), out NgayNhap))
                {
                    dtNgayNhap.Value = NgayNhap;
                }
                cbLoaiSP.Text = loaiSP;
            }
        }

        private void frmSanpham_Load(object sender, EventArgs e)
        {
            try
            {
                var listLoais = loaiservice.GetAll();
                var listSanPhams = sanPhamService.GetAll();
                FillLoaiCombobox(listLoais);
                BindGrid(listSanPhams);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void BindGrid(List<SanPham> listsv)
        {
      
            lvSanpham.Items.Clear();
            foreach (SanPham student in listsv)
            {
                ListViewItem item = new ListViewItem();
                item.Text = student.MaSP;
                item.SubItems.Add(new ListViewSubItem(item, student.TenSP));
                item.SubItems.Add(new ListViewSubItem(item, student.Ngaynhap.ToString()));
                item.SubItems.Add(new ListViewSubItem(item, student.LoaiSP.TenLoai));
                lvSanpham.Items.Add(item);
            }
        }

        private void FillLoaiCombobox(List<LoaiSP> listclass)
        {
            this.cbLoaiSP.DataSource = listclass;
            this.cbLoaiSP.DisplayMember = "TenLoai";
            this.cbLoaiSP.ValueMember = "MaLoai";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem(txtMaSP.Text);
            item.SubItems.Add(txtTenSP.Text);
            item.SubItems.Add(dtNgayNhap.Value.ToString());
            item.SubItems.Add(cbLoaiSP.Text);
            lvSanpham.Items.Add(item);
            txtMaSP.Text = null;
            txtTenSP.Text = null;
            dtNgayNhap.Value = DateTime.Now;
            cbLoaiSP.SelectedIndex = 0;
            CountRow = lvSanpham.Items.Count;
            UpdateSaveandUnsaveButtonState();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            while (lvSanpham.SelectedItems.Count > 0)
            {
                Console.WriteLine(lvSanpham.SelectedItems[0].Index);
                lvSanpham.Items.RemoveAt(lvSanpham.SelectedItems[0].Index);
                txtMaSP.Text = null;
                txtMaSP.Text = null;
                dtNgayNhap.Value = DateTime.Now;
                cbLoaiSP.SelectedIndex = 0;
                CountRow = lvSanpham.Items.Count;
                UpdateSaveandUnsaveButtonState();

            }
        }

            private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
            "Do you want to exit ?",
            "Close",
            MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                return;
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lvSanpham.SelectedItems.Count > 0)
            {
                ListViewItem lv = lvSanpham.SelectedItems[0];
                lv.SubItems[0].Text = txtMaSP.Text;
                lv.SubItems[1].Text = txtTenSP.Text;
                lv.SubItems[2].Text = dtNgayNhap.Value.ToString();
                lv.SubItems[3].Text = cbLoaiSP.Text;
                txtMaSP.Text = null;
                txtTenSP.Text = null;
                dtNgayNhap.Value = DateTime.Now;
                cbLoaiSP.SelectedIndex = 0;
                UpdateSaveandUnsaveButtonState();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<SanPham> newSP = new List<SanPham>();

            foreach (ListViewItem item in lvSanpham.Items)
            {
                SanPham SP = new SanPham();
                SP.MaSP = item.Text;
                SP.TenSP = item.SubItems[1].Text;

                DateTime ngayNhap;
                if (DateTime.TryParse(item.SubItems[2].Text, out ngayNhap))
                {
                    SP.Ngaynhap = ngayNhap;
                }
                var tenLoai = item.SubItems[3].Text;

                var maLoai = db.LoaiSPs
                    .Where(loai => loai.TenLoai == tenLoai)
                    .Select(loai => loai.MaLoai)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(maLoai))
                {
                    SP.MaLoai = maLoai;
                }

                newSP.Add(SP);
            }

            var maSPsInNewSP = newSP.Select(sp => sp.MaSP).ToList();

            var existingEntities = db.SanPhams.Where(s => maSPsInNewSP.Contains(s.MaSP)).ToList();

            foreach (var existingEntity in existingEntities)
            {
                var correspondingNewSP = newSP.FirstOrDefault(sp => sp.MaSP == existingEntity.MaSP);
                if (correspondingNewSP != null)
                {
                    existingEntity.TenSP = correspondingNewSP.TenSP;
                    existingEntity.Ngaynhap = correspondingNewSP.Ngaynhap;
                }
            }

            var entitiesToDelete = db.SanPhams.Where(s => !maSPsInNewSP.Contains(s.MaSP)).ToList();
            db.SanPhams.RemoveRange(entitiesToDelete);

   
            var entitiesToAdd = newSP.Where(sp => !existingEntities.Any(ee => ee.MaSP == sp.MaSP)).ToList();
            db.SanPhams.AddRange(entitiesToAdd);

            db.SaveChanges();
        }

        private void UpdateSaveandUnsaveButtonState()
        {
            btnSave.Enabled = lvSanpham.Items.Count >= OriginalRow || lvSanpham.Items.Count < OriginalRow;
            btnNotSave.Enabled = lvSanpham.Items.Count >= OriginalRow || lvSanpham.Items.Count < OriginalRow;
        }
    }
}
