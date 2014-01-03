
ko.bindingHandlers.theme = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        // First get the latest data that we're bound to
        var value = valueAccessor(), allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.unwrap(value);
        if(bespoke.sph.Theme) {
            var theme = new bespoke.sph.Theme();
            if (theme[valueUnwrapped]) {
                theme[valueUnwrapped](element, viewModel, allBindings, bindingContext);
            }
            
        }

    }
};