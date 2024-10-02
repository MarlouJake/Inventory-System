
let spinner = `
    <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true">      
    </span>
`;

let overlayDiv = ''

let spinnerContainer = ` <div  class="text-center ms-5 mt-5 " style="width: 100vw;">
                            <p class="ms-1">${spinner} Loading...</p>
                        </div>`;


let noItemContainer = ` <div  class="text-center text-muted ms-5 mt-5 ps-3" style="width: 100vw;">
                            <p>No items found</p>
                        </div>`;
let noContent = `<div class="text-nowrap"> <p class="ms-1">No content found</p> </div>`;

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

