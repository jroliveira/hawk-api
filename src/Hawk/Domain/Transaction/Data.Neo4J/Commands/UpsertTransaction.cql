﻿// (:Account)

MATCH
  (account:Account)
WHERE
  account.email = $email

// (:Payee)

MATCH
  (payee:Payee)-[:BELONGS]->(account)
WHERE
  payee.name = $payee

OPTIONAL MATCH
  (payee)-[:AT]->(coordinate:Coordinate)

// (:Category)

MATCH
  (category:Category)-[:BELONGS]->(account)
WHERE
  category.name = $category

// (:Currency)

MATCH
  (currency:Currency)-[:BELONGS]->(account)
WHERE
  currency.code = $currency

// (:PaymentMethod)

MATCH
  (paymentMethod:PaymentMethod)-[:BELONGS]->(account)
WHERE
  paymentMethod.name = $method

// (:Transaction)

MERGE
  (transaction:Transaction { 
    id: $id
  })
REMOVE 
  transaction:Income:Expense
SET
  transaction:#type#,
  transaction.status = $status,
  transaction.description = $description,
  transaction.value = $value,
  transaction.year = $year,
  transaction.month = $month,
  transaction.day = $day
WITH
  account,
  payee,
  COLLECT(DISTINCT coordinate) AS coordinates,
  category,
  currency,
  paymentMethod,
  transaction

OPTIONAL MATCH
  (transaction)-[relationships]->()
DELETE
  relationships
MERGE
  (transaction)-[:BELONGS]->(account)
MERGE
  (transaction)-[:IN]->(payee)
MERGE
  (transaction)-[:WITH]->(category)
MERGE
  (transaction)-[:PAID_IN]->(currency)
MERGE
  (transaction)-[:PAID_WITH]->(paymentMethod)
WITH
  account,
  payee,
  coordinates,
  category,
  currency,
  paymentMethod,
  transaction

// (:Tag)

UNWIND
  $tags AS tagName
MERGE
  (tag:Tag { 
    name: tagName
  })
MERGE
  (tag)-[:BELONGS]->(account)
MERGE
  (payee)-[:HAS]->(tag)
MERGE
  (transaction)-[:HAS]->(tag)
WITH
  account,
  transaction,
  payee,
  coordinates,
  category,
  paymentMethod,
  currency,
  COLLECT(DISTINCT tag) AS tags

// (:Installment)

UNWIND 
  $installment AS installmentParams
MERGE 
  (installment:Installment { 
    id: installmentParams.id
  })
SET
  installment.frequency = installmentParams.frequency,
  installment.total = installmentParams.total
MERGE
  (installment)-[:BELONGS]->(account)
MERGE
  (transaction)-[:IS_LINKED]->(installment)
WITH
  account,
  payee,
  coordinates,
  category,
  currency,
  paymentMethod,
  transaction,
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
