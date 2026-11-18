using MongoDB.Bson;

namespace BaseDatos.Documents;

public class PaseSalida
{
    public ObjectId Id { get; set; }
    public long numControl { get; set; }
    public string carrera { get; set; } = string.Empty;
    public int semestre { get; set; }
    public string grupo { get; set; } = string.Empty;
    public DateTime fecha { get; set; }
    public string telefono { get; set; } = string.Empty;
    public string nombreTutor { get; set; } = string.Empty;
    public string parentesco { get; set; } = string.Empty;
    public string motivo { get; set; } = string.Empty;
    public string? comentarios { get; set; }
    
}