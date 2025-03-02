-- Step 1: Drop existing tables if they exist (for re-creation)
DROP TABLE IF EXISTS VehicleModels;
DROP TABLE IF EXISTS VehicleMakes;

-- Step 2: Create VehicleMakes Table with Auto-increment ID and necessary columns
CREATE TABLE VehicleMakes (
    Id INT PRIMARY KEY IDENTITY(1,1), -- For SQL Server (Auto-Increment)
    -- Id INT AUTO_INCREMENT PRIMARY KEY, -- For MySQL
    Name NVARCHAR(100) NOT NULL,
    Abrv NVARCHAR(10) NOT NULL
);

-- Step 3: Create VehicleModels Table with Foreign Key, Auto-increment ID, and necessary columns
CREATE TABLE VehicleModels (
    Id INT PRIMARY KEY IDENTITY(1,1), -- For SQL Server (Auto-Increment)
    -- Id INT AUTO_INCREMENT PRIMARY KEY, -- For MySQL
    Name NVARCHAR(100) NOT NULL,
    MakeId INT NOT NULL,
    Abrv NVARCHAR(10) NOT NULL,
    FOREIGN KEY (MakeId) REFERENCES VehicleMakes(Id) ON DELETE CASCADE
);

-- Step 4: Add indexes for better performance on frequent queries
CREATE INDEX IX_VehicleMakes_Name ON VehicleMakes(Name);
CREATE INDEX IX_VehicleModels_MakeId ON VehicleModels(MakeId);

-- Step 5: Insert sample data into VehicleMakes and VehicleModels
-- VehicleMakes Insertions
INSERT INTO VehicleMakes (Name, Abrv) VALUES
('Honda', 'H'),
('Tesla', 'T'),
('BMW', 'B'),
('Ford', 'F');

-- VehicleModels Insertions
INSERT INTO VehicleModels (Name, MakeId, Abrv) VALUES
('Civic', 1, 'CIV'),
('Accord', 1, 'ACC'),
('Model S', 2, 'MS'),
('Model 3', 2, 'M3'),
('X5', 3, 'X5'),
('Mustang', 4, 'MUS');
