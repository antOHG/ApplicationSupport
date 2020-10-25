select * from property where prop_ref = 'XWYNFOR20724'
select * from property where prop_ref = 'XWYNFOR20730'

update property 
set voidman_live = '1'
where prop_ref = 'XWYNFOR20724'

select * from vmrecord where vm_propref = 'XWYNFOR20724/005'

update vmrecord 
set vm_complete = '0'
where vm_propref = 'XWYNFOR20724/005'