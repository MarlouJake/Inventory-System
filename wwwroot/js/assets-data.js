jQueryAjaxPost = form => {

    try {
        const token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            headers: {
                'RequestVerificationToken': token
            },
            success: function (res) {
                console.log("AJAX Success Response:", res);
                if (res.isValid) {
                    $("#view-all").html(res.html);
                    $("#view-myModal .modal-body").html('');
                    $("#view-myModal").modal('hide');
                    NewItemAdded();
                    // Show success message
                    $("#success-message").text(res.successMessage).fadeIn().delay(500).fadeOut();


                }
                else {
                    console.log("AJAX Fail Response:", res);
                    $("#view-myModal .modal-body").html(res.html);
                    $("#error-message").text(res.failedMessage).fadeIn().delay(500).fadeOut();
                    console.error('Attempt failed: "', res.failedMessage + ' "'); showMessage(response.failedMessage, 'error');
                    showMessage(response.failedMessage, 'error');
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

jQueryAjaxAdminPost = form => {

    try {
        //const token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            /*headers: {
                'RequestVerificationToken': token
            },*/
            success: function (res) {
                console.log("AJAX Success Response:", res);
                if (res.isValid) {
                    $("#admin-view").html(res.html);
                    $("#view-myModal .modal-body").html('');
                    $("#view-myModal").modal('hide');
                    NewItemAdded();
                    // Show success message
                    $("#success-message").text(res.successMessage).fadeIn().delay(500).fadeOut();


                }
                else {
                    console.log("AJAX Fail Response:", res);
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
                    $('#loading-modal .modal-body').html(`
                        <div class="text-center">
                            <div class="spinner-border text-primary" role="status">
                                <span class="sr-only p-5">Loading...</span>
                            </div>
                            <p class="mt-2">Logging in...</p>
                        </div>
                    `);

                    // Show modal
                    $('#loading-modal').modal({
                        backdrop: 'static',
                        keyboard: false
                    }).modal('show');



                    //$("#loading-animation").show();

                    //$('#response-message').removeClass('alert-danger').addClass('alert-success').text(response.successMessage).show();


                    // Redirect to the admin page
                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000); // Ensure redirect happens after the success message fades out
                    // Show success message
                    $("#success-message").text(res.successMessage).fadeIn().delay(3000).fadeOut(); // Show for 2 seconds

                }
                else {
                    // Hide loading modal
                    setTimeout(() => $('#loading-modal').modal('hide'), 500); // Ensure modal hides after error message fades out
                    $("#error-message").text(res.failedMessage).fadeIn().delay(500).fadeOut();
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

getStatuses = function () {
    $.ajax({
        url: '/api/values/get-statuses',
        type: 'GET',
        success: function (data) {
            var $statusDropdown = $('#statusDropdown');
            $.each(data, function (index, item) {
                var $option = $('<option></option>').val(item.value).text(item.text);
                if (item.value === "Complete") {
                    $option.addClass('text-success'); // Set color attribute
                } else if (item.value === "Incomplete(Usable)") {
                    $option.addClass('text-primary');
                } else if (item.value === "Incomplete(Unusable)") {
                    $option.addClass('text-danger');
                }
                $statusDropdown.append($option);
            });
        },
        error: function (xhr, status, error) {
            console.log('Error fetching statuses: ' + error);
        }
    });

};

getOptions = function () {
    $.ajax({
        url: '/api/values/get-options',
        type: 'GET',
        success: function (data) {
            var $updateDropdown = $('#updateDropdown');
            $.each(data, function (index, item) {
                var $option = $('<option></option>').val(item.value).text(item.text);

                if (item.value === "N/A") {
                    $option.addClass('text-secondary'); // Apply gray color
                } else if (item.value.startsWith("YES")) {
                    $option.addClass('text-success'); // Apply green color
                } else if (item.value.startsWith("NO")) {
                    $option.addClass('text-danger'); // Apply red color
                }

                $updateDropdown.append($option);
            });
        },
        error: function (xhr, status, error) {
            console.log('Error fetching options: ' + error);
        }
    });
};



/*
function loadpage(viewurl) {
    fetch(viewurl)
        .then(response => response.text())
        .then(html => {
            document.getElementById('view-content').innerHTML = html;
        })
        .catch(error => console.error('Error loading view:', error));
};

document.getElementById('summary-link').addEventListener('click', function (event) {
    event.preventDefault();
    loadpage('dashboard/summary'); // URL of the summary view
});

document.getElementById('table-link').addEventListener('click', function (event) {
    event.preventDefault();
    loadpage('dashboard'); // URL of the table view
});

            */