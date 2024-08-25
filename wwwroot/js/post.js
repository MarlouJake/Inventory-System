function getUrl(url) {

    const fullUrl = `${window.location.origin}${url}`;

    return fullUrl;
    
}

function Validations() {
    const Post = methods[0];
    const Login = actions[0];
    const Logout = actions[1];

    const PostSuccess = SuccessMessages[0];
    const ResponseValid = SuccessMessages[1];
    const LoginSuccess = SuccessMessages[2];

    const LoginFailed = ErrorMessages[0];
    const PostFailed = ErrorMessages[3];
    const ResponseInvalid = ErrorMessages[4];

    const FillRequiredFields = ValidateFields[1];
    const UsernamePasswordIncorrect = ValidateFields[2];
    const UsernameEmpty = ValidateFields[4];
    const PasswordEmpty = ValidateFields[6];
    const UsernameLength = ValidateFields[7];
    const PasswordLegth = ValidateFields[8];
    const MatchFound = ValidateFields[9];
    const InvalidCredentials = ValidateFields[10];

    const LogoutSuccess = SuccessMessages[3]; 
    const LogoutFailed = ErrorMessages[1];

    const LogingIn = LoadingMessages[0]; 
    const LogingOut = LoadingMessages[1];
    const PleaseWait = LoadingMessages[2]; 

    const PostError = ErrorMessages[2];

 

    return validations = {
        Post, //0
        Login, //1
        PostSuccess, //2
        ResponseValid, //3
        LoginSuccess, //4
        LoginFailed, //5
        PostFailed, //6
        ResponseInvalid, //7
        UsernameEmpty, //8
        PasswordEmpty, //9
        UsernameLength, //10
        PasswordLegth, //11
        LogoutSuccess, //12
        LogoutFailed, //13
        LogingIn, //14
        LogingOut, //15
        PleaseWait, //16
        Logout, //17
        PostError, //18
        MatchFound, //19
        InvalidCredentials, //20
        UsernamePasswordIncorrect, //21
        FillRequiredFields //22
    };
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

function jsonResult(
    method, _action, _actionMessage1, Val1, _actionMessage2, Val2, _actionMessage3, data,
    resMessage, api, resStatus, targetRoute, _browser, Val3) {

    const ResponseMessage = resMessage;
    const ApiUrl = api;
    const ResponseStatus = resStatus;
    const TagerRoute = getUrl(targetRoute);
    const Action = _action
    const ActionMessage1 = _actionMessage1;
    const ActionMessage2 = _actionMessage2;
    const ActionMessage3 = _actionMessage3;
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
            [ActionMessage2]: [Validity2],
            message: ActionMessage3
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


const validate = Validations();
const browserInfo = getBrowserInfo();

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


UserRequest = (form) => {

    const apiUrl = form.action;
    const formData = new FormData(form);
    const data = {
        username: formData.get("username"),
        password: formData.get("password"),
        loginrole: 'standard'
    };

    $('#username-error').text('');
    $('#password-error').text('');
    $('.form-control').removeClass('input-error');

    if (!$(form).valid()) {

        if (!data.username || data.username.trim() === '') {
            $('#username').addClass('input-error');
            $('#username-error').text(validate.UsernameEmpty);
           
        } else if (data.username.length < 3) {
            $('#username').addClass('input-error');
            $('#username-error').text(validate.UsernameLength);
 
        }


        if (!data.password || data.password.trim() === '') {
            $('#password').addClass('input-error');
            $('#password-error').text(validate.PasswordEmpty);

        } else if (data.password.length < 8) {
            $('#password').addClass('input-error');
            $('#password-error').text(validate.PasswordLegth);

        }
      
        return false; // Prevent form submission if invalid
    }

    try {
        $.ajax({
            type: validate.Post,
            url: apiUrl,
            data: JSON.stringify(data),
            contentType: 'application/json',  
            dataType: 'json',
            success: function (res) {
                loading(validate.LogingIn);
                if (res.isValid) {
                    const result = jsonResult(
                        validate.Post,
                        validate.Login,
                        validate.LoginSuccess,
                        true,
                        validate.LoginFailed,
                        false,
                        validate.MatchFound,
                        data,
                        validate.PostSuccess,
                        apiUrl,
                        validate.ResponseValid,
                        res.redirectUrl,
                        browserInfo.name,
                        browserInfo.version
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));
                    
                    setTimeout(() => $('#loading-modal').modal('hide'), 1000);
                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000);

                } else {

                    setTimeout(() => $('#loading-modal').modal('hide'), 500);
                    setTimeout(() => {
                        $("#error-message").text(res.errorMessage).fadeIn().delay(1000).fadeOut();
                    }, 500);
                   

                    const result = jsonResult(
                        validate.Post,
                        validate.Login,
                        validate.LoginSuccess,
                        false,
                        validate.LoginFailed,
                        true,
                        validate.UsernamePasswordIncorrect,
                        data,
                        validate.PostSuccess,
                        apiUrl,
                        validate.ResponseValid,
                        `/dashboard/${data.username}`,
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
                    validate.Post,
                    validate.Login,
                    validate.LoginSuccess,
                    false,
                    validate.LoginFailed,
                    true,
                    valid.FillRequiredFields,
                    data,
                    validate.PostFailed,
                    apiUrl,
                    validate.ResponseInvalid,
                    `/dashboard/${data.username}`,
                    browserInfo.name,
                    browserInfo.version
                );

                console.log("Submission Data", JSON.stringify(result, null, 2));
            }
        });
        
    }
    catch (ex) {
        setTimeout(() => $('#loading-modal').modal('hide'), 500);

        const result = jsonResult(
            validate.Post,
            validate.Login,
            validate.LoginSuccess,
            false,
            validate.LoginFailed,
            true,
            validate.InvalidCredentials+'\n'+ex,
            data,
            validate.PostError,
            apiUrl,
            validate.ResponseInvalid,
            `/dashboard/${data.username}`,
            browserInfo.name,
            browserInfo.version
        );

        console.log("Submission Error Data", JSON.stringify(result, null, 2));
    }
    return false;
}

AdminRequest = (form) => {

    const apiUrl = form.action;
    const formData = new FormData(form);
    const data = {
        username: formData.get("user"),
        password: formData.get("pwd"),
        loginrole: 'admin'
    };

    $('#user-error').text('');
    $('#pwd-error').text('');
    $('.form-control').removeClass('input-error');

    if (!$(form).valid()) {

        if (!data.username || data.username.trim() === '') {
            $('#user').addClass('input-error');
            $('#user-error').text(validate.UsernameEmpty);

        } else if (data.username.length < 3) {
            $('#user').addClass('input-error');
            $('#user-error').text(validate.UsernameLength);

        }


        if (!data.password || data.password.trim() === '') {
            $('#pwd').addClass('input-error');
            $('#pwd-error').text(validate.PasswordEmpty);

        } else if (data.password.length < 8) {
            $('#pwd').addClass('input-error');
            $('#pwd-error').text(validate.PasswordLegth);

        }

        return false; // Prevent form submission if invalid
    }

    try {
        $.ajax({
            type: validate.Post,
            url: apiUrl,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            success: function (res) {
                loading(validate.LogingIn);
                if (res.isValid) {
                    const result = jsonResult(
                        validate.Post,
                        validate.Login,
                        validate.LoginSuccess,
                        true,
                        validate.LoginFailed,
                        false,
                        validate.MatchFound,
                        data,
                        validate.PostSuccess,
                        apiUrl,
                        validate.ResponseValid,
                        res.redirectUrl,
                        browserInfo.name,
                        browserInfo.version
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));

                    setTimeout(() => $('#loading-modal').modal('hide'), 1000);
                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000);

                } else {

                    setTimeout(() => $('#loading-modal').modal('hide'), 500);
                    setTimeout(() => {
                        $("#error-message").text(res.errorMessage).fadeIn().delay(1000).fadeOut();
                    }, 500);


                    const result = jsonResult(
                        validate.Post,
                        validate.Login,
                        validate.LoginSuccess,
                        false,
                        validate.LoginFailed,
                        true,
                        validate.UsernamePasswordIncorrect,
                        data,
                        validate.PostSuccess,
                        apiUrl,
                        validate.ResponseValid,
                        `/dashboard/${data.username}`,
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
                    validate.Post,
                    validate.Login,
                    validate.LoginSuccess,
                    false,
                    validate.LoginFailed,
                    true,
                    validate.FillRequiredFields,
                    data,
                    validate.PostFailed,
                    apiUrl,
                    validate.ResponseInvalid,
                    `/dashboard/${data.username}`,
                    browserInfo.name,
                    browserInfo.version
                );

                console.log("Submission Data", JSON.stringify(result, null, 2));
            }
        });

    }
    catch (ex) {
        setTimeout(() => $('#loading-modal').modal('hide'), 500);

        const result = jsonResult(
            validate.Post,
            validate.Login,
            validate.LoginSuccess,
            false,
            validate.LoginFailed,
            true,
            validate.InvalidCredentials + '\n' + ex,
            data,
            validate.PostError,
            apiUrl,
            validate.ResponseInvalid,
            `/dashboard/${data.username}`,
            browserInfo.name,
            browserInfo.version
        );

        console.log("Submission Error Data", JSON.stringify(result, null, 2));
    }
    return false;
}

