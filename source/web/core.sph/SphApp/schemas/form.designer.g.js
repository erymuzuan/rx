﻿
/// <reference path="~/scripts/knockout-3.1.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.FormDesign = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FormDesign, domain.sph",
        Name: ko.observable(''),
        Description: ko.observable(''),
        ConfirmationText: ko.observable(''),
        ImageStoreId: ko.observable(''),
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
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
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

    v.DefaultValue = ko.observable('');

    v.AutoCompletionEntity = ko.observable('');

    v.AutoCompletionField = ko.observable('');

    v.AutoCompletionQuery = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.TextBox, domain.sph";


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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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

    v.Rows = ko.observable('');

    v.IsHtml = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.TextAreaElement, domain.sph";


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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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

    v.Icon = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.MapElement, domain.sph";


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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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
        Caption: ko.observable(''),
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

    v.BlockOptionsPath = ko.observable('');

    v.FloorOptionsPath = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.AddressElement, domain.sph";


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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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
        PropertyName: ko.observable(''),
        TypeName: ko.observable(''),
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


    if (bespoke.sph.domain.DefaultValuePartial) {
        return _(model).extend(new bespoke.sph.domain.DefaultValuePartial(model));
    }
    return model;
};



bespoke.sph.domain.FieldValidation = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FieldValidation, domain.sph",
        IsRequired: ko.observable(false),
        Pattern: ko.observable(''),
        Mode: ko.observable(''),
        Message: ko.observable(''),
        Min: ko.observable(),
        Max: ko.observable(),
        MinLength: ko.observable(),
        MaxLength: ko.observable(),
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


    if (bespoke.sph.domain.FieldValidationPartial) {
        return _(model).extend(new bespoke.sph.domain.FieldValidationPartial(model));
    }
    return model;
};


// placeholder for Performer

bespoke.sph.domain.BusinessRule = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.BusinessRule, domain.sph",
        Description: ko.observable(''),
        Name: ko.observable(''),
        ErrorLocation: ko.observable(''),
        ErrorMessage: ko.observable(''),
        RuleCollection: ko.observableArray([]),
        FilterCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.BusinessRulePartial) {
        return _(model).extend(new bespoke.sph.domain.BusinessRulePartial(model));
    }
    return model;
};


// placeholder for Rule

bespoke.sph.domain.FileUploadElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.AllowedExtensions = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.FileUploadElement, domain.sph";


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


    if (bespoke.sph.domain.FileUploadElementPartial) {
        return _(v).extend(new bespoke.sph.domain.FileUploadElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComboBoxLookup = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ComboBoxLookup, domain.sph",
        Entity: ko.observable(''),
        ValuePath: ko.observable(''),
        DisplayPath: ko.observable(''),
        Query: ko.observable(''),
        IsComputedQuery: ko.observable(false),
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


    if (bespoke.sph.domain.ComboBoxLookupPartial) {
        return _(model).extend(new bespoke.sph.domain.ComboBoxLookupPartial(model));
    }
    return model;
};



bespoke.sph.domain.ChildEntityListView = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Entity = ko.observable('');

    v.Query = ko.observable('');

    v.IsAllowAddItem = ko.observable(false);

    v.NewItemFormRoute = ko.observable('');

    v.NewItemMappingSource = ko.observable('');

    v.NewItemMappingDestination = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.ChildEntityListView, domain.sph";

    v.ViewColumnCollection = ko.observableArray([]);
    v.SortCollection = ko.observableArray([]);
    v.ConditionalFormattingCollection = ko.observableArray([]);

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


    if (bespoke.sph.domain.ChildEntityListViewPartial) {
        return _(v).extend(new bespoke.sph.domain.ChildEntityListViewPartial(v));
    }
    return v;
};



bespoke.sph.domain.ListView = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.ChildItemType = ko.observable('');

    v.IsChildItemFunction = ko.observable(false);

    v["$type"] = "Bespoke.Sph.Domain.ListView, domain.sph";

    v.ListViewColumnCollection = ko.observableArray([]);

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


    if (bespoke.sph.domain.ListViewPartial) {
        return _(v).extend(new bespoke.sph.domain.ListViewPartial(v));
    }
    return v;
};



bespoke.sph.domain.ListViewColumn = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ListViewColumn, domain.sph",
        Label: ko.observable(''),
        Path: ko.observable(''),
        Input: ko.observable(),
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


    if (bespoke.sph.domain.ListViewColumnPartial) {
        return _(model).extend(new bespoke.sph.domain.ListViewColumnPartial(model));
    }
    return model;
};



