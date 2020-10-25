begin tran
select * from propattr where prop_ref = 'JUNIPE000024'
insert into propattr 
(prop_ref, 
att_ref,
att_group, --should be 001 as it is presumed that it is a heating appliance
next_service, --this is (+1 year)-1 day from last_service
last_service, --this is taken from the gas safe certificate
tenant_maint, -- 1 if OHG responsible, 0 if OHG not responsible
tenant_addition, -- 1 if managing agent responsible, 0 if managing agent not responsible
--propattr_sid, --this should auto-populate as it does when entered via the front end *
--tstamp, --this should auto-populate as it does when entered via the front end *
asb_hep_num, -- set as 100, unsure why but this is the default value entered when added via the front end
comp_avail, -- set as 001
u_resp_sup, -- set as ODML if ODML (Gas), otherwise choose from the Supplier table, only populate if you have this data
att_isactive, -- set as 1 
dtstamp) -- set as getdate(), this acts as a semi-audit of when your team added the attribute)
Values('JUNIPE000024','001','001', '2019-12-10 00:00:00','2018-12-11 00:00:00', '1', '0', '100', '001', 'ODM', '1', getdate())
select * from propattr where prop_ref = 'JUNIPE000024'
Commit
