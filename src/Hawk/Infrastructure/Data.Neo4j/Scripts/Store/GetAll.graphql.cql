MATCH 
  (account:Account)-[:HAS]->(transaction:Transaction)-[:IN]->(store:Store)
WHERE
  account.email = {email}
RETURN
{ 
  name: store.name,
  total: count(store.name),
  transactions: {
	period: transaction.year + "/" + transaction.month,
  	amount: sum(transaction.value),
  	quantity: count(transaction),
	type: labels(transaction)
  }
} as data;