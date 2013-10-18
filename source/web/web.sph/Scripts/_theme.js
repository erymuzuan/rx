
ko.bindingHandlers.theme = {
    init: function(element, valueAccessor) {
        var $element = $(element);
        if(bespoke.sph.Theme) {
            var theme = new bespoke.sph.Theme();
            if (theme.init) {
                theme.init($element);
            }
            
        }

    }
};