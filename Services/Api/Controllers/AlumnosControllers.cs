using BaseDatos.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("Alumnos")]
public class AlumnosControllers : BaseApiControllers
{
    private readonly AlumnosRepository repository;
    public AlumnosControllers(AlumnosRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> ListarAlumnos()
    {
       var alumnos = await this.repository.ListarTodos();
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

        return Ok(alumno);
    }
}