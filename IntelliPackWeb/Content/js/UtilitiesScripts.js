/// Funcion Utilizada para obtener la visualizacion de los editables de configuración para cargas FTP
function saveModalConfig(element, nameResultObject) {

    $.ajax({
        url: $(element).data("url"),
        type: 'GET',
        // we set cache: false because GET requests are often cached by browsers
        // IE is particularly aggressive in that respect
        cache: false,
        data: $("#AddConfigUpdate").serialize(),
        success: function (response) {
            $('#' + nameResultObject).html(response);
           
                $(element).hide();
        },
        error: function () {
            alert("error");
        }
    });

}

/// Funcion Utilizada para obtener la visualizacion de los editables de configuración para cargas FTP
function saveModalConfigForFile(element, nameResultObject) {
    var dataFile = new FormData($("#AddConfigUpdate").get()[0]);
    $.ajax({
        url: $(element).data("url"),
        type: 'POST',
        dataType: "json",
        contentType: false,
        processData: false,
        // we set cache: false because GET requests are often cached by browsers
        // IE is particularly aggressive in that respect
        cache: false,
        data: dataFile,
        success: function (response) {
            $('#' + nameResultObject).html(response);
            
            //alert(response);

            if (response.includes("Error")) {
                alert(response);
            }
            else {
                $(element).hide();
            }
        },
        error: function () {
            alert("error");
        }
    });

}
/// Funcion Utilizada para obtener la visualizacion de los editables de configuración para cargas FTP
function saveModalConfigInfo(element, nameResultObject, invoice, selectionId, originUrl) {

    $.ajax({
        url: $(element).data("url"),
        type: 'POST',
        // we set cache: false because GET requests are often cached by browsers
        // IE is particularly aggressive in that respect
        cache: false,
        data: $("#AddConfigUpdate").serialize(),
        success: function (response) {
            $('#' + nameResultObject).html(response);
            //$(element).attr("disabled", "true");
            $(element).hide();
            
            if (selectionId === '1')
            {
                window.open(invoice, '_blank');
                window.location.assign(originUrl);
            }            
        },
        error: function () {
            alert("error");
        }
    });

}

/// Funcion Utilizada para obtener la visualizacion de los editables de configuración para cargas FTP
function showModalConfig(element, Rol_id, partialView) {

    $.ajax({
        url: $(element).data("url"),
        type: 'GET',
        // we set cache: false because GET requests are often cached by browsers
        // IE is particularly aggressive in that respect
        cache: false,
        data: { Id: Rol_id, partial_view: partialView },
        success: function (response) {
            $('#content-modal_info').html(response);
            $('#modalDetail').modal();
        },
        error: function () {
            alert("error");
        }
    });

}


/// Funcion Utilizada para obtener la visualizacion de los editables de configuración para cargas FTP
function showModalConfigTwoParam(element, Rol_id, tracking,partialView) {

    $.ajax({
        url: $(element).data("url"),
        type: 'GET',
        // we set cache: false because GET requests are often cached by browsers
        // IE is particularly aggressive in that respect
        cache: false,
        data: { Id: Rol_id, tracking: tracking, partial_view: partialView },
        success: function (response) {
            $('#content-modal_info').html(response);
            $('#modalDetail').modal();
        },
        error: function () {
            alert("error");
        }
    });

}