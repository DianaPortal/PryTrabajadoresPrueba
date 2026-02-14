using System;
using System.Collections.Generic;

namespace PryTrabajadoresPrueba.Domain.Entities;

public partial class Trabajador
{
    public string IdTrb { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string FotoUrl { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }
}
