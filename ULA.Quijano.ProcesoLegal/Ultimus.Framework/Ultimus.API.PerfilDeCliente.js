/*------- VARIABLES GLOBALES -------*/
var DataTables = [
    ['Trident', 'Internet Explorer 4.0', 'Win 95+', '4', 'X'],   
    ['Gecko', 'Epiphany 2.20', 'Gnome', '1.8', 'A'],
    ['Webkit', 'Safari 1.2', 'OSX.3', '125.5', 'A'],   
    ['KHTML', 'Konqureror 3.5', 'KDE 3.5', '3.5', 'A'],
    ['Tasman', 'Internet Explorer 4.5', 'Mac OS 8-9', '-', 'X'],    
    ['Misc', 'Links', 'Text only', '-', 'X']   
];

jQuery(document).ready(function () {

    /*### REGION INICIALIZAR CONTROLES ###*/
    
    $("#formStatusCumplimiento").EnableValidationToolTip();

    $('#tbStatusFundacion').dataTable({
        "data": DataTables,
        "columns": [
            { "title": "Engine" },
            { "title": "Browser" },
            { "title": "Platform" },
            { "title": "Version", "class": "center" },
            { "title": "Grade", "class": "center" }
        ]
    });
    /*### END REGION INICIALIZAR CONTROLES ###*/


    /*### REGION EVENTOS ###*/

    $("#chkbReqInfoAdic").click(function () {
        var txtarea = document.getElementById("txtaComentario");
        if (this.checked) {
            txtarea.disabled = false;
        }
        else {
            txtarea.disabled = true;
        }

    });

    $("#btnPlantillaInstruccion").click(function () {

        if (!$("#formStatusCumplimiento").valid()) {
            toastr.warning("Hay campos que son requeridos por el formulario", Message_Warning);
            return false;
        }
        else {

            var ddlNuevoCliente = document.getElementById("ddlNuevoCliente");
            var ddlSocFund = document.getElementById("ddlSocFund");
            var ddlStatusCumpl = document.getElementById("ddlStatusCumpl");

            var ddlNuevoClienteOpcion = ddlNuevoCliente.options[ddlNuevoCliente.selectedIndex].value;
            var ddlSocFundOpcion = ddlSocFund.options[ddlSocFund.selectedIndex].value;
            var ddlStatusCumplOpcion = ddlStatusCumpl.options[ddlStatusCumpl.selectedIndex].value;

            if (ddlNuevoClienteOpcion == 0 && ddlSocFundOpcion == 0 && ddlStatusCumplOpcion == 1) {
                
            }
            else {
                toastr.info("Las condiciones del cliente no se cumplen para habilitar este boton.", Message_Info);
            }
        }

    });
    
    /*### END REGION EVENTOS ###*/


    /*### REGION FUNCIONES ###*/
        

    /*### END REGION FUNCIONES ###*/

   

    
});