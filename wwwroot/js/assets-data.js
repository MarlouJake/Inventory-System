getValues = function (api, dropdownElementId) {
    var $dropdownElement = $(`#${dropdownElementId}`)
   
    $.ajax({
        url: api,
        type: 'GET',
        success: function (data) {
            $dropdownElement.empty();
            $.each(data, function (index, item) {
                var $value = item.Value
                var $option = $('<option></option>').val($value).text(item.Text);
                switch ($value) {
                    case 'Unknown':
                        $option.css('color', 'gray' );
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
        error: function (xhr, status, error) {
            console.log('Error fetching options: ' + error);
            $('<option></option>').val('').text('No option fetched');            
        }
    });
};

var text = [
    "Updated", //0
    "Not Updated", //1
    "N/A", //2
    "Complete", //3
    "Incomplete(Usable)", //4
    "Incomplete(Unusable)", //5   
    "Not Applicable", //6
    "No Category", //7
    "No Status", //8
    "Robots", //9
    "Books", //10
    "Materials", //11
    "Missing", //12
    "Unknown", //13
    "Incomplete", //14
    "New", //15
    "Good", //16
    "Damaged", //17
    "Unknown", //18
    "Defective" //19
];

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
};


/** 
 * Function to dynamically load page content in the dashboard.
 * Active button in sidebar also changes based on the URL or URI of the content loaded.
 * 
 * Returns HTML content for the partial view.
 * @param {string} url - URI or URL of the API
 * @returns {string} Returns HTML content for the partial view.
 **/
loadContent = function (url) {
    $('#view-all').html(spinner + 'Loading...');

    $.ajax({
        url: url,
        type: 'GET',
        success: function (response) {
            // Replace the content of #view-all based on the view url
            $('#view-all').html(response);
            $('#sidebar .nav-link').removeClass('disabled').find('.spinner').hide();
            $('#sidebar .nav-link').removeClass('active bgc-orange'); // Remove active from all links
            $('#sidebar .nav-link[data-url="' + url + '"]').addClass('active bgc-orange');
        },
        error: function () {
            $('#view-all').html('<div class="alert alert-danger">Error loading content. Please try again.</div>');
            $('#sidebar .nav-link').removeClass('disabled').find('.spinner').hide();
        }
    });
};



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

