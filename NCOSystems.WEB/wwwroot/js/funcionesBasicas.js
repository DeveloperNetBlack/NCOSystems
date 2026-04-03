// ==========================================
// Validación RUT
// ==========================================
$("#RutPersonal").Rut({
    on_error: function () {
        alert('Rut incorrecto');
    },
    format_on: 'keyup'
});

$("#RutPersonal").blur(function () {
    const rut = $(this).val();

    if (!rut) return;

    $.ajax({
        url: validarRut_Url, // Definir en la vista: var validarRut_Url = '@Url.Action("ValidarRut", "Personal")';
        type: "GET",
        dataType: "JSON",
        data: { rutPersonal: rut },
        success: function (data) {
            if (data.existe) {
                Swal.fire({
                    icon: 'warning',
                    title: 'RUT ya registrado',
                    text: 'El RUT ingresado ya existe en el sistema.',
                    confirmButtonText: 'OK'
                }).then(() => {
                    $("#RutPersonal").val("").focus();
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo validar el RUT. Intente nuevamente.',
                confirmButtonText: 'OK'
            });
        }
    });
});