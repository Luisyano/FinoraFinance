document.addEventListener('DOMContentLoaded', function () {
    var modal = document.getElementById('modalEtiqueta');
    var btnNueva = document.getElementById('btnNuevaEtiqueta');
    var cerrar = document.getElementById('cerrarModal');
    var guardar = document.getElementById('guardarEtiqueta');

    if (btnNueva) {
        btnNueva.onclick = function () {
            if (modal) modal.style.display = 'flex';
        };
    }

    if (cerrar) {
        cerrar.onclick = function () {
            if (modal) modal.style.display = 'none';
        };
    }

    if (guardar) {
        guardar.onclick = function () {
            var nombre = document.getElementById('nuevaEtiquetaNombre').value;
            if (nombre) {
                fetch('/Etiquetas/CrearAjax', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        Nombre: nombre,
                        Descripcion: document.getElementById('nuevaEtiquetaDescripcion').value
                    })
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            location.reload();
                        }
                    });
            }
        };
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            if (modal) modal.style.display = 'none';
        }
    };
});