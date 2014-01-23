///#source 1 1 /SphApp/schemas/form.designer.g.js

/// <reference path="~/scripts/knockout-3.0.0.debug.js" />
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

    v.MinLength = ko.observable();//nillable
    v.MaxLength = ko.observable();//nillable

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
        MaxLength: ko.observable(0),
        MinLength: ko.observable(0),
        Pattern: ko.observable(''),
        Mode: ko.observable(''),
        Message: ko.observable(''),
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



bespoke.sph.domain.ListView = function (optionOrWebid) {

    var v = new bespoke.sph.domain.FormElement(optionOrWebid);

    v.ChildItemType = ko.observable('');
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
        MemberCollection: ko.observableArray([]),
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
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
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
        Visibilty: ko.observable(''),
        FilterCollection: ko.observableArray([]),
        ViewColumnCollection: ko.observableArray([]),
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
        Field: ko.observable(''),
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


///#source 1 1 /SphApp/schemas/report.builder.g.js

/// <reference path="~/scripts/knockout-3.0.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ReportDefinition = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ReportDefinition, domain.sph",
        ReportDefinitionId: ko.observable(0),
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
        ReportDeliveryId: ko.observable(0),
        IsActive: ko.observable(false),
        Title: ko.observable(''),
        Description: ko.observable(''),
        ReportDefinitionId: ko.observable(0),
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
        ReportContentId: ko.observable(0),
        ReportDefinitionId: ko.observable(0),
        ReportDeliveryId: ko.observable(0),
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


///#source 1 1 /SphApp/schemas/sph.domain.g.js

/// <reference path="~/scripts/knockout-3.0.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.ContractHistory = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ContractHistory, domain.sph",
        ContractNo: ko.observable(''),
        DateFrom: ko.observable(moment().format('DD/MM/YYYY')),
        DateStart: ko.observable(moment().format('DD/MM/YYYY')),
        Name: ko.observable(''),
        DateEnd: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ContractHistoryPartial) {
        return _(model).extend(new bespoke.sph.domain.ContractHistoryPartial(model));
    }
    return model;
};



bespoke.sph.domain.Tenant = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Tenant, domain.sph",
        TenantId: ko.observable(0),
        IdSsmNo: ko.observable(''),
        Name: ko.observable(''),
        BussinessType: ko.observable(''),
        Phone: ko.observable(''),
        Fax: ko.observable(''),
        MobilePhone: ko.observable(''),
        Email: ko.observable(''),
        RegistrationNo: ko.observable(''),
        Username: ko.observable(''),
        ContractHistoryCollection: ko.observableArray([]),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        CustomFieldValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.TenantPartial) {
        return _(model).extend(new bespoke.sph.domain.TenantPartial(model));
    }
    return model;
};



bespoke.sph.domain.PropertyScheduleAndInterest = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.PropertyScheduleAndInterest, domain.sph",
        Mukim: ko.observable(''),
        LotNo: ko.observable(''),
        TypeAndOwnershipNo: ko.observable(''),
        LandShare: ko.observable(''),
        LeasingRegistrationNo: ko.observable(''),
        MortgageRegistrationNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.PropertyScheduleAndInterestPartial) {
        return _(model).extend(new bespoke.sph.domain.PropertyScheduleAndInterestPartial(model));
    }
    return model;
};



bespoke.sph.domain.RealProperty = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.RealProperty, domain.sph",
        LandId: ko.observable(0),
        Size: ko.observable(0.00),
        Mukim: ko.observable(''),
        LotNo: ko.observable(''),
        TypeAndOwnershipNo: ko.observable(''),
        District: ko.observable(''),
        WakafSize: ko.observable(''),
        WakafType: ko.observable(''),
        WakafPurposed: ko.observable(''),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        Contact: ko.observable(new bespoke.sph.domain.Contact()),
        PropertyScheduleAndInterestCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.RealPropertyPartial) {
        return _(model).extend(new bespoke.sph.domain.RealPropertyPartial(model));
    }
    return model;
};



bespoke.sph.domain.AssetProperty = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.AssetProperty, domain.sph",
        LandId: ko.observable(0),
        PropertyType: ko.observable(''),
        Location: ko.observable(''),
        WakafType: ko.observable(''),
        WakafPurposed: ko.observable(''),
        Model: ko.observable(''),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        Contact: ko.observable(new bespoke.sph.domain.Contact()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.AssetPropertyPartial) {
        return _(model).extend(new bespoke.sph.domain.AssetPropertyPartial(model));
    }
    return model;
};



bespoke.sph.domain.Witness = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Witness, domain.sph",
        Contact: ko.observable(new bespoke.sph.domain.Contact()),
        FirstWitnessSignature: ko.observable(),
        SecondWitnessSignature: ko.observable(),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.WitnessPartial) {
        return _(model).extend(new bespoke.sph.domain.WitnessPartial(model));
    }
    return model;
};



