SELECT DISTINCT
       LTRIM(RTRIM(tag_ref))	AS 'Tenancy_ID',
       cot      AS 'Start_Date',
       eot      AS 'End_Date',
       t.prop_ref AS 'PropertyID'       

FROM 
tenagree t
INNER JOIN property p ON t.prop_ref = p.prop_ref
INNER JOIN TEMP_ASB_PROPERTY p2 on p2.[PropertyRef] = p.prop_ref 
WHERE 
eot = '19000101'
AND p.level_code = 3