window.descargarArchivo = (nombreArchivo, tipoContenido, base64) => {
    const link = document.createElement("a");

    link.download = nombreArchivo;
    link.href = `data:${tipoContenido};base64,${base64}`;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.dibujarGraficoBarras = (canvasId, labels, data, titulo) => {
    const canvas = document.getElementById(canvasId);

    if (!canvas) {
        console.error(`No se encontró el canvas con id: ${canvasId}`);
        return;
    }

    if (typeof Chart === "undefined") {
        console.error("Chart.js no se encuentra cargado.");
        return;
    }

    // Destruye el gráfico anterior cuando se cambia el filtro.
    const graficoAnterior = Chart.getChart(canvas);

    if (graficoAnterior) {
        graficoAnterior.destroy();
    }

    new Chart(canvas, {
        type: "bar",

        data: {
            labels: labels,
            datasets: [
                {
                    label: titulo,
                    data: data,
                    backgroundColor: "rgba(16, 28, 69, 0.85)",
                    borderColor: "#101c45",
                    borderWidth: 1,
                    borderRadius: 6
                }
            ]
        },

        options: {
            responsive: true,
            maintainAspectRatio: false,

            plugins: {
                legend: {
                    display: false
                },

                title: {
                    display: true,
                    text: titulo
                },

                tooltip: {
                    callbacks: {
                        label: context => {
                            const valor = Number(context.raw);

                            return `S/ ${valor.toLocaleString("es-PE", {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2
                            })}`;
                        }
                    }
                }
            },

            scales: {
                y: {
                    beginAtZero: true,

                    ticks: {
                        callback: value =>
                            `S/ ${Number(value).toLocaleString("es-PE")}`
                    }
                }
            }
        }
    });
};

window.destruirGraficoBarras = canvasId => {
    if (typeof Chart === "undefined") {
        return;
    }

    const grafico = Chart.getChart(canvasId);

    if (grafico) {
        grafico.destroy();
    }
};

window.exportarGrafico = (canvasId, nombreArchivo) => {
    const canvasOriginal = document.getElementById(canvasId);

    if (!canvasOriginal) {
        console.error(`No se encontró el gráfico: ${canvasId}`);
        return;
    }

    // Canvas temporal para agregar fondo blanco
    const canvasExportacion = document.createElement("canvas");

    canvasExportacion.width = canvasOriginal.width;
    canvasExportacion.height = canvasOriginal.height;

    const contexto = canvasExportacion.getContext("2d");

    // Fondo blanco
    contexto.fillStyle = "#ffffff";
    contexto.fillRect(
        0,
        0,
        canvasExportacion.width,
        canvasExportacion.height
    );

    // Copiar la gráfica encima del fondo blanco
    contexto.drawImage(canvasOriginal, 0, 0);

    const enlace = document.createElement("a");

    enlace.href = canvasExportacion.toDataURL("image/png", 1.0);
    enlace.download = nombreArchivo || "dashboard-ventas.png";

    document.body.appendChild(enlace);
    enlace.click();
    enlace.remove();
};
