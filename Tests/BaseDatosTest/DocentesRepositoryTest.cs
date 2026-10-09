using BaseDatos;
using BaseDatos.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace BaseDatosTest;
public class DocentesRepositoryTest
{
    private ServiceProvider serviceProvider;
    [SetUp]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<MongoDBSettings>(
            configuration.GetSection("MongoDB"));
        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });
        services.AddTransient<DocentesRepository>();
        this.serviceProvider = services.BuildServiceProvider();
    }
    [TearDown]
    public void TearDown()
    {
        if (this.serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
#pragma warning disable NUnit1027 // The test method has parameters, but no arguments are supplied by attributes
    [Test]
#pragma warning restore NUnit1027 // The test method has parameters, but no arguments are supplied by attributes
    public async Task ObtenerDatosDocenteDocentesRepository()
    {
        
        var repository =
            this.serviceProvider.GetRequiredService<DocentesRepository>();
        var list = repository.Listar();
        int cuantosDocentes = list.Count;
        Assert.That(cuantosDocentes > 0, "No contiene docentes");
        Assert.That(cuantosDocentes == 10, "No se trajeron todos los registros");
    }
}

internal class MongoDBSettings
{
    public MongoClientSettings ConnectionString { get; internal set; }
}