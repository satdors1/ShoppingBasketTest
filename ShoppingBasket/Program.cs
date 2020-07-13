using ShoppingBasket.Business;
using ShoppingBasket.Business.Enums;
using ShoppingBasket.Business.Models;
using System;
using System.Collections.Generic;

namespace ShoppingBasket
{
    class Program
    {
        private static readonly GiftVoucherDiscount GiftVoucherObject = new GiftVoucherDiscount();
        private static readonly OfferVoucherDiscount OfferVoucherObject = new OfferVoucherDiscount();
        public static List<ShoppingItem> ShoppingList = new List<ShoppingItem>();
        private static List<Voucher> Vouchers = new List<Voucher>();
        static void Main(string[] args)
        {
            Basket1_Calc();
            Basket2_Calc();
            Basket3_Calc();
            Basket4_Calc();
            Basket5_Calc();
            Basket6_Calc();
        }

        private static void Basket1_Calc()
        {
            var shopObject = new Business.ShoppingBasket(GiftVoucherObject);

            Vouchers = new List<Voucher>() {
                new Voucher() { Amount = 5,VoucherType = (int)VoucherType.Gift ,ItemCategory = "REGULAR", VoucherId="XXX-XXX"}
            };
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem() { Name = "Hat", Price = 10.50m,Qty = 1,Category = "HATS",DiscountQualified = true},
                new ShoppingItem() { Name = "Jumper", Price = 54.65m,Qty = 1,Category = "JUMPERS",DiscountQualified = true}
                 };

            shopObject.Vouchers = Vouchers;
            shopObject.ShoppingList = ShoppingList;
            var Result = shopObject.GetFinalBill();
            Console.WriteLine("Basket 1");
            PrintBasketItems(ShoppingList);
            PrintVouchers(Vouchers);
            PrintResult(Result);

        }

        private static void Basket2_Calc()
        {
            var shopObject = new Business.ShoppingBasket(OfferVoucherObject);

            Vouchers = new List<Voucher>() {
                new Voucher() { Amount = 5,VoucherType = (int)VoucherType.Offer ,ItemCategory = "HEAD GEAR", VoucherId="YYY-YYY"}
            };
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem() { Name = "Hat", Price = 25.00m,Qty = 1,Category = "HATS",DiscountQualified = true},
                new ShoppingItem() { Name = "Jumper", Price = 26.00m,Qty = 1,Category = "JUMPERS",DiscountQualified = true}
                 };

            shopObject.Vouchers = Vouchers;
            shopObject.ShoppingList = ShoppingList;

