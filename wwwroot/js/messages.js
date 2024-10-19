const LoadingMessages = [
    'Loging in', //0
    'Loging out', //1
    'Please wait' //2
];

const SuccessMessages = [
    'Post Success', //0
    'Valid', //1
    'Login Success', //2
    'Logout Success', //3
    'Adding Sucess', //4
    'Put Success', //5
    'Update Success' //6
];

const ErrorMessages = [
    'Login Failed', //0
    'Logout Failed', //1
    'Post Error', //2
    'Post Failed', //3
    'Invalid', //4
    'Login Error', //5
    'Logout Error', //6
    'Adding Failed', //7
    'Adding Error', //8
    'Put Failed', //9
    'Put Error', //10
    'Update Failed' //11
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
    'Invalid credentials', //10
    'Adding to database Successfull', //11
    'Adding to database Failed', //12
    'Invalid Input' //13
];

const DataType = [
    'application/json', //0
    'json', //1
];


const ActionMessages = [
    'Update Successful', //0
    'Failed To Update', //1
];


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

const validationMessages = Validations();