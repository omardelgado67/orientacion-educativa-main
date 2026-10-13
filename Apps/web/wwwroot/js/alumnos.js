window.addEventListener("DOMContentLoaded", ()=>{
    const vm = new ListadoAlumnosViewModel();
    ko.applyBindings(vm);

    vm.init();
});

function ListadoAlumnosViewModel()
{
    const self = this;

    this.registros = ko.observableArray();

    const obtenerAlumnos = async () => {
        const respuesta = await fetch("http://localhost:5000/alumnos");

        if(respuesta.ok){
            const datos = await respuesta.json();
        }
    };

    this.init = () => {
        obtenerAlumnos();
    };
}