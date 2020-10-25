SELECT     '' AS Ref, rtrim (c.title) + ' ' + RTRIM(c.initials) + ' ' + RTRIM(c.forename) + ' ' + RTRIM(c.surname) AS Name, d.house_ref AS [Search Name], RTRIM(c.title) + ' ' + RTRIM(c.forename) + ' ' + RTRIM(c.surname) 
                      AS [Contact Name], RTRIM(d.house_ref) + '/0' + RTRIM(c.person_no) AS [Tenant Parent Ref], a.post_preamble AS [Building Name], a.post_desig AS [Road Number], 
                      b.aline1 AS [Road Name 1], b.aline2 AS [Road Name 2], b.aline3 AS Town, b.aline4 AS County, b.post_code AS Postcode, '' AS [Credit Rating], e.Telephone AS Phone, 
                      '' AS [Direct Phone], e.Mobile AS [Mobile Phone], '' AS Fax, e.Email
FROM         property AS a INNER JOIN
                      member AS c ON a.house_ref = c.house_ref INNER JOIN
                      postcode AS b ON a.post_code = b.post_code INNER JOIN
                      househ AS d ON a.prop_ref = d.prop_ref INNER JOIN
                          (SELECT con_key, con_ref, con_name, short_address, Telephone, Mobile, Email, Other
                            FROM          alan.dbo.contact_data_b) AS e ON c.house_ref = e.con_ref
ORDER BY [Search Name] desc
                            
                            ---duplicate numbers in contact_data_b causing duplicates in this sql need to look at this.