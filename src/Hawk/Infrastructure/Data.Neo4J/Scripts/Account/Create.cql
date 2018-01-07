MERGE 
  (account:Account { 
    email: {email}
  })
SET 
  account.creationDate = {creationDate}

RETURN 
{ 
  id: account.id,
  email: account.email,
  creationDate: account.creationDate
} as data;