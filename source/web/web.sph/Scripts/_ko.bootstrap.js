ko.bindingHandlers.bootstrapDropDown = {
    init: function (element) {

        var opened = false,
            dropDown = function (e) {
                e.preventDefault();
                e.stopPropagation();
                var button = $(this);

                if (opened) {
                    button.parent().removeClass("open");
                    opened = false;

                } else {
                    button.parent().addClass("open");
                    opened = true;
                    $(document).one('click', function () {
                        button.parent().removeClass("open");
                        opened = false;
                    });
                }


            };
        $(element).click(dropDown);
    }
};