/**
 * Fetches values from a specified API and populates a dropdown element with the retrieved options.
 * 
 * This function performs an AJAX GET request to the provided API endpoint. Upon successful 
 * retrieval of data, it populates a dropdown element identified by its ID with options. 
 * Each option's text and color are set based on the value retrieved from the API response. 
 * If an error occurs during the request, it logs the error to the console and adds a 
 * default option indicating that no options were fetched.
 * 
 * @param {string} api - The URL of the API endpoint to fetch data from.
 * @param {string} dropdownElementId - The ID of the HTML dropdown element to populate with options.
 * 
 * @returns {void} This function does not return a value; it modifies the dropdown element 
 *                 directly by adding option elements.
 * 
 * @example
 * // Example usage to populate a dropdown with values from an API
 * getValues('https://api.example.com/values', 'myDropdown');
 */
getValues = function (api, dropdownElementId) {
    var $dropdownElement = $(`#${dropdownElementId}`);

    $.ajax({
        url: api,
        type: 'GET',
        timeout: 10000,
        success: function (data) {
            $dropdownElement.empty();
            $.each(data, function (index, item) {
                var $value = item.Value;
                var $option = $('<option></option>').val($value).text(item.Text);
                switch ($value) {
                    case 'Unknown':
                        $option.css('color', 'gray');
                        break;
                    case 'New':
                    case 'Good':
                    case 'Complete':
                    case 'Updated':
                    case 'Materials':
                        $option.css('color', 'green');
                        break;
                    case 'Incomplete(Usable)':
                    case 'Books':
                    case 'Poor':
                        $option.css('color', 'blue');
                        break;
                    case 'Incomplete':
                    case 'Missing':
                    case "Defective":
                    case "Damaged":
                    case 'Incomplete(Unusable)':
                    case 'Not Updated':
                        $option.css('color', 'red');
                        break;
                    case 'Robots':
                        $option.css('color', '#fd7e14');
                        break;
                    default:
                        $option.css('color', 'black');
                        break;
                }

                $dropdownElement.append($option);
            });
        },
        error: function (textStatus, error) {
            console.log('Error fetching options: ' + error);
            $('<option></option>').val('').text('No option fetched').appendTo($dropdownElement);

            if (textStatus === 'timeout') {
                console.log('The request timed out.');
            } else {
                console.log("Error loading page: ", textStatus);
            }
        }
    });
};


/**
 * An array of status and category options for case checking.
 * 
 * This array is utilized to verify and validate the status or category of 
 * items in various contexts. It provides a predefined list of acceptable 
 * values that can be checked against input data. This is useful for 
 * ensuring that the input matches expected categories or statuses.
 * 
 * @type {string[]}
 * @constant
 * @example
 * // Usage in case checking
 * function isValidStatus(status) {
 *     return text.includes(status);
 * }
 * 
 * @returns {string[]} An array of status and category strings for validation.
 */
var text = [
    "Updated",                  // 0: Indicates the item has been updated.
    "Not Updated",              // 1: Indicates the item has not been updated.
    "N/A",                      // 2: Not Applicable, used when the status does not apply.
    "Complete",                 // 3: Indicates the item is complete.
    "Incomplete(Usable)",       // 4: Indicates the item is incomplete but usable.
    "Incomplete(Unusable)",     // 5: Indicates the item is incomplete and unusable.
    "Not Applicable",           // 6: Same as N/A; indicates non-applicability.
    "No Category",              // 7: Indicates that the item has no assigned category.
    "No Status",                // 8: Indicates that there is no status assigned to the item.
    "Robots",                   // 9: Indicates the category related to robots.
    "Books",                    // 10: Indicates the category related to books.
    "Materials",                // 11: Indicates the category related to materials.
    "Missing",                  // 12: Indicates that the item is missing.
    "Unknown",                  // 13: Indicates an unknown status.
    "Incomplete",               // 14: Indicates that the item is incomplete.
    "New",                      // 15: Indicates that the item is new.
    "Good",                     // 16: Indicates that the item is in good condition.
    "Damaged",                  // 17: Indicates that the item is damaged.
    "Unknown",                  // 18: Indicates an unknown status.
    "Defective"                 // 19: Indicates that the item is defective.
];



