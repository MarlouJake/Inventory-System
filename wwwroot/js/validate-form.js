const valid = "text-success";
const invalid = "text-danger";
const defaultColor = "text-muted";




function ValidateSignUpForm() {
    const username = document.getElementById('signup-username');
    const usernameError = document.getElementById('usernameSignup-error');

    const email = document.getElementById('signup-email');
    const emailError = document.getElementById('emailSignup-error');

    const password = document.getElementById('signup-password');
    const passwordGroup = document.getElementById('signupPasswordGroup');

    const confirm = document.getElementById('signup-retypepassword');
    const confirmPasswordGroup = document.getElementById('confirmPasswordGroup');
    const confirmPasswordError = document.getElementById('signupPass-confirmError');

    const signUpButton = document.getElementById('signup-button');

    const validationElements = {
        hasSpaces: $('#hasSpaces'),
        passwordLength: $('#passwordLength'),
        hasNumber: $('#hasNumber'),
        specialChar: $('#specialChar'),
        upperCaseChar: $('#upperCaseChar'),
        lowerCaseChar: $('#lowerCaseChar')
    };

    const checkFunctions = [
        () => validateField(username, usernameError, "Username", 3, 64),
        () => validateField(email, email, emailError, "Email", 3, 64),
        () => validateConfirmPassword(password, confirm, confirmPasswordError, passwordGroup, confirmPasswordGroup),
        () => checkPasswordLength(password, validationElements.passwordLength),
        () => checkForNumbers(password, validationElements.hasNumber),
        () => checkSpaces(password, validationElements.hasSpaces),
        () => checkSpecialChar(password, validationElements.specialChar),
        () => checkUpperLower(password, validationElements.upperCaseChar, /[A-Z]/, 'uppercase'),
        () => checkUpperLower(password, validationElements.lowerCaseChar, /[a-z]/, 'lowercase')
    ];

    function runValidations() {
        let allValid = true;
        checkFunctions.forEach(fn => {
            if (!fn()) {
                allValid = false;
            }
        });

        // Enable or disable the button based on validity
        signUpButton.disabled = !allValid;
    }

    runValidations(); 
    username.addEventListener('input', runValidations);
    email.addEventListener('input', runValidations);
    password.addEventListener('input', () => {
        validateConfirmPassword(password, confirm, confirmPasswordError, passwordGroup, confirmPasswordGroup);
        runValidations();
    });

    password.addEventListener('blur', () => {
        validateConfirmPassword(password, confirm, confirmPasswordError, passwordGroup, confirmPasswordGroup);
        runValidations();
    });
    confirm.addEventListener('input', () => {
        validateConfirmPassword(password, confirm, confirmPasswordError, passwordGroup, confirmPasswordGroup);
        runValidations();
    });
    confirm.addEventListener('blur', () => {
        validateConfirmPassword(password, confirm, confirmPasswordError, passwordGroup, confirmPasswordGroup);
        runValidations();
    });
}
function ValidateSignup(data) {  
    const username = document.getElementById('signup-username');
    const usernameError = document.getElementById('usernameSignup-error');

    const email = document.getElementById('signup-email');
    const emailError = document.getElementById('emailSignup-error');

    const password = document.getElementById('signup-password');
    const passwordGroup = document.getElementById('signupPasswordGroup');

    const confirm = document.getElementById('signup-retypepassword');
    const confirmPasswordGroup = document.getElementById('confirmPasswordGroup');
    const confirmPasswordError = document.getElementById('signupPass-confirmError');

    const signUpButton = document.getElementById('signup-button');

    let isValid = true;
    resetErrors();

    isValid &= validateField(data.username, username, usernameError, "Username", 3, 64);
    isValid &= validateField(data.email, email, emailError, "Email", 3, 64);

    const validationElements = {
        hasSpaces: $('#hasSpaces'),
        passwordLength: $('#passwordLength'),
        hasNumber: $('#hasNumber'),
        specialChar: $('#specialChar'),
        upperCaseChar: $('#upperCaseChar'),
        lowerCaseChar: $('#lowerCaseChar')
    };

    if (!checkSpaces(password, validationElements.hasSpaces)) {
        console.error('Validation failed: Password contains spaces');
        isValid = false;
    }
    if (!checkForNumbers(password, validationElements.hasNumber)) {
        console.error('Validation failed: Password does not contain a number');
        isValid = false;
    }
    if (!checkPasswordLength(password, validationElements.passwordLength)) {
        console.error('Validation failed: Password length is invalid');
        isValid = false;
    }
    if (!checkSpecialChar(password, validationElements.specialChar)) {
        console.error('Validation failed: Password does not contain a special character');
        isValid = false;
    }

    if (!validateConfirmPassword(password, confirm, confirmPasswordError, passwordGroup, confirmPasswordGroup)) {
        isValid = false;
    }
    return isValid;
}
function resetErrors() {
    $('.signup-errors').text('');
    $('.form-control').removeClass('input-error');
}
function validateField(field, errorElement, fieldName, minLength, maxLength) {
    if (field.value.trim() === '') {
        return false;
    }
    if (!validateLength(field, minLength, maxLength, errorElement, fieldName)) {
        field.classList.add('input-error');
        return false;
    }
    return true;
}
function validateLength(input, minLength, maxLength, errorElement, fieldName) {
    if (input.length < minLength) {
        errorElement.textContent = `${fieldName} should be at least ${minLength} characters`;
        return false;
    } else if (input.length > maxLength) {
        errorElement.textContent = `${fieldName} should not exceed ${maxLength} characters`;
        return false;
    }
    return true;
}

