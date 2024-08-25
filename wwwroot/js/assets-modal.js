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


//ShowPopUp(url, title)
//Modal PopUp - CRUD Dashboard
function ShowPopUp(url) {
    try {
        $.ajax({
            type: "GET",
            url: url,
            success: function (res) {
                $("#view-myModal .modal-body").html(res);
                //$("#view-myModal .modal-title").html(title);
                $("#view-myModal").modal('show');
            },
            error: function (xhr, status) {
                alert("An error occured while loading the form: " + { status });
            }
        });
    }
    catch(ex){
        alert("An error occured while loading the form: " + { ex });
    }
}