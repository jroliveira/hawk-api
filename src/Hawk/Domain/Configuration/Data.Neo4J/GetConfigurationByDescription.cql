MATCH 
  (configuration:Configuration)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND configuration.description = $description
WITH
  account,
  configuration

OPTIONAL MATCH
  (configuration)-[:CONFIGURED_WITH]->(store:Store)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  store

OPTIONAL MATCH 
  (configuration)-[:CONFIGURED_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  store,
  paymentMethod
  
OPTIONAL MATCH 
  (configuration)-[:CONFIGURED_WITH]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  store,
  paymentMethod,
  currency

OPTIONAL MATCH
  (configuration)-[:CONFIGURED_WITH]->(tag:Tag)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  store,
  paymentMethod,
  currency,
  tag

OPTIONAL MATCH
  (store)-[:HAS]->(storeTag:Tag)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  store,
  paymentMethod,
  currency,
  tag,
  storeTag

RETURN 
{ 
  type: configuration.type,
  description: configuration.description,
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
} AS data;
