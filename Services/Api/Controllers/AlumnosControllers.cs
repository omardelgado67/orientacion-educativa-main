using BaseDatos.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("Alumnos")]
public class AlumnosControllers : BaseApiControllers
{
    private readonly AlumnosRepository repository;
    private const string FOTOS_ALUMNOS = "http://localhost:5000/fotos/alumnos/";
    public AlumnosControllers(AlumnosRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> ListarAlumnos()
    {
       var alumnos = await this.repository.ListarTodos();
       alumnos.ForEach(x =>
       {
           if (!string.IsNullOrEmpty(x.Foto))
           {
               x.Foto = $"{FOTOS_ALUMNOS}{x.Foto}";
           }
       });
       return Ok(alumnos);    
    }

    [HttpGet("{numControl:long}")]
    public async Task<IActionResult> ObtenerDatosAlumno(long numControl)
    {
        var alumno = await this.repository.ObtenerAlumnoPorNumeroDeControl(numControl);
        if(alumno == null)
        {
            return NotFound($"No existe alumno con numero de control {numControl}");
        }

        if (!string.IsNullOrEmpty(alumno.Foto))
        {
            alumno.Foto = $"{FOTOS_ALUMNOS}{alumno.Foto}";
        }

        return Ok(alumno);
    }

    [HttpPatch("{numControl:long}/foto")]
    public async Task<IActionResult> ActualizarFoto(long numControl)
    {
        // Validar que exista el alumno
        var alumno = await this.repository.ObtenerAlumnoPorNumeroDeControl(numControl);
        if(alumno == null)
        {
            return NotFound($"No existe un alumno con numero de control {numControl}");
        }

        // Revisar el content type
        if (!this.Request.HasFormContentType)
        {
            return BadRequest("Debe proporcionar Form content type");
        }

        // Revisar que exista un archivo
        if(this.Request.Form.Files.Count == 0)
        {
            return BadRequest("Debe proporcionar el archivo de imagen");
        }

        // Validar que el archivo sea una imagen
        var archivo = this.Request.Form.Files.First();
        var contentType = archivo.ContentType;
        if (!contentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Debe proporcioar un archivo de imagen");
        }
        // Almacenar el archivo
        string extensionArchivo = System.IO.Path.GetExtension(archivo.FileName);

        using var fs = new FileStream($"wwwroot/fotos/alumnos/{numControl}{extensionArchivo}", FileMode.OpenOrCreate);
        var stream = archivo.OpenReadStream();
        stream.CopyTo(fs);

        // Actualizar la foto del alumno
        await this.repository.ActualizarFoto(numControl, $"{numControl}{extensionArchivo}");

        return NoContent(); 
    }

    [HttpDelete("{numControl:long}/foto")]
    public async Task<IActionResult> EliminarFoto(long numControl)
    {
        // Validar que exista el alumno
        var alumno = await this.repository.ObtenerAlumnoPorNumeroDeControl(numControl);
        if(alumno == null)
        {
            return NotFound($"No existe un alumno con numero de control {numControl}");
        }

        // Eliminar la foto
        await this.repository.EliminarFoto(numControl);

        return NoContent(); 
    }
}