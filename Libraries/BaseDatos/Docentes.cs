using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BaseDatos
{
    public class Docentes
    {
        [BsonElement("NumControl")]
        public long NumControl { get; set; }

        [BsonElement("Paterno")]
        public required string Paterno { get; set; }

        [BsonElement("Materno")]
        public required string Materno { get; set; }

        [BsonElement("Nombre")]
        public required string Nombre { get; set; }

        [BsonElement("Generacion")]
        public int Generacion { get; set; }

        [BsonElement("Carrera")]
        public required string Carrera { get; set; }

        [BsonElement("Semestre")]
        public int Semestre { get; set; }

        [BsonElement("Grupo")]
        public required string Grupo { get; set; }
    }
}