bespoke.sph.domain.RegistrationOfficer = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.RegistrationOfficer, domain.sph",
        Name: ko.observable(''),
        IcNo: ko.observable(''),
        ApplicationReceiveDate: ko.observable(moment().format('DD/MM/YYYY')),
        IsApplicationTrue: ko.observable(false),
        Note: ko.observable(''),
        Signature: ko.observable(''),
        SignatureDate: ko.observable(moment().format('DD/MM/YYYY')),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.RegistrationOfficerPartial) {
        return _(model).extend(new bespoke.sph.domain.RegistrationOfficerPartial(model));
    }
    return model;
};



bespoke.sph.domain.Investigation = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Investigation, domain.sph",
        InvestigationDate: ko.observable(moment().format('DD/MM/YYYY')),
        Time: ko.observable(''),
        Purposed: ko.observable(''),
        InvestigationReport: ko.observable(''),
        ReportDate: ko.observable(moment().format('DD/MM/YYYY')),
        PreparedBy: ko.observable(''),
        HeadUnitReview: ko.observable(''),
        HeadUnitReviewDate: ko.observable(moment().format('DD/MM/YYYY')),
        PsuReview: ko.observable(''),
        PsuReviewDate: ko.observable(moment().format('DD/MM/YYYY')),
        InvestigatorCollection: ko.observableArray([]),
        PresenterPartyCollection: ko.observableArray([]),
        PhotoCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.InvestigationPartial) {
        return _(model).extend(new bespoke.sph.domain.InvestigationPartial(model));
    }
    return model;
};



bespoke.sph.domain.Investigator = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Investigator, domain.sph",
        Name: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.InvestigatorPartial) {
        return _(model).extend(new bespoke.sph.domain.InvestigatorPartial(model));
    }
    return model;
};



bespoke.sph.domain.PresenterParty = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.PresenterParty, domain.sph",
        Name: ko.observable(''),
        Designation: ko.observable(''),
        TelephoneNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.PresenterPartyPartial) {
        return _(model).extend(new bespoke.sph.domain.PresenterPartyPartial(model));
    }
    return model;
};



bespoke.sph.domain.AggrementPronouncement = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.AggrementPronouncement, domain.sph",
        ApplicantName: ko.observable(''),
        ApplicantIcNo: ko.observable(''),
        ReceiverName: ko.observable(''),
        PronouncementDate: ko.observable(moment().format('DD/MM/YYYY')),
        Signature: ko.observable(''),
        Purpose: ko.observable(''),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.AggrementPronouncementPartial) {
        return _(model).extend(new bespoke.sph.domain.AggrementPronouncementPartial(model));
    }
    return model;
};



bespoke.sph.domain.Land = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Land, domain.sph",
        LandId: ko.observable(0),
        Unit: ko.observable(''),
        Title: ko.observable(''),
        Location: ko.observable(''),
        Size: ko.observable(0.00),
        SizeUnit: ko.observable(''),
        RezabNo: ko.observable(''),
        SheetNo: ko.observable(''),
        ApprovedPlanNo: ko.observable(''),
        LandOffice: ko.observable(''),
        Usage: ko.observable(''),
        Note: ko.observable(''),
        OwnLevel: ko.observable(''),
        PlanNo: ko.observable(''),
        Status: ko.observable(''),
        IsApproved: ko.observable(false),
        District: ko.observable(''),
        Mukim: ko.observable(''),
        LotNo: ko.observable(''),
        StaticTerm: ko.observable(''),
        FileNo: ko.observable(''),
        Type: ko.observable(''),
        PropertyValue: ko.observable(0.00),
        Owner: ko.observable(new bespoke.sph.domain.Owner()),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        LeaseExpiryDate: ko.observable(),
        LeasePeriod: ko.observable(),
        ApprovedDateTime: ko.observable(),
        ApprovedBy: ko.observable(),
        PhotoCollection: ko.observableArray([]),
        AssetProperty: ko.observable(new bespoke.sph.domain.AssetProperty()),
        RealProperty: ko.observable(new bespoke.sph.domain.RealProperty()),
        AttachmentCollection: ko.observableArray([]),
        Witness: ko.observable(new bespoke.sph.domain.Witness()),
        RegistrationOfficer: ko.observable(new bespoke.sph.domain.RegistrationOfficer()),
        Investigation: ko.observable(new bespoke.sph.domain.Investigation()),
        AggrementPronouncement: ko.observable(new bespoke.sph.domain.AggrementPronouncement()),
        OwnershipCollection: ko.observableArray([]),
        MarketEvaluationCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.LandPartial) {
        return _(model).extend(new bespoke.sph.domain.LandPartial(model));
    }
    return model;
};