/**
 * Changes the background color of a specified HTML element based on its text content.
 * 
 * This function checks the text content of the specified element against several arrays 
 * of strings (greenArray, redArray, grayArray, blueArray, and orangeArray) to determine 
 * the appropriate background color. If the text matches an entry in any of the arrays, 
 * the corresponding color will be applied to the background of the element. If no matches 
 * are found, the default background color will be black.
 * 
 * @param {string} element - The selector for the HTML element whose background color 
 *                           will be changed. This can be a CSS selector string.
 * @param {string[]} greenArray - An array of strings representing values that will 
 *                                 set the background color to green.
 * @param {string[]} redArray - An array of strings representing values that will 
 *                               set the background color to red.
 * @param {string[]} grayArray - An array of strings representing values that will 
 *                               set the background color to gray.
 * @param {string[]} blueArray - An array of strings representing values that will 
 *                               set the background color to blue.
 * @param {string[]} orangeArray - An array of strings representing values that will 
 *                                 set the background color to orange.
 * 
 * @returns {void} This function does not return a value; it modifies the background 
 *                 color of the specified element directly.
 * 
 * @example
 * // Example usage with the provided text array
 * ChangeBackgroundColor('#myElement', 
 *                       ['Complete', 'Good', 'Updated'],     // Green conditions
 *                       ['Damaged', 'Defective', 'Missing'], // Red conditions
 *                       ['Incomplete(Usable)', 'Incomplete(Unusable)'], // Gray conditions
 *                       ['Books', 'Materials'],               // Blue conditions
 *                       ['New', 'Unknown']);                   // Orange conditions
 */
function ChangeBackgroundColor(element, greenArray, redArray, grayArray, blueArray, orangeArray) {
    var container = $(element);

    if (container.length) {
        var textcontent = container.text().trim(); // Get the text and trim any whitespace

        // Variable to hold the color to apply
        let colorToApply = 'black'; // Default color

        // Determine the color to apply based on textcontent
        switch (true) {
            case greenArray && greenArray.includes(textcontent):
                colorToApply = 'green'; // Set to green if in greenArray
                break;
            case redArray && redArray.includes(textcontent):
                colorToApply = 'red'; // Set to red if in redArray
                break;
            case grayArray && grayArray.includes(textcontent):
                colorToApply = 'gray'; // Set to gray if in grayArray
                break;
            case blueArray && blueArray.includes(textcontent):
                colorToApply = 'blue'; // Set to blue if in blueArray
                break;
            case orangeArray && orangeArray.includes(textcontent):
                colorToApply = '#fd7e14'; // Set to orange if in orangeArray
                break;
            // No additional cases needed since default is black
        }

        // Apply the determined background color
        container.css('background-color', colorToApply);
    }
}



/**
 * Changes the background color of elements based on a specified category.
 * 
 * This function uses the `ChangeBackgroundColor` function to apply different 
 * background colors to elements representing firmware, status, and category 
 * based on the provided category type. It adjusts the colors based on predefined 
 * conditions associated with 'Robots', 'Books', and 'Materials'.
 * 
 * @param {string} baseOnCategory - The category that determines how the 
 *                                  background colors are set. Can be 'Robots', 
 *                                  'Books', or 'Materials'.
 * @param {string} firmware - The selector for the HTML element representing the 
 *                            firmware status.
 * @param {string} status - The selector for the HTML element representing the 
 *                          status of the item.
 * @param {string} category - The selector for the HTML element representing the 
 *                            category of the item.
 * 
 * @returns {void} This function does not return a value; it modifies the background 
 *                 color of the specified elements directly based on the category.
 * 
 * @example
 * // Example usage to change background based on category
 * changeBgBaseOnCategory('Robots', '#firmwareElement', '#statusElement', '#categoryElement');
 */
function changeBgBaseOnCategory(baseOnCategory, firmware, status, category) {
    switch (baseOnCategory) {
        case 'Robots':
            ChangeBackgroundColor(firmware, ["Updated"], ["Not Updated"], ["Unknown"], null, null);
            ChangeBackgroundColor(status, ["Complete"], ["Incomplete(Unusable)", "Damaged", "Defective", "Missing"], ["Unknown"], ["Incomplete(Usable)"], null);
            ChangeBackgroundColor(category, null, null, ["Materials"], ["Books"], ["Robots"]);
            break;
        case 'Books':
        case 'Materials':
            ChangeBackgroundColor(status, ["New"], ["Damaged", "Missing"], ["Poor", "Unknown"], ["Good"], null);
            ChangeBackgroundColor(category, null, null, ["Materials"], ["Books"], ["Robots"]);
            break;
        default:
            break;
    }
}

