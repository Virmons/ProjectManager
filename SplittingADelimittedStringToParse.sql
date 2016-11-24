CREATE PROCEDURE dbo.usp_SprintsGetByTaskID

	@TaskIDList		VARCHAR(MAX)

AS
BEGIN

	SET NOCOUNT ON 

	SELECT st.ID "ID", st.SprintID "SprintID", st.TaskID "TaskID", st.Active "Active"
	FROM SprintTask st
	JOIN Task t ON st.TaskID = t.ID
	WHERE t.ID IN (Select convert(INT,VALUE) FROM dbo.ufncSplit('1,2,3',','))

END