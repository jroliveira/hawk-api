MATCH 
  (account:Account)-[:HAS]->(:Transaction)<-[:BELONGS]-(tag:Tag)
WHERE
  account.email = {email}
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