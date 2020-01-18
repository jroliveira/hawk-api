MATCH 
  (store:Store)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: store.name,
  transactions: SIZE((:Transaction)-[:IN]->(store))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
