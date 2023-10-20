using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quanlyquancafe.DTO;
namespace Quanlyquancafe.DAO
{
    public class TableDAO
    {
        public static TableDAO instance;
        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set {TableDAO.instance = value;}
        }

        public static int TableWidth = 80;
        public static int TableHeight = 80;  
        private TableDAO() { }
        
        public List<Table> LoadTableList()
        {
            List<Table> TableList = new List<Table>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                TableList.Add(table);
            }
            return TableList;                
        }

        public void SwitchTable(int id1, int id2)
        {
            string query = "USP_SwitchTable @idTable1 , @idTable2";
            DataProvider.Instance.ExecuteQuery(query, new object[] { id1, id2 });
        }
    }
}
