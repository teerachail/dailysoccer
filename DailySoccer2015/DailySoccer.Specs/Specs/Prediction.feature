Feature: Prediction
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize background
	Given Create mocking
	And UserProfile accounts in the system are
	| id                    | Points | OrderedCoupon | PhoneNo      | VerifierCode | VerifiedPhoneDate | IsFacebookVerified | FavouriteTeamId |
	| u01                   | 1000   | 0             | +66912345678 | v01          | 1/1/2016          | true               | t01             |

@mock
Scenario: ผู้ใช้ทายผลในขณะที่สามารถทายผลได้ ระบบบันทึกการทายผล
	And Matches in the system are
	| id  | BeginDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015  |             |               | t01        | 110           | t02        | 120           | 150        |
	And Predictions in the system are
	| id | PredictionTeamId | CompletedDate | ActualPoints | PredictionPoints | CreatedDate |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: 't01', IsCancel: 'false'
	Then ระบบทำการบันทึกการทายผลให้กับ UserId: 'u01', MatchId: 'm01', TeamId: 't01', PredictionPoints: '110'
