:params {email:"junior@gmail.com", tag: "jogo-na-mesa"}

MATCH 
  (tag:Tag {name: {tag}})<-[:HAS]-(transaction:Transaction)-[:BELONGS]->(account:Account)
WHERE
  account.email = {email}
WITH
  account,
  transaction

OPTIONAL MATCH
  (transaction)-[:IN]->(payee:Payee)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee

OPTIONAL MATCH 
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  paymentMethod
  
OPTIONAL MATCH 
  (transaction)-[:PAID_IN]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  paymentMethod,
  currency

OPTIONAL MATCH
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
WITH
  account,
  payee,
  paymentMethod,
  currency,
  tag,
  transaction

OPTIONAL MATCH
  (payee)-[:HAS]->(payeeTag:Tag)-[:BELONGS]->(account)
WITH
  account,
  payee,
  paymentMethod,
  currency,
  tag,
  payeeTag,
  transaction
  
RETURN 
{ 
  type: labels(transaction),
  account: {
    id: account.id,
	email: account.email
  },
  payee: CASE WHEN payee IS null THEN NULL ELSE {
    name: payee.name,
	tags: collect(distinct payeeTag.name)
  } END,
  currency: {
    name: currency.name
  },
  paymentMethod: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	name: paymentMethod.name
  } END,
  tags: collect(distinct tag.name)
} as data;