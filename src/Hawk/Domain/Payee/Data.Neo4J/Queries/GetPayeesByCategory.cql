MATCH 
  (transaction:Transaction)-[:BELONGS]->(account:Account)
MATCH 
  (transaction)-[:IN]->(payee:Payee)-[:BELONGS]->(account)
MATCH
  (transaction)-[:WITH]->(category:Category)-[:BELONGS]->(account)
WHERE
  account.email = $email
  AND category.name = $category
WITH
  DISTINCT payee

OPTIONAL MATCH
  (payee)-[:AT]->(coordinate:Coordinate)
WITH
  account,
  payee,
  COLLECT(DISTINCT coordinate) AS coordinates

RETURN
{ 
  name: payee.name,
  location: {
    coordinates: [coordinate IN coordinates | {
      latitude: coordinate.latitude,
      longitude: coordinate.longitude
    }]
  },
  transactions: SIZE((:Transaction)-[:IN]->(payee))
} AS data

ORDER BY
  payee.name

SKIP 
  $skip 

LIMIT 
  $limit;
