using System.IO.Pipes;
using BaseDatos;
using BaseDatos.Documents;
using BaseDatos.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;




namespace BaseDatosTest;




public class PaseSalidaRepositoryTest
{
    private ServiceProvider serviceProvider = null!;
    [SetUp]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();




        var services = new ServiceCollection();




         services.AddSingleton<IConfiguration>(configuration);
        services.Configure<MongoDBSettings>(
            configuration.GetSection("MongoDb"));




        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
            return new MongoClient(settings.ConnectionString);
        });




        services.AddTransient<PasesSalidaRepository>();




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




    //1.Listar pases de salida para un alumno existente
    [Test]
    public async Task ListarPasesAlumno()
    {
        var repository = this.serviceProvider.GetRequiredService<PasesSalidaRepository>();
        var list = await repository.ListarPasesAlumnos(25328051050581);
        int cantidadPasesEsparados = 2;




        int cantidadPasesObtenidos = list.Count;




        Assert.That(cantidadPasesEsparados == cantidadPasesObtenidos, $"se esperaban {cantidadPasesEsparados} registros y se obtuvieron {cantidadPasesObtenidos} de la base de datos");
    }
}