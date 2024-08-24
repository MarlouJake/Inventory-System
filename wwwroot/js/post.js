

const LoadingMessages = [
    'Loging in',
    'Loging out',
    'Please wait'
];

const SuccessMessages = [
    'Post Success',
    'Valid',
    'Login Success'
];

const ErrorMessages = [
    'Login Failed',
    'Logout Failed',
    'Post Error',
    'Post Failed',
    'Invalid',
    'Login Error',
    'Logout Error',
    'Enter valid credentials',
    'Username or Password incorrect'
];

const methods = [
    'POST',
    'GET',
    'PUT'
];

const actions = [
    'Login',
    'Logout',
    'Add',
    'Delete',
    'Modify',
    'Open',
    'Exit',
    'Create',
    'Refresh'
];

function getUrl(url) {

    const fullUrl = `${window.location.origin}${url}`;

    return fullUrl;
    
}


function getBrowserInfo() {
    const userAgent = navigator.userAgent;

    let browserName = "Unknown Browser";
    let browserVersion = "Unknown Version";

    // Check for various browsers
    if (userAgent.indexOf("Firefox") > -1) {
        browserName = "Firefox";
        browserVersion = userAgent.substring(userAgent.indexOf("Firefox") + 8);
    } else if (userAgent.indexOf("Opera") > -1 || userAgent.indexOf("OPR") > -1) {
        browserName = "Opera";
        browserVersion = userAgent.substring(userAgent.indexOf("Opera") + 6);
        if (userAgent.indexOf("OPR") > -1) {
            browserName = "Opera";
            browserVersion = userAgent.substring(userAgent.indexOf("OPR") + 4);
        }
    } else if (userAgent.indexOf("Trident") > -1) {
        browserName = "Internet Explorer";
        browserVersion = userAgent.substring(userAgent.indexOf("rv:") + 3);
    } else if (userAgent.indexOf("Edge") > -1) {
        browserName = "Edge";
        browserVersion = userAgent.substring(userAgent.indexOf("Edge/") + 5);
    } else if (userAgent.indexOf("Chrome") > -1) {
        browserName = "Chrome";
        browserVersion = userAgent.substring(userAgent.indexOf("Chrome/") + 7);
    } else if (userAgent.indexOf("Safari") > -1) {
        browserName = "Safari";
        browserVersion = userAgent.substring(userAgent.indexOf("Version/") + 8);
    }

    return {
        name: browserName,
        version: browserVersion.split(" ")[0] // Remove additional info if any
    };
}

const browserInfo = getBrowserInfo();


function jsonResult(
    method,   _action, _actionMessage1, Val1, _actionMessage2, Val2, data,
    resMessage, api, resStatus, targetRoute, _browser, Val3) {

    const ResponseMessage = resMessage;
    const ApiUrl = getUrl(api);
    const ResponseStatus = resStatus;
    const TagerRoute = getUrl(targetRoute);
    const Action = _action 
    const ActionMessage1 = _actionMessage1;
    const ActionMessage2 = _actionMessage2;
    const Validity1 = Val1;
    const Validity2 = Val2;
    const BrowserInfo = _browser;
    const version = Val3;

    // Format timestamp as "MMMM dd, yyyy hh:mm:ss tt"
    const options = {
        year: 'numeric', month: 'long', day: '2-digit',
        hour: '2-digit', minute: '2-digit', second: '2-digit',
        hour12: true
    };
    const timestamp = new Date().toLocaleString('en-US', options);

    return {
        method,
        actions: {
            action: Action,
            [ActionMessage1]: [Validity1],
            [ActionMessage2]: [Validity2]
        },
        data,
        message: ResponseMessage,
        apiurl: ApiUrl,
        response: ResponseStatus,
        targeroute: TagerRoute,     
        browserdetails: {
            [BrowserInfo]: version
        },
        timestamp
    };
}

async function loading(message) {
    $('#loading-modal .modal-body').html(`
        <div class="text-center">
            <div class="spinner-border text-primary" role="status">
                <span class="sr-only p-5">Loading...</span>
            </div>
            <p class="mt-2">${message}...</p>
        </div>
    `);

    await $('#loading-modal').modal({
        backdrop: 'static',
        keyboard: false
    }).modal('show');
}