bespoke.sph.domain.Building = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Building, domain.sph",
        TemplateId: ko.observable(0),
        TemplateName: ko.observable(''),
        BuildingId: ko.observable(0),
        Type: ko.observable(''),
        Name: ko.observable(''),
        UnitNo: ko.observable(''),
        BuildingSize: ko.observable(0.00),
        Status: ko.observable(''),
        Floors: ko.observable(0),
        Note: ko.observable(''),
        BuildingType: ko.observable(''),
        Blocks: ko.observable(0),
        FloorSize: ko.observable(0.00),
        BuildingRegistrationNo: ko.observable(''),
        LandRegistrationNo: ko.observable(''),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        FloorCollection: ko.observableArray([]),
        Height: ko.observable(),
        CustomFieldValueCollection: ko.observableArray([]),
        BlockCollection: ko.observableArray([]),
        CustomListValueCollection: ko.observableArray([]),
        CompletedDateTime: ko.observable(),
        BuiltDateTime: ko.observable(),
        MarketEvaluationCollection: ko.observableArray([]),
        Cost: ko.observable(),
        HandingOverDateTime: ko.observable(),
        RegisteredDateTime: ko.observable(),
        PhotoCollection: ko.observableArray([]),
        MovingInDateTime: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.BuildingPartial) {
        return _(model).extend(new bespoke.sph.domain.BuildingPartial(model));
    }
    return model;
};



bespoke.sph.domain.Block = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Block, domain.sph",
        Name: ko.observable(''),
        Description: ko.observable(''),
        Size: ko.observable(0.00),
        FloorPlanStoreId: ko.observable(''),
        FloorCollection: ko.observableArray([]),
        Floors: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.BlockPartial) {
        return _(model).extend(new bespoke.sph.domain.BlockPartial(model));
    }
    return model;
};



bespoke.sph.domain.LatLng = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.LatLng, domain.sph",
        Lat: ko.observable(0.00),
        Lng: ko.observable(0.00),
        Elevation: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.LatLngPartial) {
        return _(model).extend(new bespoke.sph.domain.LatLngPartial(model));
    }
    return model;
};



bespoke.sph.domain.Address = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Address, domain.sph",
        UnitNo: ko.observable(''),
        Floor: ko.observable(''),
        Block: ko.observable(''),
        Street: ko.observable(''),
        City: ko.observable(''),
        Postcode: ko.observable(''),
        State: ko.observable(''),
        Country: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.AddressPartial) {
        return _(model).extend(new bespoke.sph.domain.AddressPartial(model));
    }
    return model;
};



bespoke.sph.domain.Floor = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Floor, domain.sph",
        Name: ko.observable(''),
        Size: ko.observable(0.00),
        Number: ko.observable(''),
        Note: ko.observable(''),
        UnitCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.FloorPartial) {
        return _(model).extend(new bespoke.sph.domain.FloorPartial(model));
    }
    return model;
};



bespoke.sph.domain.Unit = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Unit, domain.sph",
        No: ko.observable(''),
        Size: ko.observable(0.00),
        FloorNo: ko.observable(''),
        BlockNo: ko.observable(''),
        IsSpace: ko.observable(false),
        Usage: ko.observable(''),
        FillOpacity: ko.observable(0.00),
        FillColor: ko.observable(''),
        PlanStoreId: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.UnitPartial) {
        return _(model).extend(new bespoke.sph.domain.UnitPartial(model));
    }
    return model;
};



bespoke.sph.domain.Space = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Space, domain.sph",
        SpaceId: ko.observable(0),
        TemplateId: ko.observable(0),
        TemplateName: ko.observable(''),
        BuildingId: ko.observable(0),
        UnitNo: ko.observable(''),
        FloorName: ko.observable(''),
        Size: ko.observable(0.00),
        Category: ko.observable(''),
        RentalType: ko.observable(''),
        IsOnline: ko.observable(false),
        RegistrationNo: ko.observable(''),
        IsAvailable: ko.observable(false),
        ContactPerson: ko.observable(''),
        ContactNo: ko.observable(''),
        State: ko.observable(''),
        City: ko.observable(''),
        BuildingName: ko.observable(''),
        BuildingUnit: ko.observable(''),
        RentalRate: ko.observable(0.00),
        Location: ko.observable(''),
        Description: ko.observable(''),
        MapIcon: ko.observable(''),
        SmallIcon: ko.observable(''),
        Icon: ko.observable(''),
        Furnishing: ko.observable(''),
        UnitCollection: ko.observableArray([]),
        CustomFieldValueCollection: ko.observableArray([]),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        PhotoCollection: ko.observableArray([]),
        CustomListValueCollection: ko.observableArray([]),
        FeatureDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.SpacePartial) {
        return _(model).extend(new bespoke.sph.domain.SpacePartial(model));
    }
    return model;
};



