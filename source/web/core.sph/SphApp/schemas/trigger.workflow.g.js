﻿/// <reference path="~/scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />
/// <reference path="~/Scripts/require.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.Trigger = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.Trigger, domain.sph",
            Id: ko.observable("0"),
            Name: ko.observable(""),
            Entity: ko.observable(""),
            TypeOf: ko.observable(""),
            Note: ko.observable(""),
            IsActive: ko.observable(false),
            IsFiredOnAdded: ko.observable(false),
            IsFiredOnDeleted: ko.observable(false),
            IsFiredOnChanged: ko.observable(false),
            FiredOnOperations: ko.observable(""),
            EnableTracking: ko.observable(false),
            RuleCollection: ko.observableArray([]),
            ActionCollection: ko.observableArray([]),
            ReferencedAssemblyCollection: ko.observableArray([]),
            RequeueFilterCollection: ko.observableArray([]),
            ShouldProcessedOnceAccepted: ko.observable(),
            ShouldProcessedOncePersisted: ko.observable(),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TriggerPartial) {
        return _(model).extend(new bespoke.sph.domain.TriggerPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.AssemblyField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Location = ko.observable("");

    v.TypeName = ko.observable("");

    v.Method = ko.observable("");

    v.IsAsync = ko.observable(false);

    v.AsyncTimeout = ko.observable(0);

    v.LoadInCurrentAppDomain = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.AssemblyField, domain.sph";

    v.MethodArgCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.AssemblyFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.AssemblyFieldPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.JavascriptExpressionField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Expression = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.JavascriptExpressionField, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.JavascriptExpressionFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.JavascriptExpressionFieldPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.RouteParameterField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.TypeName = ko.observable("");

    v.DefaultValue = ko.observable("");

    v.IsOptional = ko.observable(false);

    v.Constraints = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.RouteParameterField, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.RouteParameterFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.RouteParameterFieldPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.FunctionField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Script = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.FunctionField, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FunctionFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.FunctionFieldPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ConstantField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.TypeName = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ConstantField, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ConstantFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.ConstantFieldPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.DocumentField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.XPath = ko.observable("");

    v.NamespacePrefix = ko.observable("");

    v.TypeName = ko.observable("");

    v.Path = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DocumentField, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DocumentFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.DocumentFieldPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.PropertyChangedField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Path = ko.observable("");

    v.TypeName = ko.observable("");

    v.OldValue = ko.observable("");

    v.NewValue = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.PropertyChangedField, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.PropertyChangedFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.PropertyChangedFieldPartial(v, optionOrWebid));
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
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.RulePartial) {
        return _(model).extend(new bespoke.sph.domain.RulePartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.EmailAction = function (optionOrWebid) {

    var v = new bespoke.sph.domain.CustomAction(optionOrWebid);

    v.From = ko.observable("");

    v.To = ko.observable("");

    v.SubjectTemplate = ko.observable("");

    v.BodyTemplate = ko.observable("");

    v.Bcc = ko.observable("");

    v.Cc = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.EmailAction, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.EmailActionPartial) {
        return _(v).extend(new bespoke.sph.domain.EmailActionPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.SetterAction = function (optionOrWebid) {

    var v = new bespoke.sph.domain.CustomAction(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.SetterAction, domain.sph";

    v.SetterActionChildCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SetterActionPartial) {
        return _(v).extend(new bespoke.sph.domain.SetterActionPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.SetterActionChild = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.SetterActionChild, domain.sph",
            Path: ko.observable(""),
            Field: ko.observable(),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SetterActionChildPartial) {
        return _(model).extend(new bespoke.sph.domain.SetterActionChildPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.MethodArg = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.MethodArg, domain.sph",
            Name: ko.observable(""),
            TypeName: ko.observable(""),
            ValueProvider: ko.observable(),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.MethodArgPartial) {
        return _(model).extend(new bespoke.sph.domain.MethodArgPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.StartWorkflowAction = function (optionOrWebid) {

    var v = new bespoke.sph.domain.CustomAction(optionOrWebid);

    v.WorkflowDefinitionId = ko.observable("");

    v.Name = ko.observable("");

    v.Version = ko.observable(0);

    v["$type"] = "Bespoke.Sph.Domain.StartWorkflowAction, domain.sph";

    v.WorkflowTriggerMapCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.StartWorkflowActionPartial) {
        return _(v).extend(new bespoke.sph.domain.StartWorkflowActionPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.WorkflowTriggerMap = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.WorkflowTriggerMap, domain.sph",
            VariablePath: ko.observable(""),
            Field: ko.observable(),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkflowTriggerMapPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowTriggerMapPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.AssemblyAction = function (optionOrWebid) {

    var v = new bespoke.sph.domain.CustomAction(optionOrWebid);

    v.IsAsyncMethod = ko.observable(false);

    v.Assembly = ko.observable("");

    v.TypeName = ko.observable("");

    v.Method = ko.observable("");

    v.ReturnType = ko.observable("");

    v.IsVoid = ko.observable(false);

    v.IsStatic = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.AssemblyAction, domain.sph";

    v.MethodArgCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.AssemblyActionPartial) {
        return _(v).extend(new bespoke.sph.domain.AssemblyActionPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.WorkflowDefinition = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.WorkflowDefinition, domain.sph",
            Id: ko.observable("0"),
            Name: ko.observable(""),
            Note: ko.observable(""),
            IsActive: ko.observable(false),
            SchemaStoreId: ko.observable(""),
            Version: ko.observable(0),
            ActivityCollection: ko.observableArray([]),
            VariableDefinitionCollection: ko.observableArray([]),
            ReferencedAssemblyCollection: ko.observableArray([]),
            CorrelationSetCollection: ko.observableArray([]),
            CorrelationTypeCollection: ko.observableArray([]),
            TryScopeCollection: ko.observableArray([]),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkflowDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowDefinitionPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.Workflow = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.Workflow, domain.sph",
            Id: ko.observable("0"),
            WorkflowDefinitionId: ko.observable(""),
            Name: ko.observable(""),
            State: ko.observable(""),
            IsActive: ko.observable(false),
            Version: ko.observable(0),
            VariableValueCollection: ko.observableArray([]),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkflowPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.DecisionActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.DecisionActivity, domain.sph";

    v.DecisionBranchCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DecisionActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.DecisionActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.DecisionBranch = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.IsDefault = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.DecisionBranch, domain.sph";

    v.Expression = ko.observable();//type but not nillable

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DecisionBranchPartial) {
        return _(v).extend(new bespoke.sph.domain.DecisionBranchPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.NotificationActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.From = ko.observable("");

    v.Subject = ko.observable("");

    v.Body = ko.observable("");

    v.To = ko.observable("");

    v.UserName = ko.observable("");

    v.Cc = ko.observable("");

    v.Bcc = ko.observable("");

    v.IsHtmlEmail = ko.observable(false);

    v.IsMessageSuppressed = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.NotificationActivity, domain.sph";

    v.Retry = ko.observable();//nillable
    v.RetryInterval = ko.observable();//nillable

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.NotificationActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.NotificationActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.SimpleVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.SimpleVariable, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SimpleVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.SimpleVariablePartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ComplexVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ComplexVariable, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ComplexVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.ComplexVariablePartial(v, optionOrWebid));
    }
    return v;
};


// placeholder for FormDesign

bespoke.sph.domain.VariableValue = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.VariableValue, domain.sph",
            Name: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.VariableValuePartial) {
        return _(model).extend(new bespoke.sph.domain.VariableValuePartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.EndActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.IsTerminating = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.EndActivity, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.EndActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.EndActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.Performer = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.Performer, domain.sph",
            UserProperty: ko.observable(""),
            Value: ko.observable(""),
            IsPublic: ko.observable(false),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.PerformerPartial) {
        return _(model).extend(new bespoke.sph.domain.PerformerPartial(model, optionOrWebid));
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
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WorkflowDesignerPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowDesignerPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.SimpleMapping = function (optionOrWebid) {

    var v = new bespoke.sph.domain.PropertyMapping(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.SimpleMapping, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SimpleMappingPartial) {
        return _(v).extend(new bespoke.sph.domain.SimpleMappingPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.FunctoidMapping = function (optionOrWebid) {

    var v = new bespoke.sph.domain.PropertyMapping(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.FunctoidMapping, domain.sph";

    v.Functoid = ko.observable();//type but not nillable

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FunctoidMappingPartial) {
        return _(v).extend(new bespoke.sph.domain.FunctoidMappingPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.CreateEntityActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.EntityType = ko.observable("");

    v.ReturnValuePath = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.CreateEntityActivity, domain.sph";

    v.PropertyMappingCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CreateEntityActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.CreateEntityActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ExpressionActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ExpressionActivity, domain.sph";

    v.Expression = ko.observable();//type but not nillable

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ExpressionActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ExpressionActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.DeleteEntityActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.EntityType = ko.observable("");

    v.EntityIdPath = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DeleteEntityActivity, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DeleteEntityActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.DeleteEntityActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.UpdateEntityActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.Entity = ko.observable("");

    v.EntityIdPath = ko.observable("");

    v.UseVariable = ko.observable("");

    v.IsUsingVariable = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.UpdateEntityActivity, domain.sph";

    v.PropertyMappingCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.UpdateEntityActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.UpdateEntityActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ScriptFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Expression = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ScriptFunctoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ScriptFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ScriptFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ConfirmationOptions = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.ConfirmationOptions, domain.sph",
            Type: ko.observable(""),
            Value: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ConfirmationOptionsPartial) {
        return _(model).extend(new bespoke.sph.domain.ConfirmationOptionsPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.ReceiveActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.PortType = ko.observable("");

    v.Operation = ko.observable("");

    v.MessagePath = ko.observable("");

    v.CancelMessageBody = ko.observable("");

    v.CancelMessageSubject = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ReceiveActivity, domain.sph";

    v.InitializingCorrelationSetCollection = ko.observableArray([]);
    v.FollowingCorrelationSetCollection = ko.observableArray([]);
    v.CorrelationPropertyCollection = ko.observableArray([]);
    v.Performer = ko.observable(new bespoke.sph.domain.Performer());

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ReceiveActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ReceiveActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.SendActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.PortType = ko.observable("");

    v.Adapter = ko.observable("");

    v.Method = ko.observable("");

    v.AdapterAssembly = ko.observable("");

    v.IsSynchronous = ko.observable(false);

    v.ArgumentPath = ko.observable("");

    v.ReturnValuePath = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.SendActivity, domain.sph";

    v.ExceptionFilterCollection = ko.observableArray([]);
    v.InitializingCorrelationSetCollection = ko.observableArray([]);
    v.FollowingCorrelationSetCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SendActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.SendActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ListenActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ListenActivity, domain.sph";

    v.ListenBranchCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ListenActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ListenActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ParallelActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ParallelActivity, domain.sph";

    v.ParallelBranchCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParallelActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ParallelActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.JoinActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.Placeholder = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.JoinActivity, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.JoinActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.JoinActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.DelayActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.Expression = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DelayActivity, domain.sph";

    v.Miliseconds = ko.observable();//type but not nillable
    v.Seconds = ko.observable();//type but not nillable
    v.Hour = ko.observable();//type but not nillable
    v.Days = ko.observable();//type but not nillable

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DelayActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.DelayActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ThrowActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.Message = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ThrowActivity, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ThrowActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ThrowActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ParallelBranch = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ParallelBranch, domain.sph";

    v.ActivityCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParallelBranchPartial) {
        return _(v).extend(new bespoke.sph.domain.ParallelBranchPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ListenBranch = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.IsWaitingAsync = ko.observable(false);

    v.IsDestroyed = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ListenBranch, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ListenBranchPartial) {
        return _(v).extend(new bespoke.sph.domain.ListenBranchPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ValueObjectVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v.ValueObjectDefinition = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ValueObjectVariable, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ValueObjectVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.ValueObjectVariablePartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ClrTypeVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v.Assembly = ko.observable("");

    v.CanInitiateWithDefaultConstructor = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ClrTypeVariable, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ClrTypeVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.ClrTypeVariablePartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ScheduledTriggerActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ScheduledTriggerActivity, domain.sph";

    v.IntervalScheduleCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ScheduledTriggerActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ScheduledTriggerActivityPartial(v, optionOrWebid));
    }
    return v;
};


// placeholder for IntervalSchedule

bespoke.sph.domain.Tracker = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.Tracker, domain.sph",
            Id: ko.observable("0"),
            WorkflowId: ko.observable(""),
            WorkflowDefinitionId: ko.observable(""),
            ForbiddenActivities: ko.observableArray([]),
            ExecutedActivityCollection: ko.observableArray([]),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TrackerPartial) {
        return _(model).extend(new bespoke.sph.domain.TrackerPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.ExecutedActivity = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.ExecutedActivity, domain.sph",
            InstanceId: ko.observable(""),
            ActivityWebId: ko.observable(""),
            WorkflowDefinitionId: ko.observable(""),
            User: ko.observable(""),
            Name: ko.observable(""),
            Type: ko.observable(""),
            IsCancelled: ko.observable(false),
            Initiated: ko.observable(),
            Run: ko.observable(),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ExecutedActivityPartial) {
        return _(model).extend(new bespoke.sph.domain.ExecutedActivityPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.Breakpoint = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.Breakpoint, domain.sph",
            IsEnabled: ko.observable(false),
            ActivityWebId: ko.observable(""),
            WorkflowDefinitionId: ko.observable(""),
            ConditionExpression: ko.observable(""),
            HitCount: ko.observable(0),
            Label: ko.observable(""),
            WhenHitPrintMessage: ko.observable(false),
            WhenHitContinueExecution: ko.observable(false),
            MessageExpression: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.BreakpointPartial) {
        return _(model).extend(new bespoke.sph.domain.BreakpointPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.ReferencedAssembly = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.ReferencedAssembly, domain.sph",
            Name: ko.observable(""),
            FullName: ko.observable(""),
            Version: ko.observable(""),
            Location: ko.observable(""),
            IsGac: ko.observable(false),
            IsStrongName: ko.observable(false),
            RuntimeVersion: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ReferencedAssemblyPartial) {
        return _(model).extend(new bespoke.sph.domain.ReferencedAssemblyPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.MappingActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.MappingDefinition = ko.observable("");

    v.DestinationType = ko.observable("");

    v.OutputPath = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.MappingActivity, domain.sph";

    v.MappingSourceCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.MappingActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.MappingActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.MappingSource = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.MappingSource, domain.sph",
            TypeName: ko.observable(""),
            Variable: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.MappingSourcePartial) {
        return _(model).extend(new bespoke.sph.domain.MappingSourcePartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.TransformDefinition = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.TransformDefinition, domain.sph",
            Id: ko.observable("0"),
            Name: ko.observable(""),
            Description: ko.observable(""),
            InputTypeName: ko.observable(""),
            OutputTypeName: ko.observable(""),
            IsPublished: ko.observable(false),
            MapCollection: ko.observableArray([]),
            FunctoidCollection: ko.observableArray([]),
            InputCollection: ko.observableArray([]),
            ReferencedAssemblyCollection: ko.observableArray([]),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TransformDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.TransformDefinitionPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.DirectMap = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Map(optionOrWebid);

    v.Source = ko.observable("");

    v.TypeName = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DirectMap, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DirectMapPartial) {
        return _(v).extend(new bespoke.sph.domain.DirectMapPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.FunctoidMap = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Map(optionOrWebid);

    v.__uuid = ko.observable("");

    v.Functoid = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.FunctoidMap, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FunctoidMapPartial) {
        return _(v).extend(new bespoke.sph.domain.FunctoidMapPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.StringConcateFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.StringConcateFunctoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.StringConcateFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.StringConcateFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ParseBooleanFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Format = ko.observable("");

    v.SourceField = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseBooleanFunctoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseBooleanFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseBooleanFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ParseDoubleFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Styles = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseDoubleFunctoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseDoubleFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseDoubleFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ParseDecimalFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Styles = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseDecimalFunctoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseDecimalFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseDecimalFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ParseInt32Functoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Styles = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseInt32Functoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseInt32FunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseInt32FunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ParseDateTimeFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Format = ko.observable("");

    v.Styles = ko.observable("");

    v.Culture = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseDateTimeFunctoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseDateTimeFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseDateTimeFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.FormattingFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Format = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.FormattingFunctoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FormattingFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.FormattingFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.FunctoidArg = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.FunctoidArg, domain.sph",
            Name: ko.observable(""),
            TypeName: ko.observable(""),
            Description: ko.observable(""),
            Label: ko.observable(""),
            Comment: ko.observable(""),
            IsOptional: ko.observable(false),
            Functoid: ko.observable(""),
            Constant: ko.observable(""),
            Default: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FunctoidArgPartial) {
        return _(model).extend(new bespoke.sph.domain.FunctoidArgPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.ConstantFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.TypeName = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ConstantFunctoid, domain.sph";

    v.Value = ko.observable();//type but not nillable

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ConstantFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ConstantFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.SourceFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Field = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.SourceFunctoid, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SourceFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.SourceFunctoidPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.ExceptionFilter = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.ExceptionFilter, domain.sph",
            TypeName: ko.observable(""),
            Filter: ko.observable(""),
            Interval: ko.observable(),
            IntervalPeriod: ko.observable(),
            MaxRequeue: ko.observable(),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ExceptionFilterPartial) {
        return _(model).extend(new bespoke.sph.domain.ExceptionFilterPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.CorrelationType = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.CorrelationType, domain.sph",
            Name: ko.observable(""),
            CorrelationPropertyCollection: ko.observableArray([]),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CorrelationTypePartial) {
        return _(model).extend(new bespoke.sph.domain.CorrelationTypePartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.CorrelationSet = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.CorrelationSet, domain.sph",
            Type: ko.observable(""),
            Name: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CorrelationSetPartial) {
        return _(model).extend(new bespoke.sph.domain.CorrelationSetPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.CorrelationProperty = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.CorrelationProperty, domain.sph",
            Path: ko.observable(""),
            Name: ko.observable(""),
            Origin: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CorrelationPropertyPartial) {
        return _(model).extend(new bespoke.sph.domain.CorrelationPropertyPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.ChildWorkflowActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.WorkflowDefinitionId = ko.observable("");

    v.Version = ko.observable(0);

    v["$type"] = "Bespoke.Sph.Domain.ChildWorkflowActivity, domain.sph";

    v.PropertyMappingCollection = ko.observableArray([]);
    v.ExecutedPropertyMappingCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ChildWorkflowActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ChildWorkflowActivityPartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.TryScope = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Scope(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.TryScope, domain.sph";

    v.CatchScopeCollection = ko.observableArray([]);

    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TryScopePartial) {
        return _(v).extend(new bespoke.sph.domain.TryScopePartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.CatchScope = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Scope(optionOrWebid);

    v.ExceptionType = ko.observable("");

    v.ExceptionVar = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.CatchScope, domain.sph";


    var context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                // array
                if (ko.isObservable(v[n]) && 'push' in v[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        v[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }
                if (ko.isObservable(v[n])) {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CatchScopePartial) {
        return _(v).extend(new bespoke.sph.domain.CatchScopePartial(v, optionOrWebid));
    }
    return v;
};


bespoke.sph.domain.SendPort = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.SendPort, domain.sph",
            Name: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SendPortPartial) {
        return _(model).extend(new bespoke.sph.domain.SendPortPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.SendPortDefinition = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.SendPortDefinition, domain.sph",
            Name: ko.observable(""),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SendPortDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.SendPortDefinitionPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.DataTransferDefinition = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.DataTransferDefinition, domain.sph",
            Name: ko.observable(""),
            InboundAdapter: ko.observable(""),
            InboundMap: ko.observable(""),
            SelectStatement: ko.observable(""),
            Table: ko.observable(""),
            Entity: ko.observable(""),
            BatchSize: ko.observable(0),
            IgnoreMessaging: ko.observable(false),
            DelayThrottle: ko.observable(),
            SqlRetry: ko.observable(),
            SqlWait: ko.observable(),
            EsRetry: ko.observable(),
            EsWait: ko.observable(),
            IntervalScheduleCollection: ko.observableArray([]),
            ScheduledDataTransferCollection: ko.observableArray([]),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DataTransferDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.DataTransferDefinitionPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.ScheduledDataTransfer = function (optionOrWebid) {

    var model = {
            "$type": "Bespoke.Sph.Domain.ScheduledDataTransfer, domain.sph",
            ScheduleWebId: ko.observable(""),
            NotifyOnError: ko.observable(false),
            NotifyOnSuccess: ko.observable(false),
            TruncateData: ko.observable(false),
            isBusy: ko.observable(false),
            WebId: ko.observable()
        },
        context = require("services/datacontext");
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (ko.isObservable(model[n]) && 'push' in model[n]) {
                    var values = optionOrWebid[n].$values || optionOrWebid[n];
                    if (_(values).isArray()) {
                        model[n](_(values).map(function (ai) {
                            return context.toObservable(ai);
                        }));
                        continue;
                    }
                }

                if (ko.isObservable(model[n])) {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ScheduledDataTransferPartial) {
        return _(model).extend(new bespoke.sph.domain.ScheduledDataTransferPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.Field = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Field, domain.sph",
        Name: ko.observable(""),
        Note: ko.observable(""),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.FieldPartial) {
        return _(model).extend(new bespoke.sph.domain.FieldPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.CustomAction = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.CustomAction, domain.sph",
        Title: ko.observable(""),
        IsActive: ko.observable(false),
        TriggerId: ko.observable(""),
        Note: ko.observable(""),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.CustomActionPartial) {
        return _(model).extend(new bespoke.sph.domain.CustomActionPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.Activity = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Activity, domain.sph",
        IsInitiator: ko.observable(false),
        NextActivityWebId: ko.observable(""),
        Name: ko.observable(""),
        TryScope: ko.observable(""),
        CatchScope: ko.observable(""),
        Note: ko.observable(""),
        WorkflowDesigner: ko.observable(new bespoke.sph.domain.WorkflowDesigner()),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.ActivityPartial) {
        return _(model).extend(new bespoke.sph.domain.ActivityPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.Variable = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Variable, domain.sph",
        Name: ko.observable(""),
        TypeName: ko.observable(""),
        DefaultValue: ko.observable(""),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.VariablePartial) {
        return _(model).extend(new bespoke.sph.domain.VariablePartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.PropertyMapping = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.PropertyMapping, domain.sph",
        Source: ko.observable(""),
        Destination: ko.observable(""),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.PropertyMappingPartial) {
        return _(model).extend(new bespoke.sph.domain.PropertyMappingPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.Functoid = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Functoid, domain.sph",
        Name: ko.observable(""),
        OutputTypeName: ko.observable(""),
        Label: ko.observable(""),
        Comment: ko.observable(""),
        X: ko.observable(0.00),
        Y: ko.observable(0.00),
        ArgumentCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.FunctoidPartial) {
        return _(model).extend(new bespoke.sph.domain.FunctoidPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.Map = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Map, domain.sph",
        Destination: ko.observable(""),
        SourceTypeName: ko.observable(""),
        DestinationTypeName: ko.observable(""),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.MapPartial) {
        return _(model).extend(new bespoke.sph.domain.MapPartial(model, optionOrWebid));
    }
    return model;
};


bespoke.sph.domain.Scope = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Scope, domain.sph",
        Id: ko.observable(""),
        Name: ko.observable(""),
        isBusy: ko.observable(false),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }

    if (bespoke.sph.domain.ScopePartial) {
        return _(model).extend(new bespoke.sph.domain.ScopePartial(model, optionOrWebid));
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
        IS_NULL: 'IsNull',
        IS_NOT_NULL: 'IsNotNull',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();

        