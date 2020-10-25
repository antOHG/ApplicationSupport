-- This script will pull back the avaliable permissions for each security group. 
-- There are 4 permission avaliable (Add, Edit, Del and ex) ex is the avalibility to see that screen/programme in UH.
-- to run just replace all mentions of the above with the quere you wish to return. 


DECLARE @option_id varchar(20) SET @option_id=''
DECLARE @permission varchar(10) SET @permission=''
DECLARE @Temp TABLE (option_id varchar(20), usertype varchar(10), permission varchar(10))


DECLARE CrsOrfLine CURSOR FOR
SELECT DISTINCT id FROM approg 
 ORDER BY id

OPEN CrsOrfLine
FETCH NEXT FROM CrsOrfLine INTO  @option_id
WHILE (@@FETCH_STATUS = 0) 
BEGIN
     INSERT INTO @Temp 
        select @option_id AS option_desc, a.usertype, 'Avail' as permission from usertype a 
     where usertype not in (select Data1 from (  SELECT  
              a.id,
              a.option_desc,
              SplitText.Data as 'Data1'
  FROM  approg a
  CROSS APPLY
        dbo.SplitText(a.exgroup, ',') as SplitText
  WHERE a.exgroup <> '') b where b.id = @option_id) and (Select exgroup from approg where id = @option_id) <> ''

FETCH NEXT FROM CrsOrfLine INTO @option_id
END /*WHILE*/
CLOSE CrsOrfLine
DEALLOCATE CrsOrfLine

select * from @Temp
