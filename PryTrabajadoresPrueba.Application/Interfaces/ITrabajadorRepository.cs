using PryTrabajadoresPrueba.Domain.Entities;

namespace PryTrabajadoresPrueba.Application.Interfaces
{
    public interface ITrabajadorRepository
    {
       Task<IEnumerable<Trabajador>> ListarTrabajadores(string? nombre, string? sexo);
       Task<Trabajador?> BuscarPorId(string id);
       Task RegistrarTrabajador(Trabajador trabajador);
       Task EditarTrabajador(Trabajador trabajador);
       Task EliminarTrabajador(String id);

       Task<Trabajador?> BuscarPorDocumento(string numeroDocumento);

    }
}