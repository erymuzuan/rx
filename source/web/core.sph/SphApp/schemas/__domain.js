
/// <reference path="~/scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.FormDesign = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FormDesign, domain.sph",
        Name: ko.observable(""),
        Description: ko.observable(""),
        ConfirmationText: ko.observable(""),
        ImageStoreId: ko.observable(""),
        FormElementCollection: ko.observableArray([]),
        LabelColLg: ko.observable(),
        LabelColMd: ko.observable(),
        LabelColSm: ko.observable(),
        LabelColXs: ko.observable(),
        InputColLg: ko.observable(),
        InputColMd: ko.observable(),
        InputColSm: ko.observable(),
        InputColXs: ko.observable(),
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


    if (bespoke.sph.domain.FormDesignPartial) {
        return _(model).extend(new bespoke.sph.domain.FormDesignPartial(model));
    }
    return model;
};



bespoke.sph.domain.TextBox = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.DefaultValue = ko.observable("");

    v.AutoCompletionEntity = ko.observable("");

    v.AutoCompletionField = ko.observable("");

    v.AutoCompletionQuery = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.TextBox, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TextBoxPartial) {
        return _(v).extend(new bespoke.sph.domain.TextBoxPartial(v));
    }
    return v;
};



bespoke.sph.domain.CheckBox = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.CheckBox, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CheckBoxPartial) {
        return _(v).extend(new bespoke.sph.domain.CheckBoxPartial(v));
    }
    return v;
};



bespoke.sph.domain.DatePicker = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.DatePicker, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DatePickerPartial) {
        return _(v).extend(new bespoke.sph.domain.DatePickerPartial(v));
    }
    return v;
};



bespoke.sph.domain.DateTimePicker = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.DateTimePicker, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DateTimePickerPartial) {
        return _(v).extend(new bespoke.sph.domain.DateTimePickerPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComboBox = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.ComboBox, domain.sph";

    v.ComboBoxItemCollection = ko.observableArray([]);
    v.ComboBoxLookup = ko.observable(new bespoke.sph.domain.ComboBoxLookup());

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ComboBoxPartial) {
        return _(v).extend(new bespoke.sph.domain.ComboBoxPartial(v));
    }
    return v;
};



bespoke.sph.domain.TextAreaElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Rows = ko.observable("");

    v.IsHtml = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.TextAreaElement, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TextAreaElementPartial) {
        return _(v).extend(new bespoke.sph.domain.TextAreaElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.WebsiteFormElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.WebsiteFormElement, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.WebsiteFormElementPartial) {
        return _(v).extend(new bespoke.sph.domain.WebsiteFormElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.EmailFormElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.EmailFormElement, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.EmailFormElementPartial) {
        return _(v).extend(new bespoke.sph.domain.EmailFormElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.NumberTextBox = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Step = ko.observable(0);

    v["$type"] = "Bespoke.Sph.Domain.NumberTextBox, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.NumberTextBoxPartial) {
        return _(v).extend(new bespoke.sph.domain.NumberTextBoxPartial(v));
    }
    return v;
};



bespoke.sph.domain.MapElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Icon = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.MapElement, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.MapElementPartial) {
        return _(v).extend(new bespoke.sph.domain.MapElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.SectionFormElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.SectionFormElement, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SectionFormElementPartial) {
        return _(v).extend(new bespoke.sph.domain.SectionFormElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComboBoxItem = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ComboBoxItem, domain.sph",
        Caption: ko.observable(""),
        Value: ko.observable(""),
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


    if (bespoke.sph.domain.ComboBoxItemPartial) {
        return _(model).extend(new bespoke.sph.domain.ComboBoxItemPartial(model));
    }
    return model;
};



bespoke.sph.domain.AddressElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.IsUnitNoVisible = ko.observable(false);

    v.IsFloorVisible = ko.observable(false);

    v.IsBlockVisible = ko.observable(false);

    v.BlockOptionsPath = ko.observable("");

    v.FloorOptionsPath = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.AddressElement, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.AddressElementPartial) {
        return _(v).extend(new bespoke.sph.domain.AddressElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.HtmlElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.HtmlElement, domain.sph";

    v.Text = ko.observable();//type but not nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.HtmlElementPartial) {
        return _(v).extend(new bespoke.sph.domain.HtmlElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.DefaultValue = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DefaultValue, domain.sph",
        PropertyName: ko.observable(""),
        TypeName: ko.observable(""),
        IsNullable: ko.observable(false),
        Value: ko.observable(),
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


    if (bespoke.sph.domain.DefaultValuePartial) {
        return _(model).extend(new bespoke.sph.domain.DefaultValuePartial(model));
    }
    return model;
};



bespoke.sph.domain.FieldValidation = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FieldValidation, domain.sph",
        IsRequired: ko.observable(false),
        Pattern: ko.observable(""),
        Mode: ko.observable(""),
        Message: ko.observable(""),
        Min: ko.observable(),
        Max: ko.observable(),
        MinLength: ko.observable(),
        MaxLength: ko.observable(),
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


    if (bespoke.sph.domain.FieldValidationPartial) {
        return _(model).extend(new bespoke.sph.domain.FieldValidationPartial(model));
    }
    return model;
};


// placeholder for Performer

bespoke.sph.domain.BusinessRule = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.BusinessRule, domain.sph",
        Description: ko.observable(""),
        Name: ko.observable(""),
        ErrorLocation: ko.observable(""),
        ErrorMessage: ko.observable(""),
        RuleCollection: ko.observableArray([]),
        FilterCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.BusinessRulePartial) {
        return _(model).extend(new bespoke.sph.domain.BusinessRulePartial(model));
    }
    return model;
};


// placeholder for Rule

bespoke.sph.domain.FileUploadElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.AllowedExtensions = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.FileUploadElement, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.FileUploadElementPartial) {
        return _(v).extend(new bespoke.sph.domain.FileUploadElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComboBoxLookup = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ComboBoxLookup, domain.sph",
        Entity: ko.observable(""),
        ValuePath: ko.observable(""),
        DisplayPath: ko.observable(""),
        Query: ko.observable(""),
        IsComputedQuery: ko.observable(false),
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


    if (bespoke.sph.domain.ComboBoxLookupPartial) {
        return _(model).extend(new bespoke.sph.domain.ComboBoxLookupPartial(model));
    }
    return model;
};



bespoke.sph.domain.ChildEntityListView = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Entity = ko.observable("");

    v.Query = ko.observable("");

    v.IsAllowAddItem = ko.observable(false);

    v.NewItemFormRoute = ko.observable("");

    v.NewItemMappingSource = ko.observable("");

    v.NewItemMappingDestination = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ChildEntityListView, domain.sph";

    v.ViewColumnCollection = ko.observableArray([]);
    v.SortCollection = ko.observableArray([]);
    v.ConditionalFormattingCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ChildEntityListViewPartial) {
        return _(v).extend(new bespoke.sph.domain.ChildEntityListViewPartial(v));
    }
    return v;
};



bespoke.sph.domain.ListView = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.ChildItemType = ko.observable("");

    v.IsChildItemFunction = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ListView, domain.sph";

    v.ListViewColumnCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ListViewPartial) {
        return _(v).extend(new bespoke.sph.domain.ListViewPartial(v));
    }
    return v;
};



bespoke.sph.domain.ListViewColumn = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ListViewColumn, domain.sph",
        Label: ko.observable(""),
        Path: ko.observable(""),
        Input: ko.observable(),
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


    if (bespoke.sph.domain.ListViewColumnPartial) {
        return _(model).extend(new bespoke.sph.domain.ListViewColumnPartial(model));
    }
    return model;
};



bespoke.sph.domain.Button = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Command = ko.observable("");

    v.UseClick = ko.observable(false);

    v.CommandName = ko.observable("");

    v.LoadingText = ko.observable("");

    v.CompleteText = ko.observable("");

    v.IconClass = ko.observable("");

    v.IsToolbarItem = ko.observable(false);

    v.Operation = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.Button, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ButtonPartial) {
        return _(v).extend(new bespoke.sph.domain.ButtonPartial(v));
    }
    return v;
};



