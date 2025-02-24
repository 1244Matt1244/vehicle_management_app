-- Create the VehicleMakes table
CREATE TABLE VehicleMakes (
    Id INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Abrv VARCHAR(10) NOT NULL
);

-- Create the VehicleModels table
CREATE TABLE VehicleModels (
    Id INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    MakeId INT,
    FOREIGN KEY (MakeId) REFERENCES VehicleMakes(Id)
);

-- Insert initial data into VehicleMakes
INSERT INTO VehicleMakes (Id, Name, Abrv) VALUES
(1, 'Honda', 'H'),
(2, 'Tesla', 'T'),
(3, 'BMW', 'B'),
(4, 'Ford', 'F');

-- Insert initial data into VehicleModels
INSERT INTO VehicleModels (Id, Name, MakeId) VALUES
(1, 'Civic', 1),
(2, 'Accord', 1),
(3, 'Model S', 2),
(4, 'Model 3', 2),
(5, 'X5', 3),
(6, 'Mustang', 4);
