AddRequest = (form) => {
    var type = 'POST';
    var add = validationMessages.Add;
    var addsuccess = validationMessages.AddingSuccess;
    var addfailed = validationMessages.AddingFailed;
    const requestroute = form.action;
    //const user = sessionStorage.getItem('storedUsername');
    const formData = new FormData(form);
    const itemdata = {
        itemcode: formData.get('itemCode'),
        itemname: formData.get('itemName'),
        itemdescription: formData.get('itemDescription'),
        status: formData.get('statusDropdown'),
        firmwareupdated: formData.get('updateDropdown'),
        category: formData.get('categoryDropdown'),
        datecreated: new Date().toLocaleString('en-US', DateFormatOptions)
    };


    if (!ValidateDataInput(itemdata, '#itemCode', '#itemName', "#itemDescription", '#itemCode-error', '#itemName-error', '#itemDescription-errror')) {
        
        return false;
    }

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

                    console.log("API Data: ", JSON.stringify(result, null, 2));

                    if (response.RedirectUrl) {
                        AddItem(itemdata, response.RedirectUrl);
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
                    ShowError(details);

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
                    `/add/${itemdata.id}`
                );

         
                console.log("Request Error: ", JSON.stringify(result, null, 2));
                ShowError(details);
 
                setTimeout(() => {
                    $("#error-message").text(message).fadeIn().delay(100).fadeOut();
                }, 500);
            }


        });
    }
    catch (ex){
        ShowError(ex);
    }

    return false; 
};

