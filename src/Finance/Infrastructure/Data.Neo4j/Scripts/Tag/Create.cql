MATCH 
  (transaction:Transaction)
WHERE
  id(transaction) = {transaction}

WITH
  transaction

UNWIND
  {tags} AS tagName
MERGE
  (tag:Tag { 
    name: tagName
  })
CREATE UNIQUE
  (transaction)<-[:BELONGS]-(tag)