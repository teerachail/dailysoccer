Feature: Prediction
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize background
	Given Create mocking
	And UserProfile accounts in the system are
	| id  | Points | OrderedCoupon | PhoneNo      | VerifierCode | VerifiedPhoneDate | IsFacebookVerified | FavouriteTeamId |
	| u01 | 1000   | 0             | +66912345678 | v01          | 1/1/2016          | true               | t01             |
	And Predictions in the system are
	| id | PredictionTeamId | CompletedDate | ActualPoints | PredictionPoints | CreatedDate |

@mock
Scenario: ผู้ใช้ทายผลทีมเจ้าบ้านชนะในขณะที่สามารถทายผลได้ ระบบบันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: 't01', IsCancel: 'false'
	Then ระบบทำการบันทึกการทายผลให้กับ UserId: 'u01', MatchId: 'm01', TeamId: 't01', PredictionPoints: '110'
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ทายผลทีมเยือนชนะในขณะที่สามารถทายผลได้ ระบบบันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: 't02', IsCancel: 'false'
	Then ระบบทำการบันทึกการทายผลให้กับ UserId: 'u01', MatchId: 'm01', TeamId: 't02', PredictionPoints: '120'
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ทายผลว่าแมช์จะเสมอในขณะที่สามารถทายผลได้ ระบบบันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: '', IsCancel: 'false'
	Then ระบบทำการบันทึกการทายผลให้กับ UserId: 'u01', MatchId: 'm01', TeamId: '', PredictionPoints: '150'
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ยกเลิกการทายผลในขณะที่สามารถทายผลได้ ระบบบันทึกการยกเลิก
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	And Predictions in the system are
	| id  | PredictionTeamId | CompletedDate | ActualPoints | PredictionPoints | CreatedDate |
	| p01 | t01              |               |              | 110              | 1/1/2015    |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: '', IsCancel: 'true'
	Then ระบบบันทึกการยกเลิกการทายผลของ UserId: 'u01', MatchId: 'm01'
	And ระบบไม่ทำการบันทึกการทายผล

@mock
Scenario: ผู้ใช้ทายผลในขณะที่แมช์ได้เริ่มแข่งแล้ว ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   | 1/1/2015    |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ทายผลในขณะที่แมช์แข่งจบแล้ว ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   | 1/1/2015    | 1/1/2015      | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ทายผลในขณะที่ระบบไม่มีแมช์การแข่งขัน ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ที่ไม่มีในระบบทำการทายผล ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'unknow-user-id', MatchId: 'm01', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ที่ไม่มีในระบบทำการทายผล (Empty) ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: '', MatchId: 'm01', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ที่ไม่มีในระบบทำการทายผล (NULL) ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'NULL', MatchId: 'm01', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ทายผลที่รหัสแมช์ไม่ถูกต้อง ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'unknow-match-id', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ทายผลที่รหัสแมช์ไม่ถูกต้อง (Empty) ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: '', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ทายผลที่รหัสแมช์ไม่ถูกต้อง (NULL) ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'NULL', TeamId: 't01', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ยกเลิกการทายผลในขณะที่แมช์ได้เริ่มแข่งไปแล้ว ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   | 1/1/2015    |               | t01        | 110           | t02        | 120           | 150        |
	And Predictions in the system are
	| id  | PredictionTeamId | CompletedDate | ActualPoints | PredictionPoints | CreatedDate |
	| p01 | t01              |               |              | 110              | 1/1/2015    |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: '', IsCancel: 'true'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ยกเลิกการทายผลในขณะที่แมช์แข่งจบไปแล้ว ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   | 1/1/2015    | 1/1/2015      | t01        | 110           | t02        | 120           | 150        |
	And Predictions in the system are
	| id  | PredictionTeamId | CompletedDate | ActualPoints | PredictionPoints | CreatedDate |
	| p01 | t01              |               |              | 110              | 1/1/2015    |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: '', IsCancel: 'true'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล

@mock
Scenario: ผู้ใช้ทายผลที่รหัสทีมไม่ถูกต้อง ระบบไม่บันทึกการทายผล
	And Matches in the system are
	| id  | BeginDateTimeUTC | FilterDate | StartedDate | CompletedDate | TeamHomeId | TeamHomePoint | TeamAwayId | TeamAwayPoint | DrawPoints |
	| m01 | 1/1/2015         | 20150101   |             |               | t01        | 110           | t02        | 120           | 150        |
	When Call PUT api/prediction UserId: 'u01', MatchId: 'm01', TeamId: 'unknow-team-id', IsCancel: 'false'
	Then ระบบไม่ทำการบันทึกการทายผล
	And ระบบไม่บันทึกการยกเลิกการทายผล