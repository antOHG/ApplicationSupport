USE uh630live

SELECT
	LTRIM(RTRIM(p.prop_ref)) as 'prop ref',
	LTRIM(RTRIM(p.major_ref)) as 'major ref',
	LTRIM(RTRIM(p.short_address)) as 'short address',
	LTRIM(RTRIM(p.post_code)) as 'post code',
	(case when CONVERT(VARCHAR(10), p.handover, 103) = '01/01/1900' then '' else CONVERT(VARCHAR(10), p.handover, 104) end) as 'handover',
	COALESCE(LTRIM(RTRIM(te.house_desc)), '') as 'tenant',
	LTRIM(RTRIM(os.occ_desc)) as 'occ status',
	LTRIM(RTRIM(lc.lu_desc)) as 'company',
	LTRIM(RTRIM(lr.lu_desc)) as 'region',
	LTRIM(RTRIM(ll.lu_desc)) as 'proptype',
	LTRIM(RTRIM(pt.pt_prop_desc)) as 'bus stream',
	COALESCE(LTRIM(RTRIM(ls.lu_desc)), '') as 'prop subtype',
	LTRIM(RTRIM(la.la_name2)) as 'la',
	'UNKNOWN' as 'bus stream group',
	(case when CONVERT(VARCHAR(10), p.u_last_servicedate, 103) = '01/01/1900' then '' else CONVERT(VARCHAR(10), p.u_last_servicedate, 104) end) as 'last service',
	(case when CONVERT(VARCHAR(10), p.u_next_servicedate, 103) = '01/01/1900' then '' else CONVERT(VARCHAR(10), p.u_next_servicedate, 104) end) as 'next service',
	COALESCE(LTRIM(RTRIM(mc.own_managed_cat)), '') as 'own managed cat',
	(case when CONVERT(VARCHAR(10), p.u_build_date, 103) = '01/01/1900' then '' else CONVERT(VARCHAR(10), p.u_build_date, 104) end) as 'build date',
	LTRIM(RTRIM(p.prop_ref)) as 'uprn',
	te.telephone,
	(case when CONVERT(VARCHAR(10), te.DateTenantMovedintoProperty, 103) = '01/01/1900' then '' else CONVERT(VARCHAR(10), te.DateTenantMovedintoProperty, 104) end) as 'Date Tenant Moved into Property',
	'Not Known' as propertydescription,
	'Not Known' as ListedORProtected,
	'Not Known' as SpecificAreasofNoInterest
FROM property p
LEFT JOIN occstat os ON p.occ_stat = os.occ_status
LEFT JOIN lookup lr ON lr.lu_ref = p.region
LEFT JOIN lookup lc ON lc.lu_ref = p.company
LEFT JOIN
	(SELECT lu_ref, lu_desc
	FROM lookup 
	WHERE lu_type ='PST' and lu_desc not like '\%'
	) ls ON ls.lu_ref = p.subtyp_code
LEFT JOIN lulevel ll ON ll.lu_ref = p.level_code
LEFT JOIN proptype pt ON pt.pt_prop_code = p.cat_type
LEFT JOIN lauth la ON la.la_ref = p.la_ref
LEFT JOIN u_own_managed_category mc ON mc.prop_ref = p.prop_ref 
LEFT JOIN 
	(SELECT t.prop_ref, hh.house_desc,hh.telephone,t.cot as DateTenantMovedintoProperty,r.build_type
	 FROM tenagree t
	 JOIN rent r ON t.prop_ref = r.prop_ref
	 JOIN househ hh ON hh.house_ref = r.house_ref
	 WHERE t.eot = '1900-01-01 00:00:00') te ON p.prop_ref = te.prop_ref

WHERE lr.lu_type = 'uRE' and lr.lu_desc not like '\%'
AND lc.lu_type = 'uCM' and lc.lu_desc not like '\%'
AND la.la_name not like '\%'
ORDER BY p.prop_ref