bespoke.sph.domain.Button = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Command = ko.observable('');

    v.UseClick = ko.observable(false);

    v.CommandName = ko.observable('');

    v.LoadingText = ko.observable('');

    v.CompleteText = ko.observable('');

    v.IconClass = ko.observable('');

    v.IsToolbarItem = ko.observable(false);

    v.Operation = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.Button, domain.sph";


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


    if (bespoke.sph.domain.ButtonPartial) {
        return _(v).extend(new bespoke.sph.domain.ButtonPartial(v));
    }
    return v;
};



bespoke.sph.domain.EntityDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityDefinition, domain.sph",
        EntityDefinitionId: ko.observable(0),
        Name: ko.observable(''),
        Plural: ko.observable(''),
        IconStoreId: ko.observable(''),
        IconClass: ko.observable(''),
        RecordName: ko.observable(''),
        IsPublished: ko.observable(false),
        IsShowOnNavigationBar: ko.observable(false),
        MemberCollection: ko.observableArray([]),
        BusinessRuleCollection: ko.observableArray([]),
        EntityOperationCollection: ko.observableArray([]),
        AuthorizedRoleCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.EntityDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Member = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Member, domain.sph",
        Name: ko.observable(''),
        TypeName: ko.observable(''),
        IsNullable: ko.observable(false),
        IsNotIndexed: ko.observable(false),
        IsAnalyzed: ko.observable(false),
        IsFilterable: ko.observable(false),
        IsExcludeInAll: ko.observable(false),
        Boost: ko.observable(0),
        MemberCollection: ko.observableArray([]),
        FieldPermissionCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.MemberPartial) {
        return _(model).extend(new bespoke.sph.domain.MemberPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityForm = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityForm, domain.sph",
        EntityFormId: ko.observable(0),
        EntityDefinitionId: ko.observable(0),
        Name: ko.observable(''),
        Route: ko.observable(''),
        Note: ko.observable(''),
        IsAllowedNewItem: ko.observable(false),
        IconClass: ko.observable(''),
        IconStoreId: ko.observable(''),
        IsPublished: ko.observable(false),
        IsDefault: ko.observable(false),
        IsWatchAvailable: ko.observable(false),
        IsEmailAvailable: ko.observable(false),
        IsPrintAvailable: ko.observable(false),
        IsAuditTrailAvailable: ko.observable(false),
        IsRemoveAvailable: ko.observable(false),
        IsImportAvailable: ko.observable(false),
        IsExportAvailable: ko.observable(false),
        Operation: ko.observable(''),
        Entity: ko.observable(''),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        Rules: ko.observableArray([]),
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


    if (bespoke.sph.domain.EntityFormPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityFormPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityView = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityView, domain.sph",
        EntityViewId: ko.observable(0),
        IconClass: ko.observable(''),
        IconStoreId: ko.observable(''),
        EntityDefinitionId: ko.observable(0),
        Name: ko.observable(''),
        Route: ko.observable(''),
        Note: ko.observable(''),
        Query: ko.observable(''),
        IsPublished: ko.observable(false),
        Visibilty: ko.observable(''),
        TileColour: ko.observable(''),
        CountMessage: ko.observable(''),
        Entity: ko.observable(''),
        FilterCollection: ko.observableArray([]),
        ViewColumnCollection: ko.observableArray([]),
        SortCollection: ko.observableArray([]),
        ConditionalFormattingCollection: ko.observableArray([]),
        Performer: ko.observable(new bespoke.sph.domain.Performer()),
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


    if (bespoke.sph.domain.EntityViewPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityViewPartial(model));
    }
    return model;
};



bespoke.sph.domain.Filter = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Filter, domain.sph",
        Term: ko.observable(''),
        Operator: ko.observable('Operator'),
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


    if (bespoke.sph.domain.FilterPartial) {
        return _(model).extend(new bespoke.sph.domain.FilterPartial(model));
    }
    return model;
};



bespoke.sph.domain.ViewColumn = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ViewColumn, domain.sph",
        Path: ko.observable(''),
        Header: ko.observable(''),
        Sort: ko.observable(''),
        IsLinkColumn: ko.observable(false),
        FormRoute: ko.observable(''),
        IconCssClass: ko.observable(''),
        IconStoreId: ko.observable(''),
        Format: ko.observable(''),
        ConditionalFormattingCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.ViewColumnPartial) {
        return _(model).extend(new bespoke.sph.domain.ViewColumnPartial(model));
    }
    return model;
};



bespoke.sph.domain.Sort = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Sort, domain.sph",
        Path: ko.observable(''),
        Direction: ko.observable('SortDirection'),
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
            if (typeof v[n] === "function") {
                v[n](optionOrWebid[n]);
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

    v.Entity = ko.observable('');

    v.IconClass = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.DownloadLink, domain.sph";


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


    if (bespoke.sph.domain.DownloadLinkPartial) {
        return _(v).extend(new bespoke.sph.domain.DownloadLinkPartial(v));
    }
    return v;
};