bespoke.sph.domain.EntityDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityDefinition, domain.sph",
        Id: ko.observable("0"),
        Name: ko.observable(""),
        Plural: ko.observable(""),
        IconClass: ko.observable(""),
        RecordName: ko.observable(""),
        IsPublished: ko.observable(false),
        TreatDataAsSource: ko.observable(false),
        MemberCollection: ko.observableArray([]),
        BusinessRuleCollection: ko.observableArray([]),
        StoreInDatabase: ko.observable(),
        StoreInElasticsearch: ko.observable(),
        ServiceContract: ko.observable(new bespoke.sph.domain.ServiceContract()),
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


    if (bespoke.sph.domain.EntityDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.ValueObjectDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ValueObjectDefinition, domain.sph",
        Id: ko.observable("0"),
        Name: ko.observable(""),
        MemberCollection: ko.observableArray([]),
        BusinessRuleCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.ValueObjectDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.ValueObjectDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.SimpleMember = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Member(optionOrWebid);

    v.TypeName = ko.observable("");

    v.IsNullable = ko.observable(false);

    v.IsNotIndexed = ko.observable(false);

    v.IsAnalyzed = ko.observable(false);

    v.IsFilterable = ko.observable(false);

    v.IsExcludeInAll = ko.observable(false);

    v.Boost = ko.observable(0);

    v["$type"] = "Bespoke.Sph.Domain.SimpleMember, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.SimpleMemberPartial) {
        return _(v).extend(new bespoke.sph.domain.SimpleMemberPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComplexMember = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Member(optionOrWebid);

    v.EmptyField = ko.observable("");

    v.TypeName = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ComplexMember, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ComplexMemberPartial) {
        return _(v).extend(new bespoke.sph.domain.ComplexMemberPartial(v));
    }
    return v;
};



bespoke.sph.domain.ValueObjectMember = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Member(optionOrWebid);

    v.ValueObjectName = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ValueObjectMember, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ValueObjectMemberPartial) {
        return _(v).extend(new bespoke.sph.domain.ValueObjectMemberPartial(v));
    }
    return v;
};



bespoke.sph.domain.EntityForm = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityForm, domain.sph",
        Id: ko.observable("0"),
        EntityDefinitionId: ko.observable(""),
        Name: ko.observable(""),
        Route: ko.observable(""),
        Note: ko.observable(""),
        IsAllowedNewItem: ko.observable(false),
        IconClass: ko.observable(""),
        IconStoreId: ko.observable(""),
        IsPublished: ko.observable(false),
        IsDefault: ko.observable(false),
        IsWatchAvailable: ko.observable(false),
        IsEmailAvailable: ko.observable(false),
        IsPrintAvailable: ko.observable(false),
        IsAuditTrailAvailable: ko.observable(false),
        IsRemoveAvailable: ko.observable(false),
        IsImportAvailable: ko.observable(false),
        IsExportAvailable: ko.observable(false),
        Operation: ko.observable(""),
        Entity: ko.observable(""),
        Partial: ko.observable(""),
        Caption: ko.observable(""),
        Layout: ko.observable(""),
        OperationSuccessMesage: ko.observable(""),
        OperationSuccessNavigateUrl: ko.observable(""),
        OperationSuccessCallback: ko.observable(""),
        OperationFailureCallback: ko.observable(""),
        OperationMethod: ko.observable(""),
        DeleteOperation: ko.observable(""),
        DeleteOperationSuccessMesage: ko.observable(""),
        DeleteOperationSuccessNavigateUrl: ko.observable(""),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        Rules: ko.observableArray([]),
        RouteParameterCollection: ko.observableArray([]),
        FormLayoutCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.EntityFormPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityFormPartial(model));
    }
    return model;
};



bespoke.sph.domain.WorkflowForm = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.WorkflowForm, domain.sph",
        Id: ko.observable("0"),
        WorkflowDefinitionId: ko.observable(""),
        Name: ko.observable(""),
        Route: ko.observable(""),
        Note: ko.observable(""),
        IsAllowedNewItem: ko.observable(false),
        IconClass: ko.observable(""),
        IconStoreId: ko.observable(""),
        Operation: ko.observable(""),
        Variable: ko.observable(""),
        Partial: ko.observable(""),
        Caption: ko.observable(""),
        Layout: ko.observable(""),
        OperationSuccessMesage: ko.observable(""),
        OperationSuccessNavigateUrl: ko.observable(""),
        OperationSuccessCallback: ko.observable(""),
        OperationFailureCallback: ko.observable(""),
        OperationMethod: ko.observable(""),
        IsPublished: ko.observable(false),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        Rules: ko.observableArray([]),
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


    if (bespoke.sph.domain.WorkflowFormPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowFormPartial(model));
    }
    return model;
};



bespoke.sph.domain.FormLayout = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FormLayout, domain.sph",
        Name: ko.observable(""),
        Position: ko.observable(""),
        IsForm: ko.observable(false),
        IsAuditTrail: ko.observable(false),
        Content: ko.observable(),
        XsmallCol: ko.observable(),
        MediumCol: ko.observable(),
        SmallCol: ko.observable(),
        LargeCol: ko.observable(),
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


    if (bespoke.sph.domain.FormLayoutPartial) {
        return _(model).extend(new bespoke.sph.domain.FormLayoutPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityView = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityView, domain.sph",
        Id: ko.observable("0"),
        IconClass: ko.observable(""),
        IconStoreId: ko.observable(""),
        EntityDefinitionId: ko.observable(""),
        Name: ko.observable(""),
        Route: ko.observable(""),
        Note: ko.observable(""),
        IsPublished: ko.observable(false),
        Visibilty: ko.observable(""),
        TileColour: ko.observable(""),
        CountMessage: ko.observable(""),
        Entity: ko.observable(""),
        Partial: ko.observable(""),
        Template: ko.observable(""),
        DisplayOnDashboard: ko.observable(false),
        Endpoint: ko.observable(""),
        ViewColumnCollection: ko.observableArray([]),
        ConditionalFormattingCollection: ko.observableArray([]),
        Performer: ko.observable(new bespoke.sph.domain.Performer()),
        RouteParameterCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.EntityViewPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityViewPartial(model));
    }
    return model;
};



bespoke.sph.domain.Filter = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Filter, domain.sph",
        Term: ko.observable(""),
        Operator: ko.observable('Operator'),
        Field: ko.observable(),
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


    if (bespoke.sph.domain.FilterPartial) {
        return _(model).extend(new bespoke.sph.domain.FilterPartial(model));
    }
    return model;
};



bespoke.sph.domain.ViewColumn = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ViewColumn, domain.sph",
        Path: ko.observable(""),
        Header: ko.observable(""),
        Sort: ko.observable(""),
        IsLinkColumn: ko.observable(false),
        FormRoute: ko.observable(""),
        IconCssClass: ko.observable(""),
        IconStoreId: ko.observable(""),
        Format: ko.observable(""),
        RouteValueField: ko.observable(""),
        ConditionalFormattingCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.ViewColumnPartial) {
        return _(model).extend(new bespoke.sph.domain.ViewColumnPartial(model));
    }
    return model;
};



bespoke.sph.domain.Sort = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Sort, domain.sph",
        Path: ko.observable(""),
        Direction: ko.observable('SortDirection'),
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


    if (bespoke.sph.domain.SortPartial) {
        return _(model).extend(new bespoke.sph.domain.SortPartial(model));
    }
    return model;
};



bespoke.sph.domain.ImageElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.IsThumbnail = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ImageElement, domain.sph";

    v.Width = ko.observable();//nillable
    v.Height = ko.observable();//nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ImageElementPartial) {
        return _(v).extend(new bespoke.sph.domain.ImageElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.DownloadLink = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.IsTransformTemplate = ko.observable(false);

    v.TemplateId = ko.observable(0);

    v.Entity = ko.observable("");

    v.IconClass = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DownloadLink, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.DownloadLinkPartial) {
        return _(v).extend(new bespoke.sph.domain.DownloadLinkPartial(v));
    }
    return v;
};