LogoutRequest = (id, name) => {
    const url = '/api/authenticate/logout';

    const logoutdata = {
        userid: id,
        username: name,
        logoutrole: 'anonymous'
    };

    try {
        $.ajax({
            type: validate.Post,
            url: url,
            data: JSON.stringify(logoutdata),
            success: function (res) {

                if (res.isValid) {

                    const result = jsonResult(
                        validate.Post,
                        validate.Logout,
                        validate.LogoutSuccess,
                        true,
                        validate.LogoutFailed,
                        false,
                        validate.LogoutSuccess,
                        logoutdata,
                        validate.PostSuccess,
                        getUrl(url),
                        validate.ResponseValid,
                        res.redirectUrl,
                        browserInfo.name,
                        browserInfo.version
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));

                    loading(validate.LogingOut);

                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000);

                    $("#success-message").text(res.successMessage).fadeIn().delay(3000).fadeOut();
                }
                else {
                    setTimeout(() => $('#loading-modal').modal('hide'), 500);

                    $("#error-message").text(res.failedMessage).fadeIn().delay(500).fadeOut();

                    const result = jsonResult(
                        validate.Post,
                        validate.action,
                        validate.LogoutSuccess,
                        false,
                        validate.LogoutFailed,
                        true,
                        validate.LogoutFailed,
                        logoutdata,
                        validate.PostSuccess,
                        getUrl(url),
                        validate.ResponseValid,
                        '/home/login',
                        browserInfo.name,
                        browserInfo.version
                    );

                    console.log("API Data", JSON.stringify(result, null, 2));

                }
                
            }, 

            error: function(err) {
                const result = jsonResult(
                    validate.Post,
                    validate.action,
                    validate.LogoutSuccess,
                    false,
                    validate.LogoutFailed,
                    true,
                    err,
                    null,
                    validate.PostFailed,
                    getUrl(url),
                    validate.ResponseInvalid,
                    '/home/login',
                    browserInfo.name,
                    browserInfo.version
                );

                console.log("Submission Data", JSON.stringify(result, null, 2));
            }

        });
    }
    catch (ex){
        setTimeout(() => $('#loading-modal').modal('hide'), 500);
        const result = jsonResult(
            validate.Post,
            validate.action,
            validate.LogoutSuccess,
            false,
            validate.LogoutFailed,
            true,
            validate.InvalidCredentials + '\n' + ex,
            null,
            validate.PostError,
            getUrl(url),
            validate.ResponseInvalid,
            '/home/login',
            browserInfo.name,
            browserInfo.version
        );

        console.log("Submission Error Data", JSON.stringify(result, null, 2));

    }
    return false;
}

