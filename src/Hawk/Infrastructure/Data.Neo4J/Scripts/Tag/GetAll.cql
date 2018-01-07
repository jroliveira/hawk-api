MATCH 
  (account:Account)-[:HAS]->(:Transaction)<-[:BELONGS]-(tag:Tag)
WHERE
  account.email = {email}
RETURN
{ 
  name: tag.name,
  total: count(tag.name)
} as data

SKIP 
  {skip} 

LIMIT 
  {limit};