:params {email:"junior@gmail.com", id: "8dddb309-0a53-4737-87d0-14a4c9b426dc", description: "TRANSFERENCIA - VENCIMENTO"}

MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
WHERE
  account.email = {email}
  AND transaction.id = {id}
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
  
MERGE 
  (configuration:Configuration { 
    description: {description},
    type: labels(transaction)
  })
CREATE UNIQUE
  (configuration)-[:BELONGS]->(account)
CREATE UNIQUE
  (configuration)-[:CONFIGURED_WITH]->(store)
CREATE UNIQUE
  (configuration)-[:CONFIGURED_WITH]->(paymentMethod)
CREATE UNIQUE
  (configuration)-[:CONFIGURED_WITH]->(currency)
CREATE UNIQUE
  (configuration)-[:CONFIGURED_WITH]->(tag)
  

RETURN 
{ 
  description: configuration.description,
  type: labels(transaction),
  account: {
    id: account.id,
	email: account.email
  },
  store: CASE WHEN store IS null THEN NULL ELSE {
    name: store.name,
	tags: collect(storeTag.name)
  } END,
  currency: {
    name: currency.name
  },
  paymentMethod: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	name: paymentMethod.name
  } END,
  tags: collect(tag.name)
} as data;