bespoke.sph.domain.RentalApplication = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.RentalApplication, domain.sph",
        RentalApplicationId: ko.observable(0),
        TemplateId: ko.observable(0),
        TemplateName: ko.observable(''),
        CompanyName: ko.observable(''),
        CompanyRegistrationNo: ko.observable(''),
        DateStart: ko.observable(moment().format('DD/MM/YYYY')),
        DateEnd: ko.observable(moment().format('DD/MM/YYYY')),
        Purpose: ko.observable(''),
        CompanyType: ko.observable(''),
        SpaceId: ko.observable(0),
        Status: ko.observable(''),
        Experience: ko.observable(''),
        IsRecordExist: ko.observable(false),
        PreviousAddress: ko.observable(''),
        IsCompany: ko.observable(false),
        Type: ko.observable(''),
        Remarks: ko.observable(''),
        RegistrationNo: ko.observable(''),
        ApplicationDate: ko.observable(moment().format('DD/MM/YYYY')),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        Contact: ko.observable(new bespoke.sph.domain.Contact()),
        AttachmentCollection: ko.observableArray([]),
        Offer: ko.observable(new bespoke.sph.domain.Offer()),
        Space: ko.observable(new bespoke.sph.domain.Space()),
        CustomFieldValueCollection: ko.observableArray([]),
        CustomListValueCollection: ko.observableArray([]),
        FeatureCollection: ko.observableArray([]),
        BankCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.RentalApplicationPartial) {
        return _(model).extend(new bespoke.sph.domain.RentalApplicationPartial(model));
    }
    return model;
};



bespoke.sph.domain.Attachment = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Attachment, domain.sph",
        Type: ko.observable(''),
        Name: ko.observable(''),
        IsRequired: ko.observable(false),
        IsReceived: ko.observable(false),
        StoreId: ko.observable(''),
        IsCompleted: ko.observable(false),
        ReceivedDateTime: ko.observable(),
        ReceivedBy: ko.observable(),
        Note: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.AttachmentPartial) {
        return _(model).extend(new bespoke.sph.domain.AttachmentPartial(model));
    }
    return model;
};



bespoke.sph.domain.Bank = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Bank, domain.sph",
        Name: ko.observable(''),
        Location: ko.observable(''),
        AccountNo: ko.observable(''),
        AccountType: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.BankPartial) {
        return _(model).extend(new bespoke.sph.domain.BankPartial(model));
    }
    return model;
};



bespoke.sph.domain.Contact = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Contact, domain.sph",
        ContractId: ko.observable(0),
        Name: ko.observable(''),
        MobileNo: ko.observable(''),
        OfficeNo: ko.observable(''),
        Email: ko.observable(''),
        IcNo: ko.observable(''),
        Title: ko.observable(''),
        Designation: ko.observable(''),
        Age: ko.observable(0),
        Job: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ContactPartial) {
        return _(model).extend(new bespoke.sph.domain.ContactPartial(model));
    }
    return model;
};



bespoke.sph.domain.ContractTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ContractTemplate, domain.sph",
        ContractTemplateId: ko.observable(0),
        Type: ko.observable(''),
        Description: ko.observable(''),
        Status: ko.observable(''),
        InterestRate: ko.observable(0.00),
        DocumentTemplateCollection: ko.observableArray([]),
        TopicCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ContractTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.ContractTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.DocumentTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DocumentTemplate, domain.sph",
        Name: ko.observable(''),
        StoreId: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.DocumentTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.Contract = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Contract, domain.sph",
        ContractId: ko.observable(0),
        ReferenceNo: ko.observable(''),
        Type: ko.observable(''),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Value: ko.observable(0.00),
        Title: ko.observable(''),
        Remarks: ko.observable(''),
        Period: ko.observable(0),
        PeriodUnit: ko.observable(''),
        StartDate: ko.observable(moment().format('DD/MM/YYYY')),
        EndDate: ko.observable(moment().format('DD/MM/YYYY')),
        RentalApplicationId: ko.observable(0),
        Status: ko.observable(''),
        RentType: ko.observable(''),
        InterestRate: ko.observable(0.00),
        IsEnd: ko.observable(false),
        DocumentCollection: ko.observableArray([]),
        Owner: ko.observable(new bespoke.sph.domain.Owner()),
        ContractingParty: ko.observable(new bespoke.sph.domain.ContractingParty()),
        Option: ko.observable(),
        Tenant: ko.observable(new bespoke.sph.domain.Tenant()),
        Space: ko.observable(new bespoke.sph.domain.Space()),
        TopicCollection: ko.observableArray([]),
        Termination: ko.observable(new bespoke.sph.domain.Termination()),
        Extension: ko.observable(new bespoke.sph.domain.Extension()),
        CustomFieldValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ContractPartial) {
        return _(model).extend(new bespoke.sph.domain.ContractPartial(model));
    }
    return model;
};



bespoke.sph.domain.Document = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Document, domain.sph",
        Title: ko.observable(''),
        Extension: ko.observable(''),
        DocumentVersionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.DocumentPartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentPartial(model));
    }
    return model;
};



bespoke.sph.domain.DocumentVersion = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DocumentVersion, domain.sph",
        StoreId: ko.observable(''),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        CommitedBy: ko.observable(''),
        No: ko.observable(''),
        Note: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.DocumentVersionPartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentVersionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Owner = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Owner, domain.sph",
        Name: ko.observable(''),
        TelephoneNo: ko.observable(''),
        FaxNo: ko.observable(''),
        Email: ko.observable(''),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.OwnerPartial) {
        return _(model).extend(new bespoke.sph.domain.OwnerPartial(model));
    }
    return model;
};



