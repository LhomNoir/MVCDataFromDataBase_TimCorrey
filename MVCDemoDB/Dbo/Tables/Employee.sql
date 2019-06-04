CREATE TABLE [dbo].[Employee]
(
	[Id] INT NOT NULL PRIMARY KEY Identity, 
    [EmployeeId] INT NOT NULL, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [EmailAdress] NVARCHAR(50) NOT NULL
)
