select '' as Ref, 'GBP' as [Default Currency Code], sup_name as Name, sup_ref as [Search Name], post_desig as [Building Name], 
address as Address, post_code as Postcode, '' as [VAT Prefix], '' as [VAT Reg No], '' as [Registered Company No]
from supplier

SELECT     a.u_estate AS Ref, l.lu_desc AS Name, '' as  [Search Name], '' AS [Manager Code], 'GBP' AS [Default Currency Code]
FROM         property AS a INNER JOIN
                          (SELECT     lu_ref, lu_type, lu_desc
                            FROM          lookup
                            WHERE      (lu_type = 'uES')) AS l ON a.u_estate = l.lu_ref
WHERE     (a.level_code = 1)

