CREATE TABLE VehicleMake (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50),
    Abrv NVARCHAR(10)
);

CREATE TABLE VehicleModel (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MakeId INT,
    Name NVARCHAR(50),
    Abrv NVARCHAR(10),
    FOREIGN KEY (MakeId) REFERENCES VehicleMake(Id)
);