bespoke.sph.domain.AuditTrail = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.AuditTrail, domain.sph",
        User: ko.observable(''),
        DateTime: ko.observable(moment().format('DD/MM/YYYY')),
        Operation: ko.observable(''),
        Type: ko.observable(''),
        EntityId: ko.observable(0),
        ChangeCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.AuditTrailPartial) {
        return _(model).extend(new bespoke.sph.domain.AuditTrailPartial(model));
    }
    return model;
};



bespoke.sph.domain.Change = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Change, domain.sph",
        PropertyName: ko.observable(''),
        OldValue: ko.observable(''),
        NewValue: ko.observable(''),
        Action: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ChangePartial) {
        return _(model).extend(new bespoke.sph.domain.ChangePartial(model));
    }
    return model;
};



bespoke.sph.domain.Organization = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Organization, domain.sph",
        Name: ko.observable(''),
        RegistrationNo: ko.observable(''),
        Email: ko.observable(''),
        OfficeNo: ko.observable(''),
        FaxNo: ko.observable(''),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.OrganizationPartial) {
        return _(model).extend(new bespoke.sph.domain.OrganizationPartial(model));
    }
    return model;
};



bespoke.sph.domain.Offer = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Offer, domain.sph",
        BusinessPlan: ko.observable(''),
        SpaceId: ko.observable(0),
        Size: ko.observable(0),
        Building: ko.observable(''),
        Floor: ko.observable(''),
        Deposit: ko.observable(0.00),
        Rent: ko.observable(0.00),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        ExpiryDate: ko.observable(moment().format('DD/MM/YYYY')),
        Period: ko.observable(0),
        PeriodUnit: ko.observable(''),
        Option: ko.observable(0),
        OfferConditionCollection: ko.observableArray([]),
        BusinessPlanText: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.OfferPartial) {
        return _(model).extend(new bespoke.sph.domain.OfferPartial(model));
    }
    return model;
};



bespoke.sph.domain.OfferCondition = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.OfferCondition, domain.sph",
        Title: ko.observable(''),
        Description: ko.observable(''),
        Note: ko.observable(''),
        IsRequired: ko.observable(false),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.OfferConditionPartial) {
        return _(model).extend(new bespoke.sph.domain.OfferConditionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Topic = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Topic, domain.sph",
        Title: ko.observable(''),
        Description: ko.observable(''),
        Text: ko.observable(''),
        ClauseCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.TopicPartial) {
        return _(model).extend(new bespoke.sph.domain.TopicPartial(model));
    }
    return model;
};



bespoke.sph.domain.Clause = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Clause, domain.sph",
        Title: ko.observable(''),
        Description: ko.observable(''),
        No: ko.observable(''),
        Text: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ClausePartial) {
        return _(model).extend(new bespoke.sph.domain.ClausePartial(model));
    }
    return model;
};



bespoke.sph.domain.ContractingParty = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ContractingParty, domain.sph",
        Name: ko.observable(''),
        RegistrationNo: ko.observable(''),
        Contact: ko.observable(new bespoke.sph.domain.Contact()),
        Address: ko.observable(new bespoke.sph.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ContractingPartyPartial) {
        return _(model).extend(new bespoke.sph.domain.ContractingPartyPartial(model));
    }
    return model;
};



bespoke.sph.domain.DepositPayment = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DepositPayment, domain.sph",
        DepositPaymentId: ko.observable(0),
        ReceiptNo: ko.observable(''),
        Amount: ko.observable(0.00),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        RegistrationNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.DepositPaymentPartial) {
        return _(model).extend(new bespoke.sph.domain.DepositPaymentPartial(model));
    }
    return model;
};



bespoke.sph.domain.Payment = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Payment, domain.sph",
        PaymentId: ko.observable(0),
        Amount: ko.observable(0.00),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        ContractNo: ko.observable(''),
        TenantIdSsmNo: ko.observable(''),
        ReceiptNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.PaymentPartial) {
        return _(model).extend(new bespoke.sph.domain.PaymentPartial(model));
    }
    return model;
};



bespoke.sph.domain.UserProfile = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.UserProfile, domain.sph",
        Username: ko.observable(''),
        FullName: ko.observable(''),
        Designation: ko.observable(''),
        Telephone: ko.observable(''),
        Mobile: ko.observable(''),
        RoleTypes: ko.observable(''),
        StartModule: ko.observable(''),
        Email: ko.observable(''),
        UserProfileId: ko.observable(0),
        Department: ko.observable(''),
        HasChangedDefaultPassword: ko.observable(false),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.UserProfilePartial) {
        return _(model).extend(new bespoke.sph.domain.UserProfilePartial(model));
    }
    return model;
};



