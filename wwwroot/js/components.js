
let spinner = `
    <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true">      
    </span>
`;

let overlayDiv = ''

let spinnerOnly = ` <div class="text-center">
                            ${spinner}
                    </div>`;

let spinnerContainer = ` <div  class="text-center ms-5 mt-5 ">
                            <p class="ms-1">${spinner} Loading...</p>
                        </div>`;


let noItemContainer = ` <div  class="text-center text-muted ms-5 mt-5 ps-3">
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

let defaultTitle = `
                    <h6 id="default-header" class="d-flex gap-2 default-header" style="padding: 1rem 0.8rem; padding-bottom: 0.7rem;">
                        <span class="default-icon"><i class="bi bi-box-seam-fill"></i></span>
                        <span class="default-title">Title</span>
                    </h6>
        `;

let navDropdown = `
                <div class="custom-select">
                    <button class="select-button">
                        <span class="ms-1 view-title"></span>
                        <span class="dropdown-icon ms-4.5">
                            <i class="bi bi-caret-down-fill"></i>
                        </span>
                    </button>

                    <ul class="dropdown-options nav tabs flex-grow-1 mt-0">
                        <li class="nav-item m-0 p-0">
                            <a class="nav-link sidebar-link" id="dashboardlink" data-url="dashboard">
                                <i class="bi bi-speedometer2"></i>
                                <span class="ms-2-7">Dashboard</span>
                            </a>
                        </li>

                        <li class="nav-item tab m-0 p-0">
                            <a class="nav-link sidebar-link" id="inventory-link" data-url="inventory" data-string="All">
                                <i class="bi bi-boxes"></i>
                                <span class="ms-2-7">Inventory</span>
                            </a>
                        </li>

                        <li class="nav-item m-0 p-0">
                            <a class="nav-link sidebar-link" id="request-link" data-url="requests">
                                <i class="fa-solid fa-user-group"></i>
                                <span class="ms-2-4">Requests</span>
                            </a>
                        </li>

                        <li class="nav-item m-0 p-0 ">
                            <a class="nav-link d-fle w-100" id="user-logout">
                                <i class="bi bi-box-arrow-left text-danger"></i>
                                <span class="ms-2-9">Logout</span>
                            </a>
                        </li>
                    </ul>
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

