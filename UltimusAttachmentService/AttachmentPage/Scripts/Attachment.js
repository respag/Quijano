var server = GetRootUrl();

function GetRootUrl() {
    if (location.host == "localhost:57781")
        return "";
    else
        return "/AttachmentPage";
}

/*------- VARIABLES GLOBALES -------*/
jQuery(document).ready(function () {

    if ($("#enableUpload").val() == "True")
    {
        $(function () {
            'use strict';

            // Change this to the location of your server-side upload handler:
            var url = server + "/Home/UploadFile?processName=" + $("#process").val() + "&incident=" + $("#incident").val() + "&step=" + $("#etapa").val() + "&attachmentMaxSize=" + $("#attachmentMaxSize").val() + "&maxFiles=" + $("#maxFiles").val();
            var uploadButton = $("<button class='btn btn-primary btn-sm marginR5'>" +
                                     "<i class='glyphicon glyphicon-upload marginR5'></i>" +
                                      "<span>Subir</span>" +
                                 "</button>")
                    .on('click', function (event) {
                        event.preventDefault();
                        var $this = $(this),
                            data = $this.data();
                        //$this.off('click').text('Cancelar')
                        //    .on('click', function () {
                        //        data.abort();
                        //    });                    
                        data.submit().always(function (e) {
                            $("#uplFile").filestyle('clear');
                            if (e.result == 'OK') {
                                $this.parent().remove();
                                LoadExistingFiles();
                            }
                        });
                    });

            var cancelButton = $("<button class='btn btn-primary btn-sm'>" +
                                     "<i class='glyphicon glyphicon-ban-circle marginR5'></i>" +
                                      "<span>Cancelar</span>" +
                                 "</button>")
                    .on('click', function (event) {
                        event.preventDefault();
                        $(this).parent().remove();
                        $("#uplFile").filestyle('clear');
                        LoadExistingFiles();
                    });

            var acceptTypes = $("#acceptOnlyImages").val() == "1" ? /(.|\/)(gif|jpe?g|png|pdf)$/i : /(.|\/)(gif|jpe?g|png|pdf|rar|zip|xlsx|xls|exe|txt|docx|ppt)$/i;

            $('#uplFile').fileupload({
                url: url,
                dataType: 'json',
                autoUpload: false,
                autoSubmit: false,
                multiple: true,
                acceptFileTypes: acceptTypes,
                maxFileSize: $("#attachmentMaxSize").val(),
                maxNumberOfFiles: $("#maxFiles").val(),
                disableImageHead: true,
                disableImageMetaDataLoad: true
                // Enable image resizing, except for Android and Opera,
                // which actually support image resizing, but fail to
                // send Blob objects via XHR requests:
                //disableImageResize: /Android(?!.*Chrome)|Opera/
                //    .test(window.navigator.userAgent),
            }).on('fileuploadadd', function (e, data) {
                $.each(data.files, function (index, file) {
                    if (data.originalFiles[0]['size'] > $("#attachmentMaxSize").val()) {
                        $("#spnError").text("El Adjunto no debe superar los " + ($("#attachmentMaxSize").val() / (1024 * 1024)) + "MB.");
                        $("#LblError").removeAttr('style');
                        $("#uplFile").filestyle('clear');
                        return;
                    }

                    if (data.originalFiles[0]['type'].length && !acceptTypes.test(data.originalFiles[0]['name'])) {
                        $("#spnError").text("El tipo del adjunto no es aceptado.");
                        $("#LblError").removeAttr('style');
                        $("#uplFile").filestyle('clear');
                        return;
                    }

                    $("#LblError").attr('style', 'display: none');

                    var tplPresentation = $("#tblPresentation > tbody");
                    var tpl = $("<tr class='working template-upload fade in'>" +
                                "<td>" +
                                    "<span class='preview'><canvas width='80' height='37'></canvas></span>" +
                                "</td>" +
                                "<td>" +
                                    "<p class='name'>" + file.name + "</p>" +
                                    "<span style='font-weight: bold;' class='text-danger'\>" +
                                "</td>" +
                                "<td>" +
                                    "<p class='size'>" + formatFileSize(file.size) + "</p>" +
                                    "<div class='row'>" +
                                        "<div class='col-md-9'>" +
                                            "<div id='" + file.size + "' class='progress progress-striped active' role='progressbar' aria-valuemin='0' aria-valuemax='100' aria-valuenow='0'>" +
                                                "<div class='progress-bar progress-bar-success' style='width:0%;'></div>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class='col-md-3'>" +
                                            "<span style='float: left;' id='sp" + file.size + "'>0%</span>" +
                                        "</div>" +
                                    "</div>" +
                                "</td>");
                    tpl.append(uploadButton.clone(true).data(data));
                    tpl.append(cancelButton.clone(true));
                    tpl.append("</tr>");
                    data.context = tplPresentation.append(tpl);
                    $("#uplFile").filestyle('clear');
                });
                //}).on('fileuploadprocessalways', function (e, data) {
                //    var index = data.index,
                //        file = data.files[index],
                //        node = $(data.context.children()[index]);
                //    if (file.preview) {
                //        node
                //            .prepend('<br>')
                //            .prepend(file.preview);
                //    }
                //    if (file.error) {
                //        node
                //            .append('<br>')
                //            .append($('<span class="text-danger"/>').text(file.error));
                //    }
                //    if (index + 1 === data.files.length) {
                //        data.context.find('button')
                //            .text('Upload')
                //            .prop('disabled', !!data.files.error);
                //    }
            }).on('fileuploadprogressall', function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $("#" + data.total + " > div").css('width', progress + '%');
                $("#sp" + data.total).text(progress + '%');
                //}).on('fileuploaddone', function (e, data) {
                //    $.each(data.files, function (index, file) {
                //        if (file.url) {
                //            var link = $('<a>')
                //                .attr('target', '_blank')
                //                .prop('href', file.url);
                //            $(data.context.children()[index])
                //                .wrap(link);
                //        } else if (file.error) {
                //            var error = $("<span style='font-weight: bold;' class='text-danger'/>").text(file.error);
                //            $(data.context.children()[index])
                //                .append('<br>')
                //                .append(error);
                //        }
                //    });
            }).on('fileuploadfail', function (e, data) {
                $.each(data.files, function (index) {
                    var error = data._response.jqXHR.responseText;
                    $("p:contains('" + data.files[index].name + "')").parent().children().last().text('Error: [' + error + ']');
                    $("#" + data.files[index].size + " > div").css('width', '0%');
                });
            }).prop('disabled', !$.support.fileInput).parent().addClass($.support.fileInput ? undefined : 'disabled');

            // Prevent the default action when a file is dropped on the window
            $(document).on('drop dragover', function (e) {
                e.preventDefault();
            });

            // Helper function that formats the file sizes
            function formatFileSize(bytes) {
                if (typeof bytes !== 'number') {
                    return '';
                }

                if (bytes >= 1073741824) {
                    return (bytes / 1073741824).toFixed(2) + ' GB';
                }

                if (bytes >= 1048576) {
                    return (bytes / 1048576).toFixed(2) + ' MB';
                }

                return (bytes / 1024).toFixed(2) + ' KB';
            }

        });

        $("#LblError > button").click(function () {
            $("#LblError").attr('style', 'display: none');
        });
    }
    else
    {
        jQuery(function ($) {
            $('.panel-heading span.clickable').on("click", function (e) {
                if ($(this).hasClass('panel-collapsed')) {
                    // expand the panel
                    $(this).parents('.panel').find('.panel-body').slideDown();
                    $(this).removeClass('panel-collapsed');
                    $(this).find('i').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
                }
                else {
                    // collapse the panel
                    $(this).parents('.panel').find('.panel-body').slideUp();
                    $(this).addClass('panel-collapsed');
                    $(this).find('i').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
                }
            });
        });
    }
});

