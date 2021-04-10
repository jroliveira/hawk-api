MERGE 
  (account:Account { 
    email: $email
  })
SET 
  account.id = $id,
  account.creationDate = $creationDate,
  account.hiddenMoney = $hiddenMoney,
  account.currencyCode = $currencyCode

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
