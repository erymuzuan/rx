 -- =============================================
-- Create database template
-- =============================================
USE master
GO

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'Sph'
)
DROP DATABASE Sph
GO

CREATE DATABASE Sph
GO