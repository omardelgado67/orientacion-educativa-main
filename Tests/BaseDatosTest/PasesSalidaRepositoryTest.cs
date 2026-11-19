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
     [Test]
    public async Task ObtenerPase()
    {
        var repository = this.serviceProvider.GetRequiredService<PasesSalidaRepository>();
        ObjectId id = ObjectId.Parse("6af5e54c47a58b307fec9cc0");
        var pase = await repository.Consultar(id);
        Assert.That(pase is not null, "no se encontro el pase con el ID proporcionado");
    }

    [Test]
    public async Task Insertar()
    {
        var repository = this.serviceProvider.GetRequiredService<PasesSalidaRepository>();
        PaseSalida pase = new PaseSalida();
        pase.carrera = "Electronica";
        pase.fecha = new DateTime(2026, 11, 1, 9, 15, 0);
        pase.grupo = "E";
        pase.motivo = "cuestiones familiares";
        pase.nombreTutor = "Sr. Juan Perez";
        pase.numControl = 24328051052222;
        pase.parentesco = "padre";
        pase.semestre = 1;
        pase.telefono = "8331234567";

        await repository.Insertar(pase);
    }

    [Test]
    public async Task Modificar()
    {
        var repository = this.serviceProvider.GetRequiredService<PasesSalidaRepository>();
        PaseSalidaModificar pase = new PaseSalidaModificar();
        pase.Id = ObjectId.Parse("6afc65ae82ad27e3fc66b766");
        pase.fecha = new DateTime(2026, 10, 30, 9, 15, 0);
        pase.motivo = "cuestiones familiares";
        pase.nombreTutor = "Sr. Juan Perez";
        pase.parentesco = "padre";
        pase.telefono = "8331234587";
        pase.comentarios = " el padre comento que saldrian de viaje por las festividades de dia de muertos";

        await repository.Modificar(pase);
    }

     [Test]
    public async Task Eliminar()
    {
         var repository = this.serviceProvider.GetRequiredService<PasesSalidaRepository>();
         ObjectId id = ObjectId.Parse("6afc65ae82ad27e3fc66b766");

         await repository.Eliminar(id);
    }
}