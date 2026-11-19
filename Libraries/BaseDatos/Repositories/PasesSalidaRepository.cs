using BaseDatos;
using BaseDatos.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;


namespace BaseDatos.Repositories;


public class PasesSalidaRepoository;
public class PasesSalidaRepository : Repository
{
   /*
a) Insertar un pase de salida
b) Modificar un pase de salida
c) Eliminar un pase de salida
d) Consultar un pase de salida
e) Consultar los pases de un alumno
*/ private readonly IMongoCollection <PaseSalida> pases;
    public PasesSalidaRepository(IMongoClient client, IOptions<MongoDBSettings> options) : base(client, options)
    {
          var db = client.GetDatabase(options.Value.DatabaseName);
        this.pases = db. GetCollection<PaseSalida>("pases-salida");
    }


    public async Task Insertar(PaseSalida item)
    {
        await this.pases.InsertOneAsync(item);
    }


    public async Task Modificar(PaseSalidaModificar item)
    {
        await this.pases.UpdateOneAsync(
            x => x.Id == item.Id,
            Builders<PaseSalida>.Update
            .Set(x => x.fecha, item.fecha)
            .Set(x => x.motivo, item.motivo)
            .Set(x => x.nombreTutor, item.nombreTutor)
            .Set(x => x.parentesco, item.parentesco)
            .Set(x => x.telefono, item.telefono)
            .Set(x => x.comentarios, item.comentarios)
        );
    }


    public async Task Eliminar(ObjectId id)
    {
      await this.pases.DeleteOneAsync(x => x.Id == id);
    }
    
    public async Task<PaseSalida> Consultar (ObjectId id)
    {
      var respuesta =await this.pases.FindAsync(x => x.Id == id);
      var pase = await respuesta.FirstOrDefaultAsync();
      return pase;
    }
        
      public async Task<List<PaseSalida>> ListarPasesAlumnos(long numControl)
    {
         var respuesta =await this.pases.FindAsync(x => x.numControl == numControl);
      var pases = await respuesta.ToListAsync();
      return pases;
    }   
    }