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

    //Function for changing color of text in Userdashboard
    NewItemAdded();


    //Client-side input field validation
    UserLoginValidateField();
    AdminLoginValidateField();

    DisplaySuccessAndError();
    const loginButton = document.getElementById('loginButton');
    if (loginButton) {
        loginButton.addEventListener('click', function () {
            window.location.href = '/';
        });
    }
});










