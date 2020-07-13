// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using Moq;
using NUnit.Framework;
using ShoppingBasket.Business;
using ShoppingBasket.Business.Enums;
using ShoppingBasket.Business.Interfaces;
using ShoppingBasket.Business.Models;
using System.Collections.Generic;

namespace ShoppingBasket.Tests
{


    [TestFixture]
    public class GiftAndOfferVoucherTests
    {
        private Mock<IStrategy> MockGiftVoucherObject;
        private Mock<IStrategy> MockOfferVoucherObject;
        private Mock<IStrategy> MockNoVoucherObject;
        [SetUp]
        public void Setup()
        {
            MockGiftVoucherObject = new Mock<IStrategy>();
            MockOfferVoucherObject = new Mock<IStrategy>();
            MockNoVoucherObject = new Mock<IStrategy>(); 
        }

        [Test]
        public void Test_To_Ensure_No_More_Than_One_Offer_Voucher_Accepted()
        {
            var Vouchers = new List<Voucher>() {
                new Voucher(){Amount=5,VoucherType=(int)VoucherType.Offer ,ItemCategory="REGULAR",VoucherId="SSS-SSS"},
                new Voucher(){Amount=5,VoucherType=(int)VoucherType.Offer ,ItemCategory="REGULAR",VoucherId="TTT-TTT"}
            };
            var ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Hat", Price=10.50m,Qty=1,Category="HATS",DiscountQualified=true},
                new ShoppingItem(){Name="Jumper", Price=54.65m,Qty=1,Category="JUMPERS",DiscountQualified=true}
                };


            MockGiftVoucherObject.Setup(x => x.GetFinalBill(ShoppingList, Vouchers)).Returns(
                   new FinalBillObject()
                   {
                       ErrorMessage = "No of Offer vouchers more than 1",
                       FinalBillAmount = 0,
                       AppliedDiscount = 0,
                       IsError = true
                   }
                );



            var shopObject = new Business.ShoppingBasket(MockGiftVoucherObject.Object)
            {
                Vouchers = Vouchers,
                ShoppingList = ShoppingList
            };

            var Result = shopObject.GetFinalBill();

            Assert.IsTrue(Result.IsError);


        }
        [Test]
        public void Test1_GiftVoucher_Price_Deducted_From_ShoppingBasket_Total()
        {

            var Vouchers = new List<Voucher>() {
                new Voucher(){Amount=5,VoucherType=(int)VoucherType.Gift ,ItemCategory="REGULAR"}
            };

            var ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Hat", Price=10.50m,Qty=1,Category="HATS",DiscountQualified=true},
                new ShoppingItem(){Name="Jumper", Price=54.65m,Qty=1,Category="JUMPERS",DiscountQualified=true}
                };


            MockGiftVoucherObject.Setup(x => x.GetFinalBill(ShoppingList, Vouchers)).Returns(
                 new FinalBillObject()
                 {
                     AppliedDiscount = 5m,
                     ErrorMessage = "",
                     FinalBillAmount = 60.15m,
                     IsError = false

                 }
                 );

            var shopObject = new Business.ShoppingBasket(MockGiftVoucherObject.Object)
            {
                Vouchers = Vouchers,
                ShoppingList = ShoppingList
            };