/**
 * Validates that the confirmation password matches the original password.
 * 
 * This function checks if the password and confirmPassword fields are filled out
 * correctly. It ensures that:
 * - The password field is not empty.
 * - The confirm password field is not empty if the password is filled.
 * - The confirm password matches the password.
 * 
 * If any of these checks fail, appropriate error messages are displayed, 
 * and error classes are applied to the input elements.
 * 
 * @param {HTMLInputElement} password - The password input element.
 * @param {HTMLInputElement} confirmPassword - The confirmation password input element.
 * @param {HTMLInputElement} confirmPasswordErrorElement - The element to display error messages for confirm password.
 * @param {HTMLInputElement} passwordGroupElement - The element representing the password input group.
 * @param {HTMLInputElement} confirmPasswordGroupElement - The element representing the confirmation password input group.
 * 
 * @returns {boolean} - Returns true if the validation passes; otherwise, returns false.
 */
function validateConfirmPassword(password, confirmPassword, confirmPasswordErrorElement, passwordGroupElement, confirmPasswordGroupElement) {
    if (password.value.length === 0) {
        confirmPasswordErrorElement.textContent = '';
        return false;
    } else if (password.value.length > 0 && confirmPassword.value.length === 0) {
        confirmPasswordErrorElement.textContent = 'Retype your password';
        return false;
    } else if (password.value !== confirmPassword.value) {
        confirmPasswordErrorElement.textContent = 'Passwords do not match';
        passwordGroupElement.classList.add('input-error');
        confirmPasswordGroupElement.classList.add('input-error');
        return false;
    } else {
        confirmPasswordErrorElement.textContent = '';
        passwordGroupElement.classList.remove('input-error');
        confirmPasswordGroupElement.classList.remove('input-error');
        return true;
    }
    return true;
}