function LoadExistingFiles() {
    var url = server + "/Home/LoadFiles?processName=" + $("#process").val() + "&incident=" + $("#incident").val() + "&step=" + $("#etapa").val() + "&enableUpload=" + $("#enableUpload").val();

    $.ajax({
        url: url,
        type: "POST",
        data: {},
        async: false,
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        },
        success: function (response, textStatus, jqXHR) {
            var data = response;
            if (data != null) {
                var html = "";
                if ($("#view").val() == '1') {
                    $("#view1").empty();
                    $("#LblCount").text(data.length);
                    if (data.length > 0) {                        
                        $.each(data, function (key, item) {
                            html = "";
                            if (item.EnableDelete) {
                                html = "<itemtemplate>" +
                                    "<div class='col-xs-6 col-sm-4 col-md-3 text-center'>" +
                                    "<div class='thumbnail'>" +
                                    "<a id='btnDeleteFile' class='pull-left' href=\"javascript:DeleteFile('" + item.FileName + "')\">" +
                                    "<img src='Images/trash-16.png'/>" +
                                    "</a>" +
                                    "<a id='btnGetFile' class='pull-left' href=\"javascript:GetFile('" + item.FileName + "')\">" +
                                        "<img src='Images/download-16.png' />" +
                                    "</a>" +
                                    "<div class='center-block'>" +
                                        "<img src='Images/Icons/" + item.FileType + ".png' />" +
                                    "</div>" +
                                    "<h6>" + item.FileName + "</h6>" +
                                    "</div>" +
                                    "</div>" +
                                    "</itemtemplate>";
                            }
                            else {
                                html = "<itemtemplate>" +
                                    "<div class='col-xs-6 col-sm-4 col-md-3 text-center'>" +
                                    "<div class='thumbnail'>" +
                                    "<a id='btnGetFile' class='pull-left' href=\"javascript:GetFile('" + item.FileName + "')\">" +
                                        "<img src='Images/download-16.png' />" +
                                    "</a>" +
                                    "<div class='center-block'>" +
                                        "<img src='Images/Icons/" + item.FileType + ".png' />" +
                                    "</div>" +
                                    "<h6>" + item.FileName + "</h6>" +
                                    "</div>" +
                                    "</div>" +
                                    "</itemtemplate>";
                            }
                            $("#view1").append(html);
                            
                        });                        
                    }

                }
                else {
                    $('#view2').empty();
                    $("#LblCount").text(data.length);
                    if (data.length > 0) {
                        html = "<itemtemplate>" +
                                "<tr>" +
                                    "<td>" +
                                        "<img src='Images/Icons/" + item.FileType + ".png' />" +
                                    "</td>" +
                                    "<td>" +
                                        "<label>@(attachment.FileName)</label>" +
                                    "</td>" +
                                    "<td>" +
                                        "<a id='btnGetFile' class='pull-left' href=\"javascript:GetFile('" + item.FileName + "')\">" +
                                            "<img src='Images/download-16.png' />" +
                                        "</a>" +
                                    "</td>" +
                                    "<td>" +
                                        "<a id='btnDeleteFile' class='pull-left' href=\"javascript:DeleteFile('" + item.FileName + "')\">" +
                                            "<img src='Images/trash-16.png' />" +
                                        "</a>" +
                                    "</td>" +
                                "</tr>" +
                                "</itemtemplate>";
                        $("#view2").append(html);
                    }
                }
            }
            else {

                //toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
            }
        }
    });
}

function DeleteFile(fileName) {
    var url = server + "/Home/DeleteFile?processName=" + $("#process").val() + "&incident=" + $("#incident").val() + "&step=" + $("#etapa").val() + "&fileName=" + encodeURIComponent(fileName);

    $.ajax({
        url: url,
        type: "POST",
        data: {},
        async: false,
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            //jqXHR.responseText
            //toastr.error("Ha ocurrido un error inesperado. Vuelva a intentarlo mas tarde", Message_Error);
        },
        success: function (response, textStatus, jqXHR) {
            LoadExistingFiles();
        }
    });
}

function GetFile(fileName) {
    window.location = server + "/Home/GetFile?processName=" + $("#process").val() + "&incident=" + $("#incident").val() + "&step=" + $("#etapa").val() + "&fileName=" + fileName;
}