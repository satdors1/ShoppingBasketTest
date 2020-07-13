using ShoppingBasket.Business.Enums;
using ShoppingBasket.Business.Interfaces;
using ShoppingBasket.Business.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingBasket.Business
{
    public class OfferVoucherDiscount : IStrategy
    {
        FinalBillObject IStrategy.GetFinalBill(List<ShoppingItem> Items, List<Voucher> Vouchers)
        {

            decimal TotalBill = Items.Sum(x => (x.Qty * x.Price));
            decimal DiscountAmount = 0;
            string Message = "";
            decimal ThresholdValue = 50.01m;
            decimal QualifiedInGeneral = 0, QualifiedByCategory = 0, UnQualifiedInGeneral = 0;

            //select one voucher only
            var NoofOfferVouchers = Vouchers.Where(x => x.VoucherType == (int)VoucherType.Offer).Count();
            if (NoofOfferVouchers > 1)
            {
                return new FinalBillObject()
                {
                    ErrorMessage = "No of Offer vouchers more than 1",
                    FinalBillAmount = 0,
                    AppliedDiscount = 0,
                    IsError = true
                };
            }

            var voucher = Vouchers.Where(x => x.VoucherType == (int)VoucherType.Offer).FirstOrDefault();
            decimal ItemInOffer = 0;

            try
            {
                //If Regular voucher
                ItemInOffer = Items.Where(x => x.DiscountQualified == true).Sum(p => p.Price * p.Qty);
                switch (voucher.ItemCategory.ToUpper())
                {
                    case "REGULAR":
                        {
                            QualifiedInGeneral = Items.Where(x => x.DiscountQualified == true).Sum(p => p.Price * p.Qty);

                            UnQualifiedInGeneral = Items.Where(x => x.DiscountQualified == false).Sum(p => p.Price * p.Qty);
                            DiscountAmount = GetAmountToBeDeducted(voucher.Amount, ItemInOffer);

                            if (QualifiedInGeneral < ThresholdValue)
                            {
                                Message = string.Format("You have not reached the spend threshold for voucher {0}. " +
                                    " Spend another £{1} to receive £5.00 discount from your basket total", voucher.VoucherId, ThresholdValue - QualifiedInGeneral);

                                if (UnQualifiedInGeneral > 0)
                                {
                                    DiscountAmount = 0;
                                }

                            }
                        }

                        break;

                    default:
                        QualifiedInGeneral = Items.Where(x => x.DiscountQualified == true).Sum(p => p.Price * p.Qty);
                        QualifiedByCategory = Items.Where(x => x.Category.ToLower() == voucher.ItemCategory.ToLower() && x.DiscountQualified == true).Sum(p => p.Price * p.Qty);

                        //If there are no qualified items
                        if (QualifiedByCategory == 0)
                        {
                            Message = string.Format("There are no products in your basket applicable to voucher Voucher {0}", voucher.VoucherId);
                        }
                        DiscountAmount = GetAmountToBeDeducted(voucher.Amount, QualifiedByCategory);
                        if (QualifiedInGeneral < ThresholdValue)
                        {
                            Message = string.Format("You have not reached the spend threshold for voucher {0}. " +
                                " Spend another £{1} to receive £5.00 discount from your basket total", voucher.VoucherId, ThresholdValue - QualifiedInGeneral);
                        }

                        break;
                }

                return new FinalBillObject()
                {
                    ErrorMessage = Message,
                    FinalBillAmount = TotalBill,
                    AppliedDiscount = DiscountAmount,
                    IsError = false
                };
            }
            catch
            {
                throw;
            }
        }

        private decimal GetAmountToBeDeducted(decimal VoucherAmount, decimal OfferItemsTotal)
        {
            return (VoucherAmount >= OfferItemsTotal) ? OfferItemsTotal : VoucherAmount;
        }
    }
}