async function requestEvent(isFininsheed, userEvent, data = null, element = null, clickedElement = null) {
    if (isFininsheed) {
        switch (userEvent) {
            case 'sidebar':
                element.remove();
                $(clickedElement).addClass('active');
                $('#sidebar .nav-link').removeClass('active bgc-orange');
                let id = $(clickedElement).attr('id');
                if (id === 'inventory-link' || id === 'parts-inventory') {
                    $('#inventory-list-button').addClass('outline-orange');   
                    $('#inventory-list').css('background-color', 'rgba(236, 240, 241, 0.1)');
                } else {
                    $('#inventory-list-button').removeClass('outline-orange');
                }

                break;
            case 'category':
                $('.category-button').removeClass('ctg-selected').addClass('ctg-notselected').prop('disabled', false);
                $('#category-buttons .category-button').removeClass('disabled');
                $(`.category-button[data-string="${data}"]`).addClass('ctg-selected').removeClass('ctg-notselected').text(data);
                break;
            default:
                console.error(`Event: ${userEvent} not found.`);
                break;
        };
    } 
}

async function ajaxRequest(url, type = 'GET', data = {}, cache = false, timeout = 5000){
    return  await $.ajax({
        url: url,
        type: type,
        data: data,
        cache: cache,
        timeout: timeout,
    });
}

async function ajaxFailure(requestType, error, status) {

    if (error.textStatus === 'timeout') {
        console.log('The request timed out.');
    }

    switch (requestType) {
        case 'category':
             if (status == 404) {
                $('#items-list').html(noItemContainer);
                checkForItems(status);
                console.log(`Server responsed with status code: ${error.status}\n
                             Server message: ${error.responseJSON.Message}`);
            } else {
                console.log(`An error occurred: ${JSON.stringify(error.responseJSON, null, 2)}`);
            }
            break;
        case 'search':
            $('.card-container').html(noItemContainer);
            if (textStatus === 'timeout') {
                console.log('The request timed out.');
            } else {
                console.log('An error occurred: ' + error.textStatus);
            }
            break;
        default:
            console.error(`Request Type: ${requestType} not found.`);
            break;
    }
}

/** 
 * Function to dynamically load page content in the dashboard.
 * Active button in sidebar also changes based on the URL or URI of the content loaded.
 * 
 * Returns HTML content for the partial view.
 * @param {string} url - URI or URL of the API
 * @returns {string} Returns HTML content for the partial view.
 **/
loadContent = async function (url, element) {
    let done = false;
   
    try {
        
        const response = await ajaxRequest(url);

        $('#content-handler').html(response);
                  
        setTimeout(async () => {
            $('#sidebar .nav-link[data-url="' + url + '"]').addClass('active bgc-orange');
        }, 1);

        done = true;
    }
    catch (error) {
        
        if (error.textStatus === 'timeout') {
            console.log('The request timed out.');
        } else {
            console.log("Error loading page: ", error.textStatus);
        }

        done = true;
    }

    if (done === false) {
        $('body').append(`<div id="cover-element" class="text-orange
        text-center justify-content-center align-content-center">${spinner} Loading</div>`);
        $('#cover-element').css({
            "position": "fixed",
            "top": 0,
            "left": 0,
            "width": "100%",
            "height": "100%",
            "background-color": "rgba(255, 255, 255, 0.2)",
            "z-index": 9999
        });
    }
    requestEvent(done, 'sidebar', null, $('#cover-element'), element);
};

/*
async function loadPage(url, category, pageNumber) {
    try {
        const data = {
            category: category,
            page: pageNumber
        }
        const response = await ajaxRequest(url, 'GET', data);
        $('#items-list').html(response);
        updatePaginationControls(response);
    }
    catch (error) {
        $('#view-all').html(noContent);

        if (textStatus === 'timeout') {
            console.log('The request timed out.');
        } else {
            console.log("Error loading page: ", textStatus);
        }
    }

}



    $.ajax({
        url: 'inventory/items/uncategorized',
        type: 'GET',
        timeout: 10000,
        cache: false,
        data: { category: category, page: pageNumber },
        success: function (response) {
            $('#items-list').html(response);        
            updatePaginationControls(response);          
        },
        error: function (textStatus) {
            
        }
    });
}*/