bespoke.sph.domain.FieldPermission = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FieldPermission, domain.sph",
        Role: ko.observable(""),
        IsHidden: ko.observable(false),
        IsReadOnly: ko.observable(false),
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


    if (bespoke.sph.domain.FieldPermissionPartial) {
        return _(model).extend(new bespoke.sph.domain.FieldPermissionPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityPermission = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityPermission, domain.sph",
        Role: ko.observable(""),
        IsHidden: ko.observable(false),
        IsReadOnly: ko.observable(false),
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


    if (bespoke.sph.domain.EntityPermissionPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityPermissionPartial(model));
    }
    return model;
};



bespoke.sph.domain.OperationEndpoint = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.OperationEndpoint, domain.sph",
        Name: ko.observable(""),
        Route: ko.observable(""),
        IsHttpPut: ko.observable(false),
        IsHttpPatch: ko.observable(false),
        IsHttpPost: ko.observable(false),
        IsHttpDelete: ko.observable(false),
        Note: ko.observable(""),
        Entity: ko.observable(""),
        Resource: ko.observable(""),
        IsPublished: ko.observable(false),
        IsConflictDetectionEnabled: ko.observable(false),
        EntityPermissionCollection: ko.observableArray([]),
        Rules: ko.observableArray([]),
        SetterActionChildCollection: ko.observableArray([]),
        PatchPathCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.OperationEndpointPartial) {
        return _(model).extend(new bespoke.sph.domain.OperationEndpointPartial(model));
    }
    return model;
};


// placeholder for SetterActionChild

bespoke.sph.domain.EntityChart = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityChart, domain.sph",
        Id: ko.observable("0"),
        EntityDefinitionId: ko.observable(""),
        Entity: ko.observable(""),
        Name: ko.observable(""),
        Type: ko.observable(""),
        EntityViewId: ko.observable(""),
        Query: ko.observable(""),
        Aggregate: ko.observable(""),
        Field: ko.observable(""),
        DateInterval: ko.observable(""),
        IsDashboardItem: ko.observable(false),
        DasboardItemPosition: ko.observable(0),
        HistogramInterval: ko.observable(),
        SeriesCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.EntityChartPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityChartPartial(model));
    }
    return model;
};



bespoke.sph.domain.Series = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Series, domain.sph",
        Name: ko.observable(""),
        Entity: ko.observable(""),
        Color: ko.observable(""),
        Query: ko.observable(""),
        Aggregate: ko.observable(""),
        Field: ko.observable(""),
        DateInterval: ko.observable(""),
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


    if (bespoke.sph.domain.SeriesPartial) {
        return _(model).extend(new bespoke.sph.domain.SeriesPartial(model));
    }
    return model;
};



bespoke.sph.domain.ConditionalFormatting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ConditionalFormatting, domain.sph",
        CssClass: ko.observable(""),
        Condition: ko.observable(""),
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


    if (bespoke.sph.domain.ConditionalFormattingPartial) {
        return _(model).extend(new bespoke.sph.domain.ConditionalFormattingPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityLookupElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Entity = ko.observable("");

    v.DisplayMemberPath = ko.observable("");

    v.ValueMemberPath = ko.observable("");

    v.DisplayTemplate = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.EntityLookupElement, domain.sph";

    v.LookupColumnCollection = ko.observableArray([]);
    v.FilterCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.EntityLookupElementPartial) {
        return _(v).extend(new bespoke.sph.domain.EntityLookupElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.CurrencyElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Currency = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.CurrencyElement, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CurrencyElementPartial) {
        return _(v).extend(new bespoke.sph.domain.CurrencyElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.RouteParameter = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.RouteParameter, domain.sph",
        Name: ko.observable(""),
        Type: ko.observable(""),
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


    if (bespoke.sph.domain.RouteParameterPartial) {
        return _(model).extend(new bespoke.sph.domain.RouteParameterPartial(model));
    }
    return model;
};



bespoke.sph.domain.PartialJs = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.PartialJs, domain.sph",
        Path: ko.observable(""),
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


    if (bespoke.sph.domain.PartialJsPartial) {
        return _(model).extend(new bespoke.sph.domain.PartialJsPartial(model));
    }
    return model;
};



bespoke.sph.domain.ViewTemplate = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ViewTemplate, domain.sph",
        Name: ko.observable(""),
        Note: ko.observable(""),
        ViewModelType: ko.observable(""),
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


    if (bespoke.sph.domain.ViewTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.ViewTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.PatchSetter = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.PatchSetter, domain.sph",
        Path: ko.observable(""),
        IsRequired: ko.observable(false),
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


    if (bespoke.sph.domain.PatchSetterPartial) {
        return _(model).extend(new bespoke.sph.domain.PatchSetterPartial(model));
    }
    return model;
};



bespoke.sph.domain.FormDialog = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FormDialog, domain.sph",
        Id: ko.observable("0"),
        Title: ko.observable(""),
        IsAllowCancel: ko.observable(false),
        Entity: ko.observable(""),
        MemberPath: ko.observable(""),
        Route: ko.observable(""),
        IsPublished: ko.observable(false),
        Note: ko.observable(""),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        DialogButtonCollection: ko.observableArray([]),
        Rules: ko.observableArray([]),
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


    if (bespoke.sph.domain.FormDialogPartial) {
        return _(model).extend(new bespoke.sph.domain.FormDialogPartial(model));
    }
    return model;
};



bespoke.sph.domain.DialogButton = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DialogButton, domain.sph",
        Text: ko.observable(""),
        IsDefault: ko.observable(false),
        IsCancel: ko.observable(false),
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


    if (bespoke.sph.domain.DialogButtonPartial) {
        return _(model).extend(new bespoke.sph.domain.DialogButtonPartial(model));
    }
    return model;
};



bespoke.sph.domain.PartialView = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.PartialView, domain.sph",
        Id: ko.observable("0"),
        Name: ko.observable(""),
        Route: ko.observable(""),
        Entity: ko.observable(""),
        MemberPath: ko.observable(""),
        IsPublished: ko.observable(false),
        Note: ko.observable(""),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
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


    if (bespoke.sph.domain.PartialViewPartial) {
        return _(model).extend(new bespoke.sph.domain.PartialViewPartial(model));
    }
    return model;
};



bespoke.sph.domain.ChildView = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ChildView, domain.sph",
        PartialView: ko.observable(""),
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


    if (bespoke.sph.domain.ChildViewPartial) {
        return _(model).extend(new bespoke.sph.domain.ChildViewPartial(model));
    }
    return model;
};



bespoke.sph.domain.TabControl = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.TabControl, domain.sph",
        PartialView: ko.observable(""),
        TabPanelCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.TabControlPartial) {
        return _(model).extend(new bespoke.sph.domain.TabControlPartial(model));
    }
    return model;
};



bespoke.sph.domain.TabPanel = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.TabPanel, domain.sph",
        Header: ko.observable(""),
        PartialView: ko.observable(""),
        Path: ko.observable(""),
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


    if (bespoke.sph.domain.TabPanelPartial) {
        return _(model).extend(new bespoke.sph.domain.TabPanelPartial(model));
    }
    return model;
};



bespoke.sph.domain.QueryEndpoint = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.QueryEndpoint, domain.sph",
        Id: ko.observable("0"),
        CacheProfile: ko.observable(""),
        Name: ko.observable(""),
        Route: ko.observable(""),
        IsReturnSource: ko.observable(""),
        Entity: ko.observable(""),
        Note: ko.observable(""),
        IsPublished: ko.observable(false),
        Resource: ko.observable(""),
        FilterCollection: ko.observableArray([]),
        SortCollection: ko.observableArray([]),
        RouteParameterCollection: ko.observableArray([]),
        MemberCollection: ko.observableArray([]),
        CacheFilter: ko.observable(),
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


    if (bespoke.sph.domain.QueryEndpointPartial) {
        return _(model).extend(new bespoke.sph.domain.QueryEndpointPartial(model));
    }
    return model;
};



