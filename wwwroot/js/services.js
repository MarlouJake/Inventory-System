AddRequest = (form, id)  => {
    var type = 'POST';
    var add = validate.Add;
    var addsuccess = validate.AddingSuccess;
    var addfailed = validate.AddingFailed;
    const requestroute = form.action;
    //const user = sessionStorage.getItem('storedUsername');
    const formData = new FormData(form);
    const itemdata = {
        //username: user,
        itemid: id,
        itemcode: formData.get('itemCode'),
        itemname: formData.get('itemName'),
        itemdescription: formData.get('itemDescription'),
        status: formData.get('statusDropdown'),
        firmwareupdated: formData.get('updateDropdown'),
        datecreated: new Date().toLocaleString('en-US', DateFormatOptions)
    };

    try {
        $.ajax({
            type: type,
            url: requestroute,
            data: JSON.stringify(itemdata),
            contentType: 'application/json',
            dataType: 'json',
            success: function (response, textStatus, jqXHR) {
                var status = `Status Code: ${jqXHR.status}`;
                var details = `Server responded with status code: ${jqXHR.status} ${textStatus}`;
                if (response.IsValid) {

                    var message = response.Message || validate.MatchFound;

                    let result = jsonResult(
                        type,
                        add,
                        addsuccess,
                        true,
                        addfailed,
                        false,
                        response.Message,
                        status,
                        requestroute,
                        details,
                        response.RedirectUrl
                    );

                    // Log the result for debugging
                    console.log("API Data: ", JSON.stringify(result, null, 2));

                    if (response.RedirectUrl) {
                        AddItem(itemdata, response.RedirectUrl);
                        console.log('Redirect URL:' + response.RedirectUrl);
                    }
                    else {
                        var details = "Route sent by the server missing.";
                        ShowError(details);
                    }
                }
                else {
                    let result = jsonResult(
                        type,
                        add,
                        addsuccess,
                        false,
                        addfailed,
                        true,
                        response.Message,
                        status,
                        requestroute,
                        details,
                        response.RedirectUrl
                    );

                    // Log the result for debugging
                    console.log("API Data: ", JSON.stringify(result, null, 2));
                }
            },
            error: function (jqXHR, textStatus) {
                // Parse the response as JSON
                var response = jqXHR.responseJSON;

                // Extract the message from the response, or use a default message
                var message = response.Message;

                // Extract the status message
                var statusMessage = jqXHR.statusText || 'Unknown Error';

                // Construct the full error message
                var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
                var details = responsemessage;

                // Build the result object (assuming jsonResult is a function that handles this)
                let result = jsonResult(
                    type,
                    add,
                    addsuccess,
                    false,
                    addfailed,
                    true,
                    message,
                    statusMessage,
                    getUrl(requestroute),
                    details,
                    `/add-item/${itemdata.id}`
                );

                // Log the result for debugging
                console.log("Request Error: ", JSON.stringify(result, null, 2));

                // Display the message using the modal or error message div
                setTimeout(() => {
                    $("#error-message").text(message).fadeIn().delay(100).fadeOut();
                }, 500);
            }


        });
    }
    catch {

    }

    return false; // Prevent default form submission
};

AddItem = (data, url) => {
    try
    {
        $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            success: function (response, textStatus, jqXHR) {
                var status = `Status Code: ${jqXHR.status}`;
                var details = `Server responded with status code: ${jqXHR.status} ${textStatus}`;
                if (response.IsValid) {
                    const newItemHtml = `
                        <div class="card">
                            <div class="card-body">
                                <h6 class="card-title mb-1 fs-6"><strong>${data.itemname}</strong></h6>
                                <p class="card-text mb-1"><strong>Code:</strong>${data.itemcode}</p>
                                <p class="card-text mb-0"><strong>Status:</strong>${data.itemstatus}</p>
                            </div>
                            <div class="card-footer">
                                <a onclick="ModalShow('dashboard/details/${data.id}')"
                                   class="btn btn-primary btn-crud btn-sm form-control">
                                    View
                                </a>
                            </div>
                        </div>
                    `;



                    $("#view-all .card-container").append(newItemHtml); // Append the new item to the list
                    //loadItems(currentPage);
                    $("#dynamic-modal").modal('hide');
                    NewItemAdded();
                    $("#message-success").text(validate.AddingSuccess).fadeIn().delay(500).fadeOut();
                }
                else{
                    console.error('An error occured while adding item:'+ response.Message);
                    var details = "Failed to add item, try again";
                    ShowError(details);
                }
            },
            error: function (jqXHR, textStatus) {
                // Parse the response as JSON
                var response = jqXHR.responseJSON;

                // Extract the message from the response, or use a default message
                var message = response.Message;

                // Extract the status message
                var statusMessage = jqXHR.statusText || 'Unknown Error';

                // Construct the full error message
                var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
                var details = responsemessage;

                // Build the result object (assuming jsonResult is a function that handles this)
                let result = jsonResult(
                    method,
                    login,
                    loginsuccess,
                    false,
                    loginfailed,
                    true,
                    message,
                    statusMessage,
                    getUrl(url),
                    details,
                    `/dashboard/${data.username}`
                );

                // Log the result for debugging
                console.log("Request Error: ", JSON.stringify(result, null, 2));

                // Display the message using the modal or error message div
                setTimeout(() => {
                    $("#error-message").text(message).fadeIn().delay(100).fadeOut();
                }, 500);
            }


        });
    }
    catch (ex) {
        var message = response.Message || 'An error occured while trying to send request.';
        var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
        var details = `${validate.LoginFailed}: ${ex}<br>${responsemessage}<br>${message}<br>${ex}`;

        let result = jsonResult(
            method,
            login,
            loginsuccess,
            false,
            loginfailed,
            true,
            message,
            textStatus,
            getUrl(url),
            responsemessage,
            `/dashboard/${data.username}`
        );

        console.log("Execption Data", JSON.stringify(result, null, 2));
        ShowError(details);
    }

    return false;
};

