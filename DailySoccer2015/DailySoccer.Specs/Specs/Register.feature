Feature: Register
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: Initialize background
	Given Create mocking

@mock
Scenario: Create new guest account, system create new account success
	When Call POST api/profile
	Then System create new guest account
	And System return the new account to the caller
	And the new account data should be newly account