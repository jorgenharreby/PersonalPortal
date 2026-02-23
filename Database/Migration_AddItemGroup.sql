-- Migration Script: Add ItemGroup to ChecklistItems
-- Run this script to update your existing database

USE PersonalPortal;
GO

-- Add ItemGroup column to ChecklistItems if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ChecklistItems') AND name = 'ItemGroup')
BEGIN
    PRINT 'Adding ItemGroup column to ChecklistItems table...'
    ALTER TABLE ChecklistItems
    ADD ItemGroup NVARCHAR(100) NULL;
    PRINT 'ItemGroup column added successfully.'
END
ELSE
BEGIN
    PRINT 'ItemGroup column already exists.'
END
GO

-- Update existing items to have a default group
PRINT 'Updating existing items with default group...'
UPDATE ChecklistItems 
SET ItemGroup = 'General' 
WHERE ItemGroup IS NULL;
PRINT 'Existing items updated.'
GO

PRINT 'Migration completed successfully!'
GO
