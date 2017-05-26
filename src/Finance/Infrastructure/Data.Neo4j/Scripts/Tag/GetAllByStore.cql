MATCH 
  (account:Account)-[:HAS]->(transaction:Transaction)-[:IN]->(store:Store)
WHERE
  account.email = {email}
  AND store.name = {store}
WITH
  transaction

MATCH
  (transaction)<-[:BELONGS]-(tag:Tag)
WITH
{ 
  name: tag.name,
  total: count(tag.name)
} as tag

RETURN 
  tag as data

SKIP 
  {skip} 

LIMIT 
  {limit};