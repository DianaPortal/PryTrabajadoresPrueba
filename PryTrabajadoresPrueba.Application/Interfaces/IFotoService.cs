using Microsoft.AspNetCore.Http;
namespace PryTrabajadoresPrueba.Application.Interfaces
{

    public interface IFotoService
    {
        Task<string> SubirFoto(IFormFile foto);

    }
}
