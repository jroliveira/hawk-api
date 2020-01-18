MATCH 
  (store:Store)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND store.name = $name

RETURN
{ 
  name: store.name,
  transactions: SIZE((:Transaction)-[:IN]->(store))
} AS data;
