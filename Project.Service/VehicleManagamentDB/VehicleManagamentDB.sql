-- Drop existing tables if they exist
IF OBJECT_ID('dbo.VehicleModels', 'U') IS NOT NULL
    DROP TABLE dbo.VehicleModels;

IF OBJECT_ID('dbo.VehicleMakes', 'U') IS NOT NULL
    DROP TABLE dbo.VehicleMakes;

-- Create VehicleMakes table
CREATE TABLE VehicleMakes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Abrv NVARCHAR(10) NOT NULL
);

-- Create VehicleModels table
CREATE TABLE VehicleModels (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    MakeId INT NOT NULL,
    Abrv NVARCHAR(10) NOT NULL,
    FOREIGN KEY (MakeId) REFERENCES VehicleMakes(Id) ON DELETE CASCADE
);

-- Create indexes
CREATE INDEX IX_VehicleMakes_Name ON VehicleMakes(Name);
CREATE INDEX IX_VehicleModels_MakeId ON VehicleModels(MakeId);

-- Insert sample data into VehicleMakes
INSERT INTO VehicleMakes (Name, Abrv) VALUES
('Honda', 'H'),
('Tesla', 'T'),
('BMW', 'B'),
('Ford', 'F');

-- Insert sample data into VehicleModels
INSERT INTO VehicleModels (Name, MakeId, Abrv) VALUES
('Civic', 1, 'CIV'),
('Accord', 1, 'ACC'),
('Model S', 2, 'MS'),
('Model 3', 2, 'M3'),
('X5', 3, 'X5'),
('Mustang', 4, 'MUS');
