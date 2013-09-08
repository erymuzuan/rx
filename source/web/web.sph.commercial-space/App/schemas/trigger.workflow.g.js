
/// <reference path="~/scripts/knockout-2.3.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.Trigger = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Trigger, domain.commercialspace",
        Name: ko.observable(''),
        Entity: ko.observable(''),
        TypeOf: ko.observable(''),
        TriggerId: ko.observable(0),
        Note: ko.observable(''),
        IsActive: ko.observable(false),
        IsFiredOnAdded: ko.observable(false),
        IsFiredOnDeleted: ko.observable(false),
        IsFiredOnChanged: ko.observable(false),
        FiredOnOperations: ko.observable(''),
        RuleCollection: ko.observableArray([]),
        ActionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.TriggerPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.TriggerPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.FunctionField = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Field(webId);

    v.Script = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.FunctionField, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.FunctionFieldPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.FunctionFieldPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.ConstantField = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Field(webId);

    v.TypeName = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.ConstantField, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.ConstantFieldPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.ConstantFieldPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.DocumentField = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Field(webId);

    v.XPath = ko.observable('');
    v.NamespacePrefix = ko.observable('');
    v.TypeName = ko.observable('');
    v.Path = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.DocumentField, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.DocumentFieldPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.DocumentFieldPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.FieldChangeField = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Field(webId);

    v.Path = ko.observable('');
    v.TypeName = ko.observable('');
    v.OldValue = ko.observable('');
    v.NewValue = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.FieldChangeField, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.FieldChangeFieldPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.FieldChangeFieldPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.Rule = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Rule, domain.commercialspace",
        Left: ko.observable(),
        Right: ko.observable(),
        Operator: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.RulePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.RulePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.EmailAction = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.CustomAction(webId);

    v.From = ko.observable('');
    v.To = ko.observable('');
    v.SubjectTemplate = ko.observable('');
    v.BodyTemplate = ko.observable('');
    v.Bcc = ko.observable('');
    v.Cc = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.EmailAction, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.EmailActionPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.EmailActionPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.SetterAction = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.CustomAction(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.SetterAction, domain.commercialspace";

    v.SetterActionChildCollection = ko.observableArray([]);
    if (bespoke.sphcommercialspace.domain.SetterActionPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.SetterActionPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.SetterActionChild = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.SetterActionChild, domain.commercialspace",
        Path: ko.observable(''),
        Field: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.SetterActionChildPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.SetterActionChildPartial(model));
    }
    return model;
};


bespoke.sphcommercialspace.domain.Field = function (webId) {

    return {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Field, domain.commercialspace",
        Name: ko.observable(''),
        Note: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};


bespoke.sphcommercialspace.domain.CustomAction = function (webId) {

    return {
        "$type": "Bespoke.SphCommercialSpaces.Domain.CustomAction, domain.commercialspace",
        Title: ko.observable(''),
        IsActive: ko.observable(false),
        TriggerId: ko.observable(0),
        Note: ko.observable(''),
        CustomActionId: ko.observable(0),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};


bespoke.sphcommercialspace.domain.FieldType = function () {
    return {
        DOCUMENT_FIELD: 'DocumentField',
        CONSTANT_FIELD: 'ConstantField',
        FUNCTION_FIELD: 'FunctionField',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();

bespoke.sphcommercialspace.domain.Operator = function () {
    return {
        EQ: 'Eq',
        LT: 'Lt',
        LE: 'Le',
        GT: 'Gt',
        GE: 'Ge',
        SUBSTRINGOF: 'Substringof',
        STARTS_WITH: 'StartsWith',
        ENDS_WITH: 'EndsWith',
        NOT_CONTAINS: 'NotContains',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();
