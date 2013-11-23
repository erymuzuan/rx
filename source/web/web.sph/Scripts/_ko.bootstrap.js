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

ko.bindingHandlers.bootstrapPopover= {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor());
        $(element).popover({content:'<pre>' +  text + '</pre>', html:true});
    }
};
ko.bindingHandlers.bootstrapTooltip= {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor());
        $(element).tooltip({ title: text });
    }
};