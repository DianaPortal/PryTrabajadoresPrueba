using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PryTrabajadoresPrueba.Application.Interfaces;
using PryTrabajadoresPrueba.Application.DTOs;
using PryTrabajadoresPrueba.Domain.Entities;
using System.Text.Json;

namespace PryTrabajadoresPrueba.Web.Controllers
{
    public class TrabajadoresController : Controller
    {
        private readonly ITrabajadorService _trbjService;
        private readonly IFotoService _fotoService;

        public TrabajadoresController(ITrabajadorService trbjService, IFotoService fotoService)
        {
            _trbjService = trbjService;
            _fotoService = fotoService;
        }

        // GET: Trabajadores
        public async Task<IActionResult> Index(string nombreBusqueda, string filtroSexo, int pagina = 1)
        {
            var lista = await _trbjService.ListarTrabajadores(nombreBusqueda, filtroSexo);

            // PAGINACIÓN
            int registrosPorPagina = 7;
            int totalRegistros = lista.Count();
            var listPaginada = lista
                        .Skip((pagina - 1) * registrosPorPagina)
                        .Take(registrosPorPagina)
                        .ToList();

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            ViewBag.BusquedaNombre = nombreBusqueda;
            ViewBag.BusquedaSexo = filtroSexo;

            return View(listPaginada);
        }

        // GET: Trabajadores/ObtenerModal 
        [HttpGet]
        public async Task<IActionResult> ObtenerModal(string id = "")
        {
            CargarListasCbo();

            TrabajadorDTO tbj = new TrabajadorDTO();
            if (!string.IsNullOrEmpty(id))
            {
                var entity = await _trbjService.BuscarPorId(id);
                if (entity != null)
                {
                    tbj.IdTrb = entity.IdTrb;
                    tbj.Nombres = entity.Nombres;
                    tbj.Apellidos = entity.Apellidos;
                    tbj.Sexo = entity.Sexo;
                    tbj.TipoDocumento = entity.TipoDocumento;
                    tbj.NumeroDocumento = entity.NumeroDocumento;
                    tbj.FechaNacimiento = entity.FechaNacimiento;
                    tbj.FotoUrl = entity.FotoUrl;
                    tbj.Direccion = entity.Direccion;
                }

            }
            else
            {
                tbj.FechaNacimiento = DateOnly.FromDateTime(DateTime.Today.AddYears(-18));
            }
            return PartialView("_ModalFrm", tbj);
        }

        //
        [HttpPost]
        public async Task<IActionResult> Guardar(TrabajadorDTO model, IFormFile? fotoArchivo)
        {
            if (!ModelState.IsValid)
            {
                CargarListasCbo();
                return PartialView("_ModalFrm", model);
            }

            try
            {

                if (fotoArchivo != null)
                {
                    string urlFoto = await _fotoService.SubirFoto(fotoArchivo);
                    if (!string.IsNullOrEmpty(urlFoto)) model.FotoUrl = urlFoto;
                }
                // Si es nuevo y no subió foto, ponemos una por defecto
                if (string.IsNullOrEmpty(model.IdTrb) && string.IsNullOrEmpty(model.FotoUrl))
                {
                    model.FotoUrl = "https://i.ibb.co/wN12s3T/default-user.png";
                }

                var trabajador = new Trabajador
                {
                    IdTrb = model.IdTrb,
                    Nombres = model.Nombres,
                    Apellidos = model.Apellidos,
                    Sexo = model.Sexo,
                    TipoDocumento = model.TipoDocumento,
                    NumeroDocumento = model.NumeroDocumento,
                    FechaNacimiento = model.FechaNacimiento,
                    FotoUrl = model.FotoUrl,
                    Direccion = model.Direccion
                };

                // Registrar o Editar ??
                if (string.IsNullOrEmpty(trabajador.IdTrb))
                {
                    await _trbjService.RegistrarTrabajador(trabajador);
                }
                else
                {
                    await _trbjService.EditarTrabajador(trabajador);
                }

                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(string id)
        {
            try
            {
                await _trbjService.EliminarTrabajador(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // --- HELPER ---

        //Método para llenar los combos de sexo y tipo de doc.
        private void CargarListasCbo()
        {
            ViewBag.ListaSexo = new List<SelectListItem> {
                new SelectListItem { Value = "M", Text = "Masculino" },
                new SelectListItem { Value = "F", Text = "Femenino" }
            };

            ViewBag.ListaTipoDocumento = new List<SelectListItem> {
                new SelectListItem { Value = "DNI", Text = "DNI" },
                new SelectListItem { Value = "Pasaporte", Text = "Pasaporte" },
                new SelectListItem { Value = "Carnet de Extranjería", Text = "Carnet de Extranjería" },
                new SelectListItem { Value = "RUC", Text = "RUC" }
            };
        }

    }
}