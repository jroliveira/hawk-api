MERGE 
  (account:Account { 
    email: $email
  })
SET 
  account.id = $id,
  account.creationDate = $creationDate

RETURN 
{ 
  id: account.id,
  email: account.email
} AS data;