bespoke.sph.domain.ServiceContract = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ServiceContract, domain.sph",
        FullSearchEndpoint: ko.observable(new bespoke.sph.domain.FullSearchEndpoint()),
        OdataEndpoint: ko.observable(new bespoke.sph.domain.OdataEndpoint()),
        EntityResourceEndpoint: ko.observable(new bespoke.sph.domain.EntityResourceEndpoint()),
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


    if (bespoke.sph.domain.ServiceContractPartial) {
        return _(model).extend(new bespoke.sph.domain.ServiceContractPartial(model));
    }
    return model;
};



bespoke.sph.domain.QueryEndpointSetting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.QueryEndpointSetting, domain.sph",
        CacheProfile: ko.observable(""),
        Note: ko.observable(""),
        Resource: ko.observable(""),
        Performer: ko.observable(new bespoke.sph.domain.Performer()),
        CacheFilter: ko.observable(),
        CachingSetting: ko.observable(new bespoke.sph.domain.CachingSetting()),
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


    if (bespoke.sph.domain.QueryEndpointSettingPartial) {
        return _(model).extend(new bespoke.sph.domain.QueryEndpointSettingPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityResourceEndpoint = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityResourceEndpoint, domain.sph",
        IsAllowed: ko.observable(false),
        FilterExpression: ko.observable(""),
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


    if (bespoke.sph.domain.EntityResourceEndpointPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityResourceEndpointPartial(model));
    }
    return model;
};



bespoke.sph.domain.FullSearchEndpoint = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FullSearchEndpoint, domain.sph",
        IsAllowed: ko.observable(false),
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


    if (bespoke.sph.domain.FullSearchEndpointPartial) {
        return _(model).extend(new bespoke.sph.domain.FullSearchEndpointPartial(model));
    }
    return model;
};



bespoke.sph.domain.OdataEndpoint = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.OdataEndpoint, domain.sph",
        IsAllowed: ko.observable(false),
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


    if (bespoke.sph.domain.OdataEndpointPartial) {
        return _(model).extend(new bespoke.sph.domain.OdataEndpointPartial(model));
    }
    return model;
};



bespoke.sph.domain.BusinessRuleEndpoint = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.BusinessRuleEndpoint, domain.sph",
        IsAllowed: ko.observable(false),
        Performer: ko.observable(new bespoke.sph.domain.Performer()),
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


    if (bespoke.sph.domain.BusinessRuleEndpointPartial) {
        return _(model).extend(new bespoke.sph.domain.BusinessRuleEndpointPartial(model));
    }
    return model;
};



bespoke.sph.domain.ServiceContractSetting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ServiceContractSetting, domain.sph",
        ResourceEndpointSetting: ko.observable(new bespoke.sph.domain.ResourceEndpointSetting()),
        OdataEndpoint: ko.observable(new bespoke.sph.domain.OdataEndpoint()),
        SearchEndpointSetting: ko.observable(new bespoke.sph.domain.SearchEndpointSetting()),
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


    if (bespoke.sph.domain.ServiceContractSettingPartial) {
        return _(model).extend(new bespoke.sph.domain.ServiceContractSettingPartial(model));
    }
    return model;
};



bespoke.sph.domain.ResourceEndpointSetting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ResourceEndpointSetting, domain.sph",
        CachingSetting: ko.observable(new bespoke.sph.domain.CachingSetting()),
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


    if (bespoke.sph.domain.ResourceEndpointSettingPartial) {
        return _(model).extend(new bespoke.sph.domain.ResourceEndpointSettingPartial(model));
    }
    return model;
};



bespoke.sph.domain.SearchEndpointSetting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.SearchEndpointSetting, domain.sph",
        CachingSetting: ko.observable(new bespoke.sph.domain.CachingSetting()),
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


    if (bespoke.sph.domain.SearchEndpointSettingPartial) {
        return _(model).extend(new bespoke.sph.domain.SearchEndpointSettingPartial(model));
    }
    return model;
};



bespoke.sph.domain.OdataEndpointSetting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.OdataEndpointSetting, domain.sph",
        CachingSetting: ko.observable(new bespoke.sph.domain.CachingSetting()),
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


    if (bespoke.sph.domain.OdataEndpointSettingPartial) {
        return _(model).extend(new bespoke.sph.domain.OdataEndpointSettingPartial(model));
    }
    return model;
};



bespoke.sph.domain.CachingSetting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.CachingSetting, domain.sph",
        CacheControl: ko.observable(""),
        NoStore: ko.observable(false),
        Expires: ko.observable(),
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


    if (bespoke.sph.domain.CachingSettingPartial) {
        return _(model).extend(new bespoke.sph.domain.CachingSettingPartial(model));
    }
    return model;
};


bespoke.sph.domain.FormElement = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FormElement, domain.sph",
        Name: ko.observable(""),
        Label: ko.observable(""),
        Tooltip: ko.observable(""),
        Path: ko.observable(""),
        IsRequired: ko.observable(false),
        Size: ko.observable(""),
        CssClass: ko.observable(""),
        Visible: ko.observable(""),
        Enable: ko.observable(""),
        ElementId: ko.observable(""),
        HelpText: ko.observable(""),
        UseDisplayTemplate: ko.observable(false),
        ToolboxIconClass: ko.observable(""),
        IsUniqueName: ko.observable(false),
        FieldValidation: ko.observable(new bespoke.sph.domain.FieldValidation()),
        LabelColLg: ko.observable(),
        LabelColMd: ko.observable(),
        LabelColSm: ko.observable(),
        LabelColXs: ko.observable(),
        InputColLg: ko.observable(),
        InputColMd: ko.observable(),
        InputColSm: ko.observable(),
        InputColXs: ko.observable(),
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

    if (bespoke.sph.domain.FormElementPartial) {
        return _(model).extend(new bespoke.sph.domain.FormElementPartial(model));
    }
    return model;
};


bespoke.sph.domain.Member = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Member, domain.sph",
        Name: ko.observable(""),
        AllowMultiple: ko.observable(false),
        MemberCollection: ko.observableArray([]),
        FieldPermissionCollection: ko.observableArray([]),
        DefaultValue: ko.observable(),
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

    if (bespoke.sph.domain.MemberPartial) {
        return _(model).extend(new bespoke.sph.domain.MemberPartial(model));
    }
    return model;
};


bespoke.sph.domain.Field = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Field, domain.sph",
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
        return _(model).extend(new bespoke.sph.domain.FieldPartial(model));
    }
    return model;
};


// placeholder for Operatorenum

