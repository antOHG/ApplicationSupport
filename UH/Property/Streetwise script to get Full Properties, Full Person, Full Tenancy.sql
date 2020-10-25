--OHG2K5SQL01
use uh630live

--1. QUERY TO PULL FULL -PROPERTY LIST
--HEADER
--Prop_ID,Disposal_DT,AddressLine1,AddressLine2,City,County,PostCode,Y_COORD,X_COORD,Longitude,Latitude,Locality4,Locality3,Locality2,Locality1,Dwelling_code
select
[HMS Property ID] as Prop_ID
, [Dispose Date] as Disposal_DT, ltrim(rtrim([Address Line1])) as AddressLine1, ltrim(rtrim([Address Line2])) as AddressLine2, 
City, p.County, p.[Post Code] as PostCode, 
Northing as Y_COORD, Easting as X_COORD, 
pc.Longitude, pc.Latitude, Locality4, Locality3, Locality2, Locality1, DwellingCode as 'Dwelling_Code'
from ohgproddat01.ASBStreetwise.dbo.Property p inner join mark.dbo.PostCode pc on p.[Post Code]=pc.Postcode collate database_default

--2. QUERY TO PULL FULL -TENANCY LIST
--HEADER
--Tenancy_ID,Start_Date,End_Date,PropertyID
select
 [HMS Tenancy Id] AS 'Tenancy_ID'
,[HMS Tenancy Start Date] AS 'Start_Date'
,[HMS Tenancy End Date] AS End_Date
,[HMS Property ID] AS 'PropertyID'
from ohgproddat01.ASBStreetwise.dbo.Tenancy

--3. QUERY TO PULL FULL -PERSON LIST
--HEADER
--PersonID,FirstName,LastName,Tenancy_ID,Occupancy_StartDT,Occupancy_EndDT
select
PersonID, FirstName, LastName, Tenancy_ID, Occupancy_StartDT, Occupancy_EndDT
from ohgproddat01.ASBStreetwise.dbo.Person

/*
NOTE:
Saving as report file:
1. Right click on the Results
2. Click on 'Save Result As...'
3. Give the appropriate name, ex: FullPropertyList.csv
4. Open the file in the NOTEPAD++ and check if the headers are included
5. If headers not available, follow the steps to include
	Open SSMS.
	Click Tools > Options
	In the Options dialog, expand Query Results > SQL Server > Results to Grid, then check "Include column headers when copying or saving the results"
	Click OK.
	Close & RESTART SSMS.
	Ref: https://stackoverflow.com/questions/10677133/saving-results-with-headers-in-sql-server-management-studio
6. Now replace all NULL with empty space from all the 3 files
7. Re-save and send it to Streetwise email address
*/


