using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlyquancafe.DTO
{
    public class Category
    {
        public Category(int id, string categoryName) 
        {
            this.CategoryName = categoryName;
            this.ID = id;
        }
        public Category(DataRow row)
        {
            this.ID = (int)row["id"];
            this.CategoryName = row["FoodCategoryName"].ToString();
        }
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string categoryName;
        public string CategoryName
        { get { return categoryName; } set {  categoryName = value; } }


        
    }
}
