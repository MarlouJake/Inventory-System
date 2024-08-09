// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.
/*
$(() => {
    $('#modal-create-form').on('click', function () {
        $('#createUserModal').modal('show');
    });
});
*/

/*
document.addEventListener('DOMContentLoaded', () => {
    const toggleButtons = document.querySelectorAll('.toggle-password');

    toggleButtons.forEach(button => {
        button.addEventListener('click', () => {
            const targetId = button.getAttribute('data-target');
            const passwordField = document.getElementById('targetId');

            if (passwordField) {
                // Toggle the type attribute
                const type = passwordField.type === 'password' ? 'text' : 'password';
                passwordField.type = type;

                // Optionally, change the button icon based on the type
                const icon = button.querySelector('i');
                if (icon) {
                    if (type === 'password') {
                        icon.classList.remove('fa-eye-slash');
                        icon.classList.add('fa-eye');
                    } else {
                        icon.classList.remove('fa-eye');
                        icon.classList.add('fa-eye-slash');
                    }
                }
            }
        });
    });
});
*/
// Function to hide the modal
/*
function hideModal() {
    var myModal = document.getElementById('view-myModal');
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
}

// Attach event listeners to all elements with class 'btn-closeModal'
var closeButtons = document.getElementsByClassName('btn-closeModal');
Array.prototype.forEach.call(closeButtons, function (button) {
    button.addEventListener('click', hideModal);
});


*/
/*
document.addEventListener("DOMContentLoaded", function () {
    var errorMessage = document.getElementById('error-message');
    if (errorMessage) {
        setTimeout(function () {
            errorMessage.style.display = 'none';
        }, 5000); // Change 5000 to the number of milliseconds you want the message to be visible
    }
});
*/

document.addEventListener('DOMContentLoaded', () => {
    /*
    var successMessage = $("#success-message");

    if (successMessage.text().trim() !== "") {
        successMessage.fadeIn().delay(1500).fadeOut(); // Show for 1.5 seconds
    }
    */
    

 
    NewItemAdded();

    // Show success message if it exists
    const successMessage = document.getElementById('success-message');
    if (successMessage && successMessage.textContent.trim() !== "") {
        successMessage.style.display = 'block';
        setTimeout(() => successMessage.style.display = 'none', 1500); // Show for 1.5 seconds
    }

    // Show error message if it exists
    const errorMessage = document.getElementById('error-message');
    if (errorMessage && errorMessage.textContent.trim() !== "") {
        errorMessage.style.display = 'block';
        setTimeout(() => errorMessage.style.display = 'none', 1500); // Show for 1.5 seconds
    }

    const userInput = document.getElementsByClassName('user-input');
    function trimInput(element) {
        element.value = element.value.trim();
    }
   
});


function NewItemAdded() {
    const statusDisplays = document.getElementsByClassName('status-container');
    const firmwareUpdateDisplay = document.getElementsByClassName('firmware-update');


    Array.from(statusDisplays).forEach(statusDisplay => {
        if (statusDisplay.textContent.trim() === "Complete") {
            statusDisplay.classList.add('text-success');
        } else if (statusDisplay.textContent.trim() === "Incomplete(Usable)") {
            statusDisplay.classList.add('text-primary');
        } else if (statusDisplay.textContent.trim() === "Incomplete(Unusable)") {
            statusDisplay.classList.add('text-danger');
        }
    });

    Array.from(firmwareUpdateDisplay).forEach(firmwareUpdateDisplay => {
        if (firmwareUpdateDisplay.textContent.trim() === "YES") {
            firmwareUpdateDisplay.classList.add('text-success');
        } else if (firmwareUpdateDisplay.textContent.trim() === "N/A") {
            firmwareUpdateDisplay.classList.add('text-muted');
        } else if (firmwareUpdateDisplay.textContent.trim() === "NO") {
            firmwareUpdateDisplay.classList.add('text-danger');
        }
    });

   
}


/*ShowPopUp(url, title)*/
function ShowPopUp(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#view-myModal .modal-body").html(res);
            //$("#view-myModal .modal-title").html(title);
            $("#view-myModal").modal('show');
        },
        error: function (xhr, status, error) {
            alert("An error occured while loading the form: " + errror);
        }
    });
}

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


function ShowPopUpLoginPage(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#view-myModal .modal-body").html(res);
            //$("#view-myModal .modal-title").html(title);
            $("#view-myModal").modal('show');
        },
        error: function (xhr, status, error) {
            var errorMsg = "An error occured while loading the form: " + errror;
            $("#error-message").text(errorMsg).fadeIn().delay(500).fadeOut(); 
        }
    });
}
/*
function ShowPopUpLoginPage(url) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#index-myModal .modal-body").html(res);
            //$("#view-myModal .modal-title").html(title);
            $("#index-myModal").modal('show');
        },
        error: function (xhr, status, error) {
            alert("An error occured while loading the form: " + errror);
        }
    });
}

*/
jQueryAjaxPost = form => {

    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $("#view-all").html(res.html);
                    $("#view-myModal .modal-body").html('');
                    $("#view-myModal").modal('hide');
                    NewItemAdded();
                    // Show success message
                    $("#success-message").text(res.successMessage).fadeIn().delay(500).fadeOut(); 


                }
                else {
                    $("#view-myModal .modal-body").html(res.html);
                    $("#error-message").text(res.failedMessage).fadeIn().delay(500).fadeOut(); 
                    console.error('Attempt failed: "', res.failedMessage + ' "');
                }
            },
            error: function (err) {
                console.error('AJAX Error: ', err.responseText);
            }

        })
    }
    catch (ex) { 
        console.log("Exception caught: ", ex);
    }

    //prevent default form submit event
    return false;
}

jQueryAjaxLoginPost = form => {

    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {           
                if (res.isValid) {
                    //Show the loading modal and prevent it from closing on outside click
                    $('#loading-modal').modal({
                        backdrop: 'static',
                        keyboard: false
                    }).modal('show');
                    // Show success message
                    $("#success-message").text(res.successMessage).fadeIn().delay(2000).fadeOut(); // Show for 2 seconds

                    //$("#loading-animation").show();

                    //$('#response-message').removeClass('alert-danger').addClass('alert-success').text(response.successMessage).show();


                    // Redirect to the admin page
                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000); // Ensure redirect happens after the success message fades out
                    

                }
                else {
                    // Hide loading modal
                    setTimeout(() => $('#loading-modal').modal('hide'),500); // Ensure modal hides after error message fades out
                    $("#error-message").text(res.failedMessage).fadeIn().delay(500).fadeOut();;
                    console.log("Error login:" + res.failedMessage);
                    //$('#error-message').removeClass('alert-success').addClass('alert-danger').text(response.errorMessage).show();
                }
            },
            error: function (err) {
                // Hide loading modal
                setTimeout(() => $('#loading-modal').modal('hide'), 500); // Ensure modal hides after error message fades out
                console.error('AJAX Error: ', err.responseText);
                $("#error-message").text(res.failedMessage).fadeIn().delay(500).fadeOut();
                console.log("Error posting: " + err);
                
            }

        })
    }
    catch (ex) {
        // Hide loading modal
        setTimeout(() => $('#loading-modal').modal('hide'), 500); // Ensure modal hides after error message fades out
        console.log("Exception caught: ", ex);    
        $("#error-message").text(res.errorMessage).fadeIn().delay(500).fadeOut();
    }

    //prevent default form submit event
    return false;
}







