MATCH 
  (account:Account)-[:HAS]->(:Transaction)-[:IN]->(store:Store)
WHERE
  account.email = {email}
RETURN
{ 
  name: store.name,
  total: count(store.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};