//Info Modal - Login Page
function ShowPopInfoUp(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#info-modal .modal-body").html(res);
            //$("#view-myModal .modal-title").html(title);
            $("#info-modal").modal('show');
        },
        error: function (xhr, status, error) {
            alert("An error occured while loading the form: " + errror);
        }
    });
}



//Modal PopUp - CRUD Dashboard
function ShowModal(url) {
    try {
        $.ajax({
            type: "GET",
            url: url,
            success: function (res) {
                $("#crud-modal .modal-body").html(res);
                $("#crud-modal").modal('show');
            },
            error: function (xhr, status) {
                console.error("Error details: ", xhr.responseText);
                alert("An error occurred while loading the form: " + xhr.status + " " + xhr.statusText + " - " + error);
            }
        });
    }
    catch(ex){
        alert("An error occurred while loading the form: " + ex);
    }
}


 