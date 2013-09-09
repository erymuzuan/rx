
var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};

bespoke.sphcommercialspace.domain.CommercialSpacePartial = function () {

    var getCustomField = function (name) {
        var cs = _(this.CustomFieldValueCollection()).find(function (v) {
            return v.Name() === name;
        });
        if (!cs) {
            throw "Cannot find custom field for " + name + " in CommercialSpace";
        }
        return cs.Value;
    };
    return {
        CustomField: getCustomField,
        StaticMap: ko.observable("/images/no-image.png"),
        ApplicationTemplateOptions : ko.observableArray([])
    };
};
bespoke.sphcommercialspace.domain.RolePartial = function() {

    return {
        permissions: ko.observableArray()
    };
};