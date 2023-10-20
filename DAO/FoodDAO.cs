using Quanlyquancafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlyquancafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        public static FoodDAO Instance
        {
            get { if (instance == null) { instance = new FoodDAO(); } return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }
        private FoodDAO() { }

        public List<Food> GetFoofFromCategory(int id)
        {
            List<Food> list = new List<Food>();
            string query = "SELECT * FROM food WHERE idCategory = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows) 
            {
                Food food = new Food(row);
                list.Add(food);
            }

            return list;
        }
        public List<Food> GetListFood()
        {
        List<Food> list = new List<Food>();
        string query = "SELECT * FROM food";
        DataTable data = DataProvider.Instance.ExecuteQuery(query);

        foreach (DataRow row in data.Rows) 
        {
            Food food = new Food(row);
            list.Add(food);
        }

        return list;
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();
            string query = string.Format("SELECT * FROM dbo.Food WHERE dbo.fuConvertToUnsign1(FoodName) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + N'%'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Food food = new Food(row);
                list.Add(food);
            }

            return list;
        }
        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("INSERT dbo.Food ( FoodName, idCategory, price ) VALUES (N'{0}', {1}, {2})", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string query = string.Format("UPDATE dbo.Food SET FoodName = N'{0}', idCategory = {1}, price = {2} WHERE id = {3}", name, id, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteFood(int idFood)
        {
            BillInforDAO.Instance.DeleteBillInfofromFoodID(idFood);
            string query = string.Format("DELETE dbo.Food WHERE id = {0}", idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

        
    }
}
