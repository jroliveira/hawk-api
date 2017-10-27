MATCH 
  (transaction:Transaction)
WHERE
  transaction.id = {transaction}

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