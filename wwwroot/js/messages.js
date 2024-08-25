const LoadingMessages = [
    'Loging in', //0
    'Loging out', //1
    'Please wait' //2
];

const SuccessMessages = [
    'Post Success', //0
    'Valid', //1
    'Login Success', //2
    'Logout Success' //3
];

const ErrorMessages = [
    'Login Failed', //0
    'Logout Failed', //1
    'Post Error', //2
    'Post Failed', //3
    'Invalid', //4
    'Login Error', //5
    'Logout Error', //6
];

const methods = [
    'POST', //0
    'GET', //1
    'PUT' //2
];

const actions = [
    'Login', //0
    'Logout', //1
    'Add', //2
    'Delete', //3
    'Modify', //4
    'Open', //5
    'Exit', //6
    'Create', //7
    'Refresh' //8
];

const ValidateFields = [
    'Enter valid credentials', //0
    'Please fill out all required fields', //1
    'Username or Password incorrect', //2
    'Username is required', //3
    'Username or Email is required', //4
    'Email format invalid', //5
    'Password is required', //6
    'Username must be at least 3 characters long', //7
    'Password must be at least 8 characters long',  //8
    'Matching credentials found', //9
    'Invalid credentials' //10
];

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
    const LogoutError = ErrorMessages[6];

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
        LogoutError //23
    };
}
