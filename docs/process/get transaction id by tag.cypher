:params {email:"junior@gmail.com", tag: "jogo-na-mesa"}

MATCH 
  (tag:Tag {name: {tag}})<-[:HAS]-(transaction:Transaction)-[:BELONGS]->(account:Account)
WHERE
  account.email = {email}
WITH
  account,
  transaction

OPTIONAL MATCH
  (transaction)-[:IN]->(store:Store)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  store

OPTIONAL MATCH 
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  store,
  paymentMethod
  
OPTIONAL MATCH 
  (transaction)-[:PAID_IN]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  store,
  paymentMethod,
  currency

OPTIONAL MATCH
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
WITH
  account,
  store,
  paymentMethod,
  currency,
  tag,
  transaction

OPTIONAL MATCH
  (store)-[:HAS]->(storeTag:Tag)-[:BELONGS]->(account)
WITH
  account,
  store,
  paymentMethod,
  currency,
  tag,
  storeTag,
  transaction
  
RETURN 
{ 
  type: labels(transaction),
  account: {
    id: account.id,
	email: account.email
  },
  store: CASE WHEN store IS null THEN NULL ELSE {
    name: store.name,
	tags: collect(distinct storeTag.name)
  } END,
  currency: {
    name: currency.name
  },
  paymentMethod: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	name: paymentMethod.name
  } END,
  tags: collect(distinct tag.name)
} as data;