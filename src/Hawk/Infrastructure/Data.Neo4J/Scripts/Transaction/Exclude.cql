MATCH 
  (account:Account)-[:HAS]->(transaction:Transaction)
WHERE
  account.email = {email}
  AND transaction.id = {id}

DETACH DELETE transaction;