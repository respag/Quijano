Ultimus.UI = function () {

    this.AddSelectOption = function (_selectId, _optionId, _text) {

        $(_selectId)
         .append($("<option></option>")
         .attr("value", _optionId)
         .text(_text));
    };

    this.CleanSelectControl = function (_selectId) {

        $(_selectId).empty();

        listItems = "<option value='' ></option>";

        $(_selectId).append(listItems);


        $(_selectId).val("");
    };


    this.CleanSelectControlfecha = function (_selectId,tipo) {

        $(_selectId).empty();


        if (tipo == "dia") {

            listItems = "<option value=''>DIA</option>";

        }
        else if (tipo == "mes") {

            listItems = "<option value=''>MES</option>";
        }
        else if (tipo == "anio") {

            listItems = "<option value=''>AÑO</option>";

        }

       
        $(_selectId).append(listItems);
    };


    this.StartSelectControlRequest = function (_selectId) {

        $(_selectId).empty();

        listItems = "<option value='-1' ><i class=\"fa fa-spinner fa-spin\"></i>Cargando catalogos, Por favor Espere...</option>";

        $(_selectId).append(listItems);
    };

    this.ENDREQUEST = function (text) {
        $("#loadingGif").hide();
    };


    this.DataTable_Language_Search = function (idControl, rowName) {
        var i = 0;
        var DataTable;

        $("#" + idControl).DataTable().clear().destroy();

        DataTable = $("#" + idControl).DataTable({
            "bInfo": false,
            "bPaginate": false,
            "language": { "sSearch": "Buscar:", "sZeroRecords": "No se encontraron resultados" },
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                $(nRow).attr('id', rowName + i++)
            }
        });
        return DataTable;
    }
         
    this.DataTable_Language_NoSearch = function (idControl, rowName) {
        var i = 0;
        var DataTable;

        $("#" + idControl).DataTable().clear().destroy();

        DataTable = $("#" + idControl).DataTable({
            "bFilter": false,
            "bInfo": false,
            "bPaginate": false,
            "language": { "sSearch": "Buscar:", "sZeroRecords": "No se encontraron resultados" },
            "fnCreatedRow": function (nRow, aData, iDataIndex) {
                $(nRow).attr('id', rowName + i++)
            }
        });
        return DataTable;
    }

    //todo: obsoleto
    this.dataTable_language = function (idControl) {
        $(idControl).dataTable({
            "language": {
                "sProcessing": "Procesando...",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "Ningún dato disponible",
                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Buscar:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                },
                "oAria": {
                    "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                    "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                }
            },
            "order": [[0, "desc"]]
        });
    };

   

    //todo: obsoleto
    this.dataTable_language_NoSearch = function (idControl) {
        $(idControl).dataTable({
            "language": {
                "sProcessing": "Procesando...",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "Ningún dato disponible",
                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Buscar:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                },
                "oAria": {
                    "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                    "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                }
            },
            //"columnDefs": [
            // { "width": "8%", "targets": 6 }
            //],
            "order": [[0, "desc"]],
            "bPaginate": false,
            "bFilter": false,
            "bInfo": false
        });
    };

    this.DataTable_Spanish = function (idControl, columnOrder, order, bPaginate, bFilter, bInfo) {
        if (idControl == "#tableBitacora") {
            $(idControl).dataTable({
                "columnDefs": [
                    { "targets": [2], "visible": false, "searchable": false },
                    { "orderData": [2], "targets": [1] }
                ],
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                "order": [[columnOrder, order]],
                "bPaginate": bPaginate,
                "bFilter": bFilter,
                "bInfo": bInfo
            });
        } else {
            $(idControl).dataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                "order": [[columnOrder, order]],
                "bPaginate": bPaginate,
                "bFilter": bFilter,
                "bInfo": bInfo
            });
        }
    };



    this.TableEditDeleteOption = function (idControl, key) {

        var html = "";

        html += "<div class=\"btn-group\">";
        html += " <button type=\"button\" class=\"btn btn-info btn-xs\" onclick=\"javascript: Modificar_" + idControl + "_OnClick('" + key + "')\"><i class=\"fa fa-pencil-square-o\"></i>&nbsp;Modificar</button>";
        html += " <button type=\"button\" class=\"btn btn-danger btn-xs\" onclick=\"javascript: Eliminar_" + idControl + "_OnClick('" + key + "')\"><i class=\"fa fa-eraser\"></i>&nbsp;Eliminar</button>";
        html += "</div>";

        return html;
    };

    this.TableElegirTerminos = function (idControl, key) {

        var html = "";

        html += "<div class=\"btn-group\">";
        html += " <button type=\"button\" class=\"btn btn-info btn-xs\" onclick=\"javascript: TableElegirTerminos_" + idControl + "_OnClick('" + key + "')\"><i class=\"fa fa-paper-plane\"></i>&nbsp;Modificar</button>";
        html += "</div>";

        return html;
    };

   

  
};
