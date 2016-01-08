Feature: BuyCoupon
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize background
	Given Create mocking
	And Reward groups in the system are
	| id   | ExpiredDate | RequiredPoints |
	| rg01 | 1/1/2016    | 200            |
	And UserProfile accounts in the system are
	| id                    | Points | OrderedCoupon | PhoneNo      | VerifierCode | VerifiedPhoneDate | IsFacebookVerified | FavouriteTeamId |
	| u01                   | 1000   | 0             | +66912345678 | v01          | 1/1/2016          | true               | t01             |
	| u02                   | 500    | 3             | +66912345678 | v02          | 1/1/2016          | true               |                 |
	| u03-unverify-facebook | 400    | 0             | +66812345678 | v03          | 1/1/2016          | false              |                 |
	| u04-unverify-phone    | 400    | 0             | +66812345678 | v03          |                   | true               |                 |

@mock
Scenario: ผู้ใช้ที่มีข้อมูลถูกต้องซื้อคูปองใบเดียว ระบบทำการซื้อคูปองสำเร็จ
	When Call POST api/Coupons UserId: 'u01', BuyAmount: '1'
	Then ระบบอัพเดท UserId: 'u01' ว่าเหลือ Points: '800' และมีจำนวนคูปอง '1' ใบ

@mock
Scenario: ผู้ใช้มีข้อมูลถูกต้องซื้อคูปองหลายใบ ระบบทำการซื้อคูปองสำเร็จ
	When Call POST api/Coupons UserId: 'u01', BuyAmount: '3'
	Then ระบบอัพเดท UserId: 'u01' ว่าเหลือ Points: '400' และมีจำนวนคูปอง '3' ใบ

@mock
Scenario: ผู้ใช้ซื้อคูปองโดยใช้ points ทั้งหมด ระบบทำการซื้อคูปองสำเร็จ
	When Call POST api/Coupons UserId: 'u01', BuyAmount: '5'
	Then ระบบอัพเดท UserId: 'u01' ว่าเหลือ Points: '0' และมีจำนวนคูปอง '5' ใบ

@mock
Scenario: ผู้ใช้ซื้อคูปองโดยใช้ points เกิดที่กำหนด ระบบไม่ทำการซื้อคูปอง
	When Call POST api/Coupons UserId: 'u01', BuyAmount: '6'
	Then ระบบไม่ทำการซื้อคูปอง

@mock
Scenario: ผู้ใช้ที่เคยซื้อคูปองแล้วทำการซื้อคูปองเพิ่ม ระบบทำการซื้อคูปองสำเร็จ
	When Call POST api/Coupons UserId: 'u02', BuyAmount: '2'
	Then ระบบอัพเดท UserId: 'u02' ว่าเหลือ Points: '100' และมีจำนวนคูปอง '5' ใบ

@mock
Scenario: 0ผู้ใช้ซื้อคูปอง ในขณะที่ระบบยังไม่เปิดให้ซื้อ ระบบไม่ทำการซื้อคูปอง
	Given Reward groups in the system are
	| id   | ExpiredDate | RequiredPoints |
	When Call POST api/Coupons UserId: 'u01', BuyAmount: '1'
	Then ระบบไม่ทำการซื้อคูปอง

@mock
Scenario: ผู้ใช้ที่ยังไม่ได้ยืนยัน facebook ซื้อคูปอง ระบบไม่ทำการซื้อคูปอง
	When Call POST api/Coupons UserId: 'u03-unverify-facebook', BuyAmount: '1'
	Then ระบบไม่ทำการซื้อคูปอง

@mock
Scenario: ผู้ใช้ที่ยังไม่ได้ยืนยันเบอร์โทรศัพท์ซื้อคูปอง ระบบไม่ทำการซื้อคูปอง
	When Call POST api/Coupons UserId: 'u04-unverify-phone', BuyAmount: '1'
	Then ระบบไม่ทำการซื้อคูปอง

@mock
Scenario: ผู้ใช้ที่ไม่มีในระบบซื้อคูปอง ระบบไม่ทำการซื้อคูปอง
	When Call POST api/Coupons UserId: 'unknow', BuyAmount: '1'
	Then ระบบไม่ทำการซื้อคูปอง

@mock
Scenario: ระบุชื่อผู้ใช้ไม่ถูกต้อง (ชื่อผู้ใช้ว่าง) แล้วทำการซื้อคูปอง ระบบไม่ทำการซื้อคูปอง
	When Call POST api/Coupons UserId: '', BuyAmount: '1'
	Then ระบบไม่ทำการซื้อคูปอง

@mock
Scenario: ระบุชื่อผู้ใช้ไม่ถูกต้อง (ชื่อผู้ใช้ null) แล้วทำการซื้อคูปอง ระบบไม่ทำการซื้อคูปอง
	When Call POST api/Coupons UserId: 'NULL', BuyAmount: '1'
	Then ระบบไม่ทำการซื้อคูปอง

@mock
Scenario: ผู้ใช้ซื้อคูปองโดยระบุจำนวนคูปองไม่ถูกต้อง (ระบุศูนย์ใบ) ระบบไม่ทำการซื้อคูปอง
	When Call POST api/Coupons UserId: 'u01', BuyAmount: '0'
	Then ระบบไม่ทำการซื้อคูปอง

@mock
Scenario: ผู้ใช้ซื้อคูปองโดยระบุจำนวนคูปองไม่ถูกต้อง (ระบุต่ำกว่าศูนย์ใบ) ระบบไม่ทำการซื้อคูปอง
	When Call POST api/Coupons UserId: 'u01', BuyAmount: '-1'
	Then ระบบไม่ทำการซื้อคูปอง