async function loadItemsByCategory(category, pageNumber) {
  
    let itemcode = $('#searchbar').val();
    let done = false;
   
    try {
        const url = 'inventory/items/categorized';
        const data = {
            itemcode: itemcode,
            page: pageNumber,
            category: category
        };

        const response = await ajaxRequest(url, 'GET', data);
        $('#items-list').html(response);
          
        checkForItems(200);
        resetCheckState();

        done = true;
    }
    catch (error) {
        let status = error.status;
        await ajaxFailure('category', error, status);
        done = true;
    }

    requestEvent(done, 'category', category);
}

async function loadItems(itemcode, category, pageNumber) {
    try {
        const url = 'inventory/search/';
        const data = {
            itemcode: itemcode,
            category: category,
            page: pageNumber
        };
        const response = await ajaxRequest(url, 'GET', data);

        $('#items-list').html(response);
        resetCheckState();
        updatePaginationControls(response);
    }
    catch (error){
        
    }

}


function loadPageByNumber(pageNumber, currentPage) {
    let category = $('.ctg-selected').data('string');
    if (pageNumber != currentPage) {
        loadItemsByCategory(category, pageNumber);
    } else {
        $("html, body").animate({ scrollTop: 0 }, "smooth");
        return false;
    }

}

function deleteSelectedItem() {
    let checkedBoxes = $('.item-checkbox:checked');
    let itemIds = [];
  
    checkedBoxes.each(function () {
        const data = {
            itemUID: $(this).closest('.card').data('uid'),
        }
        itemIds.push(data.itemUID);
    });

    sessionStorage.setItem('deleteLenght', itemIds.length);
    sessionStorage.setItem('deleteIds', JSON.stringify(itemIds));

    ModalShow('inventory/remove/');   
}


function checkForItems(status) {
    if (status === 404) {
        $('#showCheckbox-container, #deleteCardButton').addClass('d-none');
    } else {
        $('#showCheckbox-container, #deleteCardButton').removeClass('d-none');
    }
}
//function toggleNavbar(uri) {
//    let dashboardNav = $('#dashboard-nav');
//    let inventoryNav = $('#summary-nav');
//    let requestsNav = $('#requests-nav');
//    let itemViewcategoryUri = 'inventory/items/categorized';
//    let inventory = 'inventory';
//    let dashboard = 'dashboard';
//    let requests = 'requests';

//    // Hide all by default
//    dashboardNav.addClass('hide-con').removeClass('show-con');
//    inventoryNav.addClass('hide-con').removeClass('show-con');
//    requestsNav.addClass('hide-con').removeClass('show-con');

//    switch (uri) {
//        case inventory:
//        case itemViewcategoryUri:
//            dashboardNav.addClass('show-con').removeClass('hide-con');
//            break;
//        case dashboard:
//            summaryNav.addClass('show-con').removeClass('hide-con');
//            break;
//        case requests:
//            requestsNav.addClass('show-con').removeClass('hide-con');
//            break;
//        default:
//            // Optionally handle the default case (everything remains hidden)
//            break;
//    }
//}

//function toggleSearchContainer(url) {
//    var searchContainer = $('#hide-container');
//    var categoryContainer = $('#hide-categories');

//    switch (url) {
//        case 'dashboard/item-view/all/':
//        case 'dashboard/item-view/category/':

//            searchContainer.addClass('show-con');
//            categoryContainer.addClass('show-con');
//            searchContainer.removeClass('hide-con');
//            categoryContainer.removeClass('hide-con');
//            break;
//        default:
//            searchContainer.removeClass('show-con');
//            categoryContainer.removeClass('show-con');
//            searchContainer.addClass('hide-con');
//            categoryContainer.addClass('hide-con');
//            break;
//    }
//}

//function toggleSummaryNavbar(url) {
//    let navbar = $('#summary-nav');
//    switch (url) {
//        case 'summary':
//            navbar.removeClass('hide-con');
//            navbar.addClass('show-con');
//            break;
//        default:
//            navbar.addClass('hide-con');
//            navbar.removeClass('show-con');
//            break;
//    }
//}

/*
function updatePaginationControls(response) {
    
    var totalPages = response.totalPages; // Get total pages from response
    var currentPage = response.currentPage; // Get current page from response

    // Update pagination controls
}
*/

/**
 * Enable or Disable & Add or Remove attr required  to the firmware status dropdown
 * @param {JQuery} input - ID of input dropdown
 * @param {JQuery} target - ID of target dropdown
 * 
 * 
 **/

function toggleDropdown(input, target) {
    if (input.val() === "Robots") {
        target.prop({
            "disabled": false,
            "required": true
        });
    } else {
        target.prop({
            "disabled": true,
            "required": false
        });
    }
}

