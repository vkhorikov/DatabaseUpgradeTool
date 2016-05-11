CREATE TABLE [dbo].[User] (
    [UserID]	   BIGINT         NOT NULL,
    [Name]		   NVARCHAR (200) NOT NULL,
    [Email]        NVARCHAR (256) NOT NULL,
    [Status]       NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC)
);
