// (:Account)

MATCH 
  (account:Account)
WHERE
  account.email = $email
WITH
  account

// (:Store)

MATCH
  (store:Store)-[:BELONGS]->(account)
WHERE
  store.name = $store
WITH
  account,
  store

// (:PaymentMethod)

MATCH 
  (paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WHERE
  paymentMethod.name = $paymentMethod
WITH
  account,
  store,
  paymentMethod

// (:Currency)
  
MATCH 
  (currency:Currency)-[:BELONGS]->(account)
WHERE
  currency.name = $currency
WITH
  account,
  store,
  paymentMethod,
  currency

// (:Tag)

MATCH
  (tag:Tag)-[:BELONGS]->(account)
WHERE
  tag.name in $tags
WITH
  account,
  store,
  paymentMethod,
  currency,
  tag

OPTIONAL MATCH
  (store)-[:HAS]->(storeTag:Tag)-[:BELONGS]->(account)
WITH
  account,
  store,
  paymentMethod,
  currency,
  tag,
  storeTag

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
  (configuration)-[:CONFIGURED_WITH]->(store)
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
  store: CASE WHEN store IS null THEN NULL ELSE {
    name: store.name,
	tags: COLLECT(distinct storeTag.name)
  } END,
  currency: {
    name: currency.name
  },
  paymentMethod: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	name: paymentMethod.name
  } END,
  tags: COLLECT(distinct tag.name)
} AS data;
