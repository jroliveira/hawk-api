MATCH 
  (account:Account)-[:HAS]->(:Transaction)-[:IN]->(store:Store)
WHERE
  account.email = {email}
WITH
  distinct store.name as store

RETURN 
{
  name: store
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};