public class DatabaseTests : IAsyncLifetime
{
    private readonly ApplicationDbContext _context;

    public DatabaseTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApplicationDbContext(options);
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await SeedTestDataAsync();
    }

    private async Task SeedTestDataAsync()
    {
        _context.VehicleMakes.Add(new VehicleMake { Name = "Honda" });
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task AddMake_ShouldPersistInDatabase()
    {
        // Arrange
        var make = new VehicleMake { Name = "Tesla" };

        // Act
        _context.VehicleMakes.Add(make);
        await _context.SaveChangesAsync();

        // Assert
        var savedMake = await _context.VehicleMakes.FirstOrDefaultAsync(m => m.Name == "Tesla");
        Assert.NotNull(savedMake);
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }
}