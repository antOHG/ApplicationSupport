

select distinct p.prop_ref, 
--t.agr_type, 
--t.tag_ref, 
p.post_desig, p.post_preamble, pc.aline1, pc.aline2, pc.aline3, pc.post_code, 
h.house_desc, t.cot, m.dob, r.perm_no, 
r.occ_status, l1.lu_desc as 'housing officer', l2.lu_desc as 'Arrears Officer'
from tenagree t
left outer join househ h on t.house_ref = h.house_ref
left outer join member m on m.house_ref = h.house_ref
left outer join property p on t.prop_ref = p.prop_ref
left outer join rent r on t.prop_ref = r.prop_ref
left outer join postcode pc on pc.post_code = p.post_code
left outer join lookup l1 on l1.lu_ref = p.housing_officer and l1.lu_type = 'GHO'
left outer join lookup l2 on l2.lu_ref = p.arr_officer and l2.lu_type = 'AOF'
where p.short_address like '%Oval%' 


select * from member
select * from househ
select * from postcode
select * from property
select * from lookup where lu_desc like '%Out of Mgmnt%'
select * from rent

current ten, all memebers, full address, ho, ao

select * from househ


select distinct p.prop_ref
from tenagree t
left outer join property p on t.prop_ref = p.prop_ref
where p.short_address like '%Argyle%'