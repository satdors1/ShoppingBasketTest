Feature: ShoppingBasketDiscountTests
	In order to test Gift and Offer Discount vouchers are applied for shopping basket
	As a developer
	I want test the Shopping Basket Total is shown correctly after applying discounts, and error is shown 
	appropriately where incorrect discount voucher are used.

@mytag
Scenario: Basket 1 - £5 Gift Voucher XXX-XXX is applied
	Given I have 1 Hat @ £10.50, 1 Jumper @ £54.65 in the basket
	And I have used £5 'Gift' voucher 'XXX-XXX' of 'Regular' category
	When I call GetFinalBill function
	Then the Total Bill should be shown as £60.15

Scenario: Basket 2 - £5 Head Gear Voucher YYY-YYY is applied When Basket Does not contain qualifying item
	Given I have 1 Hat @ £25.00, 1 Jumper @ £26.00 in the basket
	And I have used £5 'Offer' voucher 'YYY-YYY' of 'HEAD GEAR' category
	When I call GetFinalBill function
	Then the Total Bill should be shown as £51.00
	And The message should be shown as 'There are no products in your basket applicable to voucher Voucher YYY-YYY'

Scenario: Basket 3 - £5 Head Gear Voucher YYY-YYY is applied When Basket Does contain qualifying item
	Given I have 1 Hat @ £25.00, 1 Jumper @ £26.00  in the basket
	And 1 Head Light @ £3.50 of 'Head Gear' Product Category also in the basket
	And I have used £5 'Offer' voucher 'YYY-YYY' of 'HEAD GEAR' category
	When I call GetFinalBill function
	Then the Total Bill should be shown as £51.00

Scenario: Basket 4 - £5 Head Gear Voucher YYY-YYY is applied When Basket Does contain qualifying item
	Given I have 1 Hat @ £25.00, 1 Jumper @ £26.00  in the basket
	And I have used £5 'Gift' voucher 'XXX-XXX' of 'Regular' category
	And I have used £5 'Offer' voucher 'YYY-YYY' of 'Regular' category
	When I call GetFinalBill function
	Then the Total Bill should be shown as £41.00

Scenario: Basket 5 - £5 Offer Voucher YYY-YYY is applied When Basket total value is above £50 but contains a
		gift card purchase which doesnot qualify for offer voucher claim.
	Given I have 1 Hat @ £25.00, 1 Gift Voucher @ £30.00 in the basket
	And I have used £5 'Offer' voucher 'YYY-YYY' of 'Regular' category
	When I call GetFinalBill function
	Then the Total Bill should be shown as £55.00

Scenario: Basket 6 - £5 Offer Voucher YYY-YYY is applied When Basket total value is above £50 but contains a
		gift card purchase which doesnot qualify for offer voucher claim.
	Given I have 1 Jumper @ £54.65, 1 Head Light @ £3.50 of 'Head Gear' category in the basket
	And I have applied no voucher
	When I call GetFinalBill function
	Then the Total Bill should be shown as £58.15