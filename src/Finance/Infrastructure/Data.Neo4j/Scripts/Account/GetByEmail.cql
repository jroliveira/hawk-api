MATCH 
  (account:Account)

WHERE
  account.email = {email}

RETURN 
{ 
  id: id(account),
  email: account.email,
  password: account.password,
  creationDate: account.creationDate
} as data;