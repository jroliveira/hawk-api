MATCH 
  (payee:Payee)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND payee.name = $name
WITH
  account,
  payee

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
} AS data;
