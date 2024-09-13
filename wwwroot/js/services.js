AddItem = (form) => {
    var media = process.JsonApp;
    const url = '/api/u/services/add-item/';
    //const user = sessionStorage.getItem('storedUsername');
    const formData = new FormData(form);
    const itemdata = {
        //username: user,
        itemcode: formData.get('itemCode'),
        itemname: formData.get('itemName'),
        itemdescription: formData.get('itemDescription'),
        status: formData.get('statusDropdown'),
        additionalinfo: formData.get('item-info'),
        firmwareupdated: formData.get('updateDropdown'),
        datecreated: new Date().toLocaleString('en-US', DateFormatOptions)
    };

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': media,
            'Accept': media
        },
        body: JSON.stringify(itemdata)
    })
        .then(response => response.json())
        .then(data => {

            if (data.isValid) {
                //$("#view-all").html(data.html); 
                //document.getElementById("view-all").innerHTML = data.html;
                // Add new item to the view

                const newItemHtml = `
                    <div class="card">
                        <div class="card-body">
                            <h6 class="card-title mb-1 fs-6"><strong>${itemdata.itemname}</strong></h6>
                            <p class="card-text mb-1"><strong>Code:</strong> ${itemdata.itemcode}</p>
                            <p class="card-text mb-0"><strong>Status:</strong> ${itemdata.status}</p>
                        </div>
                        <div class="card-footer">
                            <a onclick="ShowModal('@Url.Action("ViewDetails", "Users", new { id = item.id })')"
                               class="btn btn-primary btn-crud btn-sm form-control">
                                View
                            </a>
                        </div>
                    </div>
                `;



                $("#view-all").append(newItemHtml); // Append the new item to the list
                loadItems(currentPage);
                $("#crud-modal .modal-body").html('');
                $("#crud-modal").modal('hide');
                NewItemAdded();
                $("#message-success").text(validate.AddingSuccess).fadeIn().delay(500).fadeOut();

                const result = jsonResult(
                    validate.Post,
                    validate.Add,
                    validate.AddingSuccess,
                    true,
                    validate.AddingFailed,
                    false,
                    data.successsmessage,
                    itemdata,
                    validate.PostSuccess,
                    form.action,
                    200, // Assuming 200 is the response status for success
                    `/user-dashboard/${itemdata.userid}`,
                    browserInfo.name,
                    browserInfo.version
                );

                console.log('API Response', JSON.stringify(result, null, 2));
            } else {

                $("#message-error").text(data.errormessage).fadeIn().delay(500).fadeOut();
                $("#crud-modal .modal-body").html('');
                $("#crud-modal").modal('hide');
                const result = jsonResult(
                    validate.Post,
                    validate.Add,
                    validate.AddingSuccess,
                    false,
                    validate.AddingFailed,
                    true,
                    data.errormessage,
                    itemdata,
                    validate.PostFailed,
                    form.action,
                    400, // Assuming 400 is the response status for failure
                    `/user-dashboard/${itemdata.userid}`,
                    browserInfo.name,
                    browserInfo.version
                );

                console.log('API Response Error', JSON.stringify(result, null, 2));
            }
        })
        .catch(error => {
            $("#message-error").text('An error occurred. Please try again.').fadeIn().delay(500).fadeOut();
            $("#crud-modal .modal-body").html('');
            $("#crud-modal").modal('hide');
            const result = jsonResult(
                validate.Post,
                validate.Add,
                validate.AddingSuccess,
                false,
                validate.AddingFailed,
                true,
                error.message,
                itemdata,
                validate.PostError,
                form.action,
                error,
                `/user-dashboard/${itemdata.userid}`,
                browserInfo.name,
                browserInfo.version
            );

            console.log('Request API Error', JSON.stringify(result, null, 2));
        });

    return false; // Prevent default form submission
}


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