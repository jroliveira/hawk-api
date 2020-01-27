MATCH 
  (category:Category)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND category.name = $name

RETURN
{ 
  name: category.name,
  transactions: SIZE((:Transaction)-[:IN]->(category))
} AS data;
