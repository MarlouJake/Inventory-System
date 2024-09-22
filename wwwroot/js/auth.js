// Login API
UserRequest = (form) => {
    const method = validationMessages.Post;
    const login = validationMessages.Login;
    const loginsuccess = validationMessages.LoginSuccess;
    const loginfailed = validationMessages.LoginFailed;
    const fillRequiredFields = validationMessages.FillRequiredFields;
    const credentialsIncorrect = validationMessages.UsernamePasswordIncorrect;
    var found = validationMessages.MatchFound;
    var loginbutton = $('#user-login .form-group #loginbutton');
    var info = $('#layoutbody #success-message');
    const requestroute = form.action;
    const formData = new FormData(form);
    const data = {
        username: formData.get("username"),
        password: formData.get("password"),
    };


    if (!ValidateForm(data)) {
        return false;
    }

    
    try {
        $.ajax({
            type: method,
            url: requestroute,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            beforeSend: function () {
                // Disable the submit button and show spinner
                info.html(`${spinner} Wait a moment...`).fadeIn();
                loginbutton.prop('disabled', true);
                loginbutton.html(`${spinner} Loggin in`);
            },
            success: function (response, textStatus, jqXHR) {
                var status = `Status Code: ${jqXHR.status}`;        
                var details = `Server responded with status code: ${jqXHR.status} ${textStatus}`;
                if (response.IsValid) {
                    var message = response.Message || found;

                    let result = jsonResult(
                        method,
                        login,
                        loginsuccess,
                        true,
                        loginfailed,
                        false,
                        response.Message,
                        status,
                        requestroute,
                        details,
                        response.RedirectUrl
                    );
                    console.log("API Data", JSON.stringify(result, null, 2));

                    if (response.RedirectUrl) {
                        AuthRequest(data, response.RedirectUrl);
                    }
                    else {
                        var details = "Route sent by the server missing.";
                        ShowError(details);
                    }
                    
                }
                else {
                    var message = response.Message || credentialsIncorrect;

                    setTimeout(() => {
                        $("#error-message").text(message).fadeIn().delay(100).fadeOut();
                    }, 500);

                    let result = jsonResult(
                        method,
                        login,
                        loginsuccess,
                        false,
                        loginfailed,
                        true,
                        message,
                        status,
                        requestroute,
                        details,
                        `/dashboard/${data.username}`
                    );
                    console.log("API Data", JSON.stringify(result, null, 2));
                }

            },
            error: function (jqXHR, textStatus) {
                // Parse the response as JSON
                var response = jqXHR.responseJSON;

                var message = '';

                // Checks if the response contains errors
                if (response && response.errors) {
                    // Loops through each field in the errors object
                    for (var field in response.errors) {
                        if (response.errors.hasOwnProperty(field)) {
                            // Gets the array of errors for the field and concatenate them
                            var fieldErrors = response.errors[field];
                            message += field + ": " + fieldErrors.join(", ") + "; ";
                        }
                    }
                } else {
                    // Fallback message if no errors are present in the response
                    message = fillRequiredFields || 'An unknown error occurred.';
                }

                var statusMessage = jqXHR.statusText || 'Unknown Error';

                var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
                var details = `${responsemessage}`;

                let result = jsonResult(
                    method,
                    login,
                    loginsuccess,
                    false,
                    loginfailed,
                    true,
                    message,
                    statusMessage,
                    requestroute,
                    details,
                    `/dashboard/${data.username}`
                );

                console.log("Request Error: ", JSON.stringify(result, null, 2));

            },
            complete: function () {
                // Re-enable the submit button and reset the text
                loginbutton.prop('disabled', false);
                info.fadeOut();
                loginbutton.html('Login');     
            }

      
        });

    }
    catch (ex) {
        var message = 'An error occured while trying to send request.';
        var textStatus = 'An error is catched';
        var responsemessage = `Couldn't reach the server.`;
        var details = `${responsemessage}<br>${ex}`;
        let result = jsonResult(
            method,
            login,
            loginsuccess,
            false,
            loginfailed,
            true,
            message,
            textStatus,
            requestroute,
            responsemessage,
            `/dashboard/${data.username}`
        );

        console.log("Execption Data", JSON.stringify(result, null, 2));
        ShowError(details);  
    }
    
    return false;
};


