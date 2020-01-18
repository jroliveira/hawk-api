// (:Account)

MATCH 
  (account:Account)
WHERE
  account.email = $email
WITH
  account

// (:Payee)

MATCH
  (payee:Payee)-[:BELONGS]->(account)
WHERE
  payee.name = $payee
WITH
  account,
  payee

// (:PaymentMethod)

MATCH 
  (paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WHERE
  paymentMethod.name = $paymentMethod
WITH
  account,
  payee,
  paymentMethod

// (:Currency)
  
MATCH 
  (currency:Currency)-[:BELONGS]->(account)
WHERE
  currency.name = $currency
WITH
  account,
  payee,
  paymentMethod,
  currency

// (:Tag)

MATCH
  (tag:Tag)-[:BELONGS]->(account)
WHERE
  tag.name in $tags
WITH
  account,
  payee,
  paymentMethod,
  currency,
  tag

OPTIONAL MATCH
  (payee)-[:HAS]->(payeeTag:Tag)-[:BELONGS]->(account)
WITH
  account,
  payee,
  paymentMethod,
  currency,
  tag,
  payeeTag

// (:Configuration)
  
MERGE 
  (configuration:Configuration { 
    description: $description
  })
SET
  configuration.type = $type,
  configuration.description = $newDescription
MERGE
  (configuration)-[:BELONGS]->(account)
MERGE
  (configuration)-[:CONFIGURED_WITH]->(payee)
MERGE
  (configuration)-[:CONFIGURED_WITH]->(paymentMethod)
MERGE
  (configuration)-[:CONFIGURED_WITH]->(currency)
MERGE
  (configuration)-[:CONFIGURED_WITH]->(tag)
  

RETURN 
{ 
  description: configuration.description,
  type: configuration.type,
  account: {
    id: account.id,
	email: account.email
  },
  payee: CASE WHEN payee IS null THEN NULL ELSE {
    name: payee.name,
	tags: COLLECT(distinct payeeTag.name)
  } END,
  currency: {
    name: currency.name
  },
  paymentMethod: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	name: paymentMethod.name
  } END,
  tags: COLLECT(distinct tag.name)
} AS data;
