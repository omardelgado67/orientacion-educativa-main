using MongoDB.Driver;
using System.Collections.Generic;

namespace BaseDatos
{
    public class DocentesRepository
    {
        private readonly IMongoCollection<Docentes> docentes;

        public DocentesRepository()
        {
            var cliente = new MongoClient("mongodb://localhost:27017");
            var database = cliente.GetDatabase("BaseDatos");

            docentes = database.GetCollection<Docentes>("Docentes");
        }

        public List<Docentes> Listar()
        {
            return docentes.Find(_ => true).ToList();
        }

        public async Task ListarTodos()
        {
            throw new NotImplementedException();
        }
    }
}