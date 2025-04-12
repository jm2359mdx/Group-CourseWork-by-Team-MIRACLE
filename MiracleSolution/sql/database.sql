-- Create the database
CREATE DATABASE MiracleDB;
GO

-- Use the database
USE MiracleDB;
GO

-- Table: Properties
CREATE TABLE Properties (
    PropertyID INT PRIMARY KEY,
    Address NVARCHAR(100) NOT NULL,
    Rent DECIMAL(10, 2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Available'
);
GO

-- Table: Tenants
CREATE TABLE Tenants (
    TenantID INT PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    PropertyID INT NULL,
    FOREIGN KEY (PropertyID) REFERENCES Properties(PropertyID)
);
GO

-- Optional: Insert sample data
INSERT INTO Properties (PropertyID, Address, Rent, Status)
VALUES 
(1, '101 Main Street', 1200.00, 'Available'),
(2, '202 Lake View', 1500.00, 'Occupied');

INSERT INTO Tenants (TenantID, Name, PropertyID)
VALUES 
(1, 'John Doe', 2),
(2, 'Jane Smith', NULL);
