﻿
/// <reference path="~/scripts/knockout-2.3.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />


var bespoke = bespoke || {};
bespoke.sphcommercialspace = {};
bespoke.sphcommercialspace.domain = {};



bespoke.sphcommercialspace.domain.ReportDefinition = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ReportDefinition, domain.commercialspace",
        ReportDefinitionId: ko.observable(0),
        Title: ko.observable(''),
        IsActive: ko.observable(false),
        IsPrivate: ko.observable(false),
        IsExportAllowed: ko.observable(false),
        Description: ko.observable(''),
        ReportLayoutCollection: ko.observableArray([]),
        DataSource: ko.observable(new bespoke.sphcommercialspace.domain.DataSource()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ReportDefinitionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ReportDefinitionPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.ReportLayout = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ReportLayout, domain.commercialspace",
        Name: ko.observable(''),
        Row: ko.observable(0),
        Column: ko.observable(0),
        ColumnSpan: ko.observable(0),
        ReportItemCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ReportLayoutPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ReportLayoutPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.BarChartItem = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.ReportItem(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.BarChartItem, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.BarChartItemPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.BarChartItemPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.LineChartItem = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.ReportItem(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.LineChartItem, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.LineChartItemPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.LineChartItemPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.DataGridItem = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.ReportItem(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.DataGridItem, domain.commercialspace";

    v.ReportRowCollection = ko.observableArray([]);
    v.ReportColumnCollection = ko.observableArray([]);
    if (bespoke.sphcommercialspace.domain.DataGridItemPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.DataGridItemPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.LabelItem = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.ReportItem(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.LabelItem, domain.commercialspace";

    v.Html = ko.observable();//type but not nillable
    if (bespoke.sphcommercialspace.domain.LabelItemPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.LabelItemPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.LineItem = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.ReportItem(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.LineItem, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.LineItemPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.LineItemPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.PieChartItem = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.ReportItem(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.PieChartItem, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.PieChartItemPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.PieChartItemPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.DataSource = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.DataSource, domain.commercialspace",
        EntityName: ko.observable(''),
        Query: ko.observable(''),
        ParameterCollection: ko.observableArray([]),
        ReportFilterCollection: ko.observableArray([]),
        EntityFieldCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DataSourcePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DataSourcePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Parameter = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Parameter, domain.commercialspace",
        Name: ko.observable(''),
        Type: ko.observable(''),
        AvailableValues: ko.observable(''),
        Label: ko.observable(''),
        Value: ko.observable(),
        DefaultValue: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ParameterPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ParameterPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.ReportFilter = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ReportFilter, domain.commercialspace",
        FieldName: ko.observable(''),
        Operator: ko.observable(''),
        Value: ko.observable(''),
        ParameterName: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ReportFilterPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ReportFilterPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.EntityField = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.EntityField, domain.commercialspace",
        Name: ko.observable(''),
        Type: ko.observable(''),
        IsNullable: ko.observable(false),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.EntityFieldPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.EntityFieldPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.ReportColumn = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ReportColumn, domain.commercialspace",
        Name: ko.observable(''),
        Header: ko.observable(''),
        Value: ko.observable(''),
        Width: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ReportColumnPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ReportColumnPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.ReportRow = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ReportRow, domain.commercialspace",
        ReportColumnCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ReportRowPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ReportRowPartial(model));
    }
    return model;
};


bespoke.sphcommercialspace.domain.ReportItem = function (webId) {

    return {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ReportItem, domain.commercialspace",
        Name: ko.observable(''),
        CssClass: ko.observable(''),
        Visible: ko.observable(''),
        Tooltip: ko.observable(''),
        Icon: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};