sendRequest = (form) => {
    const method = methods[0];
    const action = actions[0];
    const successmsg0 = SuccessMessages[0];
    const successmsg1 = SuccessMessages[1];
    const successmsg2 = SuccessMessages[2];
    const errormsg1 = ErrorMessages[0];
    const errormsg3 = ErrorMessages[3];
    const errormsg4 = ErrorMessages[4];
    const formData = new FormData(form);
    const data = {
        username: formData.get("username"),
        password: formData.get("password")
    };
    
    if (!$(form).valid()) {
        $("#error-message").text(ErrorMessages[7]).fadeIn().delay(1000).fadeOut();
        return false; // Prevent form submission if invalid
    }


    try {
       
        

        $.ajax({
            type: method,
            url: form.action,
            data: JSON.stringify(data),
            contentType: 'application/json',  
            dataType: 'json',
            success: function (res) {
                loading(LoadingMessages[0]);
                if (res.isValid) {
                    const result = jsonResult(
                        method,
                        action,
                        successmsg2,
                        true,
                        errormsg1,
                        false,
                        data,
                        successmsg0,
                        form.action,
                        successmsg1,
                        res.redirectUrl,
                        browserInfo.name,
                        browserInfo.version
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));
                    
                    setTimeout(() => $('#loading-modal').modal('hide'), 1000);
                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000);

                    //$("#success-message").text(res.successMessage).fadeIn().delay(3000).fadeOut();
                } else {

                    setTimeout(() => $('#loading-modal').modal('hide'), 500);
                    setTimeout(() => {
                        $("#error-message").text(res.errorMessage).fadeIn().delay(1000).fadeOut();
                    }, 500);
                   

                    const result = jsonResult(
                        method,
                        action,
                        successmsg2,
                        false,
                        errormsg1,
                        true,
                        data,
                        successmsg0,
                        form.action,
                        res.errorMessage,
                        res.redirectUrl,
                        browserInfo.name,
                        browserInfo.version
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));
                }
  
            },
            error: function (err) {
                setTimeout(() => $('#loading-modal').modal('hide'), 500);

                $("#error-message").text(ErrorMessages[7]).fadeIn().delay(500).fadeOut();

                const result = jsonResult(
                    method,
                    action,
                    successmsg2,
                    false,
                    errormsg1,
                    true,
                    data,
                    errormsg3,
                    form.action,
                    errormsg4,
                    `dashboard/${data.username}`,
                    browserInfo.name,
                    browserInfo.version
                );

                console.log("Submission Data", JSON.stringify(result, null, 2));
            }
        });
        
    } catch (ex) {
        setTimeout(() => $('#loading-modal').modal('hide'), 500);
        console.log("Exception caught: ", ex);
    }

    return false;
}



