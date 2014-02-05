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

ko.bindingHandlers.bootstrapPopover = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor());
        $(element).popover({ content: '<pre>' + text + '</pre>', html: true });
    }
};
ko.bindingHandlers.bootstrapTooltip = {
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
                if (!s) return false;
                if (!s.href) return false;

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
            local: data
        })
        .on('typeahead:closed', function () {
            value($(this).val());
        });


    }

};



ko.bindingHandlers.autocomplete = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var va = ko.unwrap(valueAccessor()),
         entity = ko.unwrap(va.entity),
         field = ko.unwrap(va.field),
         query = ko.unwrap(va.query),
        allBindings = allBindingsAccessor();


        $(element).typeahead({
            name: 'autocomplete_' + $(element).prop("id"),
            limit: 5,
            prefetch: {
                url: String.format("/list?table={0}&column={1}&filter={2}", entity, field, query),
                ttl: 1000 * 60
            }
        })
            .on('typeahead:closed', function () {
                allBindings.value($(this).val());
            });
    }
};


ko.bindingHandlers.scroll = {
    init: function (element, valueAccessor) {
        var height = ko.unwrap(valueAccessor());
        // wait for attached
        setTimeout(function () {
            $(element).tableScroll({ height: height });
        }, 500);
    }
};
