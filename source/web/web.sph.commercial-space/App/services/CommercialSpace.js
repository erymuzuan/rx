bespoke.sphcommercialspace.domain.CommercialSpacePartial = function() {

    return {
        StaticMap: ko.observable("/images/no-image.png"),
        ApplicationTemplateOptions : ko.observableArray([])
    };
};
bespoke.sphcommercialspace.domain.RolePartial = function() {

    return {
        permissions: ko.observableArray()
    };
};