bespoke.sph.domain.FieldPermission = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FieldPermission, domain.sph",
        Role: ko.observable(''),
        IsHidden: ko.observable(false),
        IsReadOnly: ko.observable(false),
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


    if (bespoke.sph.domain.FieldPermissionPartial) {
        return _(model).extend(new bespoke.sph.domain.FieldPermissionPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityPermission = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityPermission, domain.sph",
        Role: ko.observable(''),
        IsHidden: ko.observable(false),
        IsReadOnly: ko.observable(false),
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


    if (bespoke.sph.domain.EntityPermissionPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityPermissionPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityOperation = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityOperation, domain.sph",
        Name: ko.observable(''),
        SuccessMessage: ko.observable(''),
        ShowSuccessMessage: ko.observable(false),
        NavigateSuccessUrl: ko.observable(''),
        SuccessCommand: ko.observable(''),
        EntityPermissionCollection: ko.observableArray([]),
        Rules: ko.observableArray([]),
        Permissions: ko.observableArray([]),
        SetterActionChildCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.EntityOperationPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityOperationPartial(model));
    }
    return model;
};


// placeholder for SetterActionChild

bespoke.sph.domain.EntityChart = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EntityChart, domain.sph",
        EntityChartId: ko.observable(0),
        EntityDefinitionId: ko.observable(0),
        Entity: ko.observable(''),
        Name: ko.observable(''),
        Type: ko.observable(''),
        EntityViewId: ko.observable(0),
        Query: ko.observable(''),
        Aggregate: ko.observable(''),
        Field: ko.observable(''),
        DateInterval: ko.observable(''),
        IsDashboardItem: ko.observable(false),
        DasboardItemPosition: ko.observable(0),
        HistogramInterval: ko.observable(),
        SeriesCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.EntityChartPartial) {
        return _(model).extend(new bespoke.sph.domain.EntityChartPartial(model));
    }
    return model;
};



bespoke.sph.domain.Series = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Series, domain.sph",
        Name: ko.observable(''),
        Entity: ko.observable(''),
        Color: ko.observable(''),
        Query: ko.observable(''),
        Aggregate: ko.observable(''),
        Field: ko.observable(''),
        DateInterval: ko.observable(''),
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


    if (bespoke.sph.domain.SeriesPartial) {
        return _(model).extend(new bespoke.sph.domain.SeriesPartial(model));
    }
    return model;
};



bespoke.sph.domain.SearchDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.SearchDefinition, domain.sph",
        SearchDefinitionId: ko.observable(0),
        Entity: ko.observable(''),
        Name: ko.observable(''),
        IsPublished: ko.observable(false),
        Route: ko.observable(''),
        OwnerType: ko.observable('OwnerType'),
        Owner: ko.observable(''),
        FilterCollection: ko.observableArray([]),
        ViewColumnCollection: ko.observableArray([]),
        SortCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.SearchDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.SearchDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.ConditionalFormatting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ConditionalFormatting, domain.sph",
        CssClass: ko.observable(''),
        Condition: ko.observable(''),
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


    if (bespoke.sph.domain.ConditionalFormattingPartial) {
        return _(model).extend(new bespoke.sph.domain.ConditionalFormattingPartial(model));
    }
    return model;
};



bespoke.sph.domain.EntityLookupElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Entity = ko.observable('');

    v.DisplayMemberPath = ko.observable('');

    v.ValueMemberPath = ko.observable('');

    v.DisplayTemplate = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.EntityLookupElement, domain.sph";

    v.LookupColumnCollection = ko.observableArray([]);

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


    if (bespoke.sph.domain.EntityLookupElementPartial) {
        return _(v).extend(new bespoke.sph.domain.EntityLookupElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.CurrencyElement = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.Currency = ko.observable('');

    v["$type"] = "Bespoke.Sph.Domain.CurrencyElement, domain.sph";


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


    if (bespoke.sph.domain.CurrencyElementPartial) {
        return _(v).extend(new bespoke.sph.domain.CurrencyElementPartial(v));
    }
    return v;
};


bespoke.sph.domain.FormElement = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FormElement, domain.sph",
        Name: ko.observable(''),
        Label: ko.observable(''),
        Tooltip: ko.observable(''),
        Path: ko.observable(''),
        IsRequired: ko.observable(false),
        Size: ko.observable(''),
        CssClass: ko.observable(''),
        Visible: ko.observable(''),
        Enable: ko.observable(''),
        ElementId: ko.observable(''),
        HelpText: ko.observable(''),
        UseDisplayTemplate: ko.observable(false),
        ToolboxIconClass: ko.observable(''),
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
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
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


bespoke.sph.domain.Field = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Field, domain.sph",
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

