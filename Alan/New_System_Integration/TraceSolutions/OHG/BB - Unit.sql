SELECT     a.prop_ref, '' AS Ref, '' AS Name, '' AS [Manager Code], d.house_desc AS [Address Line 1], a.post_preamble AS [Building Name], a.post_desig AS [Road Number], 
                      b.aline1 AS [Road Name 1], b.aline2 AS [Road Name 2], b.aline3 AS Town, b.aline4 AS County, b.post_code AS Postcode, '' AS [Search Name], c.tenure AS Tenure, 
                      '' AS [Whole of Property], a.cat_type AS [Property Type Code], '' AS [Location Code], '' AS [Unit Group Code], '' AS [Property Analysis Code], 
                      '' AS [Rating Authority Code], '' AS [Planning Authority Ref], '' AS [Main Use Class], '' AS [Sub Use Class], '' AS [Listed Building], '' AS [Management Start Date], 
                      '' AS [Management End Date], a.u_build_date AS [Year Constructed], a.handover AS [Year Last Refurbished], '' AS [Obsolete Unit], a.prop_ref AS UPRN, '' AS [OS Ref], 
                      '' AS [OS Point Address Ref], '' AS [Easting/Northing], '' AS [VAT Opted], '' AS [Our File Ref], '' AS [Ext File Reference], c.floor_level AS [No of Floors], 
                      '' AS [Number of Workstations], '' AS [Head Count], '' AS Description, '' AS [Photo File name], 
                      CASE WHEN a.cat_type = 'LH1' THEN 'Yes' ELSE 'No' END AS [Shared Ownership], c.sharedowner as [% Owned by Tenant], c.perm_no as [Permitted Number of People],
                      '' as [HA Earnings County], c.no_beds as [Number of Bedrooms], c.valuation as [Initial Valuation]
FROM         property AS a INNER JOIN
                      postcode AS b ON a.post_code = b.post_code INNER JOIN
                      rent AS c ON a.prop_ref = c.prop_ref INNER JOIN
                      househ AS d ON a.prop_ref = d.prop_ref
                      where a.level_code = 3