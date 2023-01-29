CREATE TABLE VerificationCode (
	Id INT PRIMARY KEY IDENTITY(1,1),
	UserId INT FOREIGN KEY REFERENCES [User](Id),
	Code VARCHAR(62) NOT NULL,
	DateCreated DATETIME NOT NULL DEFAULT GETDATE(),
	DateExpires DATETIME NOT NULL,
	IsReset BIT NOT NULL DEFAULT 0,
	DateReset DATETIME
)