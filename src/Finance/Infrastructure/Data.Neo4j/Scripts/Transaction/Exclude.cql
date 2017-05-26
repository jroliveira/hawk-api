MATCH 
  (account:Account)-[has:HAS]->(transaction:Transaction)-[relation]-()
WHERE
  account.email = {email}
  AND id(transaction) = {id}

DELETE
  relation,
  transaction,
  has