AddItem = (data, url) => {
    let activeCategory = $('.ctg-selected').data('string');
    $('#itemCode-error').text('');
    $('.form-control').removeClass('input-error').addClass('input-success');

    var type = 'POST';
    var add = validationMessages.Add;
    var addsuccess = validationMessages.AddingSuccess;
    var addfailed = validationMessages.AddingFailed;
    try
    {
        $.ajax({
            type: type,
            url: url,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            success: function (response, textStatus, jqXHR) {
                var status = `Status Code: ${jqXHR.status}`;
                var details = `Server responded with status code: ${jqXHR.status} ${textStatus}`;
                if (response.IsValid) {

                    $("#dynamic-modal").modal('hide');
                    $("#message-success").text(addsuccess).fadeIn().delay(500).fadeOut();
                    
                    //loadPage(currentPage);

                    if ($('#searchbar').val() !== "") {
                        loadItems($('#searchbar').val(), activeCategory, currentPage); // Load items based on search term
                    } else {
                        loadItemsByCategory(activeCategory, currentPage);
                    }
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

      
                var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
                var details = responsemessage;

  
                let result = jsonResult(
                    type,
                    add,
                    addsuccess,
                    false,
                    addfailed,
                    true,
                    message,
                    statusMessage,
                    getUrl(url),
                    details,
                    `/dashboard/${data.itemid}`
                );

                
                console.log("Request Error: ", JSON.stringify(result, null, 2));
                //ShowError(details);

                if (message === "Item code already in use") {
                    $('#itemCode-error').text(message);
                    $('#itemCode').addClass('input-error');
                } else {
                    setTimeout(() => {
                        $("#message-error").text(message).fadeIn().delay(100).fadeOut();
                    }, 500);
                }
                
            }


        });
    }
    catch (ex) {
        var message = response.Message || 'An error occured while trying to send request.';
        var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
        var details = `${validate.LoginFailed}: ${ex}<br>${responsemessage}<br>${message}<br>${ex}`;

        let result = jsonResult(
            method,
            add,
            addsuccess,
            false,
            addfailed,
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



UpdateRequest = (form) => {
    const formData = new FormData(form);
    const itemdata = {
        itemid: parseInt(formData.get('dataid')),
        itemcode: formData.get('updatecode'),
        itemname: formData.get('updatename'),
        itemdescription: formData.get('updatedesc'),
        status: formData.get('updatestatus'),
        category: formData.get('categoryPartial'),
        firmwareupdated: formData.get('updatefirmware')
    };

    var dataid = $('#dataid').val();

    if (!ValidateDataInput(itemdata, '#updatecode', '#updatename', "#updatedesc", '#updatecode-error', '#updatename-error', '#updatedesc-error-errror')) {

        return false;
    }

    try {
        $.ajax({
            method: 'POST',
            url: form.action,
            data: JSON.stringify(itemdata),
            accepts: 'application/json',
            contentType: 'application/json',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Content-Type', 'application/json');
                xhr.setRequestHeader('Accept', 'application/json');
            },
            success: function (response) {
                          
                if (response.IsValid) {
                    if (response.RedirectUrl) {
                        ModifyItem(itemdata, response.RedirectUrl, dataid);
                    }
                    else {
                        var details = "Route sent by the server missing.";
                        ShowError(details);
                    }
                }
                else {
                    console.log("Response Invalid: ", response);
                    ShowError("Response invalid");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error: ", jqXHR.status, textStatus, errorThrown);
                ShowError(jqXHR.status);
            },
            fail: function (jqXHR) {
                console.error("Error: ", jqXHR.status);
                ShowError(jqXHR.status);
            }
        });
    }
    catch (ex) {
        console.error("Error: ", ex);
        ShowError(ex);

    }
    return false; 
};

ModifyItem = (data, url, dataid) => {
    let activeCategory = $('.ctg-selected').data('string');
    $('#updatecode-error').text('');
    $('.form-control').removeClass('input-error').addClass('input-success');
    try {
        $.ajax({
            method: 'PUT',
            url: url,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            success: function (response, textStatus, jqXHR) {
                var status = `Status Code: ${jqXHR.status}`;
                var details = `Server responded with status code: ${jqXHR.status} ${textStatus}`;
                if (response.IsValid) {
 
                    $("#dynamic-modal").modal('hide');

                    $("#message-success").text(response.Message).fadeIn().delay(500).fadeOut();
                    //loadPage(currentPage);

                    if ($('#searchbar').val() !== "") {
                        loadItems($('#searchbar').val(), activeCategory, currentPage); // Load items based on search term
                    } else {
                        loadItemsByCategory(activeCategory, currentPage);
                    }
                    
                }
                else {
                    console.error('An error occured while updating item:' + response.Message);
                    var details = response.Message ?? "Failed to update item.";
                    ShowError(details);
                }
            },
            error: function (jqXHR, textStatus) {
                // Parse the response as JSON
                var response = jqXHR.responseJSON;

                // Extract the message from the response, or use a default message
                var message =  response.Message || "An error occured";

                // Extract the status message
                var statusMessage = jqXHR.statusText || 'Unknown Error';

                var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
                var details = responsemessage;

                let result = jsonResult(
                    'PUT',
                    'update',
                    'updatesuccess',
                    false,
                    'updatefailed',
                    true,
                    message,
                    statusMessage,
                    getUrl(url),
                    details,
                    `/modify/${dataid}`
                );

                console.log("Request Error: ", JSON.stringify(result, null, 2));
                

                if (message === "Item code already in use") {
                    $('#updatecode').addClass('input-error');
                    $('#updatecode-error').text(message);
                } else {
                    setTimeout(() => {
                        $("#message-error").text(message).fadeIn().delay(100).fadeOut();
                    }, 500);
                    ShowError(details);
                }
            }
            
        });
    }
    catch (ex) {
        ShowError(ex);
        console.log('Error: ', ex);
    }
    return false;
}

/*
DeleteRequest = (form) => {
    var deleteid = parseInt($('#delete-id').val(), 10);
   
    if (isNaN(deleteid)) {
        ShowError('No item id retrieve');
        return false;
    }

    try {
        $.ajax({
            method: 'POST',
            url: form.action,
            contentType: 'application/json',
            data: JSON.stringify(deleteid),        
            success: function (response) {

                if (response.IsValid) {
                    if (response.RedirectUrl) {
                        RemoveItem(response.RedirectUrl, deleteid);
                    }
                    else {
                        var details = "Route sent by the server missing.";
                        ShowError(details);
                    }
                }
                else {
                    console.log("Response Invalid: ", response);
                    ShowError("Response invalid");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error: ", jqXHR.status, textStatus, errorThrown);
                ShowError(jqXHR.status);
            },
            fail: function (jqXHR) {
                console.error("Error: ", jqXHR.status);
                ShowError(jqXHR.status);
            }
        });
    }
    catch (ex) {
        console.error("Error: ", ex);
        ShowError(ex);
    }
    return false;
};

RemoveItem = (url, id) => {
    let activeCategory = $('.ctg-selected').data('string');

    try {
        $.ajax({
            type: 'DELETE',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(id),
            success: function (response, textStatus, jqXHR) {
                var status = `Status Code: ${jqXHR.status}`;
                var details = `Server responded with status code: ${jqXHR.status} ${textStatus}`;

                let result = jsonResult(
                    'DELETE',
                    'delete',
                    'deletesuccess',
                    true,
                    'deletefailed',
                    false,
                    response.Message,
                    status,
                    url,
                    details,
                    'dashboard/item-view'
                );

                if (response.IsValid) {
                 
                    $("#dynamic-modal").modal('hide');

                    $("#message-success").text(response.Message).fadeIn().delay(500).fadeOut();
                    //loadPage(currentPage);

                    if ($('#searchbar').val() !== "") {
                        loadItems($('#searchbar').val(), activeCategory, currentPage); 
                    } else {
                        loadItemsByCategory(activeCategory, currentPage);
                    }

                }
                else {
                    console.error('An error occured while removing item:' + response.Message);
                    var details = "Failed to remove item, try again";
                    ShowError(details);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error: ", jqXHR.status, textStatus, errorThrown);
                ShowError(jqXHR.status);
            },
            fail: function (jqXHR) {
                console.error("Error: ", jqXHR.status);
                ShowError(jqXHR.status);
            }
        });
    }
    catch (ex) {
        ShowError(ex);
        console.log('Error: ', ex);
    }
    return false;
};
*/

function DeleteRequest(itemIds) {
    $.ajax({
        url: '/api/u/validate/remove-multiple-item',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(itemIds), 
        success: function (response) {

            if (response.IsValid) {
                if (response.RedirectUrl) {
                    RemoveConfirm(response.RedirectUrl, itemIds);
                }
                else {
                    var details = "Route sent by the server missing.";
                    ShowError(details);
                }
            }
            else {
                console.log("Response Invalid: ", response);
                ShowError("Response invalid");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error: ", jqXHR.status, textStatus, errorThrown);
            ShowError(jqXHR.status);
        },
        fail: function (jqXHR) {
            console.error("Error: ", jqXHR.status);
            ShowError(jqXHR.status);
        }

    });
}

function RemoveConfirm(url, itemIds) {
    let activeCategory = $('.ctg-selected').data('string');
    $.ajax({
        url: url,
        type: 'DELETE',
        contentType: 'application/json',
        data: JSON.stringify(itemIds),
        success: function (response, textStatus, jqXHR) {
            var status = `Status Code: ${jqXHR.status}`;
            var details = `Server responded with status code: ${jqXHR.status} ${textStatus}`;

            if (response.IsValid) {

                $("#dynamic-modal").modal('hide');

                $("#message-success").text(response.Message).fadeIn().delay(500).fadeOut();

                checkState('selectAllCheckBox');

                if ($('#searchbar').val() !== "") {
                    loadItems($('#searchbar').val(), activeCategory, currentPage); // Load items based on search term
                } else {
                    loadItemsByCategory(activeCategory, currentPage);
                }
              
            }
            else {
                console.error('An error occured while removing item:' + response.Message);
                var details = "Failed to remove item, try again";
                ShowError(details);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error: ", jqXHR.status, textStatus, errorThrown);
            ShowError(jqXHR.status);
        },
        fail: function (jqXHR) {
            console.error("Error: ", jqXHR.status);
            ShowError(jqXHR.status);
        }
    });
}

function ValidateInput(data) {
    $('#itemCode-error').text('');
    $('#itemName-error').text('');
    $('#statusDropdown-error').text('');
    $('#updateDropdown-error').text('');
    $('.form-control').removeClass('input-error').addClass('input-success');

    let isValid = true;

    if (!data.itemcode || data.itemcode.trim() === '') {
        $('#itemCode').addClass('input-error');
        $('#itemCode-error').text('Item code is required');
        isValid = false;
    } else if (data.itemcode.length < 3) {
        $('#itemCode').addClass('input-error');
        $('#itemCode-error').text('Item code is should be atleast 3 characters');
        isValid = false;
    } else if (data.itemcode.length > 20) {
        $('#itemCode').addClass('input-error');
        $('#itemCode-error').text('Item code should not exceed 20 characters');
        isValid = false;
    }

    if (!data.itemname || data.itemname.trim() === '') {
        $('#itemName').addClass('input-error');
        $('#itemName-error').text('Item name is required');
        isValid = false;
    } else if (data.itemname.length < 3) {
        $('#itemName').addClass('input-error');
        $('#itemName-error').text('Item name is should be atleast 3 characters');
        isValid = false;
    } else if (data.itemname.length > 20) {
        $('#itemName').addClass('input-error');
        $('#itemName-error').text('Item name should not exceed characters');
        isValid = false;
    }

    if (!isValid) {
        console.log('Validation errors');
    }

    return isValid;
}