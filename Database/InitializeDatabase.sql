-- Create Database (Run this separately if database doesn't exist)
-- CREATE DATABASE PersonalPortal;
-- GO
-- USE PersonalPortal;
-- GO

-- Users Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL UNIQUE,
        Password NVARCHAR(255) NOT NULL,
        Role NVARCHAR(50) NOT NULL,
        DisplayName NVARCHAR(255) NOT NULL
    );
END
GO

-- TextNotes Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TextNotes')
BEGIN
    CREATE TABLE TextNotes (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(255) NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        Created DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Updated DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- Checklists Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Checklists')
BEGIN
    CREATE TABLE Checklists (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(255) NOT NULL,
        Type NVARCHAR(100) NOT NULL,
        Created DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Updated DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- ChecklistItems Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChecklistItems')
BEGIN
    CREATE TABLE ChecklistItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ChecklistId UNIQUEIDENTIFIER NOT NULL,
        ItemName NVARCHAR(255) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        FOREIGN KEY (ChecklistId) REFERENCES Checklists(Id) ON DELETE CASCADE
    );
END
GO

-- Add ItemGroup column to ChecklistItems if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ChecklistItems') AND name = 'ItemGroup')
BEGIN
    ALTER TABLE ChecklistItems
    ADD ItemGroup NVARCHAR(100) NULL;
END
GO

-- Update existing items to have a default group
UPDATE ChecklistItems 
SET ItemGroup = 'General' 
WHERE ItemGroup IS NULL;
GO

-- Recipes Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Recipes')
BEGIN
    CREATE TABLE Recipes (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(255) NOT NULL,
        Type NVARCHAR(100) NOT NULL,
        RecipeText NVARCHAR(MAX) NOT NULL,
        Created DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Updated DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- Pictures Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pictures')
BEGIN
    CREATE TABLE Pictures (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        FileName NVARCHAR(255) NOT NULL,
        ImageData VARBINARY(MAX) NOT NULL,
        Caption NVARCHAR(500) NOT NULL,
        RecipeId UNIQUEIDENTIFIER NULL,
        Created DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Updated DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        FOREIGN KEY (RecipeId) REFERENCES Recipes(Id) ON DELETE SET NULL
    );
END
GO

-- Insert default user
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'harreby')
BEGIN
    INSERT INTO Users (Username, Password, Role, DisplayName)
    VALUES ('harreby', 'fishalot', 'Admin', 'Jørgen');
END
GO

-- Create indexes for performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_TextNotes_Updated')
BEGIN
    CREATE INDEX IX_TextNotes_Updated ON TextNotes(Updated DESC);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Checklists_Updated')
BEGIN
    CREATE INDEX IX_Checklists_Updated ON Checklists(Updated DESC);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Recipes_Updated')
BEGIN
    CREATE INDEX IX_Recipes_Updated ON Recipes(Updated DESC);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Pictures_Updated')
BEGIN
    CREATE INDEX IX_Pictures_Updated ON Pictures(Updated DESC);
END
GO
