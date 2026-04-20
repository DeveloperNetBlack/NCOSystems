$(document).ready(function () {

    // ==========================================
    // Validación de Rut
    // ==========================================
    $("#RutPersonal").blur(function () {
        const rut = $(this).val();

        if (!rut) return;

        $.ajax({
            url: validarRut_Url,
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
    function cargarComunas(regionId, callback) {
        $('#IdComuna').empty();

        if (regionId) {
            $.ajax({
                url: getComuna_Url,
                type: "GET",
                dataType: "JSON",
                data: { idRegion: regionId },
                success: function (data) {
                    $('#IdComuna').append($('<option></option>').val('').html('Seleccione una Comuna'));
                    $.each(data, function (i, item) {
                        $('#IdComuna').append($('<option></option>').val(item.idComuna).html(item.nombreComuna));
                    });
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

    $('#IdRegion').change(function () {
        cargarComunas($(this).val());
    });

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
    var currentYear = new Date().getFullYear();

    $("#FechaVctoLicencia").datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: 0
    });

    $("#FecOtorgamiento").datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: "-30Y",
        yearRange: "-30:+10"
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
            Swal.fire({
                icon: 'warning',
                title: 'Campos incompletos',
                text: 'Favor ingresar Fec. Otorgamiento, Fec. Vencimiento y seleccione el Tipo de Licencia.',
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#3085d6'
            });
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

    // ==========================================
    // Botón Grabar (fetch async)
    // ==========================================
    async function handleBtnGrabar() {
        const btn = document.getElementById("btnGrabar");

        var form = $('form').first();
        if (!form.data('validator')) {
            $.validator.unobtrusive.parse(form);
        }
        var esValido = form.data('validator') ? form.valid() : true;
        if (!esValido) return;

        const inputs = document.querySelectorAll("input.file-documento");
        const documentosFaltantes = [];

        inputs.forEach(input => {
            const nombreTipo = input.getAttribute("data-nombre");
            if (input.getAttribute("data-excluir") === "true") return;
            if (input.files.length === 0) {
                documentosFaltantes.push(nombreTipo);
                input.classList.add("is-invalid");
            } else {
                input.classList.remove("is-invalid");
            }
        });

        if (documentosFaltantes.length > 0) {
            const lista = documentosFaltantes.map(n => `<li>${n}</li>`).join("");
            Swal.fire({
                icon: 'warning',
                title: 'Documentos faltantes',
                html: `<p>Por favor adjunta los siguientes documentos:</p><ul style="text-align:left">${lista}</ul>`,
                confirmButtonText: "OK"
            });
            return;
        }

        const personalData = {
            RutPersonal: document.getElementById("RutPersonal")?.value || "",
            NombrePersonal: document.getElementById("NombrePersonal")?.value || "",
            ApPaternoPersonal: document.getElementById("ApPaternoPersonal")?.value || "",
            ApMaternoPersonal: document.getElementById("ApMaternoPersonal")?.value || "",
            TelefonoPersonal: document.getElementById("TelefonoPersonal")?.value || "",
            CorreoElectronico: document.getElementById("CorreoElectronico")?.value || "",
            IdComuna: document.getElementById("IdComuna")?.value || "",
            IdEstadoCivil: document.getElementById("IdEstadoCivil")?.value || "",
            IdEstadoLaboral: document.getElementById("IdEstadoLaboral")?.value || "",
            IdGenero: document.getElementById("IdGenero")?.value || ""
        };

        // ==========================================
        // Recolección y validación de Licencias
        // ==========================================
        const hiddenContainerPersonalTipoLicencia = document.getElementById('hiddenContainerPersonalTipoLicencia');
        const divsPersonalTipoLicencia = hiddenContainerPersonalTipoLicencia.querySelectorAll('[id^="hidden-"]');
        const datoPersonalTipoLicencia = [];
        let licenciasConErrores = false;

        divsPersonalTipoLicencia.forEach(div => {
            const uid = div.id.replace('hidden-', '');

            const idTipoLicencia = div.querySelector(`input[name="personalTipoLicenciaEntities[${uid}].IdTipoLicencia"]`).value.trim();
            const fecVctoLicencia = div.querySelector(`input[name="personalTipoLicenciaEntities[${uid}].FechaVctoLicencia"]`).value.trim();
            const fecOtorgamiento = div.querySelector(`input[name="personalTipoLicenciaEntities[${uid}].FecOtorgamiento"]`).value.trim();

            if (!idTipoLicencia || !fecVctoLicencia || !fecOtorgamiento) {
                licenciasConErrores = true;
                return;
            }

            datoPersonalTipoLicencia.push({
                idTipoLicencia,
                fecVctoLicencia,
                fecOtorgamiento
            });
        });

        console.log("Licencias ingresadas:", datoPersonalTipoLicencia);

        if (licenciasConErrores) {
            Swal.fire({
                icon: 'warning',
                title: 'Campos incompletos',
                text: 'Existen registros de licencia con campos vacíos. Por favor, complete todos los datos antes de continuar.',
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#3085d6'
            });
            return;
        }

        // ==========================================
        // Recolección de Hijos
        // ==========================================
        const hiddenContainerPersonalHijo = document.getElementById('hiddenContainerPersonalHijo');
        const divsPersonalHijo = hiddenContainerPersonalHijo.querySelectorAll('[id^="hiddenHijo-"]');
        const datoPersonalHijo = [];
        divsPersonalHijo.forEach(div => {
            const uid = div.id.replace('hiddenHijo-', '');
            datoPersonalHijo.push({
                nombreHijo: div.querySelector(`input[name="personalHijoEntities[${uid}].NombreHijo"]`).value,
                edadHijo: div.querySelector(`input[name="personalHijoEntities[${uid}].EdadHijo"]`).value
            });
        });

        if (datoPersonalTipoLicencia.length === 0) {
            Swal.fire({
                icon: 'warning',
                title: 'Tipos de Licencias faltantes',
                html: `<p>Por favor, se deben ingresar al menos un Tipo de Licencia</p>`,
                confirmButtonText: "OK"
            });
            return;
        }

        const formData = new FormData();
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        if (token) formData.append("__RequestVerificationToken", token);

        let index = 0;
        inputs.forEach(input => {
            const idTipoDocumento = input.getAttribute("data-id");
            const nombreTipo = input.getAttribute("data-nombre");
            if (input.files.length > 0) {
                formData.append(`documentos[${index}].IdTipoDocumento`, idTipoDocumento);
                formData.append(`documentos[${index}].NombreTipoDocumento`, nombreTipo);
                formData.append(`documentos[${index}].Archivo`, input.files[0]);
            }
            index++;
        });

        formData.append("personalData", JSON.stringify(personalData));
        formData.append("datoPersonalTipoLicencia", JSON.stringify(datoPersonalTipoLicencia));
        formData.append("datoPersonalHijo", JSON.stringify(datoPersonalHijo));

        btn.disabled = true;
        document.getElementById("resultadoMensaje").innerHTML =
            '<span class="text-info">Guardando datos...</span>';

        try {
            const response = await fetch("/Personal/Create", {
                method: "POST",
                body: formData
            });

            const data = await response.json();
            Swal.fire({
                icon: data.isError ? 'error' : 'success',
                title: data.mensaje,
                confirmButtonText: "OK",
                text: ''
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = data.url;
                }
            });

        } catch (err) {
            console.log(err);
            Swal.fire({
                icon: 'error',
                title: err,
                confirmButtonText: "OK"
            });
        } finally {
            btn.disabled = false;
        }
    }

    function initBtnGrabar() {
        const btn = document.getElementById("btnGrabar");

        if (!btn || !(btn instanceof HTMLElement)) {
            console.warn("initBtnGrabar: #btnGrabar no encontrado en esta página.");
            return;
        }

        btn.addEventListener("click", handleBtnGrabar);
        console.log("initBtnGrabar: listener registrado correctamente.");
    }

    initBtnGrabar();
});