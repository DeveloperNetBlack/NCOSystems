// ================================================================
// GrillaModales.js
// Manejo de modales AJAX para Hijos, Licencias y Documentos
// ================================================================

// ----------------------------------------------------------------
// Función genérica: abrir modal con contenido AJAX (GET)
// ----------------------------------------------------------------
function abrirModal(url, params, modalContentId, modalId) {
    $.get(url, params)
        .done(function (html) {
            $('#' + modalContentId).html(html);
            var modal = new bootstrap.Modal(document.getElementById(modalId));
            modal.show();
        })
        .fail(function (xhr) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Error ' + xhr.status + ': No se pudo cargar el formulario.',
                confirmButtonColor: '#d33'
            });
        });
}

// ----------------------------------------------------------------
// Función genérica: submit de formulario dentro de modal (POST)
// ----------------------------------------------------------------
function submitModal(formSelector, modalId) {
    $(document).on('submit', formSelector, function (e) {
        e.preventDefault();

        var $form = $(this);
        var tieneArchivo = $form.find('input[type="file"]').length > 0;
        var data;
        var processData, contentType;

        if (tieneArchivo) {
            data = new FormData(this); // ← captura archivos
            processData = false;
            contentType = false;
        } else {
            data = $form.serialize(); // ← comportamiento anterior
            processData = true;
            contentType = 'application/x-www-form-urlencoded; charset=UTF-8';
        }

        $.ajax({
            url: $form.attr('action'),
            method: 'POST',
            data: data,
            processData: processData,
            contentType: contentType,
            success: function (response) {
                bootstrap.Modal.getInstance(document.getElementById(modalId)).hide();

                if (response && response.success === false) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Atención',
                        text: response.mensaje,
                        confirmButtonColor: '#f0ad4e'
                    });
                    return;
                }

                Swal.fire({
                    icon: 'success',
                    title: 'Guardado',
                    text: response.mensaje,
                    confirmButtonColor: '#3085d6'
                }).then(() => location.reload());
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ocurrió un error al guardar el registro.',
                    confirmButtonColor: '#d33'
                });
            }
        });
    });
}

// ----------------------------------------------------------------
// Función genérica de eliminación con SweetAlert2
// ----------------------------------------------------------------
function confirmarEliminar(url, id, nombre, entidad) {
    Swal.fire({
        title: '¿Está seguro?',
        text: `¿Desea eliminar ${entidad}: "${nombre}"?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (!result.isConfirmed) return;

        $.ajax({
            url: url,
            type: 'POST',
            data: { id: id, __RequestVerificationToken: csrfToken },
            success: function (result) {
                if (result.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Eliminado',
                        text: 'Registro eliminado correctamente.',
                        confirmButtonColor: '#3085d6'
                    }).then(() => location.reload());
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: result.message || 'Error desconocido',
                        confirmButtonColor: '#d33'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ocurrió un error al intentar eliminar el registro.',
                    confirmButtonColor: '#d33'
                });
            }
        });
    });
}

// ================================================================
// HIJOS
// ================================================================

// Agregar Hijo
$(document).on('click', '#btn-agregar-hijo', function () {
    var idPersonal = $(this).data('personal');
    var rutPersonal = $(this).data('rut');
    abrirModal(createHijo_Url, { idPersonal: idPersonal, rutPersonal: rutPersonal }, 'modalHijoContent', 'modalHijo');
});

// Editar Hijo
$(document).on('click', '.btn-editar-hijo', function (e) {
    e.preventDefault();
    var idHijo     = $(this).data('id');
    var idPersonal = $(this).data('personal');
    abrirModal(editHijo_Url, { idPersonalHijo: idHijo, idPersonal: idPersonal }, 'modalHijoContent', 'modalHijo');
});

// Submit Hijo (Crear o Editar)
submitModal('#formHijo', 'modalHijo');

// Eliminar Hijo
$(document).on('click', '.btn-eliminar-hijo', function () {
    var id     = $(this).data('id');
    var nombre = $(this).data('nombre');
    confirmarEliminar(deleteHijo_Url, id, nombre, 'el hijo');
});

// ================================================================
// LICENCIAS
// ================================================================

// Agregar Licencia
$(document).on('click', '#btn-agregar-licencia', function () {
    var idPersonal = $(this).data('personal');
    abrirModal(createLicencia_Url, { idPersonal: idPersonal }, 'modalLicenciaContent', 'modalLicencia');
});

// Editar Licencia
$(document).on('click', '.btn-editar-licencia', function (e) {
    e.preventDefault();
    var idLicencia = $(this).data('id');
    var idPersonal = $(this).data('personal');
    abrirModal(editLicencia_Url, { idPersonalTipoLicencia: idLicencia, idPersonal: idPersonal }, 'modalLicenciaContent', 'modalLicencia');
});

// Submit Licencia (Crear o Editar)
submitModal('#formLicencia', 'modalLicencia');

// Eliminar Licencia
$(document).on('click', '.btn-eliminar-licencia', function () {
    var id     = $(this).data('id');
    var nombre = $(this).data('nombre');
    confirmarEliminar(deleteLicencia_Url, id, nombre, 'la licencia');
});

// ================================================================
// DOCUMENTOS
// ================================================================

// Agregar Documento
$(document).on('click', '#btn-agregar-documento', function () {
    var idPersonal  = $(this).data('personal');
    var rutPersonal = $(this).data('rut');
    abrirModal(createDocumento_Url, { idPersonal: idPersonal, rutPersonal: rutPersonal }, 'modalDocumentoContent', 'modalDocumento');
});

// Editar Documento
$(document).on('click', '.btn-editar-documento', function (e) {
    e.preventDefault();
    var idDocumento = $(this).data('id');
    var idPersonal  = $(this).data('personal');
    abrirModal(editDocumento_Url, { idDocumento: idDocumento, idPersonal: idPersonal }, 'modalDocumentoContent', 'modalDocumento');
});

// Submit Documento (Crear o Editar)
submitModal('#formDocumento', 'modalDocumento');

// Eliminar Documento
$(document).on('click', '.btn-eliminar-documento', function () {
    var id     = $(this).data('id');
    var nombre = $(this).data('nombre');
    confirmarEliminar(deleteDocumento_Url, id, nombre, 'el documento');
});