            var Result = shopObject.GetFinalBill();
            Assert.IsFalse(Result.IsError);
            Assert.IsTrue(Result.FinalBillAmount == 60.15m);
        }


        [Test]
        public void Test2_OfferVoucher_Price_Not_Deducted_From_ShoppingBasket_Total_If_Item_Not_Qualified()
        {

            //  var MockOfferVoucherObject = new Mock<IStrategy>();


            MockOfferVoucherObject.Setup(x => x.GetFinalBill(null, null)).Returns(
                new FinalBillObject()
                {
                    AppliedDiscount = 0,
                    ErrorMessage = "There are no products in your basket applicable to voucher Voucher YYY-YYY",
                    FinalBillAmount = 51.00m,
                    IsError = false
                }
                );
            var Msg = @"There are no products in your basket applicable to voucher Voucher YYY-YYY";
            var shopObject = new Business.ShoppingBasket(MockOfferVoucherObject.Object)
            {
                Vouchers = null,
                ShoppingList = null
            };
            var Result = shopObject.GetFinalBill();
            Assert.IsFalse(Result.IsError);
            Assert.That(Result.ErrorMessage, Is.EqualTo(Msg).IgnoreCase);
            Assert.IsTrue(Result.FinalBillAmount == 51.00m);


        }

        [Test]
        public void Test3_OfferVoucher_Amount_Not_Deducted_If_Qualified_Item_Cost_Less_Than_VoucherAmount()
        {


            var ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Hat",Price=25.00m,Qty=1,Category="HATS",DiscountQualified=true},
                new ShoppingItem(){Name="Jumper",Price=26.00m,Qty=1,Category="JUMPERS",DiscountQualified=true},
                new ShoppingItem(){Name="Head Light",Price=3.50m,Qty=1, Category="Head Gear",DiscountQualified=true}
            };
            var Vouchers = new List<Voucher>() {
                new Voucher(){Amount=5,VoucherType=(int)VoucherType.Offer,ItemCategory="Head Gear" }
            };

            MockOfferVoucherObject.Setup(x => x.GetFinalBill(ShoppingList, Vouchers)).Returns(
                new FinalBillObject()
                {
                    AppliedDiscount = 0,
                    ErrorMessage = "",
                    FinalBillAmount = 51.00m,
                    IsError = false
                }
                );

            var shopObject = new Business.ShoppingBasket(MockOfferVoucherObject.Object)
            {
                Vouchers = Vouchers,
                ShoppingList = ShoppingList
            };

            var Result = shopObject.GetFinalBill();
            Assert.IsFalse(Result.IsError);
            Assert.IsTrue(Result.FinalBillAmount == 51.00m);


        }

        [Test]
        public void Test4_Apply_Gift_Voucher_And_Then_Offer_Voucher()
        {
            List<Voucher> Vouchers = new List<Voucher>() {
                new Voucher(){Amount=5,VoucherType=(int)VoucherType.Gift },
                new Voucher(){Amount=5,VoucherType=(int)VoucherType.Offer,ItemCategory="REGULAR" }
            };

            List<ShoppingItem> ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Hat", Price=25.00m,Qty=1,Category="HATS",DiscountQualified=true},
                new ShoppingItem(){Name="Jumper", Price=26.00m,Qty=1,Category="JUMPERS",DiscountQualified=true}
            };

            //Gift Voucher
            decimal GiftVoucherDiscount = 0;

            MockGiftVoucherObject.Setup(x => x.GetFinalBill(ShoppingList, Vouchers)).Returns(
              new FinalBillObject()
              {
                  AppliedDiscount = 5,
                  ErrorMessage = "",
                  FinalBillAmount = 51.00m,
                  IsError = false
              }
              );

            var shopObject = new Business.ShoppingBasket(MockGiftVoucherObject.Object)
            {
                Vouchers = Vouchers,
                ShoppingList = ShoppingList
            };

            var Result = shopObject.GetFinalBill();
            GiftVoucherDiscount = Result.AppliedDiscount;

            //Offer Voucher
            decimal OfferVoucherDiscount = 0;
            var MockOfferVoucherObject = new Mock<IStrategy>();
            MockOfferVoucherObject.Setup(x => x.GetFinalBill(ShoppingList, Vouchers)).Returns(
              new FinalBillObject()
              {
                  AppliedDiscount = 5,
                  ErrorMessage = "",
                  FinalBillAmount = 51.00m,
                  IsError = false
              });

            shopObject = new Business.ShoppingBasket(MockOfferVoucherObject.Object)
            {
                Vouchers = Vouchers,
                ShoppingList = ShoppingList
            };

            Result = shopObject.GetFinalBill();
            OfferVoucherDiscount = Result.AppliedDiscount;

            Assert.IsFalse(Result.IsError);
            Assert.IsTrue(Result.FinalBillAmount - (GiftVoucherDiscount + OfferVoucherDiscount) == 41.00m);
        }

        [Test]
        public void Test5_Dont_Count_On_Gift_Vouchers_While_Applying_For_Offer_Discount()
        {


            List<Voucher> Vouchers = new List<Voucher>() {
                new Voucher(){Amount=5,VoucherType=(int)VoucherType.Offer,ItemCategory="GIFT" }
            };

            List<ShoppingItem> ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Hat", Price=25.00m,Qty=1,Category="HATS",DiscountQualified=true},
                new ShoppingItem(){Name="£30 Gift Voucher", Price=30.00m,Qty=1,Category="GIFT",DiscountQualified=false}
            };

            var Msg = @"You have not reached the spend threshold for voucher YYY-YYY. Spend another £25.01 to receive £5.00 discount from your basket total";

            MockOfferVoucherObject.Setup(x => x.GetFinalBill(ShoppingList, Vouchers)).Returns(
            new FinalBillObject()
            {
                AppliedDiscount = 0,
                ErrorMessage = "You have not reached the spend threshold for voucher YYY-YYY. " +
                                "Spend another £25.01 to receive £5.00 discount from your basket total",
                FinalBillAmount = 55.00m,
                IsError = false
            }
            );

            var shopObject = new Business.ShoppingBasket(MockOfferVoucherObject.Object)
            {
                Vouchers = Vouchers,
                ShoppingList = ShoppingList
            };

            var Result = shopObject.GetFinalBill();

            Assert.IsFalse(Result.IsError);
            Assert.IsTrue(Result.FinalBillAmount == 55.00m);
            Assert.That(Result.ErrorMessage, Is.EqualTo(Msg).IgnoreCase);


        }
        [Test]
        public void Test6_Basket_Total_Should_Not_Change_When_No_Discount_Vouchers_Applied()
        {

            List<Voucher> Vouchers = new List<Voucher>();
            List<ShoppingItem> ShoppingList = new List<ShoppingItem>() {
                new ShoppingItem(){Name="Jumper", Price=54.65m,Qty=1,Category="JUMPERS",DiscountQualified=true},
                new ShoppingItem(){Name="Head Light", Price=3.50m,Qty=1,Category="Head Gear",DiscountQualified=true}
            };


            MockNoVoucherObject.Setup(x => x.GetFinalBill(ShoppingList, Vouchers)).Returns(
        new FinalBillObject()
        {
            AppliedDiscount = 0,
            ErrorMessage = "You have not reached the spend threshold for voucher YYY-YYY. " +
                            "Spend another £25.01 to receive £5.00 discount from your basket total",
            FinalBillAmount = 58.15m,
            IsError = false
        }
        );
            var shopObject = new Business.ShoppingBasket(MockNoVoucherObject.Object)
            {
                Vouchers = Vouchers,
                ShoppingList = ShoppingList
            };

            var Result = shopObject.GetFinalBill();
            Assert.IsFalse(Result.IsError);
            Assert.IsTrue(Result.FinalBillAmount == 58.15m);


        }

        [TearDown]
        public void TearDown()
        {
            MockGiftVoucherObject = null;
            MockOfferVoucherObject = null;
            MockNoVoucherObject = null;
        }
    }
}
