$(document).ready(function () {

    // ==========================================
    // Validación de Rut
    // ==========================================
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


    // ==========================================
    // DDL Región -> Comunas
    // ==========================================

    // Función reutilizable para cargar comunas
    function cargarComunas(regionId, callback) {
        $('#IdComuna').empty();

        if (regionId) {
            $.ajax({
                url: getComuna_Url, // Definir en la vista: var getComuna_Url = '@Url.Action("GetComuna", "Personal")';
                type: "GET",
                dataType: "JSON",
                data: { idRegion: regionId },
                success: function (data) {
                    $('#IdComuna').append($('<option></option>').val('').html('Seleccione una Comuna'));
                    $.each(data, function (i, item) {
                        $('#IdComuna').append($('<option></option>').val(item.idComuna).html(item.nombreComuna));
                    });

                    // Ejecutar callback si existe (usado al editar)
                    if (typeof callback === 'function') {
                        callback();
                    }
                },
                error: function (ex) {
                    alert('Error al leer las comunas.' + ex);
                }
            });
        } else {
            $('#IdComuna').append($('<option></option>').val('').html('Seleccione una Comuna'));
        }
    }

    // Evento change del dropdown de Región
    $('#IdRegion').change(function () {
        cargarComunas($(this).val());
    });

    // Al cargar la página: si hay región seleccionada, cargar comunas y seleccionar la comuna guardada
    var comunaGuardada = $('#IdComuna').data('selected') || '';
    var regionActual = $('#IdRegion').val();

    if (regionActual && comunaGuardada) {
        cargarComunas(regionActual, function () {
            $('#IdComuna').val(comunaGuardada);
        });
    }

    // ==========================================
    // Datepickers
    // ==========================================
    $("#FechaVctoLicencia").datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: "01/01/1900"
    });

    $("#FecOtorgamiento").datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: "01/01/1900"
    });

    // ==========================================
    // Tabla Licencias
    // ==========================================
    let rowCount = 0;

    $("#btnAgregar").click(function () {
        const fechaVctoLicencia = $("#FechaVctoLicencia").val();
        const fechaOtorgamiento = $("#FecOtorgamiento").val();
        const tipoLicencia = $("#ddlTipoLicencia").val();
        const descripcion = $("#ddlTipoLicencia option:selected").text();

        if (!fechaVctoLicencia || !tipoLicencia || !fechaOtorgamiento) {
            alert("Por favor completa todos los campos.");
            return;
        }

        const uid = rowCount++;

        const newRow = `
            <tr id="row_${uid}">
                <td>${fechaOtorgamiento}</td>
                <td>${fechaVctoLicencia}</td>
                <td>${descripcion}</td>
                <td>
                    <button type="button" class="btn btn-danger btn-sm btnEliminar" data-row="${uid}">
                        Eliminar
                    </button>
                </td>
            </tr>`;

        $("#tablaLicencias tbody").append(newRow);

        const hiddenContainer = document.getElementById('hiddenContainerPersonalTipoLicencia');
        const div = document.createElement('div');
        div.id = `hidden-${uid}`;
        div.innerHTML = `
            <input type="hidden" name="personalTipoLicenciaEntities.Index" value="${uid}" />
            <input type="hidden" name="personalTipoLicenciaEntities[${uid}].FechaVctoLicencia" value="${fechaVctoLicencia}" />
            <input type="hidden" name="personalTipoLicenciaEntities[${uid}].FecOtorgamiento" value="${fechaOtorgamiento}" />
            <input type="hidden" name="personalTipoLicenciaEntities[${uid}].IdTipoLicencia" value="${tipoLicencia}" />
        `;
        hiddenContainer.appendChild(div);

        $("#FechaVctoLicencia").val("");
        $("#FecOtorgamiento").val("");
        $("#ddlTipoLicencia").val("");
    });

    $("#tablaLicencias").on("click", ".btnEliminar", function () {
        const rowId = $(this).data("row");
        $(`#row_${rowId}`).remove();
        const hidden = document.getElementById(`hidden-${rowId}`);
        if (hidden) hidden.remove();
    });

    // ==========================================
    // Tabla Hijos
    // ==========================================
    let rowCountHijo = 0;

    $("#btnAgregarHijo").click(function () {
        const nombreHijo = $("#NombreHijo").val();
        const edadHijo = $("#EdadHijo").val();

        if (!nombreHijo || !edadHijo) {
            alert("Por favor completa todos los campos.");
            return;
        }

        const uid = rowCountHijo++;

        const newRow = `
            <tr id="rowHijo_${uid}">
                <td>${nombreHijo}</td>
                <td>${edadHijo}</td>
                <td>
                    <button type="button" class="btn btn-danger btn-sm btnEliminarHijo" data-row="${uid}">
                        Eliminar
                    </button>
                </td>
            </tr>`;

        $("#tablaHijo tbody").append(newRow);

        const hiddenContainer = document.getElementById('hiddenContainerPersonalHijo');
        const div = document.createElement('div');
        div.id = `hiddenHijo-${uid}`;
        div.innerHTML = `
            <input type="hidden" name="personalHijoEntities.Index" value="${uid}" />
            <input type="hidden" name="personalHijoEntities[${uid}].NombreHijo" value="${nombreHijo}" />
            <input type="hidden" name="personalHijoEntities[${uid}].EdadHijo" value="${edadHijo}" />
        `;
        hiddenContainer.appendChild(div);

        $("#NombreHijo").val("");
        $("#EdadHijo").val("");
    });

    $("#tablaHijo").on("click", ".btnEliminarHijo", function () {
        const rowId = $(this).data("row");
        $(`#rowHijo_${rowId}`).remove();
        const hidden = document.getElementById(`hiddenHijo-${rowId}`);
        if (hidden) hidden.remove();
    });
                                                // 👈 4) Se llama al inicializar
});