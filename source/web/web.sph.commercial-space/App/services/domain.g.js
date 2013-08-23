
/// <reference path="~/scripts/knockout-2.3.0.debug.js" />
/// <reference path="~/Scripts/underscore.js" />
/// <reference path="~/Scripts/moment.js" />


var bespoke = bespoke || {};
bespoke.sphcommercialspace = {};
bespoke.sphcommercialspace.domain = {};



bespoke.sphcommercialspace.domain.ContractHistory = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ContractHistory, domain.commercialspace",
        ContractNo: ko.observable(''),
        DateFrom: ko.observable(moment().format('DD/MM/YYYY')),
        DateStart: ko.observable(moment().format('DD/MM/YYYY')),
        DateEnd: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContractHistoryPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContractHistoryPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Tenant = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Tenant, domain.commercialspace",
        TenantId: ko.observable(0),
        IdSsmNo: ko.observable(''),
        Name: ko.observable(''),
        BussinessType: ko.observable(''),
        Phone: ko.observable(''),
        Fax: ko.observable(''),
        MobilePhone: ko.observable(''),
        Email: ko.observable(''),
        RegistrationNo: ko.observable(''),
        ContractHistoryCollection: ko.observableArray([]),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.TenantPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.TenantPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Land = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Land, domain.commercialspace",
        LandId: ko.observable(0),
        Lot: ko.observable(''),
        Title: ko.observable(''),
        Location: ko.observable(''),
        Size: ko.observable(0.00),
        SizeUnit: ko.observable(''),
        CurrentMarketValue: ko.observable(0.00),
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
        Owner: ko.observable(new bespoke.sphcommercialspace.domain.Owner()),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        LeaseExpiryDate: ko.observable(),
        LeasePeriod: ko.observable(),
        ApprovedDateTime: ko.observable(),
        ApprovedBy: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.LandPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.LandPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Building = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Building, domain.commercialspace",
        BuildingId: ko.observable(0),
        Type: ko.observable(''),
        Name: ko.observable(''),
        LotNo: ko.observable(''),
        Size: ko.observable(0.00),
        Status: ko.observable(''),
        Floors: ko.observable(0),
        Note: ko.observable(''),
        TemplateId: ko.observable(0),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        FloorCollection: ko.observableArray([]),
        Height: ko.observable(),
        CustomFieldValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.BuildingPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.BuildingPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.LatLng = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.LatLng, domain.commercialspace",
        Lat: ko.observable(0.00),
        Lng: ko.observable(0.00),
        Elevation: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.LatLngPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.LatLngPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Address = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Address, domain.commercialspace",
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
    if (bespoke.sphcommercialspace.domain.AddressPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.AddressPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Floor = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Floor, domain.commercialspace",
        Name: ko.observable(''),
        Size: ko.observable(0.00),
        Number: ko.observable(''),
        Note: ko.observable(''),
        LotCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.FloorPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.FloorPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Lot = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Lot, domain.commercialspace",
        Name: ko.observable(''),
        Size: ko.observable(0.00),
        FloorNo: ko.observable(''),
        IsCommercialSpace: ko.observable(false),
        Usage: ko.observable(''),
        FillOpacity: ko.observable(0.00),
        FillColor: ko.observable(''),
        PlanStoreId: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.LotPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.LotPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.CommercialSpace = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.CommercialSpace, domain.commercialspace",
        CommercialSpaceId: ko.observable(0),
        BuildingId: ko.observable(0),
        LotName: ko.observable(''),
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
        BuildingLot: ko.observable(''),
        RentalRate: ko.observable(0.00),
        TemplateId: ko.observable(0),
        LotCollection: ko.observableArray([]),
        CustomFieldValueCollection: ko.observableArray([]),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.CommercialSpacePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.CommercialSpacePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.RentalApplication = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.RentalApplication, domain.commercialspace",
        RentalApplicationId: ko.observable(0),
        CompanyName: ko.observable(''),
        CompanyRegistrationNo: ko.observable(''),
        DateStart: ko.observable(moment().format('DD/MM/YYYY')),
        DateEnd: ko.observable(moment().format('DD/MM/YYYY')),
        Purpose: ko.observable(''),
        CompanyType: ko.observable(''),
        CommercialSpaceId: ko.observable(0),
        Status: ko.observable(''),
        Experience: ko.observable(''),
        IsRecordExist: ko.observable(false),
        PreviousAddress: ko.observable(''),
        IsCompany: ko.observable(false),
        Type: ko.observable(''),
        Remarks: ko.observable(''),
        RegistrationNo: ko.observable(''),
        ApplicationDate: ko.observable(moment().format('DD/MM/YYYY')),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        BankCollection: ko.observableArray([]),
        Contact: ko.observable(new bespoke.sphcommercialspace.domain.Contact()),
        CurrentYearSales: ko.observable(),
        LastYearSales: ko.observable(),
        PreviousYearSales: ko.observable(),
        AttachmentCollection: ko.observableArray([]),
        Offer: ko.observable(new bespoke.sphcommercialspace.domain.Offer()),
        CommercialSpace: ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpace()),
        CustomFieldValueCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.RentalApplicationPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.RentalApplicationPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Attachment = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Attachment, domain.commercialspace",
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
    if (bespoke.sphcommercialspace.domain.AttachmentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.AttachmentPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Bank = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Bank, domain.commercialspace",
        Name: ko.observable(''),
        Location: ko.observable(''),
        AccountNo: ko.observable(''),
        AccountType: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.BankPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.BankPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Contact = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Contact, domain.commercialspace",
        ContractId: ko.observable(0),
        Name: ko.observable(''),
        MobileNo: ko.observable(''),
        OfficeNo: ko.observable(''),
        Email: ko.observable(''),
        IcNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContactPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContactPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.ContractTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ContractTemplate, domain.commercialspace",
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
    if (bespoke.sphcommercialspace.domain.ContractTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContractTemplatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.DocumentTemplate = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.DocumentTemplate, domain.commercialspace",
        Name: ko.observable(''),
        StoreId: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DocumentTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DocumentTemplatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Contract = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Contract, domain.commercialspace",
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
        Owner: ko.observable(new bespoke.sphcommercialspace.domain.Owner()),
        ContractingParty: ko.observable(new bespoke.sphcommercialspace.domain.ContractingParty()),
        Option: ko.observable(),
        Tenant: ko.observable(new bespoke.sphcommercialspace.domain.Tenant()),
        CommercialSpace: ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpace()),
        TopicCollection: ko.observableArray([]),
        Termination: ko.observable(new bespoke.sphcommercialspace.domain.Termination()),
        Extension: ko.observable(new bespoke.sphcommercialspace.domain.Extension()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContractPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContractPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Document = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Document, domain.commercialspace",
        Title: ko.observable(''),
        Extension: ko.observable(''),
        DocumentVersionCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DocumentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DocumentPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.DocumentVersion = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.DocumentVersion, domain.commercialspace",
        StoreId: ko.observable(''),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        CommitedBy: ko.observable(''),
        No: ko.observable(''),
        Note: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DocumentVersionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DocumentVersionPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Owner = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Owner, domain.commercialspace",
        Name: ko.observable(''),
        TelephoneNo: ko.observable(''),
        FaxNo: ko.observable(''),
        Email: ko.observable(''),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.OwnerPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.OwnerPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.AuditTrail = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.AuditTrail, domain.commercialspace",
        User: ko.observable(''),
        DateTime: ko.observable(moment().format('DD/MM/YYYY')),
        Operation: ko.observable(''),
        Type: ko.observable(''),
        EntityId: ko.observable(0),
        ChangeCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.AuditTrailPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.AuditTrailPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Change = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Change, domain.commercialspace",
        PropertyName: ko.observable(''),
        OldValue: ko.observable(''),
        NewValue: ko.observable(''),
        Action: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ChangePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ChangePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Organization = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Organization, domain.commercialspace",
        Name: ko.observable(''),
        RegistrationNo: ko.observable(''),
        Email: ko.observable(''),
        OfficeNo: ko.observable(''),
        FaxNo: ko.observable(''),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.OrganizationPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.OrganizationPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Offer = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Offer, domain.commercialspace",
        BusinessPlan: ko.observable(''),
        CommercialSpaceId: ko.observable(0),
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
    if (bespoke.sphcommercialspace.domain.OfferPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.OfferPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.OfferCondition = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.OfferCondition, domain.commercialspace",
        Title: ko.observable(''),
        Description: ko.observable(''),
        Note: ko.observable(''),
        IsRequired: ko.observable(false),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.OfferConditionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.OfferConditionPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Topic = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Topic, domain.commercialspace",
        Title: ko.observable(''),
        Description: ko.observable(''),
        Text: ko.observable(''),
        ClauseCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.TopicPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.TopicPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Clause = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Clause, domain.commercialspace",
        Title: ko.observable(''),
        Description: ko.observable(''),
        No: ko.observable(''),
        Text: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ClausePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ClausePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.ContractingParty = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.ContractingParty, domain.commercialspace",
        Name: ko.observable(''),
        RegistrationNo: ko.observable(''),
        Contact: ko.observable(new bespoke.sphcommercialspace.domain.Contact()),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContractingPartyPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContractingPartyPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.DepositPayment = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.DepositPayment, domain.commercialspace",
        DepositPaymentId: ko.observable(0),
        ReceiptNo: ko.observable(''),
        Amount: ko.observable(0.00),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        RegistrationNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DepositPaymentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DepositPaymentPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Payment = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Payment, domain.commercialspace",
        PaymentId: ko.observable(0),
        Amount: ko.observable(0.00),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        ContractNo: ko.observable(''),
        TenantIdSsmNo: ko.observable(''),
        ReceiptNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.PaymentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.PaymentPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.UserProfile = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.UserProfile, domain.commercialspace",
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
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.UserProfilePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.UserProfilePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Deposit = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Deposit, domain.commercialspace",
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
    if (bespoke.sphcommercialspace.domain.DepositPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DepositPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Reply = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Reply, domain.commercialspace",
        Title: ko.observable(''),
        Text: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ReplyPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ReplyPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Setting = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Setting, domain.commercialspace",
        SettingId: ko.observable(0),
        Username: ko.observable(''),
        Key: ko.observable(),
        Value: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.SettingPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.SettingPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Rebate = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Rebate, domain.commercialspace",
        RebateId: ko.observable(0),
        ContractNo: ko.observable(''),
        Amount: ko.observable(0.00),
        StartDate: ko.observable(moment().format('DD/MM/YYYY')),
        EndDate: ko.observable(moment().format('DD/MM/YYYY')),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.RebatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.RebatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Interest = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Interest, domain.commercialspace",
        Building: ko.observable(''),
        CommercialSpaceCategory: ko.observable(''),
        Percentage: ko.observable(0.00),
        Period: ko.observable(0),
        PeriodType: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.InterestPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.InterestPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Rent = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Invoice(webId);

    v.Half = ko.observable('');
    v.Year = ko.observable(0);
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.Rent, domain.commercialspace";

    v.Month = ko.observable();//nillable
    v.Quarter = ko.observable();//nillable
    v.Tenant = ko.observable(new bespoke.sphcommercialspace.domain.Tenant());
    if (bespoke.sphcommercialspace.domain.RentPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.RentPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.AdhocInvoice = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Invoice(webId);

    v.Category = ko.observable('');
    v.F2 = ko.observable(0);
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.AdhocInvoice, domain.commercialspace";

    v.InvoiceItemCollection = ko.observableArray([]);
    v.DocumentCollection = ko.observableArray([]);
    v.SentDate = ko.observable();//nillable
    v.Tenant = ko.observable(new bespoke.sphcommercialspace.domain.Tenant());
    v.Type2 = ko.observable();//type but not nillable
    v.Note2 = ko.observable();//type but not nillable
    v.Address = ko.observable(new bespoke.sphcommercialspace.domain.Address());
    if (bespoke.sphcommercialspace.domain.AdhocInvoicePartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.AdhocInvoicePartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.InvoiceItem = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.InvoiceItem, domain.commercialspace",
        Amount: ko.observable(0.00),
        Category: ko.observable(''),
        Note: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.InvoiceItemPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.InvoiceItemPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.State = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.State, domain.commercialspace",
        Name: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.StatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.StatePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Complaint = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Complaint, domain.commercialspace",
        ComplaintId: ko.observable(0),
        TemplateId: ko.observable(0),
        Remarks: ko.observable(''),
        TenantId: ko.observable(0),
        CommercialSpace: ko.observable(''),
        Status: ko.observable(''),
        Category: ko.observable(''),
        SubCategory: ko.observable(''),
        ReferenceNo: ko.observable(''),
        Type: ko.observable(''),
        AttachmentStoreId: ko.observable(''),
        Department: ko.observable(''),
        Note: ko.observable(''),
        CustomFieldValueCollection: ko.observableArray([]),
        Tenant: ko.observable(new bespoke.sphcommercialspace.domain.Tenant()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ComplaintPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ComplaintPartial(model));
    }
    return model;
};



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



bespoke.sphcommercialspace.domain.CustomFieldValue = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.CustomFieldValue, domain.commercialspace",
        Name: ko.observable(''),
        Type: ko.observable(''),
        Value: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.CustomFieldValuePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.CustomFieldValuePartial(model));
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
        ComplaintCategoryCollection: ko.observableArray([]),
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ComplaintTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ComplaintTemplatePartial(model));
    }
    return model;
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



bespoke.sphcommercialspace.domain.Inspection = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Inspection, domain.commercialspace",
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
    if (bespoke.sphcommercialspace.domain.InspectionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.InspectionPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Maintenance = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Maintenance, domain.commercialspace",
        MaintenanceId: ko.observable(0),
        ComplaintId: ko.observable(0),
        WorkOrderNo: ko.observable(''),
        Department: ko.observable(''),
        Status: ko.observable(''),
        Resolution: ko.observable(''),
        Officer: ko.observable(''),
        AttachmentStoreId: ko.observable(''),
        AttachmentName: ko.observable(''),
        Complaint: ko.observable(new bespoke.sphcommercialspace.domain.Complaint()),
        StartDate: ko.observable(),
        EndDate: ko.observable(),
        WorkOrder: ko.observable(new bespoke.sphcommercialspace.domain.WorkOrder()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.MaintenancePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.MaintenancePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Designation = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Designation, domain.commercialspace",
        DesignationId: ko.observable(0),
        Name: ko.observable(''),
        Description: ko.observable(''),
        IsActive: ko.observable(false),
        StartModule: ko.observable(''),
        RoleCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DesignationPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DesignationPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.WorkOrder = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.WorkOrder, domain.commercialspace",
        Priority: ko.observable(''),
        Severity: ko.observable(''),
        EstimationCost: ko.observable(0.00),
        WorkOrderId: ko.observable(0),
        CommentCollection: ko.observableArray([]),
        PartsAndLaborCollection: ko.observableArray([]),
        WarrantyCollection: ko.observableArray([]),
        NonComplianceCollection: ko.observableArray([]),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.WorkOrderPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.WorkOrderPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Comment = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Comment, domain.commercialspace",
        Description: ko.observable(''),
        User: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.CommentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.CommentPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.PartsAndLabor = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.PartsAndLabor, domain.commercialspace",
        Quantity: ko.observable(0),
        Description: ko.observable(''),
        Cost: ko.observable(0.00),
        Total: ko.observable(0.00),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.PartsAndLaborPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.PartsAndLaborPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Warranty = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Warranty, domain.commercialspace",
        Description: ko.observable(''),
        YearWarranty: ko.observable(''),
        StartDate: ko.observable(moment().format('DD/MM/YYYY')),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.WarrantyPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.WarrantyPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.NonCompliance = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.NonCompliance, domain.commercialspace",
        Description: ko.observable(''),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Reason: ko.observable(''),
        Action: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.NonCompliancePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.NonCompliancePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Inventory = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Inventory, domain.commercialspace",
        Name: ko.observable(''),
        Category: ko.observable(''),
        Brand: ko.observable(''),
        Specification: ko.observable(''),
        Quantity: ko.observable(0),
        InventoryId: ko.observable(0),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.InventoryPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.InventoryPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.FunctionField = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Field(webId);

    v.Script = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.FunctionField, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.FunctionFieldPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.FunctionFieldPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.ConstantField = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Field(webId);

    v.TypeName = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.ConstantField, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.ConstantFieldPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.ConstantFieldPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.DocumentField = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Field(webId);

    v.XPath = ko.observable('');
    v.NamespacePrefix = ko.observable('');
    v.TypeName = ko.observable('');
    v.Path = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.DocumentField, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.DocumentFieldPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.DocumentFieldPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.FieldChangeField = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Field(webId);

    v.Path = ko.observable('');
    v.TypeName = ko.observable('');
    v.OldValue = ko.observable('');
    v.NewValue = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.FieldChangeField, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.FieldChangeFieldPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.FieldChangeFieldPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.Trigger = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Trigger, domain.commercialspace",
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
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.TriggerPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.TriggerPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Rule = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Rule, domain.commercialspace",
        Left: ko.observable(),
        Right: ko.observable(),
        Operator: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.RulePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.RulePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.EmailAction = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.CustomAction(webId);

    v.From = ko.observable('');
    v.To = ko.observable('');
    v.SubjectTemplate = ko.observable('');
    v.BodyTemplate = ko.observable('');
    v.Bcc = ko.observable('');
    v.Cc = ko.observable('');
    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.EmailAction, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.EmailActionPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.EmailActionPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.SetterAction = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.CustomAction(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.SetterAction, domain.commercialspace";

    v.SetterActionChildCollection = ko.observableArray([]);
    if (bespoke.sphcommercialspace.domain.SetterActionPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.SetterActionPartial(v));
    }
    return v;
};



bespoke.sphcommercialspace.domain.SetterActionChild = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.SetterActionChild, domain.commercialspace",
        Path: ko.observable(''),
        Field: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.SetterActionChildPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.SetterActionChildPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Watcher = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Watcher, domain.commercialspace",
        WatcherId: ko.observable(0),
        EntityName: ko.observable(''),
        EntityId: ko.observable(0),
        User: ko.observable(''),
        IsActive: ko.observable(false),
        DateTime: ko.observable(moment().format('DD/MM/YYYY')),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.WatcherPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.WatcherPartial(model));
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
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
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
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
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
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
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
        CustomFieldCollection: ko.observableArray([]),
        FormDesign: ko.observable(new bespoke.sphcommercialspace.domain.FormDesign()),
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



bespoke.sphcommercialspace.domain.Profile = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Profile, domain.commercialspace",
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
    if (bespoke.sphcommercialspace.domain.ProfilePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ProfilePartial(model));
    }
    return model;
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



