MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND transaction.id = $id
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
  (payee)-[:AT]->(coordinate:Coordinate)
WITH
  account,
  transaction,
  payee,
  COLLECT(DISTINCT coordinate) AS coordinates

OPTIONAL MATCH
  (transaction)-[:WITH]->(category:Category)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  coordinates,
  category

OPTIONAL MATCH 
  (transaction)-[:PAID_WITH]->(paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  coordinates,
  category,
  paymentMethod
  
OPTIONAL MATCH 
  (transaction)-[:PAID_IN]->(currency:Currency)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  coordinates,
  category,
  paymentMethod,
  currency

OPTIONAL MATCH
  (transaction)-[:HAS]->(tag:Tag)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  coordinates,
  category,
  paymentMethod,
  currency,
  COLLECT(DISTINCT tag) AS tags

OPTIONAL MATCH 
  (transaction)-[:IS_LINKED]->(installment:Installment)-[:BELONGS]->(account)
WITH
  account,
  transaction,
  payee,
  coordinates,
  paymentMethod,
  currency,
  tags,
  installment

RETURN 
{ 
  id: transaction.id,
  type: LABELS(transaction),
  status: transaction.status,
  description: transaction.description,
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
  payment: {
    year: transaction.year,
    month: transaction.month,
    day: transaction.day,
    cost: {
      value: transaction.value,
      currency: {
        code: currency.code,
        symbol: currency.symbol,
        transactions: SIZE((:Transaction)-[:PAID_IN]->(currency))
      }
    },
    method: CASE WHEN paymentMethod IS null THEN NULL ELSE {
      name: paymentMethod.name,
      transactions: SIZE((:Transaction)-[:PAID_WITH]->(paymentMethod))
    } END
  },
  installment: CASE WHEN installment IS null THEN NULL ELSE {
    id: installment.id,
    frequency: installment.frequency,
    installments: {
      total: CASE WHEN installment.total IS null THEN 0 ELSE installment.total END,
      current: SIZE((:Transaction)-[:IS_LINKED]->(installment))
    }
  } END,
  tags: [tag IN tags | {
    name: tag.name,
    transactions: SIZE((:Transaction)-[:HAS]->(tag))
  }]
} AS data;
