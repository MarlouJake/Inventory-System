getStatuses = function () {
    $.ajax({
        url: '/api/values/get-statuses',
        type: 'GET',
        success: function (data) {
            var $statusDropdown = $('#statusDropdown');
           //$statusDropdown.empty(); // Clear existing options

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
            });
        },
        error: function (xhr, status, error) {
            console.log('Error fetching statuses: ' + error);
        }
    });
};

getOptions = function () {
    $.ajax({
        url: '/api/values/get-options',
        type: 'GET',
        success: function (data) {
            var $updateDropdown = $('#updateDropdown');
            //$updateDropdown.empty(); // Clear existing options

            $.each(data, function (index, item) {
                var $option = $('<option></option>').val(item.Value).text(item.Text);

                if (item.Value === "N/A") {
                    $option.addClass('text-secondary'); // Apply gray color
                } else if (item.Value === "Yes") {
                    $option.addClass('text-success'); // Apply green color
                } else if (item.Value === "No") {
                    $option.addClass('text-danger'); // Apply red color
                }

                $updateDropdown.append($option);
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
        },
        error: function () {
            $('#view-all').html('<div class="alert alert-danger">Error loading content. Please try again.</div>');
            $('#sidebar .nav-link').removeClass('disabled').find('.spinner').hide();
        }
    });
};


