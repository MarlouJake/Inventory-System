const valid = "text-success";
const invalid = "text-danger";
const defaultColor = "text-muted";
const getPassword = $('#signupPasswordGroup');
const getConfirmPassword = $('#confirmPasswordGroup');

function ValidateSignUpForm() {
    const username = document.getElementById('signup-username');
    const email = document.getElementById('signup-email');
    const usernameError = document.getElementById('usernameSignup-error');
    const emailError = document.getElementById('emailSignup-error');
    const password = document.getElementById('signup-password');
    const confirm = document.getElementById('signup-retypepassword');
    const confirmPasswordError = $('#signupPass-confirmError');
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
        () => validateField(username, username, usernameError, "Username", 3, 64),
        () => validateField(email, email, emailError, "Email", 3, 64),
        () => validateConfirmPassword(password, confirm, confirmPasswordError, getPassword, getConfirmPassword),
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
    password.addEventListener('input', runValidations);
    confirm.addEventListener('input', () => {
        validateConfirmPassword(password, confirm, confirmPasswordError, getPassword, getConfirmPassword);
        runValidations();
    });
    confirm.addEventListener('blur', () => {
        validateConfirmPassword(password, confirm, confirmPasswordError, getPassword, getConfirmPassword);
        runValidations();
    });
}
function ValidateSignup(data) {
    const password = document.getElementById('signup-password');
    const confirm = document.getElementById('signup-retypepassword');
    const username = document.getElementById('signup-username');
    const email = document.getElementById('signup-email');

    const errors = {
        username: $('#usernameSignup-error'),
        email: $('#emailSignup-error'),
        password: $('#signupPass-error'),
        confirmPassword: $('#signupPass-confirmError')
    };

    let isValid = true;
    resetErrors();

    isValid &= validateField(data.username, username, errors.username, "Username", 3, 64);
    isValid &= validateField(data.email, email, errors.email, "Email", 3, 64);

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

    if (!validateConfirmPassword(password, confirm, errors.confirmPassword, getPassword, getConfirmPassword)) {
        isValid = false;
    }
    return isValid;
}
function resetErrors() {
    $('.signup-errors').text('');
    $('.form-control').removeClass('input-error');
}
function validateField(value, field, errorElement, fieldName, minLength, maxLength) {
    if (field.value.trim() === '') {
        return false;
    }
    if (!validateLength(value, minLength, maxLength, errorElement, fieldName)) {
        field.classList.add('input-error');
        return false;
    }
    return true;
}
function validateLength(input, minLength, maxLength, errorElement, fieldName) {
    if (input.length < minLength) {
        errorElement.text(`${fieldName} should be at least ${minLength} characters`);
        return false;
    } else if (input.length > maxLength) {
        errorElement.text(`${fieldName} should not exceed ${maxLength} characters`);
        return false;
    }
    return true;
}
function validateConfirmPassword(password, confirmPassword, confirmPasswordErrorElement, passwordElement, confirmPasswordElement) {

    if (password.value.length === 0) {
        confirmPasswordErrorElement.text('');
        return false;
    } else if (password.value.length > 0 && confirmPassword.value.length === 0) {
        confirmPasswordErrorElement.text('Retype your password');
        return false;
    } else if (password.value !== confirmPassword.value) {
        confirmPasswordErrorElement.text('Passwords do not match');
        passwordElement.addClass('input-error').removeClass('outline-orange');
        confirmPasswordElement.addClass('input-error').removeClass('outline-orange');
        return false;
    } else {
        confirmPasswordErrorElement.text('');
        passwordElement.removeClass('input-error').addClass('outline-orange');
        confirmPasswordElement.removeClass('input-error').addClass('outline-orange');
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
 * 
 * @param {jQuery} $codeError - jQuery object for the code error message element.
 * @param {jQuery} $nameError - jQuery object for the name error message element.
 * 
 * @returns {boolean} - Returns a boolean value true or false.
 */
function ValidateDataInput(data, $code, $name, $codeError, $nameError) {
    var elementIds = [$codeError, $nameError];
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
    } else if (data.itemcode.length > 20) {
        $($code).addClass('input-error');
        $($codeError).text('Item code should not exceed 20 characters');
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
    } else if (data.itemname.length > 20) {
        $($name).addClass('input-error');
        $($nameError).text('Item name should not exceed characters');
        isValid = false;
    }

    if (!isValid) {
        console.log('Validation errors');
    }

    return isValid;
}