RequestUpdate = (form) => {

    const apiUrl = form.action;
    const formData = new FormData(form);
    const data = {
        username: sessionStorage.getItem('storedUsername'),
        itemcode: formData.get("itemcode"),
        itemname: formData.get("itemname"),
        itemdescription: formData.get("itemdesc"),
        itemstatus: formData.get("itemstatus"),
        iteminfo: formData.get("iteminfo"),
        firmwareupdated: formData.get("itemfirmware"),
        role: 'standard'
    };

    $('#itemcode-error').text('');
    $('#itemname-error').text('');
    $('#status-error').text('');
    $('#firmwareupdated-error').text('');
    $('.form-control').removeClass('input-error');

    if (!$(form).valid()) {

        if (!data.itemcode || data.itemcode.trim() === '') {
            $('#itemcode').addClass('input-error');
            $('#itemcode-error').text(validate.UsernameEmpty);

        } else if (data.itemcode.includes(' ')) {
            $('#itemcode').addClass('input-error');
            $('#itemcode-error').text(validate.UsernameEmpty);
        } else if (data.itemcode.length < 3) {
            $('#itemstatus').addClass('input-error');
            $('#status-error').text(validate.UsernameLength);

        }

        if (!data.itemname || data.itemname.trim() === '') {
            $('#itemname').addClass('input-error');
            $('#itemname-error').text(validate.UsernameEmpty);

        } else if (data.itemname.length < 3) {
            $('#itemstatus').addClass('input-error');
            $('#status-error').text(validate.UsernameLength);

        }


        if (!data.itemstatus || data.itemstatus.trim() === '--Select Status--') {
            $('#itemstatus').addClass('input-error');
            $('#status-error').text(validate.UsernameEmpty);

        } else if (data.itemstatus.length < 3) {
            $('#itemstatus').addClass('input-error');
            $('#status-error').text(validate.UsernameLength);

        }


        if (!data.password || data.password.trim() === '') {
            $('#itemfirmware').addClass('input-error');
            $('#firmwareupdated-error').text(validate.PasswordEmpty);

        } else if (data.password.length < 8) {
            $('#itemfirmware').addClass('input-error');
            $('#firmwareupdated-error').text(validate.PasswordLegth);

        }

        return false; // Prevent form submission if invalid
    }

    try {
        loading(validate.PleaseWait);
        $.ajax({
            type: validate.Put,
            url: apiUrl,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            success: function (res) {

                if (res.isValid) {
                    const result = jsonResult(
                        validate.Put,
                        validate.Update,
                        validate.UpdateSuccess,
                        true,
                        validate.UpdateFailed,
                        false,
                        validate.UpdatedSuccessfully,
                        data,
                        validate.PutSuccess,
                        apiUrl,
                        validate.ResponseValid,
                        res.redirectUrl,
                        browserInfo.name,
                        browserInfo.version
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));

                    //setTimeout(() => $('#loading-modal').modal('hide'), 1000);
                    setTimeout(() => {
                        $("#emessage-success").text(validate.UpdatedSuccessfully).fadeIn().delay(1000).fadeOut();
                    }, 500);
                    window.location.href = res.redirectUrl;

                } else {

                    setTimeout(() => $('#loading-modal').modal('hide'), 500);
                    setTimeout(() => {
                        $("#message-error").text(validate.FillRequiredFields).fadeIn().delay(1000).fadeOut();
                    }, 500);

                    const result = jsonResult(
                        validate.Put,
                        validate.Update,
                        validate.UpdateSuccess,
                        false,
                        validate.UpdateFailed,
                        true,
                        validate.FailedToUpdate,
                        data,
                        validate.PutFailed,
                        apiUrl,
                        validate.ResponseInvalid,
                        `/dashboard/${data.username}`,
                        browserInfo.name,
                        browserInfo.version
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));
                }

            },
            error: function (err) {

                const result = jsonResult(
                    validate.Put,
                    validate.Update,
                    validate.UpdateSuccess,
                    false,
                    validate.UpdateFailed,
                    true,
                    validate.FailedToUpdate,
                    data,
                    validate.PutFailed,
                    apiUrl,
                    validate.ResponseInvalid,
                    `/dashboard/${data.username}`,
                    browserInfo.name,
                    browserInfo.version
                );

                console.log("Submission Error Data", JSON.stringify(result, null, 2));
                setTimeout(() => $('#loading-modal').modal('hide'), 500);
                $("#message-error").text(validate.FillRequiredFields).fadeIn().delay(500).fadeOut();
            }
        });

    }
    catch (ex) {


        const result = jsonResult(
            validate.Put,
            validate.Update,
            validate.UpdateSuccess,
            false,
            validate.UpdateFailed,
            true,
            validate.FailedToUpdate,
            data,
            validate.PutFailed + "\n" + ex,
            data,
            validate.PutError,
            apiUrl,
            validate.ResponseInvalid,
            `/dashboard/${data.username}`,
            browserInfo.name,
            browserInfo.version
        );

        console.log("Submission Failure Data", JSON.stringify(result, null, 2));
        setTimeout(() => $('#loading-modal').modal('hide'), 500);
        $("#message-error").text(validate.InvalidInput).fadeIn().delay(500).fadeOut();
    }
    setTimeout(() => $('#loading-modal').modal('hide'), 500);
    return false;
};