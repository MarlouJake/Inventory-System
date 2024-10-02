/* Format timestamp as "MMMM dd, yyyy hh:mm:ss tt"*/
function DateFormatOptions() {
    return options = {
        year: 'numeric', month: 'long', day: '2-digit',
        hour: '2-digit', minute: '2-digit', second: '2-digit',
        hour12: true
    };
}




/*Function for changing text color depending on text*/
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


/*Returns full url*/
function getUrl(url) {

    const fullUrl = `${window.location.origin}${url}`;

    return fullUrl;

};


/*Returns Browser info*/
function getBrowserInfo() {
    const userAgent = navigator.userAgent;

    let browserName = "Unknown Browser";
    let browserVersion = "Unknown Version";

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
        version: browserVersion.split(" ")[0] 
    };
};







function ProcessRequest() {
    const JsonApp = DataType[0];
    const JsonFile = DataType[1];

    return processes = {
        JsonApp,
        JsonFile
    };
}

//Returns  JSON Format text
function jsonResult( method, _action, boolvalue, body, actionresponse,
    requestmessage, api, responsestatus, _targetroute, _browserinfo) {

    const timestamp = new Date().toLocaleString('en-US', DateFormatOptions);

    return {
        method,
        actions: {
            action: _action,
            success: boolvalue,
            message: actionresponse
        },
        body,
        requestmessage: requestmessage,
        apiurl: api,
        response: responsestatus,
        targeroute: getUrl(_targetroute),
        browserdetails: {
            browserName: browserInfo.name,
            browserVersion:  browserInfo.version
        },
        timestamp
    };
};

//Function for input-error class

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
    let holdTimeout;

    // Helper function to show the password
    const showPassword = () => {
        if (input.attr('type') === 'password') {
            input.attr('type', 'text');
            icon.removeClass('fa-regular').addClass('fa-solid');
        }
    };

    // Helper function to hide the password
    const hidePassword = () => {
        input.attr('type', 'password');
        icon.removeClass('fa-solid').addClass('fa-regular');
    };

    // Handle both mousedown and touchstart for showing the password
    main.on('mousedown touchstart', function (e) {
        e.preventDefault(); // Prevent default action to avoid unwanted behavior (e.g., text selection)

        holdTimeout = setTimeout(showPassword, 50);  // Show password after 50ms delay
        $(this).addClass('holding');

        // Handle mouseup and touchend for hiding the password
        $(this).on('mouseup touchend', function () {
            clearTimeout(holdTimeout);  // Clear the timeout if the user releases early
            hidePassword();  // Hide the password again
            $(this).removeClass('holding');
        });

        // Prevent the hold action from continuing if the user moves the finger or mouse away
        $(this).on('mouseleave touchcancel', function () {
            clearTimeout(holdTimeout);
            hidePassword();
            $(this).removeClass('holding');
        });
    });
}



//initialize Validation Function

const browserInfo = getBrowserInfo();
const process = ProcessRequest();