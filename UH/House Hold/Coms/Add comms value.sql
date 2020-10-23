select con_key, * from tenagree
where house_ref in ('H0016470')

select con_key
from tenagree
where tag_ref = 'H0013962/01'

begin tran
insert into comms (comms_type, comms_value, comms_desc, comms_advocate, comms_default, con_key, entity_table, comms_default_contact, comms_default_method, comp_avail)
values ('T', '02079873814', 'Mr K Nicholson', '0', '0','75425', 'househ', '0', '0', '001')  
begin tran
insert into comms (comms_type, comms_value, comms_desc, comms_advocate, comms_default, con_key, entity_table, comms_default_contact, comms_default_method, comp_avail)
values ('MP', '07800514629', 'Mr K Nicholson', '0', '0','75425', 'househ', '0', '0', '001')  

commit tran

rollback tran


