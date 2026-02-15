var modalTrabajador;

//Iniciar el modal al cargar la página
document.addEventListener("DOMContentLoaded", function () {
    modalTrabajador = new bootstrap.Modal(document.getElementById('modalTrabajador'));
});

// Función para mostrar el modal (Crear o Editar)
function mostrarModal(id) {
    var url = '/Trabajadores/ObtenerModal';
    if (id !== '') {
        url += '?id=' + id;
    }

    // Limpiar y mostrar mensaje de carga 
    $('#modal-body-content').html('<div class="text-center p-3">Cargando...</div>');

    // Mostrar el modal
    modalTrabajador.show();

    // Cargar contenido 
    $.get(url, function (html) {
        $('#modal-body-content').html(html);

        //VALIDACIONES PARA NUMERO DE DOCUMENTO 

        configurarValidacionDocumento();
    });
}


function filtrarPorSexo() {
    var sexo = document.getElementById("filtroSexo").value;
    var url = "/Trabajadores/Index";

    if (sexo !== "") {
        url += "?sexo=" + sexo;
    }

    window.location.href = url;
}

function guardar() {

    var $form = $('#frmTrabajador');
    if (!$form.valid()) return;

    var formData = new FormData(document.getElementById('frmTrabajador'));

    // Mostrar "Guardando..."
    Swal.fire({
        title: 'Guardando...',
        text: 'Por favor espere',
        allowOutsideClick: false,
        didOpen: () => { Swal.showLoading() }
    });

    // Enviar AJAX
    $.ajax({
        url: '/Trabajadores/Guardar',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            Swal.close();

            if (response.success) {
                // Éxito
                modalTrabajador.hide();
                Swal.fire('¡Éxito!', 'Trabajador guardado correctamente.', 'success')
                    .then(() => {
                        // Recargar página para ver cambios
                        location.reload();
                    });
            } else {
                // Error de Backend 
                Swal.fire('Error', response.message, 'error');
            }
        },
        error: function () {
            Swal.close();
            Swal.fire('Error', 'Ocurrió un error al procesar la solicitud.', 'error');
        }
    });
}

function eliminar(id) {
    Swal.fire({
        title: '¿Está seguro',
        text: "de eliminar el registro?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        customClass: {
            popup: 'my-swal-size'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            // Petición AJAX de eliminación
            $.post('/Trabajadores/Eliminar', { id: id }, function (response) {
                if (response.success) {
                    Swal.fire('Eliminado', 'El registro ha sido eliminado.', 'success')
                        .then(() => {
                            location.reload();
                        });
                } else {
                    Swal.fire('Error', response.message, 'error');
                }
            });
        }
    });
}

function configurarValidacionDocumento() {
    // Elementos del formulario
    const cboTipDoc = document.getElementById('TipoDocumento');
    const txtNumDoc = document.getElementById('NumeroDocumento');

    if (!cboTipDoc || !txtNumDoc) return;

    // Función para aplicar validación según tipo de documento
    const aplicarValidacion = () => {
        const tipoDoc = cboTipDoc.value;
        // Limpiar campo al cambiar tipo de documento
        txtNumDoc.value = '';

        if (tipoDoc === 'DNI') {
            txtNumDoc.setAttribute('maxlength', '8');
            txtNumDoc.setAttribute("placeholder", "Ingrese 8 dígitos");
        }
        else if (tipoDoc === 'Pasaporte') {
            txtNumDoc.setAttribute('maxlength', '9');
            txtNumDoc.setAttribute("placeholder", "Ingrese entre 6 y 9 caracteres");
        }
        else if (tipoDoc === 'Carnet de Extranjería') {
            txtNumDoc.setAttribute('maxlength', '12');
            txtNumDoc.setAttribute("placeholder", "Ingrese entre 9 y 12 caracteres");
        }
        else if (tipoDoc === 'RUC') {
            txtNumDoc.setAttribute('maxlength', '11');
            txtNumDoc.setAttribute("placeholder", "Ingrese 11 dígitos");
        }
    };

    // Ev
    cboTipDoc.addEventListener('change', aplicarValidacion);

    // Validación en el momento - para el número de documento
    txtNumDoc.addEventListener('input', function (e) {
        const tipoDoc = cboTipDoc.value;
        let valor = txtNumDoc.value;

        if (tipoDoc === 'DNI' || tipoDoc === 'RUC') {
            e.target.value = valor.replace(/\D/g, '');
        } else {
            e.target.value = valor.replace(/[^a-zA-Z0-9]/g, '');
        }
    });

    if (cboTipDoc.value === "DNI") txtNumDoc.setAttribute('maxlength', '8');
}