MATCH 
  (installment:Installment)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND installment.id = $id
  AND NOT ()-->(installment)

DETACH DELETE installment;
