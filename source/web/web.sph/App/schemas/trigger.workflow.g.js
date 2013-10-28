
/// <reference path="~/scripts/knockout-2.3.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.Trigger = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Trigger, domain.sph",
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
    if (bespoke.sph.domain.TriggerPartial) {
        return _(model).extend(new bespoke.sph.domain.TriggerPartial(model));
    }
    return model;
};



bespoke.sph.domain.AssemblyField = function (webId) {

    var v = new bespoke.sph.domain.Field(webId);

    v.Location = ko.observable('');
    v.TypeName = ko.observable('');
    v.Method = ko.observable('');
    v.IsAsync = ko.observable(false);
    v.AsyncTimeout = ko.observable(0);
    v["$type"] = "Bespoke.Sph.Domain.AssemblyField, domain.sph";

    v.MethodArgCollection = ko.observableArray([]);
    if (bespoke.sph.domain.AssemblyFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.AssemblyFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.FunctionField = function (webId) {

    var v = new bespoke.sph.domain.Field(webId);

    v.Script = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.FunctionField, domain.sph";

    if (bespoke.sph.domain.FunctionFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.FunctionFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.ConstantField = function (webId) {

    var v = new bespoke.sph.domain.Field(webId);

    v.TypeName = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.ConstantField, domain.sph";

    if (bespoke.sph.domain.ConstantFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.ConstantFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.DocumentField = function (webId) {

    var v = new bespoke.sph.domain.Field(webId);

    v.XPath = ko.observable('');
    v.NamespacePrefix = ko.observable('');
    v.TypeName = ko.observable('');
    v.Path = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.DocumentField, domain.sph";

    if (bespoke.sph.domain.DocumentFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.DocumentFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.PropertyChangedField = function (webId) {

    var v = new bespoke.sph.domain.Field(webId);

    v.Path = ko.observable('');
    v.TypeName = ko.observable('');
    v.OldValue = ko.observable('');
    v.NewValue = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.PropertyChangedField, domain.sph";

    if (bespoke.sph.domain.PropertyChangedFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.PropertyChangedFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.Rule = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Rule, domain.sph",
        Left: ko.observable(),
        Right: ko.observable(),
        Operator: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.RulePartial) {
        return _(model).extend(new bespoke.sph.domain.RulePartial(model));
    }
    return model;
};



bespoke.sph.domain.EmailAction = function (webId) {

    var v = new bespoke.sph.domain.CustomAction(webId);

    v.From = ko.observable('');
    v.To = ko.observable('');
    v.SubjectTemplate = ko.observable('');
    v.BodyTemplate = ko.observable('');
    v.Bcc = ko.observable('');
    v.Cc = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.EmailAction, domain.sph";

    if (bespoke.sph.domain.EmailActionPartial) {
        return _(v).extend(new bespoke.sph.domain.EmailActionPartial(v));
    }
    return v;
};



bespoke.sph.domain.SetterAction = function (webId) {

    var v = new bespoke.sph.domain.CustomAction(webId);

    v["$type"] = "Bespoke.Sph.Domain.SetterAction, domain.sph";

    v.SetterActionChildCollection = ko.observableArray([]);
    if (bespoke.sph.domain.SetterActionPartial) {
        return _(v).extend(new bespoke.sph.domain.SetterActionPartial(v));
    }
    return v;
};



bespoke.sph.domain.SetterActionChild = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.SetterActionChild, domain.sph",
        Path: ko.observable(''),
        Field: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.SetterActionChildPartial) {
        return _(model).extend(new bespoke.sph.domain.SetterActionChildPartial(model));
    }
    return model;
};



bespoke.sph.domain.MethodArg = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.MethodArg, domain.sph",
        Name: ko.observable(''),
        TypeName: ko.observable(''),
        ValueProvider: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.MethodArgPartial) {
        return _(model).extend(new bespoke.sph.domain.MethodArgPartial(model));
    }
    return model;
};


bespoke.sph.domain.Field = function (webId) {

    return {
        "$type": "Bespoke.Sph.Domain.Field, domain.sph",
        Name: ko.observable(''),
        Note: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};


bespoke.sph.domain.CustomAction = function (webId) {

    return {
        "$type": "Bespoke.Sph.Domain.CustomAction, domain.sph",
        Title: ko.observable(''),
        IsActive: ko.observable(false),
        TriggerId: ko.observable(0),
        Note: ko.observable(''),
        CustomActionId: ko.observable(0),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};


bespoke.sph.domain.FieldType = function () {
    return {
        DOCUMENT_FIELD: 'DocumentField',
        CONSTANT_FIELD: 'ConstantField',
        FUNCTION_FIELD: 'FunctionField',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();

bespoke.sph.domain.Operator = function () {
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
