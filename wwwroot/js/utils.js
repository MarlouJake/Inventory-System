



// Format timestamp as "MMMM dd, yyyy hh:mm:ss tt"
function DateFormatOptions() {
    return options = {
        year: 'numeric', month: 'long', day: '2-digit',
        hour: '2-digit', minute: '2-digit', second: '2-digit',
        hour12: true
    };
}




//Function for changing text color depending on text
function NewItemAdded() {
    const statusDisplays = document.getElementsByClassName('status-container');
    const firmwareUpdateDisplay = document.getElementsByClassName('firmware-update');


    Array.from(statusDisplays).forEach(statusDisplay => {
        if (statusDisplay.textContent.trim() === "Complete") {
            statusDisplay.classList.add('text-success');
        } else if (statusDisplay.textContent.trim() === "Incomplete(Usable)") {
            statusDisplay.classList.add('text-primary');
        } else if (statusDisplay.textContent.trim() === "Incomplete(Unusable)") {
            statusDisplay.classList.add('text-danger');
        }
    });

    Array.from(firmwareUpdateDisplay).forEach(firmwareUpdateDisplay => {
        if (firmwareUpdateDisplay.textContent.trim() === "Updated") {
            firmwareUpdateDisplay.classList.add('text-success');
        } else if (firmwareUpdateDisplay.textContent.trim() === "N/A") {
            firmwareUpdateDisplay.classList.add('text-muted');
        } else if (firmwareUpdateDisplay.textContent.trim() === "Not Updated") {
            firmwareUpdateDisplay.classList.add('text-danger');
        }
    });


}


//Returns full url
function getUrl(url) {

    const fullUrl = `${window.location.origin}${url}`;

    return fullUrl;

};


//Returns Browser info
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
};

function Validations() {
    const Post = methods[0];
    const Put = methods[2];

    const Login = actions[0];
    const Logout = actions[1];
    const Add = actions[2];
    const Modify = actions[4];

    const PostSuccess = SuccessMessages[0];
    const ResponseValid = SuccessMessages[1];
    const LoginSuccess = SuccessMessages[2];
    const AddingSuccess = SuccessMessages[4];
    const PutSuccess = SuccessMessages[5];
    const UpdateSuccess = SuccessMessages[6];

    const LoginFailed = ErrorMessages[0];
    const PostFailed = ErrorMessages[3];
    const ResponseInvalid = ErrorMessages[4];
    const AddingFailed = ErrorMessages[7];
    const PutFaield = ErrorMessages[9];
    const UpdateFailed = ErrorMessages[11];


    const FillRequiredFields = ValidateFields[1];
    const UsernamePasswordIncorrect = ValidateFields[2];
    const UsernameEmpty = ValidateFields[4];
    const PasswordEmpty = ValidateFields[6];
    const UsernameLength = ValidateFields[7];
    const PasswordLegth = ValidateFields[8];
    const MatchFound = ValidateFields[9];
    const InvalidCredentials = ValidateFields[10];
    const SuccessToAddDatabase = ValidateFields[11];
    const FailedToAddDatabase = ValidateFields[12];
    const InvalidInput = ValidateFields[13];

    const UpdatedSuccessfully = ActionMessages[0];
    const FailedToUpdate = ActionMessages[1];


    const LogoutSuccess = SuccessMessages[3];
    const LogoutFailed = ErrorMessages[1];
    const LogoutError = ErrorMessages[6];
    const AddingError = ErrorMessages[8];
    const PutError = ErrorMessages[10];

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
        FillRequiredFields, //22
        LogoutError, //23
        AddingSuccess,
        AddingFailed,
        AddingError,
        Add,
        SuccessToAddDatabase,
        FailedToAddDatabase,
        InvalidInput,
        Put,
        Modify,
        PutSuccess,
        PutError,
        PutFaield,
        UpdateSuccess,
        UpdateFailed,
        UpdatedSuccessfully,
        FailedToUpdate
    };
}


function ProcessRequest() {
    const JsonApp = DataType[0];
    const JsonFile = DataType[1];

    return processes = {
        JsonApp,
        JsonFile
    };
}

//Returns  JSON Format text
function jsonResult( method, _action, _actionMessage1, boolvalue1, _actionMessage2, boolvalue2, actionresponse,
    requestmessage, api, responsestatus, _targetroute, _browserinfo) {

    const timestamp = new Date().toLocaleString('en-US', DateFormatOptions);

    return {
        method,
        actions: {
            action: _action,
            [_actionMessage1]: [boolvalue1],
            [_actionMessage2]: [boolvalue2],
            message: actionresponse
        },
        requestmessage: requestmessage,
        apiurl: api,
        response: responsestatus,
        targeroute: getUrl(_targetroute),
        browserdetails: {
            [browserInfo.name]: browserInfo.version
        },
        timestamp
    };
};

