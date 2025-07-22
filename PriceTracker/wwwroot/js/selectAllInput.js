window.selectAllNumericInput = function () {
    // Seleciona o input do campo numérico pelo atributo title
    var el = document.querySelector('input[title="Valor"]');
    console.log("selectAllNumericInput: input encontrado:", el);
    if (el) {
        el.select();
        console.log("select() chamado no input.");
    } else {
        console.log("Nenhum input numérico encontrado com title=Valor");
    }
}