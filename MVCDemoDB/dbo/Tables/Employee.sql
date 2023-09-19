CREATE TABLE [dbo].[Employee]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [EmployeeId] INT NOT NULL UNIQUE, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(100) NOT NULL
)
