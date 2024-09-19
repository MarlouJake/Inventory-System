
let spinner = `
    <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true">
    </span>
`;

let newModal = `
                    <div class="modal fade" id="display-modal" tabindex="-1" role="dialog" aria-labelledby="displaymodal" data-bs-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered">                               
                                <div class="modal-content">         
                                    <div class="modal-body">
                                        <!-- Content loaded dynamically -->
                                        <h5 class="modal-title"></h5>
                                        <p class="details-title"></p>
                                        <span class="modal-details h6 fs-6"></span>
                                        <div id="btn-footer" class="text-end">
                                            <button class="btn btn-primary mt-1" data-bs-dismiss="modal">
                                                Okay
                                            </button>
                                        </div>                                    
                                    </div>
                                </div>
                            </div>
                    </div>
                `;

let DynamicModal = `
                         <div id="dynamic-modal" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true"  style="z-index: 2000;" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered" role="document">
                                  <div class="modal-content">
                                        <div class="modal-body">
 
                                        </div>
                                  </div>
                           </div>
                        </div>
                `;


function ShowError(details) {
    var errormessage = "An Error Occurred";
    $('.modal').each(function () {
        $(this).modal('hide');
    });

    $('#error-message').text(errormessage).fadeIn().delay(300).fadeOut();

    var loginpage = $('#container-main');
    var userdashboard = $('#dashboard-main');

    if(loginpage) {
        loginpage.append(newModal);
    }
    if(userdashboard) {
        userdashboard.append(newModal);
    }
    
    $('#display-modal .details-title').text('Error Detials:').addClass("mt-1 mb-0");
    $('#display-modal .modal-details').html(details).addClass('text-danger mt-0 mb-1 p-0 text-center');
    $('#display-modal .modal-title').text(errormessage);
    $('#display-modal').modal('show').fadeIn();
};


function hideModal() {
    var myModal = document.getElementById('#info-modal');
    if (myModal) {
        var modal = bootstrap.Modal.getInstance(myModal);
        if (modal) {
            modal.hide();
        } else {
            // Initialize and show modal if instance does not exist
            var modal = new bootstrap.Modal(myModal);
            modal.hide();
        }
    }
};

function ValidateForm(data) {
    // Reset error messages and input styles
    $('#username-error').text('');
    $('#password-error').text('');
    $('.form-control').removeClass('input-error').addClass('input-success');

    let isValid = true; // Variable to track form validity

    // Manual validation for username
    if (!data.username || data.username.trim() === '') {
        $('#username').addClass('input-error');
        $('#username-error').text(validate.UsernameEmpty);
        isValid = false;
    } else if (data.username.length < 3) {
        $('#username').addClass('input-error');
        $('#username-error').text("Username or Email should be at least 3 characters");
        isValid = false;
    } else if (data.username.length > 64) {
        $('#username').addClass('input-error');
        $('#username-error').text("Username or Email shouldn't exceed 64 characters");
        isValid = false;
    }

    // Manual validation for password
    if (!data.password || data.password.trim() === '') {
        $('#password').addClass('input-error');
        $('#password-error').text(validate.PasswordEmpty);
        isValid = false;
    } else if (data.password.length < 8) {
        $('#password').addClass('input-error');
        $('#password-error').text("Password should be at least 8 characters");
        isValid = false;
    } else if (data.password.length > 128) {
        $('#password').addClass('input-error');
        $('#password-error').text("Password shouldn't exceed 128 characters");
        isValid = false;
    }

    // If form is invalid, log error and return false to prevent submission
    if (!isValid) {
        console.log('validation errors');
    }

    return isValid; // Return the form validity status
}