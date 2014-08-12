
/// <reference path="~/scripts/knockout-3.1.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.Trigger = function (optionOrWebid) {

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
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TriggerPartial) {
        return _(model).extend(new bespoke.sph.domain.TriggerPartial(model));
    }
    return model;
};



bespoke.sph.domain.AssemblyField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Location = ko.observable('');

    v.TypeName = ko.observable('');

    v.Method = ko.observable('');

    v.IsAsync = ko.observable(false);

    v.AsyncTimeout = ko.observable(0);

    v["$type"] = "Bespoke.Sph.Domain.AssemblyField, domain.sph";

    v.MethodArgCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.AssemblyFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.AssemblyFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.JavascriptExpressionField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Expression = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.JavascriptExpressionField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.JavascriptExpressionFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.JavascriptExpressionFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.FunctionField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Script = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.FunctionField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FunctionFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.FunctionFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.ConstantField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.TypeName = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ConstantField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ConstantFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.ConstantFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.DocumentField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.XPath = ko.observable('');

    v.NamespacePrefix = ko.observable('');

    v.TypeName = ko.observable('');

    v.Path = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.DocumentField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DocumentFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.DocumentFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.PropertyChangedField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Path = ko.observable('');

    v.TypeName = ko.observable('');

    v.OldValue = ko.observable('');

    v.NewValue = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.PropertyChangedField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.PropertyChangedFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.PropertyChangedFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.Rule = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Rule, domain.sph",
        Left: ko.observable(),
        Right: ko.observable(),
        Operator: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.RulePartial) {
        return _(model).extend(new bespoke.sph.domain.RulePartial(model));
    }
    return model;
};



bespoke.sph.domain.EmailAction = function (optionOrWebid) {

    var v = new bespoke.sph.domain.CustomAction(optionOrWebid);

    v.From = ko.observable('');

    v.To = ko.observable('');

    v.SubjectTemplate = ko.observable('');

    v.BodyTemplate = ko.observable('');

    v.Bcc = ko.observable('');

    v.Cc = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.EmailAction, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.EmailActionPartial) {
        return _(v).extend(new bespoke.sph.domain.EmailActionPartial(v));
    }
    return v;
};



bespoke.sph.domain.SetterAction = function (optionOrWebid) {

    var v = new bespoke.sph.domain.CustomAction(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.SetterAction, domain.sph";

    v.SetterActionChildCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SetterActionPartial) {
        return _(v).extend(new bespoke.sph.domain.SetterActionPartial(v));
    }
    return v;
};



bespoke.sph.domain.SetterActionChild = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.SetterActionChild, domain.sph",
        Path: ko.observable(''),
        Field: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SetterActionChildPartial) {
        return _(model).extend(new bespoke.sph.domain.SetterActionChildPartial(model));
    }
    return model;
};



bespoke.sph.domain.MethodArg = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.MethodArg, domain.sph",
        Name: ko.observable(''),
        TypeName: ko.observable(''),
        ValueProvider: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.MethodArgPartial) {
        return _(model).extend(new bespoke.sph.domain.MethodArgPartial(model));
    }
    return model;
};



bespoke.sph.domain.StartWorkflowAction = function (optionOrWebid) {

    var v = new bespoke.sph.domain.CustomAction(optionOrWebid);

    v.WorkflowDefinitionId = ko.observable(0);

    v.Name = ko.observable('');

    v.Version = ko.observable(0);

    v["$type"] = "Bespoke.Sph.Domain.StartWorkflowAction, domain.sph";

    v.WorkflowTriggerMapCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.StartWorkflowActionPartial) {
        return _(v).extend(new bespoke.sph.domain.StartWorkflowActionPartial(v));
    }
    return v;
};



bespoke.sph.domain.WorkflowTriggerMap = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.WorkflowTriggerMap, domain.sph",
        VariablePath: ko.observable(''),
        Field: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkflowTriggerMapPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowTriggerMapPartial(model));
    }
    return model;
};



bespoke.sph.domain.WorkflowDefinition = function (optionOrWebid) {

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
        ReferencedAssemblyCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkflowDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Workflow = function (optionOrWebid) {

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
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkflowPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowPartial(model));
    }
    return model;
};



