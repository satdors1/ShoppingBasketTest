using System.Collections.Generic;
using ShoppingBasket.Business.Models;
namespace ShoppingBasket.Business.Interfaces
{
    public interface IStrategy
    {
        FinalBillObject GetFinalBill(List<ShoppingItem> Items, List<Voucher> Vouchers);
       
    }
}
