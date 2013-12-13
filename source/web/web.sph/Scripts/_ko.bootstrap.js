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




ko.bindingHandlers.cssAutoComplete = {
    init: function (element, valueAccessor) {
        var value = valueAccessor(),
            bootstrap = _(document.styleSheets).find(function (s) {
            // TODO : what happend if were to combine the css with Bundle
            return s.href.indexOf("bootstrap") > -1;
        });
        var data = ['btn', 'btn-warning', 'btn-success', 'btn-link'];
        if (bootstrap) {
            data = _.chain(bootstrap.rules).filter(function (r) {
                return /^\./g.test(r.selectorText)
                    && !/:/g.test(r.selectorText)
                    && !/\s/g.test(r.selectorText)
                    && !/\+/g.test(r.selectorText)
                    && !/>/g.test(r.selectorText)
                    && !/\[/g.test(r.selectorText);
            }).map(function (s) {
                return s.selectorText.replace(/\./g, "");
            })
                .value();
        }
      
        $(element).typeahead({
            name: 'css_class',
            limit: 10,
            local:data
        })
        .on('typeahead:closed', function () {
            value($(this).val());
        });


    }

};