bespoke.sph.domain.ScreenActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.Title = ko.observable('');

    v.ViewVirtualPath = ko.observable('');

    v.WorkflowDefinitionId = ko.observable(0);

    v.CancelMessageSubject = ko.observable('');

    v.InvitationMessageSubject = ko.observable('');

    v.CancelMessageBody = ko.observable('');

    v.InvitationMessageBody = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ScreenActivity, domain.sph";

    v.FormDesign = ko.observable(new bespoke.sph.domain.FormDesign());
    v.Performer = ko.observable(new bespoke.sph.domain.Performer());
    v.ConfirmationOptions = ko.observable(new bespoke.sph.domain.ConfirmationOptions());

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ScreenActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ScreenActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.DecisionActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.DecisionActivity, domain.sph";

    v.DecisionBranchCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DecisionActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.DecisionActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.DecisionBranch = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.IsDefault = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.DecisionBranch, domain.sph";

    v.Expression = ko.observable();//type but not nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DecisionBranchPartial) {
        return _(v).extend(new bespoke.sph.domain.DecisionBranchPartial(v));
    }
    return v;
};



bespoke.sph.domain.NotificationActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.From = ko.observable('');

    v.Subject = ko.observable('');

    v.Body = ko.observable('');

    v.To = ko.observable('');

    v.UserName = ko.observable('');

    v.Cc = ko.observable('');

    v.Bcc = ko.observable('');

    v.IsHtmlEmail = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.NotificationActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.NotificationActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.NotificationActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.SimpleVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.SimpleVariable, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SimpleVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.SimpleVariablePartial(v));
    }
    return v;
};



bespoke.sph.domain.ComplexVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ComplexVariable, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ComplexVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.ComplexVariablePartial(v));
    }
    return v;
};


// placeholder for FormDesign

bespoke.sph.domain.VariableValue = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.VariableValue, domain.sph",
        Name: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.VariableValuePartial) {
        return _(model).extend(new bespoke.sph.domain.VariableValuePartial(model));
    }
    return model;
};



bespoke.sph.domain.Page = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Page, domain.sph",
        PageId: ko.observable(0),
        Name: ko.observable(''),
        IsRazor: ko.observable(false),
        IsPartial: ko.observable(false),
        VirtualPath: ko.observable(''),
        Tag: ko.observable(''),
        Version: ko.observable(0),
        Mode: ko.observable(''),
        Extension: ko.observable(''),
        Code: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.PagePartial) {
        return _(model).extend(new bespoke.sph.domain.PagePartial(model));
    }
    return model;
};



bespoke.sph.domain.EndActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.IsTerminating = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.EndActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.EndActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.EndActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.Performer = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Performer, domain.sph",
        UserProperty: ko.observable(''),
        Value: ko.observable(''),
        IsPublic: ko.observable(false),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.PerformerPartial) {
        return _(model).extend(new bespoke.sph.domain.PerformerPartial(model));
    }
    return model;
};



bespoke.sph.domain.WorkflowDesigner = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.WorkflowDesigner, domain.sph",
        X: ko.observable(0),
        Y: ko.observable(0),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkflowDesignerPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowDesignerPartial(model));
    }
    return model;
};



bespoke.sph.domain.SimpleMapping = function (optionOrWebid) {

    var v = new bespoke.sph.domain.PropertyMapping(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.SimpleMapping, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SimpleMappingPartial) {
        return _(v).extend(new bespoke.sph.domain.SimpleMappingPartial(v));
    }
    return v;
};



bespoke.sph.domain.FunctoidMapping = function (optionOrWebid) {

    var v = new bespoke.sph.domain.PropertyMapping(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.FunctoidMapping, domain.sph";

    v.Functoid = ko.observable();//type but not nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FunctoidMappingPartial) {
        return _(v).extend(new bespoke.sph.domain.FunctoidMappingPartial(v));
    }
    return v;
};



bespoke.sph.domain.CreateEntityActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.EntityType = ko.observable('');

    v.ReturnValuePath = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.CreateEntityActivity, domain.sph";

    v.PropertyMappingCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CreateEntityActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.CreateEntityActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.ExpressionActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ExpressionActivity, domain.sph";

    v.Expression = ko.observable();//type but not nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ExpressionActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ExpressionActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.DeleteEntityActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.EntityType = ko.observable('');

    v.EntityIdPath = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.DeleteEntityActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DeleteEntityActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.DeleteEntityActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.UpdateEntityActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.EntityType = ko.observable('');

    v.EntityIdPath = ko.observable('');

    v.UseVariable = ko.observable('');

    v.IsUsingVariable = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.UpdateEntityActivity, domain.sph";

    v.PropertyMappingCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.UpdateEntityActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.UpdateEntityActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.ScriptFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Expression = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ScriptFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ScriptFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ScriptFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.ConfirmationOptions = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ConfirmationOptions, domain.sph",
        Type: ko.observable(''),
        Value: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ConfirmationOptionsPartial) {
        return _(model).extend(new bespoke.sph.domain.ConfirmationOptionsPartial(model));
    }
    return model;
};



