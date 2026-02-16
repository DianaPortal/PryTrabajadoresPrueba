using Xunit;
using Moq;
using PryTrabajadoresPrueba.Application.Interfaces;
using PryTrabajadoresPrueba.Application.Services;
using PryTrabajadoresPrueba.Domain.Entities;

using System;
using System.Threading.Tasks;

namespace PryTrabajadoresPrueba.Tests
{
    public class TrabajadorServiceTests
    {
        //Repo simulado y servicio a probar
        private readonly Mock<ITrabajadorRepository> _mockTbRepo;
        private readonly ITrabajadorService _trbjService;

        public TrabajadorServiceTests()
        {
            _mockTbRepo = new Mock<ITrabajadorRepository>();
            //Inyección del repo simulado al servicio
            _trbjService = new TrabajadorService(_mockTbRepo.Object);
        }

        [Fact]
        public async Task LanzarError_RegisTrabajador_MenorEdad()
        {
            var trabajadorMenorEdad = new Trabajador
            {
                Nombres = "Enrique",
                Apellidos = "Lizarraga",
                TipoDocumento = "DNI",
                NumeroDocumento = "78963214",
                Sexo = "M",
                FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-17)),// Menor de edad
                Direccion = "Calle Victor Pantoja 546"
            };

            //Al intentar registrar un trabajador menor de edad, lanza una excepción
            var ex = await Assert.ThrowsAsync<Exception>(() =>
             _trbjService.RegistrarTrabajador(trabajadorMenorEdad));

            // Verifica que el mensaje de error contenga la frase "mayor de edad"
            Assert.Contains("mayor de edad", ex.Message, StringComparison.OrdinalIgnoreCase);

        }

        [Fact]
        public async Task Registrar_DeberiaLlamarAlRepo_CuandoDatosSonCorrectos()
        {
            var trabajador = new Trabajador
            {
                IdTrb = "T0001",
                Nombres = "Eduardo",
                Apellidos = "Gomez Perez",
                TipoDocumento = "DNI",
                NumeroDocumento = "74586123",
                Sexo = "M",
                FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)), // 20 años                

                Direccion = "Calle Victor Pantoja 546"
            };

            // Simulamos que NO existe duplicado de documento 
            _mockTbRepo.Setup(r => r.BuscarPorDocumento(It.IsAny<string>()))
                     .ReturnsAsync((Trabajador?)null);

            //datos correctos, no debería lanzar excepción
            await _trbjService.RegistrarTrabajador(trabajador);

            // Verificamos que el método 'RegistrarTrabajador' del repo se llamó exactamente 1 vez
            _mockTbRepo.Verify(r => r.RegistrarTrabajador(It.IsAny<Trabajador>()), Times.Once);
        }


        [Fact]
        public async Task Registrar_DeberiaFallar_SiDocumentoYaExiste()
        {
            var trabajador = new Trabajador
            {
                Nombres = "Ana Maria",
                Apellidos = "Vidal Jimenez",
                TipoDocumento = "DNI",
                NumeroDocumento = "74586123",
                Sexo = "F",
                FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
                Direccion = "Calle Victor Pantoja 546"
            };

            // Simulamos que YA EXISTE alguien con ese documento
            _mockTbRepo.Setup(r => r.BuscarPorDocumento("74586123"))
                     .ReturnsAsync(new Trabajador()); // Devuelve un objeto cualquiera

            // Al intentar registrar con un documento duplicado, lanza una excepción
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _trbjService.RegistrarTrabajador(trabajador));

            Assert.Contains("Ya existe un trabajador", ex.Message);
        }

        [Fact]
        public async Task Registrar_DeberiaFallar_SiSexoInvalido()
        {
            var t = new Trabajador
            {
                Nombres = "Ana Maria",
                Apellidos = "Vidal Jimenez",
                TipoDocumento = "DNI",
                NumeroDocumento = "74586124",
                Sexo = "X",
                FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
                Direccion = "Calle Victor Pantoja 546"
            };

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _trbjService.RegistrarTrabajador(t));

            Assert.Contains("Sexo inválido", ex.Message);
        }

        [Fact]
        public async Task Registrar_DeberiaFallar_DNIMalFormado()
        {
            var t = new Trabajador
            {
                Nombres = "Carlos Luis",
                Apellidos = "Vega Torres",
                TipoDocumento = "DNI",
                NumeroDocumento = "111", // mal
                Sexo = "M",
                FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                Direccion = "Calle Victor Pantoja 546"
            };

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _trbjService.RegistrarTrabajador(t));

            Assert.Contains("DNI", ex.Message);
        }

        [Fact]
        public async Task Registrar_DeberiaNormalizar_Textos()
        {
            var t = new Trabajador
            {
                Nombres = " Oscar Junior",
                Apellidos = " Valdes Ramirez ",
                TipoDocumento = "DNI",
                NumeroDocumento = "78987411",
                Sexo = "M",
                FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                Direccion = " Calle Victor Pantoja 546 "
            };

            _mockTbRepo.Setup(x => x.BuscarPorDocumento(It.IsAny<string>()))
                       .ReturnsAsync((Trabajador?)null);

            await _trbjService.RegistrarTrabajador(t);

            Assert.Equal("OSCAR JUNIOR", t.Nombres);
            Assert.Equal("VALDES RAMIREZ", t.Apellidos);
        }

        [Fact]
        public async Task Editar_DeberiaFallar_SiNoExiste()
        {
            var t = new Trabajador
            {
                IdTrb = "T0999",
                Nombres = "Rafael",
                Apellidos = "Diaz Sanchez",
                TipoDocumento = "DNI",
                NumeroDocumento = "78987412",
                Sexo = "M",
                FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                Direccion = "Calle Victor Pantoja 546"
            };

            _mockTbRepo.Setup(r => r.BuscarPorId("T0999"))
                       .ReturnsAsync((Trabajador?)null);

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _trbjService.EditarTrabajador(t));

            Assert.Contains("No existe", ex.Message);
        }

        [Fact]
        public async Task Eliminar_DeberiaFallar_SiNoExiste()
        {
            _mockTbRepo.Setup(r => r.BuscarPorId("T0001"))
                       .ReturnsAsync((Trabajador?)null);

            await Assert.ThrowsAsync<Exception>(() =>
                _trbjService.EliminarTrabajador("T1"));
        }

        [Fact]
        public async Task Eliminar_DeberiaFallar_SiYaEstaInactivo()
        {
            _mockTbRepo.Setup(r => r.BuscarPorId("T0001"))
                .ReturnsAsync(new Trabajador { Activo = false });

            await Assert.ThrowsAsync<Exception>(() =>
                _trbjService.EliminarTrabajador("T0001"));
        }

    }



}