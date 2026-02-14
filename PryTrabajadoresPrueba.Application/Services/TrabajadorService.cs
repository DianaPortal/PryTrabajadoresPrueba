using Microsoft.AspNetCore.Mvc.Rendering;
using PryTrabajadoresPrueba.Application.Interfaces; 
using PryTrabajadoresPrueba.Domain.Entities; 
using System.Collections.Generic; 
using System.Threading.Tasks; 
using System;

namespace PryTrabajadoresPrueba.Application.Services
{
    public class TrabajadorService : ITrabajadorService 
    {
        private readonly ITrabajadorRepository _trabajadorRepository;

        //Inyección de dependencias
        public TrabajadorService(ITrabajadorRepository trabajadorRepository)
        {
            _trabajadorRepository = trabajadorRepository;
        }

        //Método para listar trabajadores
        public async Task<IEnumerable<Trabajador>> ListarTrabajadores()
        {
            return await _trabajadorRepository.ListarTrabajadores();
        }


        //Método para buscar trabajador por id
        public async Task<Trabajador?> BuscarPorId(string id)
        {
            return await _trabajadorRepository.BuscarPorId(id);
        }

        //Método para buscar trabajador por número de documento
        public async Task<Trabajador?> BuscarPorDocumento(string numeroDocumento)
        {
            return await _trabajadorRepository.BuscarPorDocumento(numeroDocumento);
        }


        //Método para registrar trabajador
        public async Task RegistrarTrabajador(Trabajador trabajador)
        {
            ValidarDatos(trabajador);

            // Validar duplicado de documento
            var existente = await _trabajadorRepository.BuscarPorDocumento(trabajador.NumeroDocumento);
            if (existente != null)
                throw new Exception("Ya existe un trabajador con ese documento");
           
            // Auditoría
            trabajador.FechaRegistro = DateTime.Now;
            trabajador.Activo = true;


            await _trabajadorRepository.RegistrarTrabajador(trabajador);
        }

        //Método para editar trabajador
        public async Task EditarTrabajador(Trabajador trabajador)
        {
            ValidarDatos(trabajador);
            //  Validar existencia
            var existente = await _trabajadorRepository.BuscarPorId(trabajador.IdTrb);
            if (existente == null)
                throw new Exception("No existe el trabajador que se quiere editar");
            //  Validar duplicado de documento si se ha editado
            if (existente.NumeroDocumento != trabajador.NumeroDocumento)
            {
                var duplicado = await _trabajadorRepository.BuscarPorDocumento(trabajador.NumeroDocumento);
                if (duplicado != null)
                    throw new Exception("Ya existe otro trabajador con ese documento");
            }
            //Auditoría se mantiene
            trabajador.FechaRegistro = existente.FechaRegistro;

            await _trabajadorRepository.EditarTrabajador(trabajador);
        }

        //Método para eliminar trabajador
        public async Task EliminarTrabajador(string id)
        {
            // Validar si existe
            var existente = await _trabajadorRepository.BuscarPorId(id);
            if (existente == null)
                throw new Exception("No existe el trabajador que se quiere eliminar");

            // Validar estado
            if (!existente.Activo)
                throw new Exception("El trabajador ya está inactivo");

            await _trabajadorRepository.EliminarTrabajador(id);
        }

        private void ValidarDatos(Trabajador trabajador)
        {                    
            // Sexo 
            if (trabajador.Sexo != "M" && trabajador.Sexo != "F")
                throw new Exception("Sexo inválido, debe ser 'M' o 'F'");

            // Edad mínima
            var edad = DateTime.Today.Year - trabajador.FechaNacimiento.Year;
            if (trabajador.FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;
            if (edad < 18)
                throw new Exception("El trabajador debe ser mayor de edad");

            // Normalización
            trabajador.Nombres = trabajador.Nombres.Trim().ToUpper();
            trabajador.Apellidos = trabajador.Apellidos.Trim().ToUpper();
            trabajador.Direccion = trabajador.Direccion.Trim();
        }
    }
}