using PryTrabajadoresPrueba.Domain.Entities;
namespace PryTrabajadoresPrueba.Application.Interfaces
{
     public interface ITrabajadorService
    {
       Task<IEnumerable<Trabajador>> ListarTrabajadores();
       Task<Trabajador?> BuscarPorId(string id);
       Task RegistrarTrabajador(Trabajador trabajador);
       Task EditarTrabajador(Trabajador trabajador);
       Task EliminarTrabajador(String id);
       Task<Trabajador?> BuscarPorDocumento(string numeroDocumento);

     
    }
}