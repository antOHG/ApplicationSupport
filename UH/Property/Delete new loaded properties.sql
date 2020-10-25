
sumv
vmrecord
vmstat
rent
property

begin tran

delete from -- insert table here -- 
where prop_ref in ('WALLIN000001',
'WALLIN000002',
'WALLIN000003',
'WALLIN000004',
'WALLIN000005',
'WALLIN000006',
'WALLIN000000'
)

commit tran 

select * from rent where prop_ref = 'WALLIN000001'
select * from sumv

