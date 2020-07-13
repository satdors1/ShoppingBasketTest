using ShoppingBasket.Business.Interfaces;
using System.Collections.Generic;
using ShoppingBasket.Business.Models;
using ShoppingBasket.Business.Enums;
using System.Linq;

namespace ShoppingBasket.Business
{
    public class GiftVoucherDiscount : IStrategy
    {
        FinalBillObject IStrategy.GetFinalBill(List<ShoppingItem> Items, List<Voucher> Vouchers)
        {
            try
            {
                var TotalBill = Items.Sum(x => (x.Qty * x.Price));
                var TotalVoucherAmount = Vouchers.Where(x => x.VoucherType == (int)VoucherType.Gift).Sum(v => v.Amount);

                return new FinalBillObject()
                {
                    ErrorMessage = "",
                    FinalBillAmount = TotalBill,
                    AppliedDiscount = TotalVoucherAmount,
                    IsError = false
                };

            }
            catch
            {
                throw;
            }
        }


    }
}
