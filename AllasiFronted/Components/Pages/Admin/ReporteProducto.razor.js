window.descargarArchivo = (nombreArchivo, tipoContenido, base64) => {
    const link = document.createElement("a");

    link.download = nombreArchivo;
    link.href = `data:${tipoContenido};base64,${base64}`;

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};