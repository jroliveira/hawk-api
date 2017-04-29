MERGE 
  (account:Account { 
    email: {email}
  })

ON CREATE 
  SET 
    account.password = {password},
    account.creationDate = {creationDate}

RETURN 
{ 
  id: id(account),
  email: account.email,
  password: account.password,
  creationDate: account.creationDate
} as data;