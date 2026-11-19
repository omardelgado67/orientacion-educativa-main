using MongoDB.Bson;

namespace BaseDatos.Documents;

public class PaseSalidaModificar
{
    public ObjectId Id { get; set; }
    public DateTime fecha { get; set; }
    public string telefono { get; set; } = string.Empty;
    public string nombreTutor { get; set; } = string.Empty;
    public string parentesco { get; set; } = string.Empty;
    public string motivo { get; set; } = string.Empty;
    public string? comentarios { get; set; }
}