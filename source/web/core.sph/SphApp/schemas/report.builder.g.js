
/// <reference path="~/scripts/knockout-3.1.0.debug.js" />
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