bespoke.sphcommercialspace.domain.Termination = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Termination, domain.commercialspace",
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Remarks: ko.observable(''),
        ApprovalOfficer: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.TerminationPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.TerminationPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Extension = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Extension, domain.commercialspace",
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Remarks: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ExtensionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ExtensionPartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.Message = function (webId) {

    var model = {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Message, domain.commercialspace",
        MessageId: ko.observable(0),
        Subject: ko.observable(''),
        IsRead: ko.observable(false),
        Body: ko.observable(''),
        UserName: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.MessagePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.MessagePartial(model));
    }
    return model;
};



bespoke.sphcommercialspace.domain.BuildingElement = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.FormElement(webId);

    v["$type"] = "Bespoke.SphCommercialSpaces.Domain.BuildingElement, domain.commercialspace";

    if (bespoke.sphcommercialspace.domain.BuildingElementPartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.BuildingElementPartial(v));
    }
    return v;
};


bespoke.sphcommercialspace.domain.Invoice = function (webId) {

    return {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Invoice, domain.commercialspace",
        InvoiceId: ko.observable(0),
        Date: ko.observable(moment().format('DD/MM/YYYY')),
        Amount: ko.observable(0.00),
        No: ko.observable(''),
        Type: ko.observable('InvoiceType'),
        ContractNo: ko.observable(''),
        TenantIdSsmNo: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};


bespoke.sphcommercialspace.domain.Field = function (webId) {

    return {
        "$type": "Bespoke.SphCommercialSpaces.Domain.Field, domain.commercialspace",
        Name: ko.observable(''),
        Note: ko.observable(''),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
};


bespoke.sphcommercialspace.domain.CustomAction = function (webId) {

    return {
        "$type": "Bespoke.SphCommercialSpaces.Domain.CustomAction, domain.commercialspace",
        Title: ko.observable(''),
        IsActive: ko.observable(false),
        TriggerId: ko.observable(0),
        Note: ko.observable(''),
        CustomActionId: ko.observable(0),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
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


bespoke.sphcommercialspace.domain.InvoiceType = function () {
    return {
        ADHOC_INVOICE: 'AdhocInvoice',
        RENTAL: 'Rental',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();

bespoke.sphcommercialspace.domain.FieldType = function () {
    return {
        DOCUMENT_FIELD: 'DocumentField',
        CONSTANT_FIELD: 'ConstantField',
        FUNCTION_FIELD: 'FunctionField',

        DO_NOT_SELECT: 'DONTDOTHIS'
    };
}();

bespoke.sphcommercialspace.domain.Operator = function () {
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
