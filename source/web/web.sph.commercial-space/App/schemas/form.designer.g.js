
/// <reference path="~/scripts/knockout-2.3.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.CustomField = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.CustomField, domain.commercialspace",
        Order: ko.observable(0),
        Name: ko.observable(''),
        IsRequired: ko.observable(false),
        Type: ko.observable(''),
        Size: ko.observable(''),
        Listing: ko.observable(''),
        Group: ko.observable(''),
        MaxLength: ko.observable(),
        MinLength: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.CustomFieldPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.CustomFieldPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.ComplaintTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ComplaintTemplate, domain.commercialspace",
        ComplaintTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Category: ko.observable(''),
        ComplaintCategoryCollection: ko.observableArray([]),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ComplaintTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ComplaintTemplatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.BuildingTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.BuildingTemplate, domain.commercialspace",
        BuildingTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Icon: ko.observable(''),
        Category: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.BuildingTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.BuildingTemplatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.ApplicationTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ApplicationTemplate, domain.commercialspace",
        ApplicationTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Category: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ApplicationTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ApplicationTemplatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.MaintenanceTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.MaintenanceTemplate, domain.commercialspace",
        MaintenanceTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Category: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.MaintenanceTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.MaintenanceTemplatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.CommercialSpaceTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.CommercialSpaceTemplate, domain.commercialspace",
        CommercialSpaceTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Category: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.CommercialSpaceTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.CommercialSpaceTemplatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.FormDesign = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.FormDesign, domain.commercialspace",
        Name: ko.observable(''),
        Description: ko.observable(''),
        ConfirmationText: ko.observable(''),
        ImageStoreId: ko.observable(''),
        FormElementCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.FormDesignPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.FormDesignPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.TextBox = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v.DefaultValue = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.TextBox, domain.commercialspace";

    v.MinLength = ko.observable();//nillable
    v.MaxLength = ko.observable();//nillable
    if (bespoke.sphcommercialspace.domain.TextBoxPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.TextBoxPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.CheckBox = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.CheckBox, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.CheckBoxPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.CheckBoxPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.DatePicker = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.DatePicker, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.DatePickerPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.DatePickerPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.ComboBox = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.ComboBox, domain.commercialspace";

    v.ComboBoxItemCollection = ko.observableArray([]);
    if (bespoke.sphcommercialspace.domain.ComboBoxPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.ComboBoxPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.TextAreaElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v.Rows = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.TextAreaElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.TextAreaElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.TextAreaElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.WebsiteFormElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.WebsiteFormElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.WebsiteFormElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.WebsiteFormElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.EmailFormElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.EmailFormElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.EmailFormElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.EmailFormElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.NumberTextBox = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v.Step = ko.observable(0);
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.NumberTextBox, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.NumberTextBoxPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.NumberTextBoxPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.BuildingMapElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v.Icon = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.BuildingMapElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.BuildingMapElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.BuildingMapElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.BuildingBlocksElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.BuildingBlocksElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.BuildingBlocksElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.BuildingBlocksElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.BuildingFloorsElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.BuildingFloorsElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.BuildingFloorsElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.BuildingFloorsElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.SectionFormElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.SectionFormElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.SectionFormElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.SectionFormElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.ComboBoxItem = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ComboBoxItem, domain.commercialspace",
        Caption: ko.observable(''),
        Value: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ComboBoxItemPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ComboBoxItemPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.AddressElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.AddressElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.AddressElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.AddressElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.ComplaintCategoryElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v.SubCategoryLabel = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.ComplaintCategoryElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.ComplaintCategoryElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.ComplaintCategoryElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.RentalApplicationBanksElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.RentalApplicationBanksElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.RentalApplicationBanksElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.RentalApplicationBanksElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.RentalApplicationAttachmentsElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.RentalApplicationAttachmentsElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.RentalApplicationAttachmentsElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.RentalApplicationAttachmentsElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.RentalApplicationContactElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.RentalApplicationContactElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.RentalApplicationContactElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.RentalApplicationContactElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.CommercialSpaceLotsElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.CommercialSpaceLotsElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.CommercialSpaceLotsElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.CommercialSpaceLotsElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.HtmlElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.HtmlElement, domain.commercialspace";

    v.Text = ko.observable();//type but not nillable
    if (bespoke.sphcommercialspace.domain.HtmlElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.HtmlElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.BuildingElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.BuildingElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.BuildingElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.BuildingElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.ComplaintCategory = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ComplaintCategory, domain.commercialspace",
        Name: ko.observable(''),
        Description: ko.observable(''),
        SubCategoryCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ComplaintCategoryPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ComplaintCategoryPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.CustomListDefinition = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.CustomListDefinition, domain.commercialspace",
        Name: ko.observable(''),
        Label: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.CustomListDefinitionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.CustomListDefinitionPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.CustomListDefinitionElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.CustomListDefinitionElement, domain.commercialspace";

    v.CustomFieldCollection = ko.observableArray([]);
    if (bespoke.sphcommercialspace.domain.CustomListDefinitionElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.CustomListDefinitionElementPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.MaintenanceOfficerElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.MaintenanceOfficerElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.MaintenanceOfficerElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.MaintenanceOfficerElementPartial(v));
    }
    return v;
};


bespoke.sphcommercialspace.domain.FormElement = function (webId) {

    return {
        "$type": "Bespoke.SphCommercialSpaces.Domain.FormElement, domain.commercialspace",
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
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};

