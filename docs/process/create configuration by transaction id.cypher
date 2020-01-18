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
  
MERGE 
  (configuration:Configuration { 
    description: {description},
    type: labels(transaction)
  })
CREATE UNIQUE
  (configuration)-[:BELONGS]->(account)
CREATE UNIQUE
  (configuration)-[:CONFIGURED_WITH]->(payee)
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
  payee: CASE WHEN payee IS null THEN NULL ELSE {
    name: payee.name,
	tags: collect(payeeTag.name)
  } END,
  currency: {
    name: currency.name
  },
  paymentMethod: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	name: paymentMethod.name
  } END,
  tags: collect(tag.name)
} as data;