bespoke.sph.domain.Deposit = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Deposit, domain.sph",
        DepositId: ko.observable(0),
        DateTime: ko.observable(moment().format('DD/MM/YYYY')),
        Amount: ko.observable(0.00),
        IsPaid: ko.observable(false),
        IsRefund: ko.observable(false),
        IsVoid: ko.observable(false),
        PaymentDateTime: ko.observable(),
        RefundDateTime: ko.observable(),
        DueDate: ko.observable(),
        DepositPaymentCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.DepositPartial) {
        return _(model).extend(new bespoke.sph.domain.DepositPartial(model));
    }
    return model;
};



bespoke.sph.domain.Reply = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Reply, domain.sph",
        Title: ko.observable(''),
        Text: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ReplyPartial) {
        return _(model).extend(new bespoke.sph.domain.ReplyPartial(model));
    }
    return model;
};



bespoke.sph.domain.Setting = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Setting, domain.sph",
        SettingId: ko.observable(0),
        Username: ko.observable(''),
        Key: ko.observable(),
        Value: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.SettingPartial) {
        return _(model).extend(new bespoke.sph.domain.SettingPartial(model));
    }
    return model;
};



bespoke.sph.domain.Rebate = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Rebate, domain.sph",
        RebateId: ko.observable(0),
        ContractNo: ko.observable(''),
        Amount: ko.observable(0.00),
        StartDate: ko.observable(moment().format('DD/MM/YYYY')),
        EndDate: ko.observable(moment().format('DD/MM/YYYY')),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.RebatePartial) {
        return _(model).extend(new bespoke.sph.domain.RebatePartial(model));
    }
    return model;
};



bespoke.sph.domain.Interest = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Interest, domain.sph",
        Building: ko.observable(''),
        SpaceCategory: ko.observable(''),
        Percentage: ko.observable(0.00),
        Period: ko.observable(0),
        PeriodType: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.InterestPartial) {
        return _(model).extend(new bespoke.sph.domain.InterestPartial(model));
    }
    return model;
};



bespoke.sph.domain.Rent = function (webId) {

    var v = new bespoke.sph.domain.Invoice(webId);

    v.Half = ko.observable('');
    v.Year = ko.observable(0);
    v["$type"] = "Bespoke.Sph.Domain.Rent, domain.sph";

    v.Month = ko.observable();//nillable
    v.Quarter = ko.observable();//nillable
    v.Tenant = ko.observable(new bespoke.sph.domain.Tenant());
    if (bespoke.sph.domain.RentPartial) {
        return _(v).extend(new bespoke.sph.domain.RentPartial(v));
    }
    return v;
};



bespoke.sph.domain.AdhocInvoice = function (webId) {

    var v = new bespoke.sph.domain.Invoice(webId);

    v.Category = ko.observable('');
    v.F2 = ko.observable(0);
    v["$type"] = "Bespoke.Sph.Domain.AdhocInvoice, domain.sph";

    v.InvoiceItemCollection = ko.observableArray([]);
    v.DocumentCollection = ko.observableArray([]);
    v.SentDate = ko.observable();//nillable
    v.Tenant = ko.observable(new bespoke.sph.domain.Tenant());
    v.Type2 = ko.observable();//type but not nillable
    v.Note2 = ko.observable();//type but not nillable
    v.Address = ko.observable(new bespoke.sph.domain.Address());
    if (bespoke.sph.domain.AdhocInvoicePartial) {
        return _(v).extend(new bespoke.sph.domain.AdhocInvoicePartial(v));
    }
    return v;
};



bespoke.sph.domain.InvoiceItem = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.InvoiceItem, domain.sph",
        Amount: ko.observable(0.00),
        Category: ko.observable(''),
        Note: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.InvoiceItemPartial) {
        return _(model).extend(new bespoke.sph.domain.InvoiceItemPartial(model));
    }
    return model;
};



bespoke.sph.domain.State = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.State, domain.sph",
        Name: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.StatePartial) {
        return _(model).extend(new bespoke.sph.domain.StatePartial(model));
    }
    return model;
};



bespoke.sph.domain.Complaint = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Complaint, domain.sph",
        ComplaintId: ko.observable(0),
        TemplateId: ko.observable(0),
        Remarks: ko.observable(''),
        TenantId: ko.observable(0),
        Space: ko.observable(''),
        Status: ko.observable(''),
        Category: ko.observable(''),
        SubCategory: ko.observable(''),
        ReferenceNo: ko.observable(''),
        Type: ko.observable(''),
        AttachmentStoreId: ko.observable(''),
        Department: ko.observable(''),
        Note: ko.observable(''),
        CustomFieldValueCollection: ko.observableArray([]),
        Tenant: ko.observable(new bespoke.sph.domain.Tenant()),
        PhotoCollection: ko.observableArray([]),
        CustomListValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ComplaintPartial) {
        return _(model).extend(new bespoke.sph.domain.ComplaintPartial(model));
    }
    return model;
};



