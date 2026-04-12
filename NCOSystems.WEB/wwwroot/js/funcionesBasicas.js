// ==========================================
// Validación RUT
// ==========================================
$("#RutPersonal").Rut({
    on_error: function () {
        alert('Rut incorrecto');
    },
    format_on: 'keyup'
});

// ==========================================
// Validación RUT
// ==========================================
$("#RutPersona").Rut({
    on_error: function () {
        alert('Rut incorrecto');
    },
    format_on: 'keyup'
});