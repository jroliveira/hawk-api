MATCH 
  (account:Account)-[:HAS]->(:Transaction)-[:IN]->(store:Store)
WHERE
  account.email = {email}
WITH
{ 
  name: store.name,
  total: count(store.name)
} as store

RETURN 
  store as data

SKIP 
  {skip} 

LIMIT 
  {limit};