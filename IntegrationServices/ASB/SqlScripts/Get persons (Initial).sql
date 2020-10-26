SELECT DISTINCT
LTRIM(RTRIM(m.house_ref))     AS 'PersonID',
m.forename                                                                  AS 'FirstName', 
m.surname                                                                     AS 'LastName',
tag_ref                                                                                             AS 'Tenancy_ID',
cot                                                                                                      AS 'Start_Date',
eot                                                                                                     AS 'End_Date'
--p.prop_ref                                                                      AS 'PropertyID' 

FROM 
member m 
INNER JOIN tenagree t on t.house_ref = m.house_ref
INNER JOIN property p on t.prop_ref = p.prop_ref
INNER JOIN TEMP_ASB_PROPERTY p2 on p2.[PropertyRef] = p.prop_ref --this is not required any more.
WHERE 
t.eot = '19000101'
AND p.level_code = 3
AND m.relationship <> 'C'
AND m.responsible = 1
