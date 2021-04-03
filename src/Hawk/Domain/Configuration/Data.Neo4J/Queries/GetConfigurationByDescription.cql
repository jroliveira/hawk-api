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
  (payee)-[:AT]->(coordinate:Coordinate)
WITH
  account,
  configuration,
  payee,
  COLLECT(DISTINCT coordinate) AS coordinates

OPTIONAL MATCH
  (configuration)-[:CONFIGURED_WITH]->(category:Category)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee,
  coordinates,
  category

OPTIONAL MATCH 
  (configuration)-[:CONFIGURED_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee,
  coordinates,
  category,
  paymentMethod
  
OPTIONAL MATCH 
  (configuration)-[:CONFIGURED_WITH]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee,
  coordinates,
  category,
  paymentMethod,
  currency

OPTIONAL MATCH
  (configuration)-[:CONFIGURED_WITH]->(tag:Tag)-[:BELONGS]->(account)
WITH
  account,
  configuration,
  payee,
  coordinates,
  category,
  paymentMethod,
  currency,
  COLLECT(DISTINCT tag) AS tags

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
    location: {
      coordinates: [coordinate IN coordinates | {
        latitude: coordinate.latitude,
        longitude: coordinate.longitude
      }]
    },
	transactions: SIZE((:Transaction)-[:IN]->(payee))
  } END,
  category: CASE WHEN category IS null THEN NULL ELSE {
    name: category.name,
    icon: category.icon,
	transactions: SIZE((:Transaction)-[:WITH]->(category))
  } END,
  currency: {
	code: currency.code,
    symbol: currency.symbol,
    transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
  },
  paymentMethod: CASE WHEN paymentMethod IS null THEN NULL ELSE {
	name: paymentMethod.name,
    transactions: SIZE((:Transaction)-[:PAID_WITH]->(paymentMethod))
  } END,
  tags: [tag IN tags | {
    name: tag.name,
    transactions: SIZE((:Transaction)-[:HAS]->(tag))
  }]
} AS data;
