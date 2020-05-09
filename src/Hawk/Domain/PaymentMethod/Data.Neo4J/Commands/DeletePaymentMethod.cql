MATCH 
  (paymentMethod:PaymentMethod)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND paymentMethod.name = $name
  AND NOT ()-->(paymentMethod)

DETACH DELETE paymentMethod;
