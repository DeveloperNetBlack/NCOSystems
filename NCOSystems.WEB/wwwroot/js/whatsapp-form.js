(function () {
    'use strict';

    // waNumeros viene definido desde la vista (appsettings.json)
    // Fallback por si la variable no está definida
    var NUMEROS = (typeof waNumeros !== 'undefined' && waNumeros.length > 0)
        ? waNumeros
        : ['56912345678'];

    var waFiles = [];
    var panelAbierto = false;

    // ── Reloj en vista previa ──────────────────────────────────────
    function getTime() {
        var now = new Date();
        return now.getHours().toString().padStart(2, '0') + ':' +
            now.getMinutes().toString().padStart(2, '0');
    }
    document.getElementById('wa-preview-time').textContent = getTime();
    setInterval(function () {
        document.getElementById('wa-preview-time').textContent = getTime();
    }, 1000);

    // ── Abrir / cerrar panel ───────────────────────────────────────
    function abrirPanel() {
        document.getElementById('wa-panel').style.right = '0';
        document.getElementById('wa-overlay').style.display = 'block';
        panelAbierto = true;
    }

    function cerrarPanel() {
        document.getElementById('wa-panel').style.right = '-420px';
        document.getElementById('wa-overlay').style.display = 'none';
        panelAbierto = false;
    }

    document.getElementById('wa-fab').addEventListener('click', function () {
        panelAbierto ? cerrarPanel() : abrirPanel();
    });
    document.getElementById('wa-panel-close').addEventListener('click', cerrarPanel);
    document.getElementById('wa-overlay').addEventListener('click', cerrarPanel);

    // ── Vista previa del mensaje ───────────────────────────────────
    function updatePreview() {
        var nombre = (document.getElementById('wa-nombre').value || '').trim();
        var msg = (document.getElementById('wa-mensaje').value || '').trim();
        var prev = document.getElementById('wa-preview-text');
        var full = (nombre ? '*' + nombre + '*\n' : '') + msg;

        if (full) {
            prev.textContent = full;
            prev.style.color = '#111';
            prev.style.fontStyle = 'normal';
        } else {
            prev.textContent = 'El mensaje aparecerá aquí...';
            prev.style.color = '#aaa';
            prev.style.fontStyle = 'italic';
        }
    }

    document.getElementById('wa-nombre').addEventListener('input', updatePreview);
    document.getElementById('wa-mensaje').addEventListener('input', updatePreview);

    // ── Gestión de archivos ────────────────────────────────────────
    function addFile(file) {
        if (waFiles.find(function (f) { return f.name === file.name && f.size === file.size; })) return;
        waFiles.push(file);
        renderChips();
        renderPreviewAttachments();
    }

    function removeFile(index) {
        waFiles.splice(index, 1);
        renderChips();
        renderPreviewAttachments();
    }

    function renderChips() {
        var list = document.getElementById('wa-files-list');
        list.innerHTML = '';
        waFiles.forEach(function (f, i) {
            var isImage = f.type.startsWith('image/');
            var chip = document.createElement('div');
            chip.style.cssText = 'display:flex;align-items:center;gap:5px;background:#f0f0f0;' +
                'border:1px solid #ddd;border-radius:20px;padding:3px 8px 3px 5px;font-size:12px;';

            if (isImage) {
                var img = document.createElement('img');
                img.src = URL.createObjectURL(f);
                img.style.cssText = 'width:26px;height:26px;border-radius:4px;object-fit:cover;';
                chip.appendChild(img);
            } else {
                var ic = document.createElement('span');
                ic.textContent = '📄';
                chip.appendChild(ic);
            }

            var name = document.createElement('span');
            name.textContent = f.name.length > 18 ? f.name.substring(0, 16) + '…' : f.name;
            chip.appendChild(name);

            var btn = document.createElement('button');
            btn.type = 'button';
            btn.textContent = '×';
            btn.style.cssText = 'background:none;border:none;cursor:pointer;color:#888;' +
                'font-size:14px;padding:0;margin-left:2px;line-height:1;';
            btn.onclick = (function (idx) { return function () { removeFile(idx); }; })(i);
            chip.appendChild(btn);

            list.appendChild(chip);
        });
    }

    function renderPreviewAttachments() {
        var area = document.getElementById('wa-preview-attachments');
        area.innerHTML = '';
        waFiles.forEach(function (f) {
            if (f.type.startsWith('image/')) {
                var img = document.createElement('img');
                img.src = URL.createObjectURL(f);
                img.style.cssText = 'width:52px;height:52px;border-radius:6px;object-fit:cover;border:1px solid #ddd;';
                area.appendChild(img);
            } else {
                var doc = document.createElement('div');
                doc.style.cssText = 'display:flex;align-items:center;gap:5px;background:#f0f0f0;' +
                    'border-radius:6px;padding:5px 8px;font-size:11px;color:#333;';
                doc.textContent = '📄 ' + (f.name.length > 20 ? f.name.substring(0, 18) + '…' : f.name);
                area.appendChild(doc);
            }
        });
    }

    // ── Drag & drop ────────────────────────────────────────────────
    var dz = document.getElementById('wa-dropzone');

    dz.addEventListener('dragover', function (e) {
        e.preventDefault();
        dz.style.background = '#e8f9f0';
        dz.style.borderColor = '#25D366';
    });
    dz.addEventListener('dragleave', function () {
        dz.style.background = '#f8f8f8';
        dz.style.borderColor = '#ccc';
    });
    dz.addEventListener('drop', function (e) {
        e.preventDefault();
        dz.style.background = '#f8f8f8';
        dz.style.borderColor = '#ccc';
        Array.from(e.dataTransfer.files).forEach(addFile);
    });

    // ── Selección manual ───────────────────────────────────────────
    document.getElementById('wa-file-input').addEventListener('change', function () {
        Array.from(this.files).forEach(addFile);
        this.value = '';
    });

    // ── Pegar imagen con Ctrl+V ────────────────────────────────────
    document.addEventListener('paste', function (e) {
        if (!panelAbierto) return;
        var items = e.clipboardData && e.clipboardData.items;
        if (!items) return;
        Array.from(items).forEach(function (item) {
            if (item.kind === 'file') {
                var f = item.getAsFile();
                if (f) addFile(f);
            }
        });
    });

    // ── Hover botón flotante ───────────────────────────────────────
    var fab = document.getElementById('wa-fab');
    fab.addEventListener('mouseenter', function () {
        fab.style.transform = 'scale(1.1)';
        fab.style.background = '#1da851';
    });
    fab.addEventListener('mouseleave', function () {
        fab.style.transform = 'scale(1)';
        fab.style.background = '#25D366';
    });

    // ── Enviar a múltiples números ─────────────────────────────────
    document.getElementById('wa-btn-send').addEventListener('click', function () {
        var nombre = (document.getElementById('wa-nombre').value || '').trim();
        var msg = (document.getElementById('wa-mensaje').value || '').trim();

        if (!msg && waFiles.length === 0) {
            alert('Escribe un mensaje o adjunta al menos un archivo.');
            return;
        }

        var notaAdj = waFiles.length > 0
            ? '\n\n📎 Adjuntos (' + waFiles.length + '): ' +
            waFiles.map(function (f) { return f.name; }).join(', ') +
            '\n(Adjuntar manualmente desde WhatsApp)'
            : '';

        var fullMsg = (nombre ? '*' + nombre + '*\n' : '') + msg + notaAdj;

        // Abrir una pestaña por cada número con un pequeño delay
        // para evitar que el bloqueador de popups los bloquee todos
        NUMEROS.forEach(function (numero, index) {
            setTimeout(function () {
                var url = 'https://wa.me/' + numero + '?text=' + encodeURIComponent(fullMsg);
                window.open(url, '_blank');
            }, index * 600);
        });
    });

})();