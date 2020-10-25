
select * from debitem where prop_ref = 'EMILY0000056' and debitem_sid in (67855731,67855733,67855732)
order by eff_date
select * from #debitemtemp


select * into #debitemtemp from debitem
where prop_ref = 'EMILY0000056'

begin tran
delete from debitem where prop_ref = 'EMILY0000056' and debitem_sid in (67855731,67855733,67855732)
commit


begin tran
INSERT debitem
(prop_ref,	
tag_ref,	
deb_code,	
eff_date,	
term_date,	
prd_code,	
deb_next_due,	
deb_last_charge,	
deb_days,	
deb_active,	
deb_value,	
prop_deb,	
deb_source,	
actual_eff,	
actual_term,	
sponsor_ref,	
deb_trans,	
debitem_sid,	
serv_code,	
debmult,	
extdebvalue,	
debrate,	
u_debcomments,	
comp_avail,	
comp_display,	
sb_storage_no,	
sb_container,	
sc_schedule)

SELECT prop_ref,	
tag_ref,	
deb_code,	
eff_date,	
term_date,	
prd_code,	
deb_next_due,	
deb_last_charge,	
deb_days,	
deb_active,	
deb_value,	
prop_deb,	
deb_source,	
actual_eff,	
actual_term,	
sponsor_ref,	
deb_trans,	
debitem_sid,	
serv_code,	
debmult,	
extdebvalue,	
debrate,	
u_debcomments,	
comp_avail,	
comp_display,	
sb_storage_no,	
sb_container,	
sc_schedule FROM #debitemtemp
WHERE prop_ref = 'EMILY0000056' and debitem_sid in (67855731,67855733,67855732)

commit

drop table #debitemtemp