/*
async function sendRequest(url, method, data, message) {
    await $.ajax({
        url: url,
        type: method,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: 'json',
        success: function (response) {

            if (response.isValid) {

                const result = jsonResult(
                    method,
                    data,
                    SuccessMessages[0],
                    url,
                    SuccessMessages[1],
                    response.redirectUrl,
                    actions[0],
                    SuccessMessages[2],
                    true,
                    ErrorMessages[0],
                    false,
                    browserInfo.name,
                    browserInfo.version
                );
                
                console.log("Submission Data", JSON.stringify(result, null, 2));

                loading(LoadingMessages[0]);

                setTimeout(() => {
                    window.location.href = response.redirectUrl;
                }, 1000);

                $("#success-message").text(response.successMessage).fadeIn().delay(3000).fadeOut();
            } else {
                setTimeout(() => $('#loading-modal').modal('hide'), 500);

                $("#error-message").text(response.failedMessage).fadeIn().delay(500).fadeOut();

                const result = jsonResult(
                    method,
                    data,
                    SuccessMessages[0],
                    url,
                    SuccessMessages[1],
                    response.redirectUrl,
                    actions[0],
                    SuccessMessages[2],
                    false,
                    ErrorMessages[0],
                    true,
                    browserInfo.name,
                    browserInfo.version
                );

                console.log("Submission Data", JSON.stringify(result, null, 2));
            }
        },
        error: function (err) {
            if (err.status === 400) {
                const errors = err.responseJSON.errors;
                let errorMessage = "Validation Errors: <br/>";

                for (const key in errors) {
                    if (errors.hasOwnProperty(key)) {
                        errorMessage += `${errors[key].join('<br/>')}<br/>`;
                    }
                }

                $("#error-message").html(errorMessage).fadeIn().delay(3000).fadeOut();
            } else {
                $("#error-message").text(err.responseText).fadeIn().delay(500).fadeOut();
            }

            setTimeout(() => $('#loading-modal').modal('hide'), 500);

            console.error(`AJAX Error: ${err.responseText}`);


            const result = jsonResult(
                method,
                data,
                SuccessMessages[0],
                url,
                SuccessMessages[1],
                response.redirectUrl,
                actions[0],
                SuccessMessages[2],
                false,
                ErrorMessages[0],
                true,
                browserInfo.name,
                browserInfo.version
            );

            console.log("Submission Data", JSON.stringify(result, null, 2));
        }
    });
}
*/
async function logoutRequest(url, method, message) {
    await $.ajax({
        url: url,
        type: method,
        success: function (res) {
            console.log(SuccessMessages[0], `: ${url}`);

            if (res.isValid) {
                console.log(SuccessMessages[1], `: ${url}`);
                console.log(`Route: ${res.redirectUrl}`);

                loading(message);

                setTimeout(() => {
                    window.location.href = res.redirectUrl;
                }, 1000);

                $("#success-message").text(res.successMessage).fadeIn().delay(3000).fadeOut();

            }
            else {


                setTimeout(() => $('#loading-modal').modal('hide'), 500);

                $("#error-message").text(res.failedMessage).fadeIn().delay(500).fadeOut();

                console.log(ErrorMessages[1], `: ${response.failedMessage}`);
                console.log(ErrorMessages[3], `: ${res}`);
                console.log(`Route: ${url}`);
            }
        },
        error: function (err) {
            console.error(ErrorMessages[5], `: ${err.responseText}`);
            console.log(ErrorMessages[2], `: ${url}`);
            }
    });
}


async function UserLogin(event) {
    //event.preventDefault(); // Prevent the default form submission 

    const url = '/api/authenticate/user-login';
    const formData = new FormData(event.target);
    const data = {
        username: formData.get("username"),
        password: formData.get("password"),
        role: "standard"
    };
    const method = methods[0];
    const message = LoadingMessages[0]
    const apiurl = getUrl(url);
    const redirectUrl = `/dashboard/${data.username}`;
    const params = {
        apiurl,
        method,
        data,
        message
    };

    if ($("#user-login").valid()) {
           
        try {
            
            await sendRequest(
                url,
                method,
                data,
                message
            );    

            console.log('API Data', JSON.stringify(params, null, 2));
        }
        catch (error) {
            console.error('Login Error:', error);
        }
    }
    else {       
        const result = jsonResult(
            method,
            data,
            SuccessMessages[0],
            url,
            SuccessMessages[1],
            redirectUrl,
            actions[0],
            SuccessMessages[2],
            false,
            ErrorMessages[0],
            true,
            browserInfo.name,
            browserInfo.version
        );

        console.log("API Data", JSON.stringify(result, null, 2));
    }
}

async function AdminLogin(event) {
    event.preventDefault();

    const url = '/api/authenticate/admin-login';
    const formData = new FormData(event.target);
    const data = {
        username: formData.get("username"),
        password: formData.get("password"),
        role: "administrator"
    };
    const method = methods[0];
    const message = LoadingMessages[0]
    const apiurl = getUrl(url);
    const redirectUrl = `/dashboard/admin/`;
    const params = {
        apiurl,
        method,
        data,
        message
    };

    await sendRequest(
        url,
        method,
        data,
        message
    );

    console.log('API Data', JSON.stringify(params, null, 2));
    /*
    if ($("#admin-login").valid()) {

        try {

            
        }
        catch (error) {
            console.error('Login Error:', error);
        }
    }
    else {
        const result = jsonResult(
            method,
            data,
            SuccessMessages[0],
            url,
            SuccessMessages[1],
            redirectUrl,
            actions[0],
            SuccessMessages[2],
            false,
            ErrorMessages[0],
            true,
            browserInfo.name,
            browserInfo.version
        );

        console.log("API Data", JSON.stringify(result, null, 2));

    }*/
}

async function Logout(event) {
    event.preventDefault();

    console.log("Logout Button Clicked");
    const url = '/api/authenticate/logout';

    try {
        await logoutRequest(
            url,
            'POST',
            'Logging out'
        );
    }
    catch (error) {
        console.error('Logout Error:', error);
    }
}





