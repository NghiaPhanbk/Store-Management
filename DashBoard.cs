using Quanlyquancafe.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quanlyquancafe.DTO;
using System.Globalization;
using System.Threading;

namespace Quanlyquancafe
{
    public partial class DashBoard : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; changeAccount(loginAccount.Type); }
        }
        public DashBoard(Account loginAccount)
        {
            InitializeComponent();
            this.LoginAccount = loginAccount;            
            LoadTable();
            LoadCategory();
            LoadcbxTable(cbSwitchTable);

        }
        #region method

        void changeAccount(int type)
        {
            if (adminToolStripMenuItem != null)
            {
                if (type == 1)
                {
                    adminToolStripMenuItem.Enabled = true;
                }
                else
                    adminToolStripMenuItem.Enabled = false;
            }
            else
            {
                MessageBox.Show("Fail");
            }
            tttkToolStripMenuItem.Text += "(" + loginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "CategoryName";
        }

        void LoadFoodFromCategryID(int id) 
        {
            List<Food> listFood = FoodDAO.Instance.GetFoofFromCategory(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "FoodName";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();

            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList) 
            {
                Button btn = new Button()
                { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += Btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aquamarine; break;
                    default: 
                        btn.BackColor = Color.AliceBlue; break;
                }
                flpTable.Controls.Add(btn);
            }
        }

        void showbill(int id)
        {
            lsvBill.Items.Clear();
            List<Quanlyquancafe.DTO.Menu> listBillInfor = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (Quanlyquancafe.DTO.Menu item in listBillInfor)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);

            }
            CultureInfo culture = new CultureInfo("vi-VN");
            //Thread.CurrentThread.CurrentCulture = culture;
            txbTotalPrice.Text = totalPrice.ToString("c",culture);            
        }

        void LoadcbxTable(ComboBox cbx)
        {
            cbx.DataSource = TableDAO.Instance.LoadTableList();
            cbx.DisplayMember = "Name";
        }
        #endregion


        #region event
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            showbill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }
        void f_UpdateAccount(object sender, AccountEvent e)
        {
            tttkToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = loginAccount;
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.ShowDialog();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodFromCategryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showbill((lsvBill.Tag as Table).ID);
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodFromCategryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showbill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodFromCategryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                showbill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null) return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;   
            LoadFoodFromCategryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }    
            int idBill = BillDAO.Instance.GetBillIDByTableID(table.ID);
            int foodID = 0;
            foodID = (cbFood.SelectedItem as Food).Id;
            int count = 0;
            count = (int)nmFoodCount.Value;
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInforDAO.Instance.InsertBillInfor(BillDAO.Instance.GetMaxIdBill(), foodID, count);
            }
            else
            {
                BillInforDAO.Instance.InsertBillInfor(idBill, foodID, count);
            }
            showbill(table.ID);
            LoadTable();
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int discount  = (int)nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Substring(0,txbTotalPrice.Text.Length-5).Replace(".",""));
            double finalPrice =  totalPrice - (totalPrice/100 *discount);
            int idBill = BillDAO.Instance.GetBillIDByTableID((int)table.ID);
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho {0}\nTổng tiền sau giảm {2}%: {1}", table.Name, finalPrice, discount), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, finalPrice);
                    showbill(table.ID);
                }
            }
            LoadTable();
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            
            int id1 = (lsvBill.Tag as Table).ID;    
            int id2 = (cbSwitchTable.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Ban có thật sự muốn chuyển bàn {0} sang bàn {1} không?", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }
        }
        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheck_Click(this, new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());
        }

        #endregion


    }
}