AuthRequest = (data, url) => {
    const method = validationMessages.Post;
    const login = validationMessages.Login;
    const loginsuccess = validationMessages.LoginSuccess;
    const loginfailed = validationMessages.LoginFailed;
    try {
        $.ajax({
            type: method,
            url: url,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                if (response.IsValid) {

                    if (response.RedirectUrl) {
                        window.location.href = response.RedirectUrl;
                    } else {
                        var details = "No route specified.";
                        ShowdisplayModal(details);
                    }
                }
                else {
                    console.log(response.Message);
                    var details = "Failed to authenticate login, try again";
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
        var details = `${loginfailed}: ${ex}<br>${responsemessage}<br>${message}<br>${ex}`;

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


//Logout API 
LogoutRequest = (url) => {
    var logout = validationMessages.Logout;
    var logoutSuccess = validationMessages.LogoutRequest;
    var logoutFailed = validationMessages.LogoutFailed;
    var logoutError = validationMessages.LogoutError;

    const method = validate.Post;
    var info = $('#layoutbody #message-success');
    var logout = $('#user-logout');
    try {
        $.ajax({
            type: method,
            url: url,
            beforeSend: function (jqXHR) {
                info.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Wait a moment...').fadeIn();
                logout.prop('disabled', true);
                logout.html('<span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span>');
            },
            success: function (response, textStatus, jqXHR) {
                var status = `Status Code: ${jqXHR.status}`;
                var details = `Server responded with status code: ${jqXHR.status} ${textStatus}`;
                var message = response.Message || 'No error message retrieved.';
                if (response.isValid) {

                    let result = jsonResult(
                        method,
                        logout,
                        logoutSuccess,
                        true,
                        logoutFailed,
                        false,
                        message,
                        status,
                        getUrl(url),
                        details,
                        `/home/login`
                    );
                    console.log("API Data", JSON.stringify(result, null, 2));

                    window.location.href = response.redirectUrl;

                    $("#success-message").text(logoutSuccess).fadeIn().delay(3000).fadeOut();
                }
                else {

                    let result = jsonResult(
                        method,
                        logout,
                        logoutSuccess,
                        false,
                        logoutFailed,
                        true,
                        message,
                        status,
                        url,
                        details,
                        `/home/login`
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));

                    $("#error-message").text(logoutFailed).fadeIn().delay(500).fadeOut();

                }

            },

            error: function (jqXHR, textStatus, errorThrown) {
                var responseText = jqXHR.responseText; // Raw response text from server
                var response = {};
                var message = response.Message || 'No error message retrieved.';
                var statusMessage = jqXHR.statusText || 'Unknown Error';
                var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
                var details = `${responsemessage} ${message}`;

                let result = jsonResult(
                    method,
                    logout,
                    logoutSuccess,
                    false,
                    logoutFailed,
                    true,
                    message,
                    statusMessage,
                    url,
                    details,
                    `/home/login`
                );

                console.log("Request Error: ", JSON.stringify(result, null, 2));
                //ShowdisplayModal(message);
                ShowError(details);

            } 

        });
    }
    catch (ex) {
        var message = response.Message || 'An error occured while trying to send request.';
        var responsemessage = `Server responded with status code: ${jqXHR.status} ${textStatus}.`;
        var details = `${logoutError}: ${ex}<br>${responsemessage}<br>${message}`;
        let result = jsonResult(
            method,
            logout,
            logoutSuccess,
            false,
            logoutFailed,
            true,
            message,
            responsemessage,
            url,
            details,
            `/home/login`
        );

        console.log("Execption Data", JSON.stringify(result, null, 2));
        ShowError(details);
    }

    console.log('default');
    return false;
};




CreateRequest = (form) => {
    const formData = new FormData(form);
    const data = {
        username: formData.get('signup-username'),
        email: formData.get('signup-email'),
        password: formData.get('signup-password'),
        confirmpassword: formData.get('signup-retypepassword')
    };

    if (!ValidateSignup(data)) {
        return false;
    }
    console.log('to ajax');
    $.ajax({
        method: 'POST',
        url: form.action,
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(data),
        success: function (response) {
            console.log(JSON.stringify(response));
        },
        error: function (jqXHR, textStatus) {
            var response = jqXHR.responseJSON;

            console.error('failed: ', response.Message);
        }

    });
    return false;
}

CreateAccount = (data, url) => {

}