/*
async function logoutRequest(url, method, message) {
    await $.ajax({
        url: url,
        type: method,
        success: function (res) {

            if (res.isValid) {

                const result = jsonResult(
                    validate.method,
                    validate.action,
                    validate.LogoutSuccess,
                    true,
                    validate.LogoutFailed,
                    false,
                    null,
                    validate.PostSuccess,
                    getUrl(url),
                    validate.ResponseValid,
                    res.redirectUrl,
                    browserInfo.name,
                    browserInfo.version
                );

                loading(validate.LogingOut);

                setTimeout(() => {
                    window.location.href = res.redirectUrl;
                }, 1000);

                $("#success-message").text(res.successMessage).fadeIn().delay(3000).fadeOut();

            }
            else {


                setTimeout(() => $('#loading-modal').modal('hide'), 500);

                $("#error-message").text(res.failedMessage).fadeIn().delay(500).fadeOut();

                const result = jsonResult(
                    validate.method,
                    validate.action,
                    validate.LogoutSuccess,
                    true,
                    validate.LogoutFailed,
                    false,
                    null,
                    validate.PostSuccess,
                    getUrl(url),
                    validate.ResponseValid,
                    '/home/login',
                    browserInfo.name,
                    browserInfo.version
                );

                console.log("API Data", JSON.stringify(result, null, 2));

            }
        },
        error: function (err) {
            console.error(ErrorMessages[5], `: ${err.responseText}`);
            console.log(ErrorMessages[2], `: ${url}`);

            const result = jsonResult(
                validate.method,
                validate.action,
                validate.LogoutSuccess,
                false,
                validate.LogoutFailed,
                true,
                null,
                validate.PostFailed,
                getUrl(url),
                validate.ResponseInvalid,
                '/home/login',
                browserInfo.name,
                browserInfo.version
            );

            console.log("Submission Data", JSON.stringify(result, null, 2));
        }
    });
}
*/

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