bespoke.sph.domain.SortDirection = function () {
    return {
        ASC: 'Asc',
        DESC: 'Desc',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();


bespoke.sph.domain.OwnerType = function () {
    return {
        USER: 'User',
        EVERYONE: 'Everyone',
        ROLE: 'Role',
        DESIGNATION: 'Designation',
        DEPARTMENT: 'Department',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();



/// <reference path="~/Scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ReportDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportDefinition, domain.sph",
        Id: ko.observable("0"),
        Title: ko.observable(''),
        Category: ko.observable(''),
        IsActive: ko.observable(false),
        IsPrivate: ko.observable(false),
        IsExportAllowed: ko.observable(false),
        Description: ko.observable(''),
        ReportLayoutCollection: ko.observableArray([]),
        DataSource: ko.observable(new bespoke.sph.domain.DataSource()),
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


    if (bespoke.sph.domain.ReportDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.ReportDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.ReportLayout = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportLayout, domain.sph",
        Name: ko.observable(''),
        Row: ko.observable(0),
        Column: ko.observable(0),
        ColumnSpan: ko.observable(0),
        ReportItemCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.ReportLayoutPartial) {
        return _(model).extend(new bespoke.sph.domain.ReportLayoutPartial(model));
    }
    return model;
};



bespoke.sph.domain.BarChartItem = function (optionOrWebid) {

    var v = new bespoke.sph.domain.ReportItem(optionOrWebid);

    v.ValueLabelFormat = ko.observable('');

    v.HorizontalAxisField = ko.observable('');

    v.Title = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.BarChartItem, domain.sph";

    v.ChartSeriesCollection = ko.observableArray([]);

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


    if (bespoke.sph.domain.BarChartItemPartial) {
        return _(v).extend(new bespoke.sph.domain.BarChartItemPartial(v));
    }
    return v;
};



bespoke.sph.domain.LineChartItem = function (optionOrWebid) {

    var v = new bespoke.sph.domain.ReportItem(optionOrWebid);

    v.ValueLabelFormat = ko.observable('');

    v.HorizontalAxisField = ko.observable('');

    v.Title = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.LineChartItem, domain.sph";

    v.ChartSeriesCollection = ko.observableArray([]);

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


    if (bespoke.sph.domain.LineChartItemPartial) {
        return _(v).extend(new bespoke.sph.domain.LineChartItemPartial(v));
    }
    return v;
};



bespoke.sph.domain.PieChartItem = function (optionOrWebid) {

    var v = new bespoke.sph.domain.ReportItem(optionOrWebid);

    v.CategoryField = ko.observable('');

    v.ValueField = ko.observable('');

    v.Title = ko.observable('');

    v.TitlePlacement = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.PieChartItem, domain.sph";


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


    if (bespoke.sph.domain.PieChartItemPartial) {
        return _(v).extend(new bespoke.sph.domain.PieChartItemPartial(v));
    }
    return v;
};



bespoke.sph.domain.DataGridItem = function (optionOrWebid) {

    var v = new bespoke.sph.domain.ReportItem(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.DataGridItem, domain.sph";

    v.ReportRowCollection = ko.observableArray([]);
    v.DataGridColumnCollection = ko.observableArray([]);
    v.DataGridGroupDefinitionCollection = ko.observableArray([]);

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


    if (bespoke.sph.domain.DataGridItemPartial) {
        return _(v).extend(new bespoke.sph.domain.DataGridItemPartial(v));
    }
    return v;
};



bespoke.sph.domain.LabelItem = function (optionOrWebid) {

    var v = new bespoke.sph.domain.ReportItem(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.LabelItem, domain.sph";

    v.Html = ko.observable();//type but not nillable

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


    if (bespoke.sph.domain.LabelItemPartial) {
        return _(v).extend(new bespoke.sph.domain.LabelItemPartial(v));
    }
    return v;
};



bespoke.sph.domain.LineItem = function (optionOrWebid) {

    var v = new bespoke.sph.domain.ReportItem(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.LineItem, domain.sph";


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


    if (bespoke.sph.domain.LineItemPartial) {
        return _(v).extend(new bespoke.sph.domain.LineItemPartial(v));
    }
    return v;
};



bespoke.sph.domain.DataSource = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DataSource, domain.sph",
        EntityName: ko.observable(''),
        Query: ko.observable(''),
        ParameterCollection: ko.observableArray([]),
        ReportFilterCollection: ko.observableArray([]),
        EntityFieldCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.DataSourcePartial) {
        return _(model).extend(new bespoke.sph.domain.DataSourcePartial(model));
    }
    return model;
};



bespoke.sph.domain.Parameter = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Parameter, domain.sph",
        Name: ko.observable(''),
        TypeName: ko.observable(''),
        AvailableValues: ko.observable(''),
        Label: ko.observable(''),
        IsNullable: ko.observable(false),
        Value: ko.observable(),
        DefaultValue: ko.observable(),
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


    if (bespoke.sph.domain.ParameterPartial) {
        return _(model).extend(new bespoke.sph.domain.ParameterPartial(model));
    }
    return model;
};



bespoke.sph.domain.ReportFilter = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportFilter, domain.sph",
        FieldName: ko.observable(''),
        Operator: ko.observable(''),
        Value: ko.observable(''),
        TypeName: ko.observable(''),
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


    if (bespoke.sph.domain.ReportFilterPartial) {
        return _(model).extend(new bespoke.sph.domain.ReportFilterPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityField = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityField, domain.sph",
        Name: ko.observable(''),
        TypeName: ko.observable(''),
        IsNullable: ko.observable(false),
        Aggregate: ko.observable(''),
        Order: ko.observable(''),
        OrderPosition: ko.observable(0),
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


    if (bespoke.sph.domain.EntityFieldPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityFieldPartial(model));
    }
    return model;
};



bespoke.sph.domain.DataGridColumn = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DataGridColumn, domain.sph",
        Header: ko.observable(''),
        Width: ko.observable(''),
        Expression: ko.observable(''),
        Format: ko.observable(''),
        Action: ko.observable(''),
        FooterExpression: ko.observable(''),
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


    if (bespoke.sph.domain.DataGridColumnPartial) {
        return _(model).extend(new bespoke.sph.domain.DataGridColumnPartial(model));
    }
    return model;
};



bespoke.sph.domain.ReportColumn = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportColumn, domain.sph",
        Name: ko.observable(''),
        Header: ko.observable(''),
        Width: ko.observable(''),
        IsSelected: ko.observable(false),
        TypeName: ko.observable(''),
        IsFilterable: ko.observable(false),
        IsCustomField: ko.observable(false),
        IsNullable: ko.observable(false),
        Value: ko.observable(),
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


    if (bespoke.sph.domain.ReportColumnPartial) {
        return _(model).extend(new bespoke.sph.domain.ReportColumnPartial(model));
    }
    return model;
};



bespoke.sph.domain.ReportRow = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportRow, domain.sph",
        ReportColumnCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.ReportRowPartial) {
        return _(model).extend(new bespoke.sph.domain.ReportRowPartial(model));
    }
    return model;
};



bespoke.sph.domain.DailySchedule = function (optionOrWebid) {

    var v = new bespoke.sph.domain.IntervalSchedule(optionOrWebid);

    v.Recur = ko.observable(0);

    v["$type"] = "Bespoke.Sph.Domain.DailySchedule, domain.sph";


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


    if (bespoke.sph.domain.DailySchedulePartial) {
        return _(v).extend(new bespoke.sph.domain.DailySchedulePartial(v));
    }
    return v;
};



bespoke.sph.domain.WeeklySchedule = function (optionOrWebid) {

    var v = new bespoke.sph.domain.IntervalSchedule(optionOrWebid);

    v.Hour = ko.observable(0);

    v.Minute = ko.observable(0);

    v.IsSunday = ko.observable(false);

    v.IsMonday = ko.observable(false);

    v.IsTuesday = ko.observable(false);

    v.IsWednesday = ko.observable(false);

    v.IsThursday = ko.observable(false);

    v.IsFriday = ko.observable(false);

    v.IsSaturday = ko.observable(false);

    v.Recur = ko.observable(0);

    v["$type"] = "Bespoke.Sph.Domain.WeeklySchedule, domain.sph";


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


    if (bespoke.sph.domain.WeeklySchedulePartial) {
        return _(v).extend(new bespoke.sph.domain.WeeklySchedulePartial(v));
    }
    return v;
};



bespoke.sph.domain.MonthlySchedule = function (optionOrWebid) {

    var v = new bespoke.sph.domain.IntervalSchedule(optionOrWebid);

    v.Day = ko.observable(0);

    v.Hour = ko.observable(0);

    v.Minute = ko.observable(0);

    v.IsJanuary = ko.observable(false);

    v.IsFebruary = ko.observable(false);

    v.IsMarch = ko.observable(false);

    v.IsApril = ko.observable(false);

    v.IsMay = ko.observable(false);

    v.IsJune = ko.observable(false);

    v.IsJuly = ko.observable(false);

    v.IsAugust = ko.observable(false);

    v.IsSeptember = ko.observable(false);

    v.IsOctober = ko.observable(false);

    v.IsNovember = ko.observable(false);

    v.IsDecember = ko.observable(false);

    v.IsLastDay = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.MonthlySchedule, domain.sph";

    v.Days = ko.observableArray([]);

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


    if (bespoke.sph.domain.MonthlySchedulePartial) {
        return _(v).extend(new bespoke.sph.domain.MonthlySchedulePartial(v));
    }
    return v;
};



