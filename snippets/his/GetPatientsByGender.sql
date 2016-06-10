CREATE PROCEDURE [dbo].[GetPatientsByGender]
	@Gender varchar(255)
AS
	SELECT * FROM [dbo].[Patient]
	WHERE [Gender] = @Gender
RETURN 0
