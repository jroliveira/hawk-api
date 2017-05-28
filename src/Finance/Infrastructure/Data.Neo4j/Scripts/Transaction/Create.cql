MATCH 
  (account:Account)
WHERE
  account.email = {email}

CREATE 
  (transaction:Transaction:#type# { 
    value: {value},
    date: {date},
    email: {email},
    parcels: {parcels},
    parcel: {parcel}
  })

CREATE UNIQUE
  (account)-[:HAS]->(transaction)

RETURN 
  id(transaction) as id;