MATCH 
  (installment:Installment)-[:BELONGS]->(account)
WHERE
  account.email = $email
  AND installment.id = $id
WITH
  account,
  installment

RETURN 
{ 
  id: installment.id,
  frequency: installment.frequency,
  installments: {
    total: CASE WHEN installment.total IS null THEN 0 ELSE installment.total END,
    current: SIZE((:Transaction)-[:IS_LINKED]->(installment))
  }
} AS data;
