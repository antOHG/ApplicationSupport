-- Add new/update existing property attributes --

begin tran
update propattr 
set 
att_ref = '001',
att_loc = '000',
att_quantity = '1',
att_isactive = '1',
att_size = '0.00',
att_type = '0',
u_resp_sup = 'CSL' -- Responsible supplier
where prop_ref in 
('DAGMCT005600') and att_ref = '001'

commit




-- Removing property attributes by type --


begin tran

select * from propattr where propattr.prop_ref = 'REPLACE'

delete from propattr where propattr.prop_ref = 'REPLACE' and att_ref = 'REPLACE'

select * from propattr where propattr.prop_ref = 'REPLACE'

ROLLBACK

-- Gas = '001'
-- Heating = '054'


-- CHECK IF propattr matches property table --


SELECT property.prop_ref, property.u_gasinprop AS PROPERTY_u_gasinprop, propattr.att_ref AS PROPATTR_att_ref, propattr.att_isactive AS PROPATTR_att_isactive, 
               property.u_gas_ohgresp AS PROPERTY_u_gas_ohgresp, propattr.tenant_maint AS PROPATTR_OHG_resp, 
               property.u_gas_maresp AS PROPERTY_u_gas_maresp, propattr.tenant_addition AS PAManagingAgentResp, 
               property.u_last_servicedate AS PROPERTY_u_last_servicedate, propattr.last_service AS PROPATTR_last_service, 
               property.u_next_servicedate AS PROPERTY_u_next_servicedate, propattr.next_service AS PROPATTR_next_service, 
               property.u_resp_sup AS PROPERTY_u_resp_sup, propattr.u_resp_sup AS PROPATTR_u_resp_sup, 
               property.u_heathotw_notes AS PROPERTY_u_heathotw_notes, propattr.u_heathotw_notes AS PROPATTR_u_heathotw_notes, 
               property.u_gas_notes AS PROPERTY_u_gas_notes, propattr.u_gas_notes AS PROPATTR_u_gas_notes
FROM  property FULL OUTER JOIN
               propattr ON property.prop_ref = propattr.prop_ref AND propattr.att_ref = '001'
WHERE (property.housing_officer <> '950') AND (property.u_next_servicedate <> '') and property.prop_ref = 'DAGMCT005600'