bespoke.sph.domain.ReportDelivery = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportDelivery, domain.sph",
        Id: ko.observable("0"),
        IsActive: ko.observable(false),
        Title: ko.observable(''),
        Description: ko.observable(''),
        ReportDefinitionId: ko.observable(''),
        IntervalScheduleCollection: ko.observableArray([]),
        Users: ko.observableArray([]),
        Departments: ko.observableArray([]),
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


    if (bespoke.sph.domain.ReportDeliveryPartial) {
        return _(model).extend(new bespoke.sph.domain.ReportDeliveryPartial(model));
    }
    return model;
};



bespoke.sph.domain.ReportContent = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportContent, domain.sph",
        ReportDefinitionId: ko.observable(''),
        ReportDeliveryId: ko.observable(''),
        HtmlOutput: ko.observable(),
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


    if (bespoke.sph.domain.ReportContentPartial) {
        return _(model).extend(new bespoke.sph.domain.ReportContentPartial(model));
    }
    return model;
};



bespoke.sph.domain.ChartSeries = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ChartSeries, domain.sph",
        Header: ko.observable(''),
        Column: ko.observable(''),
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


    if (bespoke.sph.domain.ChartSeriesPartial) {
        return _(model).extend(new bespoke.sph.domain.ChartSeriesPartial(model));
    }
    return model;
};



bespoke.sph.domain.DataGridGroupDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DataGridGroupDefinition, domain.sph",
        Column: ko.observable(''),
        Expression: ko.observable(''),
        Style: ko.observable(''),
        FooterExpression: ko.observable(''),
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


    if (bespoke.sph.domain.DataGridGroupDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.DataGridGroupDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.DataGridGroup = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DataGridGroup, domain.sph",
        Column: ko.observable(''),
        Text: ko.observable(''),
        ReportRowCollection: ko.observableArray([]),
        DataGridGroupCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.DataGridGroupPartial) {
        return _(model).extend(new bespoke.sph.domain.DataGridGroupPartial(model));
    }
    return model;
};


bespoke.sph.domain.ReportItem = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportItem, domain.sph",
        Name: ko.observable(''),
        CssClass: ko.observable(''),
        Visible: ko.observable(''),
        Tooltip: ko.observable(''),
        Icon: ko.observable(''),
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

    if (bespoke.sph.domain.ReportItemPartial) {
        return _(model).extend(new bespoke.sph.domain.ReportItemPartial(model));
    }
    return model;
};


bespoke.sph.domain.IntervalSchedule = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.IntervalSchedule, domain.sph",
        IsEnabled: ko.observable(false),
        Start: ko.observable(moment().format('DD/MM/YYYY')),
        Expire: ko.observable(),
        Delay: ko.observable(),
        Repeat: ko.observable(),
        Duration: ko.observable(),
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

    if (bespoke.sph.domain.IntervalSchedulePartial) {
        return _(model).extend(new bespoke.sph.domain.IntervalSchedulePartial(model));
    }
    return model;
};



/// <reference path="~/scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.LatLng = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.LatLng, domain.sph",
        Lat: ko.observable(0.00),
        Lng: ko.observable(0.00),
        Elevation: ko.observable(),
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


    if (bespoke.sph.domain.LatLngPartial) {
        return _(model).extend(new bespoke.sph.domain.LatLngPartial(model));
    }
    return model;
};



bespoke.sph.domain.Document = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Document, domain.sph",
        Title: ko.observable(""),
        Extension: ko.observable(""),
        DocumentVersionCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.DocumentPartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentPartial(model));
    }
    return model;
};



bespoke.sph.domain.DocumentVersion = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DocumentVersion, domain.sph",
        StoreId: ko.observable(""),
        Date: ko.observable(moment().format("DD/MM/YYYY'")),
        CommitedBy: ko.observable(""),
        No: ko.observable(""),
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


    if (bespoke.sph.domain.DocumentVersionPartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentVersionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Owner = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Owner, domain.sph",
        Name: ko.observable(""),
        TelephoneNo: ko.observable(""),
        FaxNo: ko.observable(""),
        Email: ko.observable(""),
        Address: ko.observable(new bespoke.sph.domain.Address()),
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


    if (bespoke.sph.domain.OwnerPartial) {
        return _(model).extend(new bespoke.sph.domain.OwnerPartial(model));
    }
    return model;
};



bespoke.sph.domain.AuditTrail = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.AuditTrail, domain.sph",
        Id: ko.observable("0"),
        User: ko.observable(""),
        DateTime: ko.observable(moment().format("DD/MM/YYYY'")),
        Operation: ko.observable(""),
        Type: ko.observable(""),
        EntityId: ko.observable(""),
        ChangeCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.AuditTrailPartial) {
        return _(model).extend(new bespoke.sph.domain.AuditTrailPartial(model));
    }
    return model;
};



bespoke.sph.domain.Change = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Change, domain.sph",
        PropertyName: ko.observable(""),
        OldValue: ko.observable(""),
        NewValue: ko.observable(""),
        Action: ko.observable(""),
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


    if (bespoke.sph.domain.ChangePartial) {
        return _(model).extend(new bespoke.sph.domain.ChangePartial(model));
    }
    return model;
};



bespoke.sph.domain.Organization = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Organization, domain.sph",
        Id: ko.observable("0"),
        Name: ko.observable(""),
        RegistrationNo: ko.observable(""),
        Email: ko.observable(""),
        OfficeNo: ko.observable(""),
        FaxNo: ko.observable(""),
        Address: ko.observable(new bespoke.sph.domain.Address()),
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


    if (bespoke.sph.domain.OrganizationPartial) {
        return _(model).extend(new bespoke.sph.domain.OrganizationPartial(model));
    }
    return model;
};



bespoke.sph.domain.UserProfile = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.UserProfile, domain.sph",
        Id: ko.observable("0"),
        UserName: ko.observable(""),
        FullName: ko.observable(""),
        Designation: ko.observable(""),
        Telephone: ko.observable(""),
        Mobile: ko.observable(""),
        RoleTypes: ko.observable(""),
        StartModule: ko.observable(""),
        Email: ko.observable(""),
        Department: ko.observable(""),
        HasChangedDefaultPassword: ko.observable(false),
        Language: ko.observable(""),
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


    if (bespoke.sph.domain.UserProfilePartial) {
        return _(model).extend(new bespoke.sph.domain.UserProfilePartial(model));
    }
    return model;
};



bespoke.sph.domain.Setting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Setting, domain.sph",
        Id: ko.observable("0"),
        UserName: ko.observable(""),
        Key: ko.observable(),
        Value: ko.observable(),
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


    if (bespoke.sph.domain.SettingPartial) {
        return _(model).extend(new bespoke.sph.domain.SettingPartial(model));
    }
    return model;
};



bespoke.sph.domain.Designation = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Designation, domain.sph",
        Id: ko.observable("0"),
        Name: ko.observable(""),
        Description: ko.observable(""),
        IsActive: ko.observable(false),
        StartModule: ko.observable(""),
        EnforceStartModule: ko.observable(false),
        IsSearchVisible: ko.observable(false),
        IsMessageVisible: ko.observable(false),
        IsHelpVisible: ko.observable(false),
        HelpUri: ko.observable(""),
        Title: ko.observable(""),
        Option: ko.observable(0),
        RoleCollection: ko.observableArray([]),
        Owner: ko.observable(new bespoke.sph.domain.Owner()),
        SearchableEntityCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.DesignationPartial) {
        return _(model).extend(new bespoke.sph.domain.DesignationPartial(model));
    }
    return model;
};



bespoke.sph.domain.Watcher = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Watcher, domain.sph",
        Id: ko.observable("0"),
        EntityName: ko.observable(""),
        EntityId: ko.observable(""),
        User: ko.observable(""),
        IsActive: ko.observable(false),
        DateTime: ko.observable(moment().format("DD/MM/YYYY'")),
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


    if (bespoke.sph.domain.WatcherPartial) {
        return _(model).extend(new bespoke.sph.domain.WatcherPartial(model));
    }
    return model;
};



