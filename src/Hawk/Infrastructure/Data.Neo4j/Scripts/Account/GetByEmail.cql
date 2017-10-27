MATCH 
  (account:Account)

WHERE
  account.email = {email}

RETURN 
{ 
  id: account.id,
  email: account.email,
  password: account.password,
  creationDate: account.creationDate
} as data;