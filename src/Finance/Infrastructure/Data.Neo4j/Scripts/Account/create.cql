MERGE 
  (account:Account { 
    email: {email}
  })
SET 
  account.password = {password},
  account.creationDate = {creationDate}

RETURN 
{ 
  id: account.id,
  email: account.email,
  password: account.password,
  creationDate: account.creationDate
} as data;