bespoke.sph.domain.Profile = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Profile, domain.sph",
        FullName: ko.observable(""),
        UserName: ko.observable(""),
        Email: ko.observable(""),
        Password: ko.observable(""),
        ConfirmPassword: ko.observable(""),
        Status: ko.observable(""),
        Designation: ko.observable(""),
        Telephone: ko.observable(""),
        Mobile: ko.observable(""),
        IsNew: ko.observable(false),
        Department: ko.observable(""),
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


    if (bespoke.sph.domain.ProfilePartial) {
        return _(model).extend(new bespoke.sph.domain.ProfilePartial(model));
    }
    return model;
};



bespoke.sph.domain.Message = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Message, domain.sph",
        Id: ko.observable("0"),
        Subject: ko.observable(""),
        IsRead: ko.observable(false),
        Body: ko.observable(""),
        UserName: ko.observable(""),
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


    if (bespoke.sph.domain.MessagePartial) {
        return _(model).extend(new bespoke.sph.domain.MessagePartial(model));
    }
    return model;
};



bespoke.sph.domain.Photo = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Photo, domain.sph",
        Title: ko.observable(""),
        Description: ko.observable(""),
        StoreId: ko.observable(""),
        ThumbnailStoreId: ko.observable(""),
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


    if (bespoke.sph.domain.PhotoPartial) {
        return _(model).extend(new bespoke.sph.domain.PhotoPartial(model));
    }
    return model;
};



bespoke.sph.domain.Address = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Address, domain.sph",
        UnitNo: ko.observable(""),
        Floor: ko.observable(""),
        Block: ko.observable(""),
        Street: ko.observable(""),
        City: ko.observable(""),
        Postcode: ko.observable(""),
        State: ko.observable(""),
        Country: ko.observable(""),
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


    if (bespoke.sph.domain.AddressPartial) {
        return _(model).extend(new bespoke.sph.domain.AddressPartial(model));
    }
    return model;
};



bespoke.sph.domain.EmailTemplate = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EmailTemplate, domain.sph",
        Id: ko.observable("0"),
        Entity: ko.observable(""),
        Name: ko.observable(""),
        Note: ko.observable(""),
        SubjectTemplate: ko.observable(""),
        BodyTemplate: ko.observable(""),
        IsPublished: ko.observable(false),
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


    if (bespoke.sph.domain.EmailTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.EmailTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.DocumentTemplate = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DocumentTemplate, domain.sph",
        Id: ko.observable("0"),
        Name: ko.observable(""),
        Note: ko.observable(""),
        WordTemplateStoreId: ko.observable(""),
        IsPublished: ko.observable(false),
        Entity: ko.observable(""),
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


    if (bespoke.sph.domain.DocumentTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.QuotaPolicy = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.QuotaPolicy, domain.sph",
        Name: ko.observable(""),
        RateLimit: ko.observable(new bespoke.sph.domain.RateLimit()),
        QuotaLimit: ko.observable(new bespoke.sph.domain.QuotaLimit()),
        BandwidthLimit: ko.observable(new bespoke.sph.domain.BandwidthLimit()),
        EndpointLimitCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.QuotaPolicyPartial) {
        return _(model).extend(new bespoke.sph.domain.QuotaPolicyPartial(model));
    }
    return model;
};



bespoke.sph.domain.RateLimit = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.RateLimit, domain.sph",
        IsUnlimited: ko.observable(false),
        Calls: ko.observable(),
        RenewalPeriod: ko.observable(),
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


    if (bespoke.sph.domain.RateLimitPartial) {
        return _(model).extend(new bespoke.sph.domain.RateLimitPartial(model));
    }
    return model;
};



bespoke.sph.domain.QuotaLimit = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.QuotaLimit, domain.sph",
        IsUnlimited: ko.observable(false),
        Calls: ko.observable(),
        RenewalPeriod: ko.observable(),
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


    if (bespoke.sph.domain.QuotaLimitPartial) {
        return _(model).extend(new bespoke.sph.domain.QuotaLimitPartial(model));
    }
    return model;
};



bespoke.sph.domain.BandwidthLimit = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.BandwidthLimit, domain.sph",
        IsUnlimited: ko.observable(false),
        Size: ko.observable(),
        RenewalPeriod: ko.observable(),
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


    if (bespoke.sph.domain.BandwidthLimitPartial) {
        return _(model).extend(new bespoke.sph.domain.BandwidthLimitPartial(model));
    }
    return model;
};



bespoke.sph.domain.EndpointLimit = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EndpointLimit, domain.sph",
        Controller: ko.observable(""),
        Action: ko.observable(""),
        Parent: ko.observable(""),
        Note: ko.observable(""),
        Calls: ko.observable(),
        RenewalPeriod: ko.observable(),
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


    if (bespoke.sph.domain.EndpointLimitPartial) {
        return _(model).extend(new bespoke.sph.domain.EndpointLimitPartial(model));
    }
    return model;
};


bespoke.sph.domain.TimePeriod = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.TimePeriod, domain.sph",
        Count: ko.observable(0),
        Unit: ko.observable(""),
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

    if (bespoke.sph.domain.TimePeriodPartial) {
        return _(model).extend(new bespoke.sph.domain.TimePeriodPartial(model));
    }
    return model;
};



/// <reference path="~/scripts/knockout-3.4.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

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
        RuleCollection: ko.observableArray([]),
        ActionCollection: ko.observableArray([]),
        ReferencedAssemblyCollection: ko.observableArray([]),
        RequeueFilterCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.TriggerPartial) {
        return _(model).extend(new bespoke.sph.domain.TriggerPartial(model));
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

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.Expression = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.JavascriptExpressionField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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



bespoke.sph.domain.RouteParameterField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Expression = ko.observable("");

    v.DefaultValue = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.RouteParameterField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.RouteParameterFieldPartial) {
        return _(v).extend(new bespoke.sph.domain.RouteParameterFieldPartial(v));
    }
    return v;
};



bespoke.sph.domain.FunctionField = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Field(optionOrWebid);

    v.Script = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.FunctionField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.TypeName = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ConstantField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.XPath = ko.observable("");

    v.NamespacePrefix = ko.observable("");

    v.TypeName = ko.observable("");

    v.Path = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DocumentField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.Path = ko.observable("");

    v.TypeName = ko.observable("");

    v.OldValue = ko.observable("");

    v.NewValue = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.PropertyChangedField, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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


    if (bespoke.sph.domain.RulePartial) {
        return _(model).extend(new bespoke.sph.domain.RulePartial(model));
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


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
        Path: ko.observable(""),
        Field: ko.observable(),
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


    if (bespoke.sph.domain.SetterActionChildPartial) {
        return _(model).extend(new bespoke.sph.domain.SetterActionChildPartial(model));
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


    if (bespoke.sph.domain.MethodArgPartial) {
        return _(model).extend(new bespoke.sph.domain.MethodArgPartial(model));
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

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
        VariablePath: ko.observable(""),
        Field: ko.observable(),
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


    if (bespoke.sph.domain.WorkflowTriggerMapPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowTriggerMapPartial(model));
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

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.AssemblyActionPartial) {
        return _(v).extend(new bespoke.sph.domain.AssemblyActionPartial(v));
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


    if (bespoke.sph.domain.WorkflowDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowDefinitionPartial(model));
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


    if (bespoke.sph.domain.WorkflowPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkflowPartial(model));
    }
    return model;
};



bespoke.sph.domain.DecisionActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.DecisionActivity, domain.sph";

    v.DecisionBranchCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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


    if (bespoke.sph.domain.VariableValuePartial) {
        return _(model).extend(new bespoke.sph.domain.VariableValuePartial(model));
    }
    return model;
};



