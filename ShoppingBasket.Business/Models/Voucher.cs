using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Business.Models
{
    public class Voucher
    {
        public string VoucherId { get; set; }
        public int VoucherType { get; set; } //1-gift, 2 -offer, 0-novoucher
        public decimal Amount { get; set; }
        public string ItemCategory { get; set; } 
    }
}
