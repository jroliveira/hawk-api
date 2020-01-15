MATCH
  (paymentMethod:PaymentMethod)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: paymentMethod.name,
  total: count(paymentMethod.name)
} as data

SKIP 
  $skip 

LIMIT 
  $limit;
