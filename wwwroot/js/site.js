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

    //UserLogoutAction();
    DisplaySuccessAndError();
    const loginButton = document.getElementById('loginButton');
    if (loginButton) {
        loginButton.addEventListener('click', function () {
            window.location.href = '/';
        });
    }
 
});

function getScreenSize() {
    var width = window.innerWidth;
    var height = window.innerHeight;
    //console.log("Width: " + width + ", Height: " + height);

    // Update the element with ID 'screensizer' with the screen size information
    $('#screensizer').text("Width: " + width + ", Height: " + height);
}

// Call the function to get the initial screen size
getScreenSize();

// Optionally, you can add an event listener to handle window resize events
window.addEventListener('resize', function () {
    getScreenSize();
});


const NoInternetPopUp = `
    <div id="NoInternetPopUp" style="display: none; z-index: 2000; position: fixed; top: 5px; left: 50%; transform: translateX(-50%); padding: 5px; background-color: #FF5252; color: white; border-radius: 5px;">
    </div>
                        `;

const NoInternet =  `
                        <div class="modal fade" id="noInternetModal" style="z-index: 2000;" tabindex="-1" aria-labelledby="noInternetModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
                            <div class="modal-dialog modal-dialog-centered modal-lg modal-sm modal-md">
                                <div class="modal-content">
                                    <div class="modal-body">
                                        <span class="text-danger">
                                            <h5 class="modal-title" id="noInternetModalLabel">No Internet Connection</h5>
                                        </span>
                                        
                                        It looks like you are not connected to the internet. Please check your connection and try again.

                                    </div>
                                </div>
                            </div>
                        </div> 
                        
                     `;



let disconnected = false;
function checkInternetConnection() {
    
    if (navigator.onLine) {
        // Connection is available
     
        $('#noInternetModal').modal('hide').fadeOut().remove();  
        $('#NoInternetPopUp').remove();
        if(disconnected){
             $('#success-message').text('Connected to internet').fadeIn().delay(1000).fadeOut();
             disconnected = false;
        }
       
    } else {
        // No internet connection
        disconnected = true;
        showNoInternetConnectionMessage();
    }
}

function showNoInternetConnectionMessage() {
    // Check if the modal or message element exists
    var homescreen = $('#container-main');
    var homebody = $('#layoutbody');
    var dashboardbody = $('#dashboard-layout');
    if(homescreen) {
        homescreen.append(NoInternet);
        homebody.append(NoInternetPopUp);
    }

    if (dashboardbody) {
        dashboardbody.append(NoInternet);
        dashboardbody.append(NoInternetPopUp);
    }
    
    var noInternetModal = document.getElementById('noInternetModal');
    if (noInternetModal) { 
        $('#noInternetModal').modal('show').fadeIn();
        $('#NoInternetPopUp').text('No internet connection').fadeIn().delay(500).fadeOut();
    } else {
        $('#NoInternetPopUp').text('No internet connection').fadeIn().delay(5000).fadeOut();
    }
}

// Check the internet connection status when the page loads
document.addEventListener('DOMContentLoaded', checkInternetConnection);

// Optionally, you can add an event listener to handle online/offline events
window.addEventListener('online', checkInternetConnection);
window.addEventListener('offline', checkInternetConnection);






