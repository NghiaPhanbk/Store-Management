using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanlyquancafe.DTO
{
    public class Bill
    {
        private int id;
        public int ID { get { return id; } set { id = value; } }

        private DateTime? dateCheckIn; // J thể hiện có thể null được.
        public DateTime? DateCheckIn
        {
            get { return dateCheckIn; }
            set { dateCheckIn = value; }
        }
        private DateTime? dateCheckOut;
        public DateTime? DateCheckOut
        { get { return dateCheckOut; }
        set { dateCheckOut = value; }
        }

        private int status;
        public int Status
        { get { return status; } set { status = value; } }

        private int discount;
        public int Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public Bill(int id, DateTime? dataCheckIn, DateTime? dataCheckOut, int status, int discount = 0)
        {
            this.ID = id;
            this.DateCheckIn = dataCheckIn;
            this.DateCheckOut = dataCheckOut;
            this.Status = status;
            this.Discount = discount;
        }
        public Bill(DataRow row)
        {
            this.ID = (int)row["id"];
            var dataCheckOutTemp = row["DateCheckOUT"];
            if (dataCheckOutTemp.ToString() != "")
            { this.DateCheckOut = (DateTime?)dataCheckOutTemp; }
            this.DateCheckIn = (DateTime?)row["DateCheckIn"];
            this.Status = (int)row["status"];
            if (row["discount"].ToString() != "")
            {
                this.Discount = (int)row["discount"];
            }
        }
    }
}
