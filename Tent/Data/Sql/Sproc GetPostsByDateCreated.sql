CREATE PROCEDURE GetPostsByDateCreated (
	@DateCreated DateTime
) AS BEGIN
	SELECT * FROM Posts
	WHERE DateCreated > @DateCreated
END
GO