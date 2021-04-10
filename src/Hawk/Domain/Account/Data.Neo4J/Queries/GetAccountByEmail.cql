MATCH 
  (account:Account)
WHERE
  account.email = $email

RETURN 
{ 
  id: account.id,
  email: account.email,
  setting: {
    money: {
      hidden: account.hiddenMoney,
      currency: {
        code: account.currencyCode
      }
    }
  }
} AS data;
