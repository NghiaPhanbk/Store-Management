using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlyquancafe.DTO
{
    public class Food
    {
        private int id;
        public int Id 
        { 
            get { return id; }
            set { id = value; } 
        }

        private string foodName;
        public string FoodName
        {
            get { return foodName; }
            set { foodName = value; }
        }

        private int idCategory;
        public int IdCategory
        {
            get { return idCategory; }
            set { idCategory = value; }
        }

        private float price;
        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        public Food(int id, string foodName, float price, int idCategory)
        {
            this.Id = id;
            this.FoodName = foodName;
            this.Price = price;
            this.IdCategory = idCategory;
        }

        public Food(DataRow row)
        {
            this.Id = (int)row["id"];
            this.FoodName = row["FoodName"].ToString();
            this.IdCategory = (int)row["idCategory"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
        }
    }
}
