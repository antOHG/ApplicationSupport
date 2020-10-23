select * from househ where house_ref = '********'  

begin tran
update househ
set u_awareness = '033'
where house_ref = 'H0014638'
commit

-- use the following select to find the alert required
select * from lookup where lu_type = 'uAW' 