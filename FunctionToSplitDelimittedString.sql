USE [SSTest]
GO
/****** Object:  UserDefinedFunction [dbo].[ufncSplit]    Script Date: 24/11/2016 11:21:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[ufncSplit]
(
	@List VARCHAR(2000),
	@SplitOn VARCHAR(5)
)  
RETURNS @ReturnValue TABLE 
(
		
	ID INT IDENTITY(1,1),
	VALUE VARCHAR(100)
) 
AS  
BEGIN
	WHILE (CHARINDEX(@SplitOn,@List)>0)
	BEGIN
		INSERT INTO @ReturnValue (VALUE)
		SELECT VALUE  = LTRIM(RTRIM(SUBSTRING(@List,1,CHARINDEX(@SplitOn,@List)-1)))
		SET @List = SUBSTRING(@List,CHARINDEX(@SplitOn,@List)+LEN(@SplitOn),LEN(@List))
	END
    INSERT INTO @ReturnValue (VALUE)
    SELECT VALUE = LTRIM(RTRIM(@List))

    RETURN
END