bespoke.sph.domain.ReceiveActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.PortType = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ReceiveActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ReceiveActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ReceiveActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.SendActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.PortType = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.SendActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SendActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.SendActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.ListenActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ListenActivity, domain.sph";

    v.ListenBranchCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ListenActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ListenActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.ParallelActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ParallelActivity, domain.sph";

    v.ParallelBranchCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParallelActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ParallelActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.JoinActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.Placeholder = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.JoinActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.JoinActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.JoinActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.DelayActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.Expression = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.DelayActivity, domain.sph";

    v.Miliseconds = ko.observable();//type but not nillable
    v.Seconds = ko.observable();//type but not nillable
    v.Hour = ko.observable();//type but not nillable
    v.Days = ko.observable();//type but not nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DelayActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.DelayActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.ThrowActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.Message = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ThrowActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ThrowActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ThrowActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.ParallelBranch = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ParallelBranch, domain.sph";

    v.ActivityCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParallelBranchPartial) {
        return _(v).extend(new bespoke.sph.domain.ParallelBranchPartial(v));
    }
    return v;
};



bespoke.sph.domain.ListenBranch = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.IsWaitingAsync = ko.observable(false);

    v.IsDestroyed = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ListenBranch, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ListenBranchPartial) {
        return _(v).extend(new bespoke.sph.domain.ListenBranchPartial(v));
    }
    return v;
};



bespoke.sph.domain.ClrTypeVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v.Assembly = ko.observable('');

    v.CanInitiateWithDefaultConstructor = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ClrTypeVariable, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ClrTypeVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.ClrTypeVariablePartial(v));
    }
    return v;
};



bespoke.sph.domain.ScheduledTriggerActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ScheduledTriggerActivity, domain.sph";

    v.IntervalScheduleCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ScheduledTriggerActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ScheduledTriggerActivityPartial(v));
    }
    return v;
};


// placeholder for IntervalSchedule

bespoke.sph.domain.Tracker = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Tracker, domain.sph",
        TrackerId: ko.observable(0),
        WorkflowId: ko.observable(0),
        WorkflowDefinitionId: ko.observable(0),
        ForbiddenActivities: ko.observableArray([]),
        ExecutedActivityCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TrackerPartial) {
        return _(model).extend(new bespoke.sph.domain.TrackerPartial(model));
    }
    return model;
};



bespoke.sph.domain.ExecutedActivity = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ExecutedActivity, domain.sph",
        InstanceId: ko.observable(0),
        ActivityWebId: ko.observable(''),
        WorkflowDefinitionId: ko.observable(0),
        User: ko.observable(''),
        Name: ko.observable(''),
        Type: ko.observable(''),
        Initiated: ko.observable(),
        Run: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ExecutedActivityPartial) {
        return _(model).extend(new bespoke.sph.domain.ExecutedActivityPartial(model));
    }
    return model;
};



bespoke.sph.domain.Breakpoint = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Breakpoint, domain.sph",
        IsEnabled: ko.observable(false),
        ActivityWebId: ko.observable(''),
        WorkflowDefinitionId: ko.observable(0),
        ConditionExpression: ko.observable(''),
        HitCount: ko.observable(0),
        Label: ko.observable(''),
        WhenHitPrintMessage: ko.observable(false),
        WhenHitContinueExecution: ko.observable(false),
        MessageExpression: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.BreakpointPartial) {
        return _(model).extend(new bespoke.sph.domain.BreakpointPartial(model));
    }
    return model;
};



bespoke.sph.domain.ReferencedAssembly = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReferencedAssembly, domain.sph",
        Name: ko.observable(''),
        FullName: ko.observable(''),
        Version: ko.observable(''),
        Location: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ReferencedAssemblyPartial) {
        return _(model).extend(new bespoke.sph.domain.ReferencedAssemblyPartial(model));
    }
    return model;
};



bespoke.sph.domain.MappingActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.MappingDefinition = ko.observable('');

    v.DestinationType = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.MappingActivity, domain.sph";

    v.MappingSourceCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.MappingActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.MappingActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.MappingSource = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.MappingSource, domain.sph",
        TypeName: ko.observable(''),
        Variable: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.MappingSourcePartial) {
        return _(model).extend(new bespoke.sph.domain.MappingSourcePartial(model));
    }
    return model;
};



bespoke.sph.domain.TransformDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.TransformDefinition, domain.sph",
        TransformDefinitionId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        InputTypeName: ko.observable(''),
        OutputTypeName: ko.observable(''),
        IsPublished: ko.observable(false),
        MapCollection: ko.observableArray([]),
        FunctoidCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TransformDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.TransformDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.DirectMap = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Map(optionOrWebid);

    v.Source = ko.observable('');

    v.TypeName = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.DirectMap, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DirectMapPartial) {
        return _(v).extend(new bespoke.sph.domain.DirectMapPartial(v));
    }
    return v;
};



