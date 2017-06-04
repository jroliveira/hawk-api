MATCH 
  (account:Account)-[:HAS]->(transaction:Transaction)<-[:BELONGS]-(tag:Tag)
WHERE
  account.email = {email}
WITH
{ 
  name: tag.name,
  transactions: {
	period: substring(transaction.date, 6, 4)+"/"+left(transaction.date, 2),
  	amount: sum(transaction.value),
  	quantity: count(transaction),
	type: labels(transaction)
  }
} as data

RETURN 
  data

SKIP 
  {skip} 

LIMIT 
  {limit};