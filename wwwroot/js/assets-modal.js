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
    catch (ex) {
        alert("An error occurred while loading the form: " + ex);
    }
}

function checkSession() {
    fetch('/api/check-session')
        .then(response => response.json())
        .then(data => {
            if (!data.isValid) {
                // Define the modal HTML content
                const sessionTimeout = `
                        <div class="modal fade" id="sessionTimeoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Session Timeout</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        Your session has expired. Please log in again.
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-primary" id="loginButton">Go to Login Page</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `;

                // Append the modal HTML to the body and show it
                $('body').append(sessionTimeout);
                $('#sessionTimeoutModal').modal('show');
                $('#sessionTimeoutModal').on('hidden.bs.modal', function () {
                    window.location.href = '/'; // Redirect to login page when modal is closed
                });
            }
        });
}



// Check session validity periodically
//setInterval(checkSession, 60000); // Check every minute


