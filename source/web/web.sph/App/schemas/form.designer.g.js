﻿
/// <reference path="~/scripts/knockout-2.3.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};


bespoke.sph.domain.CustomField = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.CustomField, domain.sph",
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
    if (bespoke.sph.domain.CustomFieldPartial) {
        return _(model).extend(new bespoke.sph.domain.CustomFieldPartial(model));
    }
    return model;
};



bespoke.sph.domain.ComplaintTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ComplaintTemplate, domain.sph",
        ComplaintTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Category: ko.observable(''),
        ComplaintCategoryCollection: ko.observableArray([]),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ComplaintTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.ComplaintTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.BuildingTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.BuildingTemplate, domain.sph",
        BuildingTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Icon: ko.observable(''),
        Category: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.BuildingTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.BuildingTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.ApplicationTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ApplicationTemplate, domain.sph",
        ApplicationTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Category: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ApplicationTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.ApplicationTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.MaintenanceTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.MaintenanceTemplate, domain.sph",
        MaintenanceTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Category: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.MaintenanceTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.MaintenanceTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.SpaceTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.SpaceTemplate, domain.sph",
        SpaceTemplateId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        Category: ko.observable(''),
        DefaultMapIcon: ko.observable(''),
        DefaultSmallIcon: ko.observable(''),
        DefaultIcon: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sph.domain.FormDesign()),
        CustomListDefinitionCollection: ko.observableArray([]),
        DefaultValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.SpaceTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.SpaceTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.FormDesign = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.FormDesign, domain.sph",
        Name: ko.observable(''),
        Description: ko.observable(''),
        ConfirmationText: ko.observable(''),
        ImageStoreId: ko.observable(''),
        FormElementCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.FormDesignPartial) {
        return _(model).extend(new bespoke.sph.domain.FormDesignPartial(model));
    }
    return model;
};



bespoke.sph.domain.TextBox = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v.DefaultValue = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.TextBox, domain.sph";

    v.MinLength = ko.observable();//nillable
    v.MaxLength = ko.observable();//nillable
    if (bespoke.sph.domain.TextBoxPartial) {
        return _(v).extend(new bespoke.sph.domain.TextBoxPartial(v));
    }
    return v;
};



bespoke.sph.domain.CheckBox = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.CheckBox, domain.sph";

    if (bespoke.sph.domain.CheckBoxPartial) {
        return _(v).extend(new bespoke.sph.domain.CheckBoxPartial(v));
    }
    return v;
};



