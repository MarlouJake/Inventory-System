//Login API
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
        loading(validate.LogingIn);
        $.ajax({
            type: validate.Post,
            url: apiUrl,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            success: function (res) {

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

                    //setTimeout(() => $('#loading-modal').modal('hide'), 1000);
                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000);

                } else {
                    
                    setTimeout(() => $('#loading-modal').modal('hide'), 500);
                    setTimeout(() => {
                        $("#error-message").text(validate.UsernamePasswordIncorrect).fadeIn().delay(1000).fadeOut();
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
                setTimeout(() => $('#loading-modal').modal('hide'), 500);
                $("#error-message").text(validate.FillRequiredFields).fadeIn().delay(500).fadeOut();
            }
        });

    }
    catch (ex) {
        

        const result = jsonResult(
            validate.Post,
            validate.Login,
            validate.LoginSuccess,
            false,
            validate.LoginFailed,
            true,
            validate.InvalidCredentials + ':\n' + ex,
            data,
            validate.PostError,
            apiUrl,
            validate.ResponseInvalid,
            `/dashboard/${data.username}`,
            browserInfo.name,
            browserInfo.version
        );

        console.log("Submission Error Data", JSON.stringify(result, null, 2));
        setTimeout(() => $('#loading-modal').modal('hide'), 500);
        $("#error-message").text(validate.InvalidCredentials).fadeIn().delay(500).fadeOut();
    }
    setTimeout(() => $('#loading-modal').modal('hide'), 500);
    return false;
};

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
        loading(validate.LogingIn);
        $.ajax({
            type: validate.Post,
            url: apiUrl,
            data: JSON.stringify(data),
            contentType: 'application/json',
            dataType: 'json',
            success: function (res) {

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

                    //setTimeout(() => $('#loading-modal').modal('hide'), 1000);
                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000);

                } else {

                   
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

                    setTimeout(() => $('#loading-modal').modal('hide'), 500);
                    setTimeout(() => {
                        $("#error-message").text(validate.UsernamePasswordIncorrect).fadeIn().delay(1000).fadeOut();
                    }, 500);
                }

            },
            error: function (err) {
                
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

                setTimeout(() => $('#loading-modal').modal('hide'), 500);
                $("#error-message").text(validate.FillRequiredFields).fadeIn().delay(500).fadeOut();

            }
        });

    }
    catch (ex) {
       

        const result = jsonResult(
            validate.Post,
            validate.Login,
            validate.LoginSuccess,
            false,
            validate.LoginFailed,
            true,
            validate.InvalidCredentials + ':\n' + ex,
            data,
            validate.PostError,
            apiUrl,
            validate.ResponseInvalid,
            `/dashboard/${data.username}`,
            browserInfo.name,
            browserInfo.version
        );

        console.log("Submission Error Data", JSON.stringify(result, null, 2));
        setTimeout(() => $('#loading-modal').modal('hide'), 500);
        $("#error-message").text(validate.InvalidCredentials).fadeIn().delay(500).fadeOut();
    }
    setTimeout(() => $('#loading-modal').modal('hide'), 500);
    return false;
};


//Logout API 
LogoutRequest = (id, name) => {

    const url = '/api/authenticate/logout';

    const logoutdata = {
        userid: id,
        username: name,
        logoutrole: 'anonymous'
    };

    try {
        loading(validate.LogingOut);
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
                    
                    setTimeout(() => {
                        window.location.href = res.redirectUrl;
                    }, 1000);      

                    $("#success-message").text(validate.LogoutSuccess).fadeIn().delay(3000).fadeOut();
                }
                else {
                    
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

                    setTimeout(() => $('#loading-modal').modal('hide'), 500);
                    $("#error-message").text(validate.LogoutFailed).fadeIn().delay(500).fadeOut();

                }

            },

            error: function (err) {
                const result = jsonResult(
                    validate.Post,
                    validate.action,
                    validate.LogoutSuccess,
                    false,
                    validate.LogoutFailed,
                    true,
                    validate.LogoutFailed  + ':\n' + err,
                    logoutdata,
                    validate.PostFailed,
                    getUrl(url),
                    validate.ResponseInvalid,
                    '/home/login',
                    browserInfo.name,
                    browserInfo.version
                );

                console.log("Submission Data", JSON.stringify(result, null, 2));

                setTimeout(() => $('#loading-modal').modal('hide'), 500);
                $("#error-message").text(validate.LogoutFailed).fadeIn().delay(500).fadeOut();
            }

        });
    }
    catch (ex) {
        setTimeout(() => $('#loading-modal').modal('hide'), 500);
        const result = jsonResult(
            validate.Post,
            validate.action,
            validate.LogoutSuccess,
            false,
            validate.LogoutFailed,
            true,
            validate.LogoutError + ':\n' + ex,
            logoutdata,
            validate.PostError,
            getUrl(url),
            validate.ResponseInvalid,
            '/home/login',
            browserInfo.name,
            browserInfo.version
        );

        console.log("Submission Error Data", JSON.stringify(result, null, 2));
        setTimeout(() => $('#loading-modal').modal('hide'), 500);
        $("#error-message").text(validate.LogoutError).fadeIn().delay(500).fadeOut();

    }
    setTimeout(() => $('#loading-modal').modal('hide'), 500);
    return false;
};








