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



/** 
 * Function to dynamically load page content in the dashboard.
 * Active button in sidebar also changes based on the URL or URI of the content loaded.
 * 
 * Returns HTML content for the partial view.
 * @param {string} url - URI or URL of the API
 * @returns {string} Returns HTML content for the partial view.
 **/
loadContent = function (url) {
    $('.card-container').html(spinnerContainer);

    $.ajax({
        url: url,
        type: 'GET',
        cache: true,
        timeout: 10000,
        success: function (response) {
            $('#content-handler').html(response);
            $('#sidebar .nav-link').removeClass('disabled').find('.spinner').hide();
            $('#sidebar .nav-link').removeClass('active bgc-orange'); 

            //$('.category-button').removeClass('ctg-selected');        
            //$('.category-button[data-string="All"]').addClass('ctg-selected');

            setTimeout(() => {
                $('#sidebar .nav-link[data-url="' + url + '"]').addClass('active bgc-orange');
            }, 20);
            
        },
        error: function (textStatus) {
            $('#sidebar .nav-link').removeClass('disabled').find('.spinner').hide();

            if (textStatus === 'timeout') {
                console.log('The request timed out.');
            } else {
                console.log("Error loading page: ", textStatus);
            }
        }
    });
};

function loadPage(category, pageNumber) {
    //var itemcode = $('#searchbar').val().trim();
    
    $.ajax({
        url: 'inventory/items/uncategorized',
        type: 'GET',
        timeout: 10000,
        cache: true,
        data: { category: category, page: pageNumber },
        success: function (response) {
            $('#view-all').html(response);        
            updatePaginationControls(response);          
        },
        error: function (textStatus) {
            $('#view-all').html(noContent);
            

            if (textStatus === 'timeout') {
                console.log('The request timed out.');
            } else {
                console.log("Error loading page: ", textStatus);
            }
        }
    });
}

function loadItemsByCategory(category, pageNumber) {
    $('card-container').html(spinnerContainer);
    $.ajax({
        url: 'inventory/items/categorized',
        type: 'GET',
        timeout: 10000,
        cache: false,
        data: { category: category, page: pageNumber },
        success: function (response) {
            $('#view-all').html(response);
            $('#category-buttons .category-button').removeClass('disabled').find('.spinner').hide();
            $(`.category-button[data-string="${category}"]`).addClass('ctg-selected');
            updatePaginationControls(response);
            resetCheckState();
        },
        error: function (textStatus) {
            $('.card-container').html(noItemContainer);           
            $('.category-button').removeClass('disabled').find('.spinner').hide();

            if (textStatus === 'timeout') {
                console.log('The request timed out.');
            } else {
                console.log('An error occurred: ' + textStatus);
            }
        }
    });
}

function loadItems(itemcode, category, pageNumber) {
   $('card-container').html(spinnerContainer);
    $.ajax({
        url: 'inventory/search/', 
        type: 'GET',
        timeout: 10000,
        cache: false,
        data: { itemcode: itemcode, category: category, page: pageNumber },
        success: function (response) {
            $('#view-all').html(response);
            resetCheckState();
            updatePaginationControls(response);
        },
        error: function (textStatus) {
            $('.card-container').html(noItemContainer);
            if (textStatus === 'timeout') {
                console.log('The request timed out.');
            } else {
                console.log('An error occurred: ' + textStatus);
            }
        }
    });
}


function loadPageByNumber(pageNumber, currentPage) {
    let category = $('.ctg-selected').data('string');
    let itemCode = $('#searchbar').val().trim();
    if (pageNumber != currentPage) {
        if (itemCode === '') {
            loadItemsByCategory(category, pageNumber);
        } else {
            loadItems(itemCode, category, pageNumber);
        }
    } else {
        $("html, body").animate({ scrollTop: 0 }, "smooth");
        return false;
    }

}

function deleteSelectedItem() {
    let checkedBoxes = $('.delete-checkbox:checked');
    let itemIds = [];

    checkedBoxes.each(function () {
        const data = {
            itemId : $(this).closest('.card').data('item-id'),
            itemName : $(this).closest('.card').data('item-name'),
            itemCode: $(this).closest('.card').data('item-code'),
            itemCategory: $(this).closest('.card').data('item-category'),
            itemStatus: $(this).closest('.card').data('item-status')
        }
        console.log('Selected item: ' + JSON.stringify(data, null, 2));
        itemIds.push(data.itemId);
    });
    console.log('Total item(s): ', itemIds.length);
    alert(`Total item(s): ${itemIds.length}`);
    //DeleteRequest(itemIds);
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

function updatePaginationControls(response) {
    
    var totalPages = response.totalPages; // Get total pages from response
    var currentPage = response.currentPage; // Get current page from response

    // Update pagination controls
}

/**Enable or Disable & Add or Remove attr required  to the firmware status dropdown
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