bespoke.sph.domain.CustomFieldValue = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.CustomFieldValue, domain.sph",
        Name: ko.observable(''),
        Type: ko.observable(''),
        Value: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.CustomFieldValuePartial) {
        return _(model).extend(new bespoke.sph.domain.CustomFieldValuePartial(model));
    }
    return model;
};



bespoke.sph.domain.Inspection = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Inspection, domain.sph",
        PersonInCharge: ko.observable(''),
        AssignedDate: ko.observable(moment().format('DD/MM/YYYY')),
        Remark: ko.observable(''),
        InspectionDate: ko.observable(moment().format('DD/MM/YYYY')),
        Resolution: ko.observable(''),
        Observation: ko.observable(''),
        Contractor: ko.observable(''),
        Priority: ko.observable(''),
        Severity: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.InspectionPartial) {
        return _(model).extend(new bespoke.sph.domain.InspectionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Maintenance = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Maintenance, domain.sph",
        MaintenanceId: ko.observable(0),
        ComplaintId: ko.observable(0),
        WorkOrderNo: ko.observable(''),
        Department: ko.observable(''),
        Status: ko.observable(''),
        Resolution: ko.observable(''),
        Officer: ko.observable(''),
        AttachmentStoreId: ko.observable(''),
        AttachmentName: ko.observable(''),
        WorkOrderType: ko.observable(''),
        TemplateId: ko.observable(0),
        Complaint: ko.observable(new bespoke.sph.domain.Complaint()),
        StartDate: ko.observable(),
        EndDate: ko.observable(),
        CustomFieldValueCollection: ko.observableArray([]),
        PhotoCollection: ko.observableArray([]),
        CustomListValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.MaintenancePartial) {
        return _(model).extend(new bespoke.sph.domain.MaintenancePartial(model));
    }
    return model;
};



bespoke.sph.domain.Designation = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Designation, domain.sph",
        DesignationId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        StartModule: ko.observable(''),
        RoleCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.DesignationPartial) {
        return _(model).extend(new bespoke.sph.domain.DesignationPartial(model));
    }
    return model;
};



bespoke.sph.domain.WorkOrder = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.WorkOrder, domain.sph",
        WorkOrderId: ko.observable(0),
        Priority: ko.observable(''),
        Severity: ko.observable(''),
        EstimationCost: ko.observable(0.00),
        No: ko.observable(''),
        MaintenanceId: ko.observable(0),
        TemplateId: ko.observable(0),
        Status: ko.observable(''),
        Resolution: ko.observable(''),
        Officer: ko.observable(''),
        Department: ko.observable(''),
        StartDate: ko.observable(moment().format('DD/MM/YYYY')),
        EndDate: ko.observable(moment().format('DD/MM/YYYY')),
        CommentCollection: ko.observableArray([]),
        PartsAndLaborCollection: ko.observableArray([]),
        WarrantyCollection: ko.observableArray([]),
        NonComplianceCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.WorkOrderPartial) {
        return _(model).extend(new bespoke.sph.domain.WorkOrderPartial(model));
    }
    return model;
};



bespoke.sph.domain.Comment = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Comment, domain.sph",
        Description: ko.observable(''),
        User: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.CommentPartial) {
        return _(model).extend(new bespoke.sph.domain.CommentPartial(model));
    }
    return model;
};



bespoke.sph.domain.PartsAndLabor = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.PartsAndLabor, domain.sph",
        Quantity: ko.observable(0),
        Description: ko.observable(''),
        Cost: ko.observable(0.00),
        Total: ko.observable(0.00),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.PartsAndLaborPartial) {
        return _(model).extend(new bespoke.sph.domain.PartsAndLaborPartial(model));
    }
    return model;
};



bespoke.sph.domain.Warranty = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Warranty, domain.sph",
        Description: ko.observable(''),
        YearWarranty: ko.observable(''),
        StartDate: ko.observable(moment().format('DD/MM/YYYY')),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.WarrantyPartial) {
        return _(model).extend(new bespoke.sph.domain.WarrantyPartial(model));
    }
    return model;
};



bespoke.sph.domain.NonCompliance = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.NonCompliance, domain.sph",
        Description: ko.observable(''),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Reason: ko.observable(''),
        Action: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.NonCompliancePartial) {
        return _(model).extend(new bespoke.sph.domain.NonCompliancePartial(model));
    }
    return model;
};



bespoke.sph.domain.Inventory = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Inventory, domain.sph",
        Name: ko.observable(''),
        Category: ko.observable(''),
        Brand: ko.observable(''),
        Specification: ko.observable(''),
        Quantity: ko.observable(0),
        InventoryId: ko.observable(0),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.InventoryPartial) {
        return _(model).extend(new bespoke.sph.domain.InventoryPartial(model));
    }
    return model;
};



