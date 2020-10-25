--1) Company
Truncate table scratch..bluebox_company
INSERT INTO scratch..bluebox_company
Select '' as Ref, 'GBP' as Default_Currency_Code,  lu_desc as Name, lu_desc as Search_Name, 
'' as Building_Name, '' as Road_Number, '' as Road_Name_1, '' as Road_Name_2,
'' as Town, '' as County,  '' as Postcode, '' as VAT_Prefix,
'' as VAT_Reg_No, '' as Registered_Company_No 
from lookup where lu_type ='uCM'
and lu_desc not like '\%'

--2) Estate


TRUNCATE table  scratch..bluebox_estate
INSERT into scratch..bluebox_estate
Select '' as Ref, rtrim(lu_desc) as Name, lu_desc as Search_Name, '' as Manager_Code, 'GBP' as Default_Currency_Code 
from lookup where lu_type ='uES'

--3) Property

TRUNCATE TABLE    scratch..bluebox_property
INSERT into scratch..bluebox_property
SELECT   '' AS Ref, '' AS [Company Ref], '' AS [Manager Code], 'GBP' AS [Default Currency Code], '' AS Name, a.major_ref AS [Search Name], '' AS [Service Charges], 
                      '' AS Tenure, a.post_preamble AS [Building Name], a.post_desig AS [Road Number], b.aline1 AS [Road Name 1], b.aline2 AS [Road Name 2], b.aline3 AS Town, 
                      b.aline4 AS County, b.post_code AS Postcode, a.region AS [Location Code], a.la_ref AS [Rating Authority Ref], a.la_ref AS [Planning Authority Ref],'' AS [Listed Building], '' AS [Management Start Date], 
                      '' AS [Management End Date], a.u_build_date AS [Year Constructed], a.handover AS [Year Last Furnished], --MAX(c.floor_level) AS [No of Floors], 
                      CASE WHEN a.asbestos = '1' THEN 'Yes' WHEN a.asbestos = '0' THEN 'No' ELSE 'Unknown' END AS [Asbestos Present], '' AS [Energy Efficiency Rating], 
                      agent AS [Management Company for S/C], agent AS [Mangement Company Client Ref], '100' AS [Main Client Interest], 'N' AS [VAT Opted], 
                    --  vmrecord.vm_propcond_stt as   [Property Status Code], 
                      occ_stat as [Property Status As At], a.u_estate as [Estate Ref], '' as [HA Earnings County Code]
FROM         property AS a INNER JOIN
                     -- rent AS c ON a.prop_ref = c.prop_ref INNER JOIN
                    vmrecord ON a.prop_ref = vmrecord.prop_ref LEFT OUTER JOIN
                      postcode AS b ON a.post_code = b.post_code
WHERE     (a.level_code = '2')

--4) Creditor

Select * from property where level_code = 1 and arr_officer <>'950' and u_estate <>''