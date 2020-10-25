select * from sumv where prop_ref = 'FINWHS000700'

begin tran
update sumv
set occ_stat = 'VMR'
where prop_ref = 'FINWHS000700' and occ_phase = '7'
commit tran

--Also need to update vm records, the vm propref can be found from the void monitoring screen in UH searching by propref 
select * from vmrecord where vm_propref = 'FINWHS000700/002'
-- use the below to update required status
begin tran
update vmrecord
set vm_occstat = 'VMR'
where vm_propref = 'FINWHS000700/002' 

commit tran

begin tran
update vmrecord
set vm_propcond_stt = '350'
where vm_propref = 'FINWHS000700/002' 

commit tran

begin tran
update vmrecord
set vm_alloc_stt = '500'
where vm_propref = 'FINWHS000700/002' 

commit tran


--look up for occ status codes
select * from occstat
select * from vmoccmat

select * from lookup where lu_type = 'PRO' -- look up codes for property condition status
select * from lookup where lu_type = 'ALL' -- look up codes for allocation status status