bespoke.sph.domain.Watcher = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Watcher, domain.sph",
        WatcherId: ko.observable(0),
        EntityName: ko.observable(''),
        EntityId: ko.observable(0),
        User: ko.observable(''),
        IsActive: ko.observable(false),
        DateTime: ko.observable(moment().format('DD/MM/YYYY')),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.WatcherPartial) {
        return _(model).extend(new bespoke.sph.domain.WatcherPartial(model));
    }
    return model;
};



bespoke.sph.domain.Profile = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Profile, domain.sph",
        FullName: ko.observable(''),
        UserName: ko.observable(''),
        Email: ko.observable(''),
        Password: ko.observable(''),
        ConfirmPassword: ko.observable(''),
        Status: ko.observable(''),
        Designation: ko.observable(''),
        Telephone: ko.observable(''),
        Mobile: ko.observable(''),
        IsNew: ko.observable(false),
        Department: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ProfilePartial) {
        return _(model).extend(new bespoke.sph.domain.ProfilePartial(model));
    }
    return model;
};



bespoke.sph.domain.Termination = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Termination, domain.sph",
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Remarks: ko.observable(''),
        ApprovalOfficer: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.TerminationPartial) {
        return _(model).extend(new bespoke.sph.domain.TerminationPartial(model));
    }
    return model;
};



bespoke.sph.domain.Extension = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Extension, domain.sph",
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Remarks: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ExtensionPartial) {
        return _(model).extend(new bespoke.sph.domain.ExtensionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Message = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Message, domain.sph",
        MessageId: ko.observable(0),
        Subject: ko.observable(''),
        IsRead: ko.observable(false),
        Body: ko.observable(''),
        UserName: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.MessagePartial) {
        return _(model).extend(new bespoke.sph.domain.MessagePartial(model));
    }
    return model;
};



bespoke.sph.domain.CustomListValue = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.CustomListValue, domain.sph",
        Name: ko.observable(''),
        Label: ko.observable(''),
        MinOccurence: ko.observable(0),
        MaxOccurence: ko.observable(),
        CustomListRowCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.CustomListValuePartial) {
        return _(model).extend(new bespoke.sph.domain.CustomListValuePartial(model));
    }
    return model;
};



bespoke.sph.domain.CustomListRow = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.CustomListRow, domain.sph",
        CustomFieldValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.CustomListRowPartial) {
        return _(model).extend(new bespoke.sph.domain.CustomListRowPartial(model));
    }
    return model;
};



bespoke.sph.domain.Photo = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Photo, domain.sph",
        Title: ko.observable(''),
        Description: ko.observable(''),
        StoreId: ko.observable(''),
        ThumbnailStoreId: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.PhotoPartial) {
        return _(model).extend(new bespoke.sph.domain.PhotoPartial(model));
    }
    return model;
};



bespoke.sph.domain.MarketEvaluation = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.MarketEvaluation, domain.sph",
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Value: ko.observable(0.00),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.MarketEvaluationPartial) {
        return _(model).extend(new bespoke.sph.domain.MarketEvaluationPartial(model));
    }
    return model;
};



bespoke.sph.domain.Feature = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Feature, domain.sph",
        Name: ko.observable(''),
        Description: ko.observable(''),
        Category: ko.observable(''),
        IsRequired: ko.observable(false),
        Occurence: ko.observable(0),
        OccurenceTimeSpan: ko.observable(''),
        Quantity: ko.observable(0),
        PhotoCollection: ko.observableArray([]),
        Charge: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.FeaturePartial) {
        return _(model).extend(new bespoke.sph.domain.FeaturePartial(model));
    }
    return model;
};



bespoke.sph.domain.FeatureDefinition = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FeatureDefinition, domain.sph",
        Name: ko.observable(''),
        Description: ko.observable(''),
        Category: ko.observable(''),
        IsRequired: ko.observable(false),
        Occurence: ko.observable(0),
        OccurenceTimeSpan: ko.observable(''),
        AvailableQuantity: ko.observable(0),
        Charge: ko.observable(),
        PhotoCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.FeatureDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.FeatureDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Ownership = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Ownership, domain.sph",
        Description: ko.observable(''),
        OwnershipNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.OwnershipPartial) {
        return _(model).extend(new bespoke.sph.domain.OwnershipPartial(model));
    }
    return model;
};


bespoke.sph.domain.Invoice = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Invoice, domain.sph",
        InvoiceId: ko.observable(0),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Amount: ko.observable(0.00),
        InvoiceNo: ko.observable(''),
        Type: ko.observable('InvoiceType'),
        ContractNo: ko.observable(''),
        TenantIdSsmNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };

    if (bespoke.sph.domain.InvoicePartial) {
        return _(model).extend(new bespoke.sph.domain.InvoicePartial(model));
    }
    return model;
};


bespoke.sph.domain.InvoiceType = function () {
    return {
        ADHOC_INVOICE: 'AdhocInvoice',
        RENTAL: 'Rental',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();

///#source 1 1 /SphApp/schemas/trigger.workflow.g.js

/// <reference path="~/scripts/knockout-3.0.0.debug.js" />
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
        Title: ko.observable(''),
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

