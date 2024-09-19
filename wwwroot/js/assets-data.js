getStatuses = function () {
    var $statusDropdown = $('#statusDropdown');
    var $updatestatus = $('#updatestatus');
    $.ajax({
        url: '/api/values/get-statuses',
        type: 'GET',
        success: function (data) {          
            $.each(data, function (index, item) {
                var $option = $('<option></option>').val(item.Value).text(item.Text);

                if (item.Value === "Complete") {
                    $option.addClass('text-success'); // Set color attribute
                } else if (item.Value === "Incomplete(Usable)") {
                    $option.addClass('text-primary');
                } else if (item.Value === "Incomplete(Unusable)") {
                    $option.addClass('text-danger');
                }

                $statusDropdown.append($option);
                $updatestatus.append($option);
            });
        },
        error: function (xhr, status, error) {
            console.log('Error fetching statuses: ' + error);
        }
    });
};

getOptions = function () {
    var $updateDropdown = $('#updateDropdown');
    var $updatefirmware = $('#updatefirmware');

    $.ajax({
        url: '/api/values/get-options',
        type: 'GET',
        success: function (data) {
            
            //$updateDropdown.empty(); // Clear existing options

            $.each(data, function (index, item) {
                var $option = $('<option></option>').val(item.Value).text(item.Text);

                if (item.Value === "N/A") {
                    $option.addClass('text-secondary'); // Apply gray color
                } else if (item.Value === "Updated") {
                    $option.addClass('text-success'); // Apply green color
                } else if (item.Value === "Not Updated") {
                    $option.addClass('text-danger'); // Apply red color
                }

                $updateDropdown.append($option);
                $updatefirmware.append($option);
            });
        },
        error: function (xhr, status, error) {
            console.log('Error fetching options: ' + error);
        }
    });
};

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


