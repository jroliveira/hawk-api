MATCH 
  (configuration:Configuration)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND configuration.description = $description
WITH
  account,
  configuration

OPTIONAL MATCH
  (configuration)-[:CONFIGURED_WITH]->(payee:Payee)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee

OPTIONAL MATCH 
  (configuration)-[:CONFIGURED_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee,
  paymentMethod
  
OPTIONAL MATCH 
  (configuration)-[:CONFIGURED_WITH]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee,
  paymentMethod,
  currency

OPTIONAL MATCH
  (configuration)-[:CONFIGURED_WITH]->(tag:Tag)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee,
  paymentMethod,
  currency,
  tag

OPTIONAL MATCH
  (payee)-[:HAS]->(payeeTag:Tag)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee,
  paymentMethod,
  currency,
  tag,
  payeeTag

RETURN 
{ 
  type: configuration.type,
  description: configuration.description,
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
} AS data;
