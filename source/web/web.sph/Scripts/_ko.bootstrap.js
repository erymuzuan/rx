ko.bindingHandlers.bootstrapDropDown = {
    init: function(element) {
      
        var dropDown = function (e) {
            e.preventDefault();
            e.stopPropagation();

            var button = $(this);
            button.parent().addClass("open");

            $(document).one('click', function () {
                button.parent().removeClass("open");
            });
        };
        $(element).click(dropDown);
    }
};