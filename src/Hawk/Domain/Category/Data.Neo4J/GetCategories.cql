MATCH 
  (category:Category)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email

RETURN
{ 
  name: category.name,
  transactions: SIZE((:Transaction)-[:IN]->(category))
} AS data

SKIP 
  $skip 

LIMIT 
  $limit;
