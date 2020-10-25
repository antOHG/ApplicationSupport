select 

prop_ref, 
l2.lu_desc as 'Property Type', 
short_address, arr_officer as 'arrears officer code', 
l1.lu_desc as 'Arrears Officer', 
p.housing_officer as 'housing officer code', 
l3.lu_desc as 'Housing Officer'  

from property p

left outer join lookup l1 on l1.lu_ref = p.arr_officer and l1.lu_type = 'AOF' 
left outer join lulevel l2 on l2.lu_ref = p.level_code
left outer join lookup l3 on l3.lu_ref = p.housing_officer and l3.lu_type = 'GHO'

where p.arr_officer = '030' and maintresp in ('LSE', 'SO') and level_code = '3'







