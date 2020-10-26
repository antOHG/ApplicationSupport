select 
       t.house_ref as 'PersonID', 
       m.forename as 'FirstName', 
       m.surname as 'LastName',

       tag_ref as 'Tenancy_ID',
       cot          as 'Start_Date',
       eot          as 'End_Date'

from 
tenagree t left outer join member m on t.house_ref = m.house_ref
join TEMP_ASB_PROPERTY p on p.[PropertyRef] = prop_ref
where eot = '19000101'
AND m.relationship = 'C'