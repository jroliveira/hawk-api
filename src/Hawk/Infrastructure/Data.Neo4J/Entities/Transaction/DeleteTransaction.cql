MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
WHERE
  account.email = {email}
  AND transaction.id = {id}

DETACH DELETE transaction;
