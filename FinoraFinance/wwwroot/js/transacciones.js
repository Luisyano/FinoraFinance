document.addEventListener('DOMContentLoaded', function () {
    const tipoBtns = document.querySelectorAll('.tipo-btn');
    const tipoInput = document.getElementById('Tipo');

    if (tipoBtns.length > 0 && tipoInput) {
        tipoBtns.forEach(btn => {
            btn.addEventListener('click', function () {
                tipoBtns.forEach(b => b.classList.remove('active'));
                this.classList.add('active');
                tipoInput.value = this.dataset.tipo;
            });
        });
    }


    const cuentaSelect = document.getElementById('CuentaId');
    const monedaSpan = document.getElementById('monedaIndicador');

    if (cuentaSelect && monedaSpan) {
        cuentaSelect.addEventListener('change', function () {
            const text = this.options[this.selectedIndex]?.text || '';
            if (text.includes('PEN')) monedaSpan.textContent = 'PEN';
            else if (text.includes('USD')) monedaSpan.textContent = 'USD';
        });
    }


    const modal = document.getElementById('modalEtiqueta');
    const btnNueva = document.getElementById('btnNuevaEtiqueta');
    const cerrar = document.getElementById('cerrarModal');
    const guardar = document.getElementById('guardarEtiqueta');

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
            const nombre = document.getElementById('nuevaEtiquetaNombre').value;
            if (!nombre) return;

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
                        const select = document.getElementById('EtiquetaId');
                        const option = document.createElement('option');
                        option.value = data.id;
                        option.text = nombre;
                        select.appendChild(option);
                        select.value = data.id;
                        if (modal) modal.style.display = 'none';
                        document.getElementById('nuevaEtiquetaNombre').value = '';
                        document.getElementById('nuevaEtiquetaDescripcion').value = '';
                    }
                });
        };
    }

    window.onclick = function (event) {
        if (event.target == modal && modal) {
            modal.style.display = 'none';
        }
    };
});