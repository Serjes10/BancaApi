// En el proyecto de pruebas (BancaApi.Tests)
using BancaApi.Data;
using Microsoft.EntityFrameworkCore;

public class DatabaseFixture : IDisposable
{
    public BancaDbContext Context { get; private set; }

    public DatabaseFixture()
    {
        // Aquí se configura el DbContext para la base de datos en memoria
        var options = new DbContextOptionsBuilder<BancaDbContext>()
            .UseSqlite("Data Source=:memory:") // Usando una base de datos SQLite en memoria para pruebas
            .Options;

        Context = new BancaDbContext(options);
        Context.Database.EnsureCreated(); // Crea la base de datos en memoria
    }

    public void Dispose()
    {
        // Aquí realizamos la limpieza después de cada prueba
        Context.Database.EnsureDeleted(); // Elimina la base de datos en memoria
        Context.Dispose();
    }
}