//Function for input-error class
function RemoveClass(element) {
    element.classList.remove('input-error');
};

function UserLoginValidateField() {
    const username = document.getElementById('username');
    const usernameError = document.getElementById('username-error');
    const password = document.getElementById('password');
    const passwordError = document.getElementById('password-error');

    //User Login Form input fields client-side validation
    if (username || password) {
        username.addEventListener('input', function () {
            usernameError.textContent = '';
            RemoveClass(username);

            if (username.value.length >= 64) {
                usernameError.textContent = "Username or Email reached maximum limit of 64 characters";
            } else {
                usernameError.textContent = '';
                RemoveClass(username);
            }
        });
        password.addEventListener('input', function () {
            passwordError.textContent = '';
            RemoveClass(password);

            if (password.value.length >= 128) {
                passwordError.textContent = "Password reached maximum limit of 128 characters";
            } else {
                passwordError.textContent = '';
                RemoveClass(password);
            }
        });

    }
}

function AdminLoginValidateField() {
    const user = document.getElementById('user');
    const userError = document.getElementById('user-error');
    const pwd = document.getElementById('pwd');
    const pwdError = document.getElementById('pwd-error');

    //Admin Login Form input fields client-side validation
    if (user || pwd) {
        user.addEventListener('input', function () {
            userError.textContent = '';
            RemoveClass(user);
            /*
            if (user.value.trim() == '') {
                userError.textContent = validate.UsernameEmpty;
            } else if (user.value.length < 3) {
                userError.textContent = validate.UsernameLength;
            } else {
                userError.textContent = '';
                RemoveClass(user);
            }*/
        });
        pwd.addEventListener('input', function () {
            pwdError.textContent = '';
            RemoveClass(pwd);
            /*
            if (pwd.value.trim() == '') {
                pwdError.textContent = validate.PasswordEmpty;
            } else if (pwd.value.length < 8) {
                pwdError.textContent = validate.PasswordLegth;
            } else {
                pwdError.textContent = '';
                RemoveClass(pwd);
            }*/
        });
    }
}

function DisplaySuccessAndError() {
    // Show success message if it exists
    const successMessage = document.getElementById('success-message');
    if (successMessage && successMessage.textContent.trim() !== "") {
        successMessage.style.display = 'block';
        setTimeout(() => successMessage.style.display = 'none', 1500); // Show for 1.5 seconds
    }

    // Show error message if it exists
    const errorMessage = document.getElementById('error-message');
    if (errorMessage && errorMessage.textContent.trim() !== "") {
        errorMessage.style.display = 'block';
        setTimeout(() => errorMessage.style.display = 'none', 1500); // Show for 1.5 seconds
    }

    const userInput = document.getElementsByClassName('user-input');
    function trimInput(element) {
        element.value = element.value.trim();
    }
}

function ShowPassword(input, main, icon) {
    main.on('mousedown', function () {
        holdTimeout = setTimeout(() => {

            if (main) {
                if (input.attr('type') === 'password') {
                    input.attr('type', 'text');
                    icon.removeClass('fa-regular').addClass('fa-solid');
                }
            }
        }, 50);
        $(this).addClass('holding');

        $(this).on('mouseup', function () {
            clearTimeout(holdTimeout);
            input.attr('type', 'password');
            icon.removeClass('fa-solid').addClass('fa-regular');
            $(this).removeClass('holding');
        });
    });
}



function ChangeTextColor(element, text1, text2, text3, optionaltext) {
    var container = $(element);

    if (container.length) {
        var textcontent = container.text().trim(); // Get the text and trim any whitespace

        switch (textcontent) {
            case text1:
                container.css('color', 'green'); // Set text color to green
                break;
            case text2:
                container.css('color', 'red'); // Set text color to red
                break;
            case text3:
                container.css('color', 'gray'); // Set text color to gray
                break;
            case optionaltext:
                container.css('color', 'blue'); // Set text color to blue
                break;
            default:
                container.css('color', 'black'); // Default text color
                break;
        }
    }
}

//initialize Validation Function
const validationMessages = Validations();
const browserInfo = getBrowserInfo();
const process = ProcessRequest();