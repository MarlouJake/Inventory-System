async function ModalShow(url) {
    var dashboard = $('#dashboard-layout');
    var home = $('#layoutbody');
    $('.modal').each(function () {
        $(this).modal('hide');
    });
    $('#dynamic-modal').remove();

    setTimeout(async () => {
        if (dashboard) {
            dashboard.append(DynamicModal);
        }
        if (home) {
            home.append(DynamicModal);
        }       
        await GetData(url);
    },10);
};

async function GetData(url) {
    try {
        const response = await $.ajax({
            type: 'GET',
            url: url,  
            cache: true
        });
        $('#dynamic-modal .modal-body').html(response);
        $('#dynamic-modal').modal('show');
    }
    catch (error) {
        $('#dynamic-modal').modal('hide');
        console.error("Error details: ", error.responseText);
        alert("An error occurred while loading the form: " + error.status + " " + error.statusText);
    }
}

function checkSession() {
    fetch('/api/check-session')
        .then(response => response.json())
        .then(data => {
            if (!data.isValid) {
                // Define the modal HTML content
                const sessionTimeout = `
                        <div class="modal fade" id="sessionTimeoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered" role="document">
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