bespoke.sph.domain.EndActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.IsTerminating = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.EndActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
        UserProperty: ko.observable(""),
        Value: ko.observable(""),
        IsPublic: ko.observable(false),
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.EntityType = ko.observable("");

    v.ReturnValuePath = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.CreateEntityActivity, domain.sph";

    v.PropertyMappingCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.EntityType = ko.observable("");

    v.EntityIdPath = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DeleteEntityActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.Entity = ko.observable("");

    v.EntityIdPath = ko.observable("");

    v.UseVariable = ko.observable("");

    v.IsUsingVariable = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.UpdateEntityActivity, domain.sph";

    v.PropertyMappingCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.Expression = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ScriptFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
        Type: ko.observable(""),
        Value: ko.observable(""),
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


    if (bespoke.sph.domain.ConfirmationOptionsPartial) {
        return _(model).extend(new bespoke.sph.domain.ConfirmationOptionsPartial(model));
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

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.Placeholder = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.JoinActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.Expression = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DelayActivity, domain.sph";

    v.Miliseconds = ko.observable();//type but not nillable
    v.Seconds = ko.observable();//type but not nillable
    v.Hour = ko.observable();//type but not nillable
    v.Days = ko.observable();//type but not nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.Message = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ThrowActivity, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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



bespoke.sph.domain.ValueObjectVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v.ValueObjectDefinition = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ValueObjectVariable, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ValueObjectVariablePartial) {
        return _(v).extend(new bespoke.sph.domain.ValueObjectVariablePartial(v));
    }
    return v;
};



bespoke.sph.domain.ClrTypeVariable = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Variable(optionOrWebid);

    v.Assembly = ko.observable("");

    v.CanInitiateWithDefaultConstructor = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ClrTypeVariable, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
        Id: ko.observable("0"),
        WorkflowId: ko.observable(""),
        WorkflowDefinitionId: ko.observable(""),
        ForbiddenActivities: ko.observableArray([]),
        ExecutedActivityCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.TrackerPartial) {
        return _(model).extend(new bespoke.sph.domain.TrackerPartial(model));
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


    if (bespoke.sph.domain.ExecutedActivityPartial) {
        return _(model).extend(new bespoke.sph.domain.ExecutedActivityPartial(model));
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


    if (bespoke.sph.domain.BreakpointPartial) {
        return _(model).extend(new bespoke.sph.domain.BreakpointPartial(model));
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


    if (bespoke.sph.domain.ReferencedAssemblyPartial) {
        return _(model).extend(new bespoke.sph.domain.ReferencedAssemblyPartial(model));
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

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
        TypeName: ko.observable(""),
        Variable: ko.observable(""),
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


    if (bespoke.sph.domain.MappingSourcePartial) {
        return _(model).extend(new bespoke.sph.domain.MappingSourcePartial(model));
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


    if (bespoke.sph.domain.TransformDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.TransformDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.DirectMap = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Map(optionOrWebid);

    v.Source = ko.observable("");

    v.TypeName = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.DirectMap, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.__uuid = ko.observable("");

    v.Functoid = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.FunctoidMap, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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



bespoke.sph.domain.ParseBooleanFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Format = ko.observable("");

    v.SourceField = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseBooleanFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseBooleanFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseBooleanFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.ParseDoubleFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Styles = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseDoubleFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseDoubleFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseDoubleFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.ParseDecimalFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Styles = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseDecimalFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseDecimalFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseDecimalFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.ParseInt32Functoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Styles = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseInt32Functoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseInt32FunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseInt32FunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.ParseDateTimeFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Format = ko.observable("");

    v.Styles = ko.observable("");

    v.Culture = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ParseDateTimeFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ParseDateTimeFunctoidPartial) {
        return _(v).extend(new bespoke.sph.domain.ParseDateTimeFunctoidPartial(v));
    }
    return v;
};



bespoke.sph.domain.FormattingFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.Format = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.FormattingFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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


    if (bespoke.sph.domain.FunctoidArgPartial) {
        return _(model).extend(new bespoke.sph.domain.FunctoidArgPartial(model));
    }
    return model;
};



bespoke.sph.domain.ConstantFunctoid = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Functoid(optionOrWebid);

    v.TypeName = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.ConstantFunctoid, domain.sph";

    v.Value = ko.observable();//type but not nillable

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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

    v.Field = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.SourceFunctoid, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
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


    if (bespoke.sph.domain.ExceptionFilterPartial) {
        return _(model).extend(new bespoke.sph.domain.ExceptionFilterPartial(model));
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


    if (bespoke.sph.domain.CorrelationTypePartial) {
        return _(model).extend(new bespoke.sph.domain.CorrelationTypePartial(model));
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


    if (bespoke.sph.domain.CorrelationSetPartial) {
        return _(model).extend(new bespoke.sph.domain.CorrelationSetPartial(model));
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


    if (bespoke.sph.domain.CorrelationPropertyPartial) {
        return _(model).extend(new bespoke.sph.domain.CorrelationPropertyPartial(model));
    }
    return model;
};



bespoke.sph.domain.ChildWorkflowActivity = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Activity(optionOrWebid);

    v.WorkflowDefinitionId = ko.observable("");

    v.Version = ko.observable(0);

    v.IsAsync = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ChildWorkflowActivity, domain.sph";

    v.PropertyMappingCollection = ko.observableArray([]);
    v.ExecutedPropertyMappingCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.ChildWorkflowActivityPartial) {
        return _(v).extend(new bespoke.sph.domain.ChildWorkflowActivityPartial(v));
    }
    return v;
};



bespoke.sph.domain.TryScope = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Scope(optionOrWebid);

    v["$type"] = "Bespoke.Sph.Domain.TryScope, domain.sph";

    v.CatchScopeCollection = ko.observableArray([]);

    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.TryScopePartial) {
        return _(v).extend(new bespoke.sph.domain.TryScopePartial(v));
    }
    return v;
};



bespoke.sph.domain.CatchScope = function (optionOrWebid) {

    var v = new bespoke.sph.domain.Scope(optionOrWebid);

    v.ExceptionType = ko.observable("");

    v.ExceptionVar = ko.observable("");

    v["$type"] = "Bespoke.Sph.Domain.CatchScope, domain.sph";


    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof v[n] === "function") {
                    v[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        v.WebId(optionOrWebid);
    }


    if (bespoke.sph.domain.CatchScopePartial) {
        return _(v).extend(new bespoke.sph.domain.CatchScopePartial(v));
    }
    return v;
};



bespoke.sph.domain.ReceivePortDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReceivePortDefinition, domain.sph",
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


    if (bespoke.sph.domain.ReceivePortDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.ReceivePortDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.ReceivePort = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReceivePort, domain.sph",
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


    if (bespoke.sph.domain.ReceivePortPartial) {
        return _(model).extend(new bespoke.sph.domain.ReceivePortPartial(model));
    }
    return model;
};



bespoke.sph.domain.SendPort = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.SendPort, domain.sph",
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


    if (bespoke.sph.domain.SendPortPartial) {
        return _(model).extend(new bespoke.sph.domain.SendPortPartial(model));
    }
    return model;
};



bespoke.sph.domain.SendPortDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.SendPortDefinition, domain.sph",
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


    if (bespoke.sph.domain.SendPortDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.SendPortDefinitionPartial(model));
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


    if (bespoke.sph.domain.DataTransferDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.DataTransferDefinitionPartial(model));
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


    if (bespoke.sph.domain.ScheduledDataTransferPartial) {
        return _(model).extend(new bespoke.sph.domain.ScheduledDataTransferPartial(model));
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
        return _(model).extend(new bespoke.sph.domain.FieldPartial(model));
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
        return _(model).extend(new bespoke.sph.domain.CustomActionPartial(model));
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
        return _(model).extend(new bespoke.sph.domain.ActivityPartial(model));
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
        return _(model).extend(new bespoke.sph.domain.VariablePartial(model));
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
        return _(model).extend(new bespoke.sph.domain.PropertyMappingPartial(model));
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
        return _(model).extend(new bespoke.sph.domain.FunctoidPartial(model));
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
        return _(model).extend(new bespoke.sph.domain.MapPartial(model));
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
        return _(model).extend(new bespoke.sph.domain.ScopePartial(model));
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

