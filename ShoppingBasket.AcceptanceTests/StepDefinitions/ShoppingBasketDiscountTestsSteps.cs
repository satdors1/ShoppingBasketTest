using NUnit.Framework;
using ShoppingBasket.Business;
using ShoppingBasket.Business.Enums;
using ShoppingBasket.Business.Models;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace ShoppingBasket.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ShoppingBasketDiscountTestsSteps : IDisposable
    {
        private List<ShoppingItem> ShoppingList;


        private FinalBillObject Result;
        private Business.ShoppingBasket shopObject;
        List<Voucher> Vouchers;
        public ShoppingBasketDiscountTestsSteps()
        {
            Vouchers = new List<Voucher>();
        }
        [Given(@"I have (.*) Hat @ £(.*), (.*) Jumper @ £(.*) in the basket")]
        public void GivenIHaveHatJumperInTheBasket(int p0, decimal p1, int p2, decimal p3)
        {
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Hat", Price=p1,Qty=p0,Category="HATS",DiscountQualified=true},
                new ShoppingItem(){Name="Jumper", Price=p3,Qty=p2,Category="JUMPERS",DiscountQualified=true}
            };
        }
        [Given(@"I have (.*) Hat @ £(.*), (.*) Gift Voucher @ £(.*) in the basket")]
        public void GivenIHaveHatGiftVoucherInTheBasket(int p0, decimal p1, int p2, decimal p3)
        {
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Hat", Price=p1,Qty=p0,Category="HATS",DiscountQualified=true},
                new ShoppingItem(){Name="Gift Voucher", Price=p3,Qty=p2,Category="GIFT",DiscountQualified=false}
            };
        }

        [Given(@"I have used £(.*) '(.*)' voucher '(.*)' of '(.*)' category")]
        public void GivenIHaveUsedVoucherOfCategory(int p0, string p1, string p2, string p3)
        {
            int VType = 0;
            switch (p1.ToLower())
            {
                case "gift":
                    VType = (int)VoucherType.Gift;
                    break;
                case "offer":
                    VType = (int)VoucherType.Offer;
                    break;
                default:
                    VType = (int)VoucherType.None;
                    break;
            }
            Vouchers.Add(new Voucher() { Amount = p0, VoucherType = VType, ItemCategory = p3, VoucherId = p2 });

        }


        [When(@"I call GetFinalBill function")]
        public void WhenICallGetFinalBillFunction()
        {

            decimal DiscountAmount = 0;
            foreach (var voucher in Vouchers)
            {
                switch (voucher.VoucherType)
                {
                    case 1:
                        shopObject = new Business.ShoppingBasket(new GiftVoucherDiscount());
                        break;
                    case 2:
                        shopObject = new Business.ShoppingBasket(new OfferVoucherDiscount());
                        break;
                    default:
                        shopObject = new Business.ShoppingBasket(new NoVoucherDiscount());
                        break;
                }
                shopObject.ShoppingList = ShoppingList;
                shopObject.Vouchers = Vouchers;
                Result = shopObject.GetFinalBill();
                DiscountAmount += Result.AppliedDiscount;
            }

            Result.AppliedDiscount = DiscountAmount;


        }

        [Given(@"I have (.*) Jumper @ £(.*), (.*) Head Light @ £(.*) of '(.*)' category in the basket")]
        public void GivenIHaveJumperHeadLightOfCategoryInTheBasket(int p0, decimal p1, int p2, decimal p3, string p4)
        {
            ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Jumper", Price=p1,Qty=p0,Category="JUMPER",DiscountQualified=true},
                new ShoppingItem(){Name="Head Light", Price=p3,Qty=p2,Category="HEAD GEAR",DiscountQualified=true}
            };
        }

        [Given(@"I have applied no voucher")]
        public void GivenIHaveAppliedNoVoucher()
        {
            shopObject = new Business.ShoppingBasket(new NoVoucherDiscount())
            {
                ShoppingList = ShoppingList,
                Vouchers = Vouchers
            };
            Result = shopObject.GetFinalBill();

        }


        [Then(@"the Total Bill should be shown as £(.*)")]
        public void ThenTheTotalBillShouldBeShownAs(decimal p0)
        {
            Assert.IsFalse(Result.IsError);
            Assert.IsTrue((Result.FinalBillAmount - Result.AppliedDiscount) == p0);

        }

        [Then(@"The message should be shown as '(.*)'")]
        public void ThenTheMessageShouldBeShownAs(string p0)
        {
            var Msg = "There are no products in your basket applicable to voucher Voucher YYY-YYY";

            Assert.That(p0, Is.EqualTo(Msg).IgnoreCase);
        }

        [Given(@"(.*) Head Light @ £(.*) of '(.*)' Product Category also in the basket")]
        public void GivenHeadLightOfProductCategoryAlsoInTheBasket(int p0, decimal p1, string p2)
        {
            ShoppingList.Add(
                new ShoppingItem() { Name = "Head Light", Price = p1, Qty = p0, Category = p2, DiscountQualified = true }
                );

        }

        public void Dispose()
        {
            shopObject = null;
            Vouchers = null;
            ShoppingList = null;
        }
    }
}
