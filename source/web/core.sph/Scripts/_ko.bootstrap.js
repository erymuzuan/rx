/// <reference path="typeahead.bundle.js" />
/// <reference path="knockout-3.4.0.debug.js" />
/// <reference path="underscore.js" />
/// <reference path="jquery-2.1.1.intellisense.js" />

ko.bindingHandlers.bootstrapDropDown = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor()) || '[Select your value]',
            anchor = $(element),
            opened = false,
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
        anchor.click(dropDown);

        if (!anchor.html()) {
            if (text.toString() === "[object Object]") {
                text = "[Select you value]";
            }
            anchor.html(text + ' <i class="fa fa-caret-down"></i>');
        }
    }
};

ko.bindingHandlers.tooltip = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor());
        $(element).tooltip({ title: text });
    }
};

ko.bindingHandlers.bootstrapPopover = {
    init: function (element, valueAccesor) {
        var text = ko.unwrap(valueAccesor()).replace(/</g, "&lt;").replace(/>/g, "&gt;");
        $(element).popover({ content: "<pre>" + text + "</pre>", html: true });
    }
};
ko.bindingHandlers.popover = {
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
         ttl = va.ttl || 300000,
         allBindings = allBindingsAccessor(),
         url = String.format("/list?table={0}&column={1}&filter={2}", entity, field, query),
         suggestions = new Bloodhound({
             datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
             queryTokenizer: Bloodhound.tokenizers.whitespace,
             prefetch: {
                 url: url,
                 tt1: ttl,
                 filter: function (list) {
                     return _(list).map(function (v) {
                         return { name: v };
                     });
                 }
             }

         });

        suggestions.initialize();
        $(element).typeahead(null, {
            name: 'autocomplete_' + $(element).prop("id"),
            displayKey: "name",
            source: suggestions.ttAdapter()
        })
           .on('typeahead:closed', function () {
               allBindings.value($(this).val());
           });

    }
};


ko.bindingHandlers.scroll = {
    init: function (element, valueAccessor) {
        var rows = ko.unwrap(valueAccessor()),
            done = false;
        // wait for attached
        //setTimeout(function () {
        //    // TODO : just do it this once rows is more than certail values
        //    $(element).tableScroll({ height: height });
        //    done = true;
        //}, 500);
    }
};
