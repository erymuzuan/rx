
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
