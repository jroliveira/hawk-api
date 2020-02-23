MATCH 
  (account:Account)
WHERE
  account.email = $email

RETURN 
{ 
  id: account.id,
  email: account.email
} AS data;
