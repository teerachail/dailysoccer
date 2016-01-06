Feature: Register
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize background
	Given Create mocking
	And Facebook accounts in the system are
	| id  | UserId |
	| f01 | u01    |
	And UserProfile accounts in the system are
	| id  | Points | OrderedCoupon | PhoneNo | VerifierCode | VerifiedPhoneDate | IsFacebookVerified | FavouriteTeamId |
	| u01 | 0      | 0             |         |              |                   | true               |                 |
	| u02 | 1000   | 5             |         |              |                   | false              | t01             |

@mock
Scenario: Create new guest account, system create new account success
	When Call POST api/profile
	Then System create new guest account
	And the new account data should be newly account

@mock
Scenario: Create new account with tie facebook (isConfirmed = true), system create new account success
	When Call POST api/profile/facebook [FacebookId: 'new-facebook-id', UserId: '', IsConfirmed: 'true']
	Then System create new guest account
	And the new account data should be newly account

@mock
Scenario: Create new account with tie facebook (isConfirmed = false), system create new account success
	When Call POST api/profile/facebook [FacebookId: 'new-facebook-id', UserId: '', IsConfirmed: 'false']
	Then System create new guest account
	And the new account data should be newly account

@mock
Scenario: Tie facebook with existing user profile, system tie user profile success
	When Call POST api/profile/facebook [FacebookId: 'new-facebook-id', UserId: 'u02', IsConfirmed: 'true']
	Then System not create new guest account
	And system return account data are
	| id  | Points | OrderedCoupon | PhoneNo | VerifierCode | VerifiedPhoneDate | IsFacebookVerified | FavouriteTeamId |
	| u02 | 1000   | 5             |         |              |                   | true               | t01             |

@mock
Scenario: Get facebook account, system return the selected user profile
	When Call POST api/profile/facebook [FacebookId: 'f01', UserId: 'u02', IsConfirmed: 'false']
	Then System not create new guest account
	And system return account data are
	| id  | Points | OrderedCoupon | PhoneNo | VerifierCode | VerifiedPhoneDate | IsFacebookVerified | FavouriteTeamId |
	| u01 | 0      | 0             |         |              |                   | true               |                 |

@mock
Scenario: Try to create new account by incorrect facebook id, then system reject the request
	When Call POST api/profile/facebook [FacebookId: '', UserId: '', IsConfirmed: 'false']
	Then System not create new guest account
	And system return null

@mock
Scenario: Try to tie account by incorrect facebook id (isConfirmed = true), then system reject the request
	When Call POST api/profile/facebook [FacebookId: 'f01', UserId: 'unknow-user-id', IsConfirmed: 'true']
	Then System not create new guest account
	And system return null

@mock
Scenario: Try to tie account by incorrect facebook id (isConfirmed = false), then system reject the request
	When Call POST api/profile/facebook [FacebookId: 'f01', UserId: 'unknow-user-id', IsConfirmed: 'false']
	Then System not create new guest account
	And system return account data are
	| id  | Points | OrderedCoupon | PhoneNo | VerifierCode | VerifiedPhoneDate | IsFacebookVerified | FavouriteTeamId |
	| u01 | 0      | 0             |         |              |                   | true               |                 |