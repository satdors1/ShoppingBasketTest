using ShoppingBasket.Business.Interfaces;
using System;
using System.Collections.Generic;
using ShoppingBasket.Business.Models;
using System.Linq;

namespace ShoppingBasket.Business
{
    public class NoVoucherDiscount : IStrategy
    {
        FinalBillObject IStrategy.GetFinalBill(List<ShoppingItem> Items, List<Voucher> Voucherss)
        {
            try
            {
                var ItemInOffer = Items.Sum(p => p.Price * p.Qty);
                return new FinalBillObject() { ErrorMessage = "", FinalBillAmount = ItemInOffer, IsError = false };
            }
            catch
            {
                throw;
            }
        }
    }
}