bespoke.sph.domain.DatePicker = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.DatePicker, domain.sph";

    if (bespoke.sph.domain.DatePickerPartial) {
        return _(v).extend(new bespoke.sph.domain.DatePickerPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComboBox = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.ComboBox, domain.sph";

    v.ComboBoxItemCollection = ko.observableArray([]);
    if (bespoke.sph.domain.ComboBoxPartial) {
        return _(v).extend(new bespoke.sph.domain.ComboBoxPartial(v));
    }
    return v;
};



bespoke.sph.domain.TextAreaElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v.Rows = ko.observable('');
    v.IsHtml = ko.observable(false);
    v["$type"] = "Bespoke.Sph.Domain.TextAreaElement, domain.sph";

    if (bespoke.sph.domain.TextAreaElementPartial) {
        return _(v).extend(new bespoke.sph.domain.TextAreaElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.WebsiteFormElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.WebsiteFormElement, domain.sph";

    if (bespoke.sph.domain.WebsiteFormElementPartial) {
        return _(v).extend(new bespoke.sph.domain.WebsiteFormElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.EmailFormElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.EmailFormElement, domain.sph";

    if (bespoke.sph.domain.EmailFormElementPartial) {
        return _(v).extend(new bespoke.sph.domain.EmailFormElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.NumberTextBox = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v.Step = ko.observable(0);
    v["$type"] = "Bespoke.Sph.Domain.NumberTextBox, domain.sph";

    if (bespoke.sph.domain.NumberTextBoxPartial) {
        return _(v).extend(new bespoke.sph.domain.NumberTextBoxPartial(v));
    }
    return v;
};



bespoke.sph.domain.BuildingMapElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v.Icon = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.BuildingMapElement, domain.sph";

    if (bespoke.sph.domain.BuildingMapElementPartial) {
        return _(v).extend(new bespoke.sph.domain.BuildingMapElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.BuildingBlocksElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.BuildingBlocksElement, domain.sph";

    if (bespoke.sph.domain.BuildingBlocksElementPartial) {
        return _(v).extend(new bespoke.sph.domain.BuildingBlocksElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.BuildingFloorsElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.BuildingFloorsElement, domain.sph";

    if (bespoke.sph.domain.BuildingFloorsElementPartial) {
        return _(v).extend(new bespoke.sph.domain.BuildingFloorsElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.SectionFormElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.SectionFormElement, domain.sph";

    if (bespoke.sph.domain.SectionFormElementPartial) {
        return _(v).extend(new bespoke.sph.domain.SectionFormElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComboBoxItem = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ComboBoxItem, domain.sph",
        Caption: ko.observable(''),
        Value: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ComboBoxItemPartial) {
        return _(model).extend(new bespoke.sph.domain.ComboBoxItemPartial(model));
    }
    return model;
};



bespoke.sph.domain.AddressElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v.IsUnitNoVisible = ko.observable(false);
    v.IsFloorVisible = ko.observable(false);
    v.IsBlockVisible = ko.observable(false);
    v.BlockOptionsPath = ko.observable('');
    v.FloorOptionsPath = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.AddressElement, domain.sph";

    if (bespoke.sph.domain.AddressElementPartial) {
        return _(v).extend(new bespoke.sph.domain.AddressElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComplaintCategoryElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v.SubCategoryLabel = ko.observable('');
    v["$type"] = "Bespoke.Sph.Domain.ComplaintCategoryElement, domain.sph";

    if (bespoke.sph.domain.ComplaintCategoryElementPartial) {
        return _(v).extend(new bespoke.sph.domain.ComplaintCategoryElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.RentalApplicationBanksElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.RentalApplicationBanksElement, domain.sph";

    if (bespoke.sph.domain.RentalApplicationBanksElementPartial) {
        return _(v).extend(new bespoke.sph.domain.RentalApplicationBanksElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.RentalApplicationAttachmentsElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.RentalApplicationAttachmentsElement, domain.sph";

    if (bespoke.sph.domain.RentalApplicationAttachmentsElementPartial) {
        return _(v).extend(new bespoke.sph.domain.RentalApplicationAttachmentsElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.RentalApplicationContactElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.RentalApplicationContactElement, domain.sph";

    if (bespoke.sph.domain.RentalApplicationContactElementPartial) {
        return _(v).extend(new bespoke.sph.domain.RentalApplicationContactElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.SpaceUnitElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.SpaceUnitElement, domain.sph";

    if (bespoke.sph.domain.SpaceUnitElementPartial) {
        return _(v).extend(new bespoke.sph.domain.SpaceUnitElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.HtmlElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.HtmlElement, domain.sph";

    v.Text = ko.observable();//type but not nillable
    if (bespoke.sph.domain.HtmlElementPartial) {
        return _(v).extend(new bespoke.sph.domain.HtmlElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.BuildingElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.BuildingElement, domain.sph";

    if (bespoke.sph.domain.BuildingElementPartial) {
        return _(v).extend(new bespoke.sph.domain.BuildingElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.ComplaintCategory = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.ComplaintCategory, domain.sph",
        Name: ko.observable(''),
        Description: ko.observable(''),
        SubCategoryCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.ComplaintCategoryPartial) {
        return _(model).extend(new bespoke.sph.domain.ComplaintCategoryPartial(model));
    }
    return model;
};



bespoke.sph.domain.CustomListDefinition = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.CustomListDefinition, domain.sph",
        Name: ko.observable(''),
        Label: ko.observable(''),
        CustomFieldCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.CustomListDefinitionPartial) {
        return _(model).extend(new bespoke.sph.domain.CustomListDefinitionPartial(model));
    }
    return model;
};



bespoke.sph.domain.CustomListDefinitionElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.CustomListDefinitionElement, domain.sph";

    v.CustomFieldCollection = ko.observableArray([]);
    if (bespoke.sph.domain.CustomListDefinitionElementPartial) {
        return _(v).extend(new bespoke.sph.domain.CustomListDefinitionElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.MaintenanceOfficerElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.MaintenanceOfficerElement, domain.sph";

    if (bespoke.sph.domain.MaintenanceOfficerElementPartial) {
        return _(v).extend(new bespoke.sph.domain.MaintenanceOfficerElementPartial(v));
    }
    return v;
};



bespoke.sph.domain.DefaultValue = function (webId) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DefaultValue, domain.sph",
        PropertyName: ko.observable(''),
        TypeName: ko.observable(''),
        IsNullable: ko.observable(false),
        Value: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sph.domain.DefaultValuePartial) {
        return _(model).extend(new bespoke.sph.domain.DefaultValuePartial(model));
    }
    return model;
};



bespoke.sph.domain.SpaceFeaturesElement = function (webId) {

    var v = new bespoke.sph.domain.FormElement(webId);

    v["$type"] = "Bespoke.Sph.Domain.SpaceFeaturesElement, domain.sph";

    if (bespoke.sph.domain.SpaceFeaturesElementPartial) {
        return _(v).extend(new bespoke.sph.domain.SpaceFeaturesElementPartial(v));
    }
    return v;
};


bespoke.sph.domain.FormElement = function (webId) {

    return {
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
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};

