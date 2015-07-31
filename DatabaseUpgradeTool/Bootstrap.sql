CREATE TABLE dbo.Settings
(
	Name nvarchar(50) NOT NULL PRIMARY KEY,
	Value nvarchar(500) NOT NULL
)

INSERT dbo.Settings (Name, Value)
VALUES ('Version', '0')
