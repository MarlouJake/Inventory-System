function customSelect() {
    $('.select-button').on('click', function () {
        let dropdownOption = $(this).siblings('.dropdown-options');
        let options = $('.dropdown-options li');

        options.toggleClass('show');
        dropdownOption.toggleClass('show');
    });
    function DropdownOptions(dropdownOption) {
        return dropdownOption.prop('hidden', dropdownOption.prop('hidden'));
    }
}
