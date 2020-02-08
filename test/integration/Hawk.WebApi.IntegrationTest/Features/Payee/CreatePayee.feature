#language:en

Feature: Create payee
  In order to create payee
  As a user
  I want to create payee

Scenario: Create payee
  Given a new payee
  When I post payee new payee
  Then a new payee should be created