function checkSpecialChar(str, element) {
    const regex = /[!@#$%^&*()\-+={}[\]:;"'<>,.?\/|\\]/;
    if (str.value.length === 0) {
        element.removeClass(valid);
        element.removeClass(invalid);
        element.addClass(defaultColor);
        return false;

    } else if (regex.test(str.value)) {
        element.removeClass(defaultColor);
        element.removeClass(invalid);
        element.addClass(valid);
        return true;
    } else {
        element.removeClass(defaultColor);
        element.removeClass(valid);
        element.addClass(invalid);
        return false;
    }
    return true;
}
function checkUpperLower(str, element, regex, type) {
    if (str.value.length === 0) {
        element.addClass(defaultColor);
        return false;
    } else if (regex.test(str.value)) {
        element.removeClass(invalid).addClass(valid).removeClass(defaultColor);
        return true;
    } else {
        element.removeClass(valid).addClass(invalid).removeClass(defaultColor);
        return false;
    }
}
function checkSpaces(str, element) {
    if (str.value.length === 0) {
        element.addClass(defaultColor);
        return false;
    } else if (/\s/.test(str.value)) {
        element.removeClass(valid).addClass(invalid).removeClass(defaultColor);
        return false;
    } else {
        element.removeClass(invalid).addClass(valid).removeClass(defaultColor);
        return true;
    }
}
function checkForNumbers(str, element) {
    if (str.value.length === 0) {
        element.addClass(defaultColor);
        return false;
    } else if (/\d/.test(str.value)) {
        element.removeClass(invalid).addClass(valid).removeClass(defaultColor);
        return true;
    } else {
        element.removeClass(valid).addClass(invalid).removeClass(defaultColor);
        return false;
    }
}
function checkPasswordLength(str, element) {
    if (str.value.length === 0) {
        element.removeClass(invalid).removeClass(valid).addClass(defaultColor);
        return false;
    } else if (str.value.length < 8 || str.value.length > 128) {
        element.removeClass(defaultColor).removeClass(valid).addClass(invalid);
        return false;
    } else {
        element.removeClass(defaultColor).removeClass(invalid).addClass(valid);
        return true;
    }
}



/**
 * Validates the input data and clears any error messages.
 *
 * This function takes the provided data and clears the text of error message elements
 * for the given fields, such as code, name, status, firmware, and category. 
 *
 * Sample element id to be passed: '#elementId'
 * 
 * @param {Object} data - The input data object to be validated.
 * 
 * @param {jQuery} $code - jQuery object for the code input element.
 * @param {jQuery} $name - jQuery object for the name input element.
 * @param {jQuery} $description - jQuery object for the description input element.
 * @param {jQuery} $codeError - jQuery object for the code error message element.
 * @param {jQuery} $nameError - jQuery object for the name error message element.
 * 
 * @returns {boolean} - Returns a boolean value true or false.
 */
function ValidateDataInput(data, $code, $name, $description, $codeError, $nameError, $descriptionError) {
    var elementIds = [$codeError, $nameError, $descriptionError];
    elementIds.forEach(function (element) {
        $(element).text('');
    });

    $('.form-control').removeClass('input-error').addClass('input-success');

    let isValid = true;

    if (!data.itemcode || data.itemcode.trim() === '') {
        $($code).addClass('input-error');
        $($codeError).text('Item code is required');
        isValid = false;
    } else if (data.itemcode.length < 3) {
        $($code).addClass('input-error');
        $($codeError).text('Item code is should be atleast 3 characters');
        isValid = false;
    } else if (data.itemcode.length > 128) {
        $($code).addClass('input-error');
        $($codeError).text('Item code should not exceed 128 characters');
        isValid = false;
    }

    if (!data.itemname || data.itemname.trim() === '') {
        $($name).addClass('input-error');
        $($nameError).text('Item name is required');
        isValid = false;
    } else if (data.itemname.length < 3) {
        $($name).addClass('input-error');
        $($nameError).text('Item name is should be atleast 3 characters');
        isValid = false;
    } else if (data.itemname.length > 128) {
        $($name).addClass('input-error');
        $($nameError).text('Item name should not exceed 128 characters');
        isValid = false;
    }

    if (data.itemdescription.length > 5000) {
        $($description).addClass('input-error');
        $($descriptionError).text('Description should not exceed 128 characters');
    }

    if (!isValid) {
        console.log('There are validation errors');
    }

    return isValid;
}

function UserLoginValidateField() {
    const username = document.getElementById('username');
    const usernameError = document.getElementById('username-error');
    const password = document.getElementById('password');
    const passwordError = document.getElementById('password-error');

    let validate = true;
    //User Login Form input fields client-side validation
    if (username || password) {
        username.addEventListener('input', function () {
            usernameError.textContent = '';
            RemoveClass(username);

            if (username.value.length >= 64) {
                usernameError.textContent = "Username or Email reached maximum limit of 64 characters";
                validate = false;
            } else {
                usernameError.textContent = '';
                RemoveClass(username);
                validate = false;
            }

        });
        password.addEventListener('input', function () {
            passwordError.textContent = '';
            RemoveClass(password);

            if (password.value.length >= 128) {
                passwordError.textContent = "Password reached maximum limit of 128 characters";
                validate = false;
            } else {
                passwordError.textContent = '';
                RemoveClass(password);
                validate = false;
            }
        });

    }

    return validate;
}

function ValidateForm(data) {
    // Reset error messages and input styles
    $('#username-error').text('');
    $('#password-error').text('');
    $('.form-control').removeClass('input-error').addClass('input-success');

    let isValid = true; // Variable to track form validity

    // Manual validation for username
    if (!data.username || data.username.trim() === '') {
        $('.username-input-group').addClass('input-error');
        $('#username-error').text(validationMessages.UsernameEmpty);
        isValid = false;
    }

    // Manual validation for password
    if (!data.password || data.password.trim() === '') {
        $('.password-input-group').addClass('input-error');
        $('#password-error').text(validationMessages.PasswordEmpty);
        isValid = false;
    }


    return isValid; // Return the form validity status
}

function RemoveClass(element) {
    element.classList.remove('input-error');
};