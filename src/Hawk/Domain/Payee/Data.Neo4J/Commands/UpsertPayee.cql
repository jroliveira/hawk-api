// (:Account)

MATCH 
  (account:Account)
WHERE
  account.email = $email

// (:Payee)

MERGE 
  (payee:Payee { 
    name: $name
  })
SET
  payee.name = $newName
MERGE
  (payee)-[:BELONGS]->(account)
WITH
  account,
  payee

// (:Coordinate)

OPTIONAL MATCH
  (payee)-[at:AT]->(:Coordinate)
DELETE
  at
WITH
  account,
  payee  

UNWIND
  $coordinates AS coordinateParam
MERGE
  (coordinate:Coordinate { 
    latitude: coordinateParam.latitude,
    longitude: coordinateParam.longitude
  })
MERGE
  (payee)-[:AT]->(coordinate)
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
