class ApiService {
    constructor() {
        //globals
        this.stringList = {
            Post: validationMessages.Post,
            Login: validationMessages.Login,
            LoginSuccess: validationMessages.LoginSuccess,
            LoginFailed: validationMessages.LoginFailed,
            FillRequiredFields: validationMessages.FillRequiredFields,
            UsernamePasswordIncorrect: validationMessages.UsernamePasswordIncorrect,
            MatchFound: validationMessages.MatchFound,
            Logout: validationMessages.Logout,
            LogoutRequest: validationMessages.LogoutRequest,
            LogoutFailed: validationMessages.LogoutFailed,
            LogoutError: validationMessages.LogoutError,
            ErrorOccured: 'An error occured',
            RegistrationFailed: 'Registration Failed'
        };


        this.method = this.stringList.Post;
    }

   

    async userRequest(form) {
        const requestroute = form.action;
        const formData = new FormData(form);
        const data = {
            username: formData.get("username"),
            password: formData.get("password"),
        };

        if (!ValidateForm(data)) {
            console.log('validation');
            return false;
        }

        try {
            const response = await this.sendRequest(this.method, requestroute, data);
            this.handleResponse('login', this.stringList.UsernamePasswordIncorrect, response, data);
            $('#user-login .form-group #loginbutton').html(spinner + 'Login').prop('disabled', true);
        } catch (ex) {
            this.handleException(ex, this.stringList.ErrorOccured.concat(" ", "while sending login request"));
        }

        return false;
    }

    async authRequest(data, url) {
        try {
            const response = await this.sendRequest(this.method, url, data);
            this.handleResponse('auth', this.stringList.UsernamePasswordIncorrect, response);
        } catch (ex) {
            this.showError(this.stringList.UsernamePasswordIncorrect);
        }

        return false;
    }

    async logoutRequest(url) {
        try {
            const response = await this.sendRequest(this.method, url);
            this.handleResponse('logout', this.stringList.LogoutFailed, response);
        } catch (ex) {
            this.handleException(ex, this.stringList.ErrorOccured.concat(" ", "while sending logout request"));
        }

        return false;
    }

    async createRequest(form) {
        const formData = new FormData(form);
        const data = {
            username: formData.get('signup-username'),
            email: formData.get('signup-email'),
            password: formData.get('signup-password'),
            confirmpassword: formData.get('signup-retypepassword')
        };

        if (!this.validateSignup(data)) {
            return false;
        }

        try {
            const response = await this.sendRequest(this.method, form.action, data);
            this.handleResponse('signup', this.stringList.RegistrationFailed, response, data);
            
        } catch (ex) {
            this.handleException(ex, this.stringList.ErrorOccured.concat(" ", "while signing up"));
        }

        return false;
    }

    async registerRequest(data, url) {

        try {
            const response = await this.sendRequest(this.method, url, data);
            this.handleResponse('register', this.stringList.RegistrationFailed, response);
        } catch (ex) {
            this.handleException(ex, this.stringList.ErrorOccured.concat(" ", "while registering"));
        }

        return false;
    }

    async sendRequest(method, url, data = null) {
        const options = {
            method: method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        };

        const response = await fetch(url, options);
       
        return await response.json();
    }

    // Handle responses and other utility methods
    handleResponse(action, customMessage, response, data = null) {
        const message = response.Message || customMessage;

        switch(action) {
            case 'login':
                this.CheckResponse(response) ? this.authRequest(data, response.RedirectUrl) : this.showError(message);
                console.log('login case');
                break;

            case 'auth':
                this.CheckResponse(response) ? this.CheckRoute(response) : this.showError(message);
                console.log('auth case');
                break;

            case 'signup':
                $('.signup-errors').text('');
                $('.input-group').removeClass('input-error');
                this.CheckResponse(response) ? this.registerRequest(data, response.RedirectUrl) : this.showError(message);
                console.log('signup case');
                break;

            case 'register':
                this.CheckResponse(response) ? this.showSuccess(message) : this.showError(message);
                console.log('register case');
                break;

            case 'logout':
                this.CheckResponse(response) ? window.location.href = response.RedirectUrl : this.showError(message);
                console.log('logout case');
                break;

            default:
                console.error('No switch case input matching');
                return false;
                break;
        }

        setTimeout(() => {
            $('#user-login .form-group #loginbutton').html('Login').prop('disabled', false);
        }, 1500);
    }

    CheckResponse(response) {
        if (response.IsValid) {

            return true;
            
        } else {
            return false;
        }
    }

    CheckRoute(response, action = null) {
        if (response.RedirectUrl) {
            window.location.href = response.RedirectUrl;

            if (action === 'register') {

            }
        } else {
            this.showError("Route is missing.");
        }
    }

    handleException(ex, customMessage) {
        console.error(customMessage, ex);
        this.showError(customMessage);
    }

    showSuccess(message) {
        if ($('#signup-form')) {
            $('.modal').each(function () {
                $(this).modal('hide');
            });
        }
        $('#success-message').text(message).fadeIn().delay(500).fadeOut();
    }

    showError(messages) {
        const UsernameInUse = 'Username is already in use';
        const EmailInUse = 'Email is already in use';

        const messageArray = Array.isArray(messages) ? messages : [messages];

        messageArray.forEach(message => {
            // Check if the message matches known errors
            if (message === UsernameInUse) {
                $('#usernameSignup-error').text(message);
                $('#username-group').addClass('input-error');
            } else if (message === EmailInUse) {
                $('#emailSignup-error').text(message);
                $('#email-group').addClass('input-error');
            } else {
                // If the message is not a known error, display it in a general error area
                $("#error-message").text(message).fadeIn().delay(500).fadeOut();
            }
        });
    }


    validateForm(data) {
        // Implement validation logic
        return true; // or false based on validation
    }

    validateSignup(data) {
        // Implement signup validation logic
        return true; // or false based on validation
    }
}


const apiService = new ApiService();


