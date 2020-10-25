begin tran
insert into propattr (prop_ref
,att_ref
,att_group
,att_location
,att_quantity
,att_size
,att_loc
,att_cond
,att_modelno
,att_serialno
,life_left
,timeunit
,last_survey
,next_repl
,next_service
,last_repl
,last_service
,serv_defto
,repl_defto
,repl_sup_ref
,guarantee_to
,serv_sup_ref
,att_notes
,con_reason
,serv_liable
,last_scost
,last_rcost
,serv_sched
,repl_sched
,tenant_maint
,tenant_addition
,tag_ref
,guarnteedet
,auto_cascade
,inherit
,maintresp
,next_addi
,last_addi
,addi_sup_ref
,addi_defto
,addi_guar_to
,last_acost
,addi_sched
,addi_guar_det
,removal_date
,last_removed
,remov_sup_ref
,remov_defto
,last_dcost
,remov_sched
,att_type
,asb_status
,asb_ptype_score
,asb_ptype_desc
,att_damage_det
,asb_streat_score
,asb_streat_desc
,asb_type_score
,asb_type_desc
,asb_rating
,asb_pfrelease
,asb_date_insp
,asb_surveyed_by
,asb_sample_taken
,asb_sample_sent_to
,asb_result_rec
,asb_res_comm
,asb_norm_act
,asb_secd_act
,asb_lod_loc
,asb_lod_acc
,asb_lod_amt
,asb_hep_num
,asb_hep_freq
,asb_hep_time
,asb_ma_type
,asb_ma_freq
,asb_priority_total
,asb_risk_total
,asb_act_avg
,asb_lod_avg
,asb_hep_avg
,asb_ma_avg
,comp_avail
,comp_display
,att_isactive
,att_custom_grp
,att_priorating_det
,att_actiontaken_dt
,att_damage_lrf
,att_priorating_lrf
,att_recommend_lrf
,att_actiontaken_lrf
,u_prev_serv
,manufacturer_sid
,manufacturer_ref
,warranty_prd
,u_gas_notes
,u_resp_sup
,u_heathotw_notes
,u_eng_name
,u_gas_saferegno
,u_gas_safelicno
,u_prop_hasgas
,u_gas_supplytype
,u_gas_supplydetails
,u_prop_cookapp
,u_prop_nongascookapp
,u_total_landlordapps
,u_total_tenantapps
,u_co2_detector
,u_mains_battery
,u_date_installed
,u_test_result
,u_stored_water
,u_coldwater_temp
,u_hotwater_temp
,u_system_type,
dtstamp)
VALUES
('COMMER000583'
,'054'   
,'001'
,''
,1.00
,0.00
,''
,''
,''
,''
,0
,''
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,''
,'1900-01-01 00:00:00'
,''
,''
,''
,'1900-01-01 00:00:00'
,0.00
,0.00
,0              
,''
,0
,0
,''
,''
,0
,0
,''
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,''
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,0.00
,''
,''
,'1900-01-01 00:00:00'
,'1900-01-01 00:00:00'
,''
,'1900-01-01 00:00:00'
,0.00
,''
,0
,''
,''
,''
,''
,''
,''
,''
,''
,0
,''
,'1900-01-01 00:00:00'
,''
,0
,''
,'1900-01-01 00:00:00'
,''
,''
,''
,''
,0  
,''
,''
,''
,''
,''
,''
,0
,0
,0
,0
,0
,0
,001                                                                                                                                                                                                     
,''
,1
,''
,''
,'1900-01-01 00:00:00'
,''
,''
,''
,''
,'1900-01-01 00:00:00.000'
,NULL
,NULL
,0
,''
,''
,''
,''
,''
,''
,''
,''
,''
,''
,''
,NULL
,NULL
,''
,''
,NULL
,''
,''
,NULL
,NULL
,'', GETDATE())


select dtstamp, * from propattr WHERE prop_ref like 'COMMER000583'

ROLLBACK

