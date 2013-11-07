
/// <reference path="~/scripts/knockout-3.0.0.debug.js" />
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



bespoke.sph.domain.WorkflowDefinition = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.WorkflowDefinition, domain.sph",
        WorkflowDefinitionId: ko.observable(0),
        Name: ko.observable(''),
        Note: ko.observable(''),
        IsActive: ko.observable(false),
        SchemaStoreId: ko.observable(''),
        Version: ko.observable(0),
        ActivityCollection: ko.observableArray([]),
        VariableDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.WorkflowDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Workflow = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Workflow, domain.sph",
        WorkflowId: ko.observable(0),
        WorkflowDefinitionId: ko.observable(0),
        Name: ko.observable(''),
        State: ko.observable(''),
        IsActive: ko.observable(false),
        Version: ko.observable(0),
        VariableValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.WorkflowPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowPartial(model));
    }
    return model;
};



bespoke.sph.domain.ScreenActivity = function (webId) {

    var v = new bespoke.sph.domain.Activity(webId);

    v.Title = ko.observable('');
    v.ViewVirtualPath = ko.observable('');
    v.WorkflowDefinitionId = ko.observable(0);
    v["$type"] = "Bespoke.Sph.Domain.ScreenActivity, domain.sph";

    v.FormDesign = ko.observable(new bespoke.sph.domain.FormDesign());
    v.Performer = ko.observable(new bespoke.sph.domain.Performer());
    if (bespoke.sph.domain.ScreenActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ScreenActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.DecisionActivity = function (webId) {

    var v = new bespoke.sph.domain.Activity(webId);

    v["$type"] = "Bespoke.Sph.Domain.DecisionActivity, domain.sph";

    v.DecisionBranchCollection = ko.observableArray([]);
    if (bespoke.sph.domain.DecisionActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.DecisionActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.DecisionBranch = function (webId) {

    var v = new bespoke.sph.domain.Activity(webId);

    v.IsDefault = ko.observable(false);
    v["$type"] = "Bespoke.Sph.Domain.DecisionBranch, domain.sph";

    v.Expression = ko.observable();//type but not nillable
    if (bespoke.sph.domain.DecisionBranchPartial) {
        return _(v).extend(new bespoke.sph.domain.DecisionBranchPartial(v));
    }
    return v;
};



bespoke.sph.domain.NotificationActivity = function (webId) {

    var v = new bespoke.sph.domain.Activity(webId);

    v.From = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.NotificationActivity, domain.sph";

    if (bespoke.sph.domain.NotificationActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.NotificationActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.SimpleVariable = function (webId) {

    var v = new bespoke.sph.domain.Variable(webId);

    v["$type"] = "Bespoke.Sph.Domain.SimpleVariable, domain.sph";

    if (bespoke.sph.domain.SimpleVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.SimpleVariablePartial(v));
    }
    return v;
};



bespoke.sph.domain.ComplexVariable = function (webId) {

    var v = new bespoke.sph.domain.Variable(webId);

    v["$type"] = "Bespoke.Sph.Domain.ComplexVariable, domain.sph";

    if (bespoke.sph.domain.ComplexVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.ComplexVariablePartial(v));
    }
    return v;
};


// placeholder for FormDesign

bespoke.sph.domain.VariableValue = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.VariableValue, domain.sph",
        Name: ko.observable(''),
        Value: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.VariableValuePartial) {
        return _(model).extend(new bespoke.sph.domain.VariableValuePartial(model));
    }
    return model;
};



bespoke.sph.domain.Page = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Page, domain.sph",
        PageId: ko.observable(0),
        Title: ko.observable(''),
        IsRazor: ko.observable(false),
        IsPartial: ko.observable(false),
        VirtualPath: ko.observable(''),
        Code: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.PagePartial) {
        return _(model).extend(new bespoke.sph.domain.PagePartial(model));
    }
    return model;
};



bespoke.sph.domain.EndActivity = function (webId) {

    var v = new bespoke.sph.domain.Activity(webId);
    v.IsTerminating = ko.observable(false);
    v["$type"] = "Bespoke.Sph.Domain.EndActivity, domain.sph";

    if (bespoke.sph.domain.EndActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.EndActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.Performer = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Performer, domain.sph",
        UserProperty: ko.observable(''),
        Value: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.PerformerPartial) {
        return _(model).extend(new bespoke.sph.domain.PerformerPartial(model));
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


bespoke.sph.domain.Activity = function (webId) {

    return {
        "$type": "Bespoke.Sph.Domain.Activity, domain.sph",
        IsInitiator: ko.observable(false),
        NextActivityWebId: ko.observable(''),
        Name: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};


bespoke.sph.domain.Variable = function (webId) {

    return {
        "$type": "Bespoke.Sph.Domain.Variable, domain.sph",
        Name: ko.observable(''),
        TypeName: ko.observable(''),
        DefaultValue: ko.observable(''),
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
