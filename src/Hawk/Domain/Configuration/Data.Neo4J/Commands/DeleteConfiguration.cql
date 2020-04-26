MATCH 
  (configuration:Configuration)-[:BELONGS]->(account:Account)
WHERE
  account.email = $email
  AND configuration.description = $description

DETACH DELETE configuration;
