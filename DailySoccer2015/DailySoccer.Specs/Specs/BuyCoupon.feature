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
	| id  | Points | OrderedCoupon | PhoneNo | VerifierCode | VerifiedPhoneDate | IsFacebookVerified | FavouriteTeamId |
	| u01 | 1000   | 0             |         |              |                   | true               |                 |

@mock
Scenario: ผู้ใช่ที่มีข้อมูลถูกต้องซื้อคูปองใบเดียว ระบบทำการซื้อคูปองสำเร็จ
	When Call POST api/Coupons UserId: 'u01', BuyAmount: '1'
	Then ระบบทำการซื้อคูปอง '1' ใบ ให้กับ UserId: 'u01' พร้อมกับลดแต้มเหลือ Points: '800'
