
/// <reference path="~/scripts/knockout-3.2.0.debug.js" />
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
            if (typeof model[n] === "function") {
                model[n](optionOrWebid[n]);
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
        Title: ko.observable(''),
        Extension: ko.observable(''),
        DocumentVersionCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.DocumentPartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentPartial(model));
    }
    return model;
};



bespoke.sph.domain.DocumentVersion = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DocumentVersion, domain.sph",
        StoreId: ko.observable(''),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        CommitedBy: ko.observable(''),
        No: ko.observable(''),
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


    if (bespoke.sph.domain.DocumentVersionPartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentVersionPartial(model));
    }
    return model;
};



bespoke.sph.domain.Owner = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Owner, domain.sph",
        Name: ko.observable(''),
        TelephoneNo: ko.observable(''),
        FaxNo: ko.observable(''),
        Email: ko.observable(''),
        Address: ko.observable(new bespoke.sph.domain.Address()),
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


    if (bespoke.sph.domain.OwnerPartial) {
        return _(model).extend(new bespoke.sph.domain.OwnerPartial(model));
    }
    return model;
};



bespoke.sph.domain.AuditTrail = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.AuditTrail, domain.sph",
        User: ko.observable(''),
        DateTime: ko.observable(moment().format('DD/MM/YYYY')),
        Operation: ko.observable(''),
        Type: ko.observable(''),
        EntityId: ko.observable(0),
        ChangeCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.AuditTrailPartial) {
        return _(model).extend(new bespoke.sph.domain.AuditTrailPartial(model));
    }
    return model;
};



bespoke.sph.domain.Change = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Change, domain.sph",
        PropertyName: ko.observable(''),
        OldValue: ko.observable(''),
        NewValue: ko.observable(''),
        Action: ko.observable(''),
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


    if (bespoke.sph.domain.ChangePartial) {
        return _(model).extend(new bespoke.sph.domain.ChangePartial(model));
    }
    return model;
};



bespoke.sph.domain.Organization = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Organization, domain.sph",
        Name: ko.observable(''),
        RegistrationNo: ko.observable(''),
        Email: ko.observable(''),
        OfficeNo: ko.observable(''),
        FaxNo: ko.observable(''),
        Address: ko.observable(new bespoke.sph.domain.Address()),
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


    if (bespoke.sph.domain.OrganizationPartial) {
        return _(model).extend(new bespoke.sph.domain.OrganizationPartial(model));
    }
    return model;
};



bespoke.sph.domain.UserProfile = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.UserProfile, domain.sph",
        UserName: ko.observable(''),
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


    if (bespoke.sph.domain.UserProfilePartial) {
        return _(model).extend(new bespoke.sph.domain.UserProfilePartial(model));
    }
    return model;
};



bespoke.sph.domain.Setting = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Setting, domain.sph",
        SettingId: ko.observable(0),
        UserName: ko.observable(''),
        Key: ko.observable(),
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


    if (bespoke.sph.domain.SettingPartial) {
        return _(model).extend(new bespoke.sph.domain.SettingPartial(model));
    }
    return model;
};



bespoke.sph.domain.Designation = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Designation, domain.sph",
        DesignationId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        StartModule: ko.observable(''),
        RoleCollection: ko.observableArray([]),
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


    if (bespoke.sph.domain.DesignationPartial) {
        return _(model).extend(new bespoke.sph.domain.DesignationPartial(model));
    }
    return model;
};



bespoke.sph.domain.Watcher = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Watcher, domain.sph",
        WatcherId: ko.observable(0),
        EntityName: ko.observable(''),
        EntityId: ko.observable(0),
        User: ko.observable(''),
        IsActive: ko.observable(false),
        DateTime: ko.observable(moment().format('DD/MM/YYYY')),
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


    if (bespoke.sph.domain.WatcherPartial) {
        return _(model).extend(new bespoke.sph.domain.WatcherPartial(model));
    }
    return model;
};



bespoke.sph.domain.Profile = function (optionOrWebid) {

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


    if (bespoke.sph.domain.ProfilePartial) {
        return _(model).extend(new bespoke.sph.domain.ProfilePartial(model));
    }
    return model;
};



bespoke.sph.domain.Message = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Message, domain.sph",
        MessageId: ko.observable(0),
        Subject: ko.observable(''),
        IsRead: ko.observable(false),
        Body: ko.observable(''),
        UserName: ko.observable(''),
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


    if (bespoke.sph.domain.MessagePartial) {
        return _(model).extend(new bespoke.sph.domain.MessagePartial(model));
    }
    return model;
};



bespoke.sph.domain.Photo = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.Photo, domain.sph",
        Title: ko.observable(''),
        Description: ko.observable(''),
        StoreId: ko.observable(''),
        ThumbnailStoreId: ko.observable(''),
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


    if (bespoke.sph.domain.PhotoPartial) {
        return _(model).extend(new bespoke.sph.domain.PhotoPartial(model));
    }
    return model;
};



bespoke.sph.domain.Address = function (optionOrWebid) {

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


    if (bespoke.sph.domain.AddressPartial) {
        return _(model).extend(new bespoke.sph.domain.AddressPartial(model));
    }
    return model;
};



bespoke.sph.domain.EmailTemplate = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.EmailTemplate, domain.sph",
        Id: ko.observable("0"),
        Entity: ko.observable(''),
        Name: ko.observable(''),
        Note: ko.observable(''),
        SubjectTemplate: ko.observable(''),
        BodyTemplate: ko.observable(''),
        IsPublished: ko.observable(false),
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


    if (bespoke.sph.domain.EmailTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.EmailTemplatePartial(model));
    }
    return model;
};



bespoke.sph.domain.DocumentTemplate = function (optionOrWebid) {

    var model = {
        "$type": "Bespoke.Sph.Domain.DocumentTemplate, domain.sph",
        Id: ko.observable("0"),
        Name: ko.observable(''),
        Note: ko.observable(''),
        WordTemplateStoreId: ko.observable(''),
        IsPublished: ko.observable(false),
        Entity: ko.observable(''),
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


    if (bespoke.sph.domain.DocumentTemplatePartial) {
        return _(model).extend(new bespoke.sph.domain.DocumentTemplatePartial(model));
    }
    return model;
};

