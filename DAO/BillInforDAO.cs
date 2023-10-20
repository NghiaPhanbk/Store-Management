using Quanlyquancafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlyquancafe.DAO
{
    public class BillInforDAO
    {
        private static BillInforDAO instance;
        public static BillInforDAO Instance
        {
            get { if (instance == null) instance = new BillInforDAO(); return BillInforDAO.instance; }
            set { instance = value; }
        }
        private BillInforDAO() { }

        public List<BillInfor> GetListBillInfor(int id)
        {
            List<BillInfor> listBillInfor = new List<BillInfor>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BillInfor WHERE idBill = " + id);
            foreach (DataRow row in data.Rows)
            {
                BillInfor infor = new BillInfor(row);
                listBillInfor.Add(infor);
            }
            return listBillInfor;
        }

        public void DeleteBillInfofromFoodID(int id)
        {
            DataProvider.Instance.ExecuteQuery("DELETE dbo.BillInfor WHERE idFood = " + id);
        }

        public void InsertBillInfor(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InserBillInfor @idBill , @idFood , @count", new object[] {idBill, idFood, count});
        }
    }
}