            var Result = shopObject.GetFinalBill();
            Console.WriteLine("Basket 2");
            PrintBasketItems(ShoppingList);
            PrintVouchers(Vouchers);
            PrintResult(Result);

        }

        private static void Basket3_Calc()
        {
            var shopObject = new Business.ShoppingBasket(OfferVoucherObject);

            Vouchers = new List<Voucher>() {
                new Voucher() { Amount = 5,VoucherType = (int)VoucherType.Offer ,ItemCategory = "HEAD GEAR", VoucherId="YYY-YYY"}
            };
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem() { Name = "Hat", Price = 25.00m,Qty = 1,Category = "HATS",DiscountQualified = true},
                new ShoppingItem() { Name = "Jumper", Price = 26.00m,Qty = 1,Category = "JUMPERS",DiscountQualified = true},
                 new ShoppingItem() { Name = "Head Light", Price = 5.00m,Qty = 1,Category = "HEAD GEAR",DiscountQualified = true}
                 };

            shopObject.Vouchers = Vouchers;
            shopObject.ShoppingList = ShoppingList;

            var Result = shopObject.GetFinalBill();
            Console.WriteLine("Basket 3");
            PrintBasketItems(ShoppingList);
            PrintVouchers(Vouchers);
            PrintResult(Result);

        }

        private static void Basket4_Calc()
        {
            var shopObject = new Business.ShoppingBasket(GiftVoucherObject);
            decimal TotalDiscount = 0;
            Vouchers = new List<Voucher>() {
                new Voucher() { Amount = 5,VoucherType = (int)VoucherType.Gift,ItemCategory = "GIFT", VoucherId="XXX-XXX"},
                new Voucher() { Amount = 5,VoucherType = (int)VoucherType.Offer ,ItemCategory = "REGULAR", VoucherId="YYY-YYY"}
            };
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem() { Name = "Hat", Price = 25.00m,Qty = 1,Category = "HATS",DiscountQualified = true},
                new ShoppingItem() { Name = "Jumper", Price = 26.00m,Qty = 1,Category = "JUMPERS",DiscountQualified = true}

                 };

            shopObject.Vouchers = Vouchers;
            shopObject.ShoppingList = ShoppingList;

            var Result = shopObject.GetFinalBill();
            TotalDiscount += Result.AppliedDiscount;

            //Offer Voucher
            shopObject = new Business.ShoppingBasket(OfferVoucherObject);
            shopObject.Vouchers = Vouchers;
            shopObject.ShoppingList = ShoppingList;
            Result = shopObject.GetFinalBill();
            TotalDiscount += Result.AppliedDiscount;
            Result.AppliedDiscount = TotalDiscount;

            Console.WriteLine("Basket 4");
            PrintBasketItems(ShoppingList);
            PrintVouchers(Vouchers);
            PrintResult(Result);

        }

        private static void Basket5_Calc()
        {
            var shopObject = new Business.ShoppingBasket(OfferVoucherObject);

            Vouchers = new List<Voucher>() {
                new Voucher() { Amount = 5,VoucherType = (int)VoucherType.Offer ,ItemCategory = "HEAD GEAR", VoucherId="YYY-YYY"}
            };
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem() { Name = "Hat", Price = 25.00m,Qty = 1,Category = "HATS",DiscountQualified = true},
                new ShoppingItem() { Name = "Gift Voucher", Price = 30.00m,Qty = 1,Category = "GIFT",DiscountQualified = false},

                 };

            shopObject.Vouchers = Vouchers;
            shopObject.ShoppingList = ShoppingList;

            var Result = shopObject.GetFinalBill();
            Console.WriteLine("Basket 5");
            PrintBasketItems(ShoppingList);
            PrintVouchers(Vouchers);
            PrintResult(Result);

        }

        private static void Basket6_Calc()
        {
            var shopObject = new Business.ShoppingBasket(OfferVoucherObject);

            Vouchers = new List<Voucher>() {
                new Voucher() { Amount = 5,VoucherType = (int)VoucherType.Offer ,ItemCategory = "HEAD GEAR", VoucherId="YYY-YYY"}
            };
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem() { Name = "Hat", Price = 54.65m,Qty = 1,Category = "HATS",DiscountQualified = true},
                 new ShoppingItem() { Name = "Head Light", Price = 3.50m,Qty = 1,Category = "HEAD GEAR",DiscountQualified = true}
                 };

            shopObject.Vouchers = Vouchers;
            shopObject.ShoppingList = ShoppingList;

            var Result = shopObject.GetFinalBill();
            Console.WriteLine("Basket 6");
            PrintBasketItems(ShoppingList);
            PrintVouchers(Vouchers);
            PrintResult(Result);

        }

        private static void PrintBasketItems(List<ShoppingItem> ShoppingList)
        {
            foreach (var item in ShoppingList)
            {
                Console.WriteLine("{0}-{1} @{2} / {3}", item.Qty, item.Name, item.Price, item.Category);
            }
        }
        private static void PrintVouchers(List<Voucher> Vouchers)
        {
            foreach (var voucher in Vouchers)
            {
                Console.WriteLine("{0} / {1} / @£{2}", PrintVoucherType(voucher.VoucherType), voucher.VoucherId, voucher.Amount, voucher.ItemCategory);
            }
        }
        private static string PrintVoucherType(int id)
        {
            switch (id)
            {
                case 1:
                    return "Gift Voucher";

                case 2:
                    return "Offer Voucher";

                default:
                    return "No Voucher";
            }
        }
        private static void PrintResult(FinalBillObject Result)
        {
            Console.WriteLine("Total: {0}", Result.FinalBillAmount - Result.AppliedDiscount);
            Console.WriteLine(Result.ErrorMessage);
        }
    }
}