bespoke.sph.domain.FunctoidMap = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Map(optionOrWebid);

    v.__uuid = ko.observable('');

    v.Functoid = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.FunctoidMap, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FunctoidMapPartial) {
        return _(v).extend(new bespoke.sph.domain.FunctoidMapPartial(v));
    }
    return v;
};



bespoke.sph.domain.StringConcateFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.StringConcateFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.StringConcateFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.StringConcateFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.BooleanFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Format = ko.observable('');

    v.SourceField = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.BooleanFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.BooleanFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.BooleanFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.DoubleFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.NumberStyles = ko.observable('');

    v.SourceField = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ParseDoubleFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DoubleFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.DoubleFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.DecimalFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.NumberStyles = ko.observable('');

    v.SourceField = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.DecimalFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DecimalFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.DecimalFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.Int32Functoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.SourceField = ko.observable('');

    v.NumberStyles = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ParseInt32Functoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.Int32FunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.Int32FunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.DateFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Dummy = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.DateFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DateFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.DateFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.FormattingFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Format = ko.observable('');

    v.SourceField = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.FormattingFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FormattingFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.FormattingFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.FunctoidArg = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FunctoidArg, domain.sph",
        Name: ko.observable(''),
        TypeName: ko.observable(''),
        Description: ko.observable(''),
        Label: ko.observable(''),
        Comment: ko.observable(''),
        IsOptional: ko.observable(false),
        Functoid: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FunctoidArgPartial) {
        return _(model).extend(new bespoke.sph.domain.FunctoidArgPartial(model));
    }
    return model;
};



bespoke.sph.domain.ConstantFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.TypeName = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ConstantFunctoid, domain.sph";

    v.Value = ko.observable();//type but not nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ConstantFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ConstantFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.SourceFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Field = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.SourceFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SourceFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.SourceFunctoidPartial(v));
    }
    return v;
};


bespoke.sph.domain.Field = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Field, domain.sph",
        Name: ko.observable(''),
        Note: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.FieldPartial) {
        return _(model).extend(new bespoke.sph.domain.FieldPartial(model));
    }
    return model;
};


bespoke.sph.domain.CustomAction = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.CustomAction, domain.sph",
        Title: ko.observable(''),
        IsActive: ko.observable(false),
        TriggerId: ko.observable(0),
        Note: ko.observable(''),
        CustomActionId: ko.observable(0),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.CustomActionPartial) {
        return _(model).extend(new bespoke.sph.domain.CustomActionPartial(model));
    }
    return model;
};


bespoke.sph.domain.Activity = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Activity, domain.sph",
        IsInitiator: ko.observable(false),
        NextActivityWebId: ko.observable(''),
        Name: ko.observable(''),
        WorkflowDesigner: ko.observable(new bespoke.sph.domain.WorkflowDesigner()),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.ActivityPartial) {
        return _(model).extend(new bespoke.sph.domain.ActivityPartial(model));
    }
    return model;
};


bespoke.sph.domain.Variable = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Variable, domain.sph",
        Name: ko.observable(''),
        TypeName: ko.observable(''),
        DefaultValue: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.VariablePartial) {
        return _(model).extend(new bespoke.sph.domain.VariablePartial(model));
    }
    return model;
};


bespoke.sph.domain.PropertyMapping = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.PropertyMapping, domain.sph",
        Source: ko.observable(''),
        Destination: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.PropertyMappingPartial) {
        return _(model).extend(new bespoke.sph.domain.PropertyMappingPartial(model));
    }
    return model;
};


bespoke.sph.domain.Functoid = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Functoid, domain.sph",
        Name: ko.observable(''),
        OutputTypeName: ko.observable(''),
        Label: ko.observable(''),
        Comment: ko.observable(''),
        X: ko.observable(0.00),
        Y: ko.observable(0.00),
        ArgumentCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.FunctoidPartial) {
        return _(model).extend(new bespoke.sph.domain.FunctoidPartial(model));
    }
    return model;
};


bespoke.sph.domain.Map = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Map, domain.sph",
        Destination: ko.observable(''),
        SourceTypeName: ko.observable(''),
        DestinationTypeName: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.MapPartial) {
        return _(model).extend(new bespoke.sph.domain.MapPartial(model));
    }
    return model;
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
        NEQ: 'Neq',
        NOT_STARTS_WITH: 'NotStartsWith',
        NOT_ENDS_WITH: 'NotEndsWith',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();

