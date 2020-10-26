SELECT 
      DISTINCT 
            RTRIM(LTRIM(P.prop_ref)) AS [HMS Property ID], 
         CASE WHEN P.u_dispose_date = '1900-01-01 00:00:00' THEN NULL WHEN P.u_dispose_date = NULL THEN NULL ELSE CONVERT(DATETIME,P.u_dispose_date,103) 

END AS [Dispose Date], 
            REPLACE(LTRIM(RTRIM(P.post_desig)), ',', ' ') + ' ' + REPLACE(LTRIM(RTRIM(P.post_preamble)), ',', ' ') + ' ' +  REPLACE(LTRIM(RTRIM

(PO.aline1)), ',', ' ') AS [Address Line1], 
            REPLACE(LTRIM(RTRIM(PO.aline2)), ',', ' ') AS [Address Line2], 
            CASE WHEN PO.aline3 <> '' THEN PO.aline3 ELSE PO.aline4 END AS City, 
            CASE WHEN PO.aline4 = '' THEN PO.aline3 ELSE PO.aline4 END AS County, 
            LTRIM(RTRIM(PO.post_code)) AS [Post Code],
			'' as Y_COORD,
			'' as X_COORD,
			'' as Longitude,
			'' as Latitude,
            '' AS Locality4, 
			'' AS Locality3,
			LTRIM(RTRIM(P.la_ref)) AS Locality2, 
			LTRIM(RTRIM(P.region)) AS Locality1, 
            level_code AS DwellingCode
FROM    
        dbo.property P 
        INNER JOIN dbo.postcode PO ON P.post_code = PO.post_code 
	INNER JOIN TEMP_ASB_PROPERTY p2 on p2.[PropertyRef] = P.prop_ref 
        WHERE P.prop_ref not in('KIDWEL006900') 
        AND P.level_code =3  AND P.arr_officer !=950
		
 