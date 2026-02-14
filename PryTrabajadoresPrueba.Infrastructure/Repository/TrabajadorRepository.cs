using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using PryTrabajadoresPrueba.Domain;
using PryTrabajadoresPrueba.Domain.Interfaces;
using PryTrabajadoresPrueba.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PryTrabajadoresPrueba.Infrastructure.Repository
{
    public class TrabajadorRepository : ITrabajadorRepository
    {
        private readonly AppDbContext _context;
        public TrabajadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trabajador>>ListarTrabajadores()
        {
            //Llamar SP Listar_trabajadores
            return await _context.Trabajadores
                .FromSqlRaw("EXEC sp_ListarTrabajadores")
                .ToListAsync();
        }

        //Método para buscar trabajador por id u
        public async Task<Trabajador?> BuscarPorId(string id)
        {
            return await _context.Trabajadores
                .FirstOrDefaultAsync(t => t.IdTrb == id);
        }

        //Método para buscar trabajador por número de documento
        public async Task<Trabajador?> BuscarPorDocumento(string numeroDocumento)
        {
            //Llamar SP Buscar_trabajador_por_documento
            var param = new SqlParameter("@NumeroDocumento", numeroDocumento);
            return await _context.Trabajadores
                .FirstOrDefaultAsync(t => t.NumeroDocumento == numeroDocumento);
        }

        // Método para registrar trabajador 
        public async Task RegistrarTrabajador(Trabajador t)
        {
            //Llamar SP Registrar_trabajador
            var prmt = new[]
            {
                new SqlParameter("@Nombres", t.Nombres),
                new SqlParameter("@Apellidos", t.Apellidos),
                new SqlParameter("@TipoDocumento", t.TipoDocumento),
                new SqlParameter("@NumeroDocumento", t.NumeroDocumento),
                new SqlParameter("@Sexo", t.Sexo),
                new SqlParameter("@FechaNacimiento", t.FechaNacimiento),
                new SqlParameter("@FotoUrl", t.FotoUrl ?? (object)DBNull.Value),
                new SqlParameter("@Direccion", t.Direccion)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_RegistrarTrabajador @Nombres, @Apellidos, @TipoDocumento, @NumeroDocumento, @Sexo, @FechaNacimiento, @FotoUrl, @Direccion", prmt);
        }

        // Método para editar trabajador
        public async Task EditarTrabajador(Trabajador t)
        {
            //Llamar SP Editar_trabajador
            var prmt = new[]
            {
                new SqlParameter("@IdTrb", t.IdTrb),
                new SqlParameter("@Nombres", t.Nombres),
                new SqlParameter("@Apellidos", t.Apellidos),
                new SqlParameter("@TipoDocumento", t.TipoDocumento),
                new SqlParameter("@NumeroDocumento", t.NumeroDocumento),
                new SqlParameter("@Sexo", t.Sexo),
                new SqlParameter("@FechaNacimiento", t.FechaNacimiento),
                new SqlParameter("@FotoUrl", t.FotoUrl ?? (object)DBNull.Value),
                new SqlParameter("@Direccion", t.Direccion)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_EditarTrabajador @IdTrb, @Nombres, @Apellidos, @TipoDocumento, @NumeroDocumento, @Sexo, @FechaNacimiento, @FotoUrl, @Direccion", prmt);
        } 

        // Método para eliminar trabajador
        public async Task EliminarTrabajador(string id)
        {
            //Llamar SP Eliminar_trabajador
            var prmt = new SqlParameter("@IdTrb", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_EliminarTrabajador @IdTrb", prmt);
        }
    }
}
