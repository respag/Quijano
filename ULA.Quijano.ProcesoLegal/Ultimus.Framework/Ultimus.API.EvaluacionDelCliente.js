



jQuery(document).ready(function () {
	
	/*### REGION INICIALIZAR CONTROLES ###*/
	$("#formEvaluacionRiesgo").EnableValidationToolTip();

	/*### END REGION INICIALIZAR CONTROLES ###*/


	/*### REGION EVENTOS ###*/
	$("#btnEvaluarCliente").click(function () {
		if (!$("#formEvaluacionRiesgo").valid()) {
			toastr.warning("Hay campos que son requeridos por el formulario", Message_Warning);
			return false;
		}
		else {
			toastr.info("Cliente evaluado...", Message_Info);
		}
	});
	/*### END REGION EVENTOS ###*/
});