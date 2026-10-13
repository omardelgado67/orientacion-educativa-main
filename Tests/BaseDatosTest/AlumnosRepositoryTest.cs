using System.Threading.Tasks;
using BaseDatos;
using BaseDatos.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BaseDatosTest;

 public class AlumnosRepositoryTest
{
    private IServiceProvider ServiceProvider;

    [SetUp]
    public void setup()
    {
        var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", false)
             .Build();

             var services = new ServiceCollection();
             services.AddSingleton<IConfiguration>(configuration);
             services.Configure<MongoDBSettings>(configuration.GetSection("MongoDb"));


             services.AddSingleton<IMongoClient>(sp =>
             {
                 var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                 return new MongoClient(settings.ConnectionString);
             });

             services.AddTransient<AlumnosRepository>();

             this.ServiceProvider = services.BuildServiceProvider();

    }

    [TearDown]
    public void TearDown()
    {
    if(this.ServiceProvider is IDisposable disposable)
    {
        disposable.Dispose();
    }
    }
    [Test]
    public async Task ObtieneDatosDeAlumnos()
    {
        var repository = this.ServiceProvider.GetRequiredService<AlumnosRepository>();
        var list = await repository.ListarTodos();

        int cuantosAlumnos = list.Count;
        Assert.That(cuantosAlumnos > 0, "No contiene alumnos");
        Assert.That(cuantosAlumnos == 5, "No se trajo todos los registros");
    }

    [Test]
    public async Task ObtieneUnAlumnosYValidaSuNombre()
    {
        long numControl = 24328051059999;
        string apellido = "contreras";
        var repository = this.ServiceProvider.GetRequiredService<AlumnosRepository>();
        var alumno = await repository.ObtenerAlumnoPorNumeroDeControl(numControl);  

        Assert.That(alumno is not null, $"No existe un alumno con el número de Control {numControl}");
        if(alumno != null)
        {
            Assert.That(
                string.Equals(alumno.Paterno, "contrerass", 
                StringComparison.OrdinalIgnoreCase), 
                $"El apellidp esperado es {apellido} y el registro obtuvo {alumno.Paterno}");
        }
    



    }

   [Test]
    public async Task BuscaUnAlumnoYObtiene_404_NotFound()
    {
        long numControl = 25328051059999;
        var repository = this.ServiceProvider.GetRequiredService<AlumnosRepository>();
        var alumno = await repository.ObtenerAlumnoPorNumeroDeControl(numControl);

        Assert.That(alumno is null, "El alumno existe y no debería existir");
    }
}