
/// <reference path="~/scripts/knockout-2.2.1.debug.js" />
/// <reference path="~/Scripts/underscore.js" />


var bespoke = bespoke || {};
bespoke.sphcommercialspace = {};
bespoke.sphcommercialspace.domain = {};



bespoke.sphcommercialspace.domain.ContractHistory = function (webId) {

    var model = {
        ContractNo: ko.observable(),
        DateFrom: ko.observable(),
        TenantName: ko.observable(),
        DateEnd: ko.observable(),
        DateStart: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContractHistoryPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContractHistoryPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Tenant = function (webId) {

    var model = {
        TenantId: ko.observable(),
        IdSsmNo: ko.observable(),
        Name: ko.observable(),
        BussinessType: ko.observable(),
        Phone: ko.observable(),
        Fax: ko.observable(),
        MobilePhone: ko.observable(),
        Email: ko.observable(),
        RegistrationNo: ko.observable(),
        ContractHistoryCollection: ko.observableArray(),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.TenantPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.TenantPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Rent = function (webId) {

    var model = {
        Half: ko.observable(),
        Quarter: ko.observable(),
        Month: ko.observable(),
        Year: ko.observable(),
        Tenant: ko.observable(new bespoke.sphcommercialspace.domain.Tenant()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.RentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.RentPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Land = function (webId) {

    var model = {
        Lot: ko.observable(),
        Title: ko.observable(),
        Location: ko.observable(),
        Size: ko.observable(),
        SizeUnit: ko.observable(),
        CurrentMarketValue: ko.observable(),
        RezabNo: ko.observable(),
        SheetNo: ko.observable(),
        ApprovedPlanNo: ko.observable(),
        LandOffice: ko.observable(),
        Usage: ko.observable(),
        Note: ko.observable(),
        OwnLevel: ko.observable(),
        PlanNo: ko.observable(),
        Status: ko.observable(),
        IsApproved: ko.observable(),
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
        return _(model).extend(new bespoke.sphcommercialspace.domain.LandPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Building = function (webId) {

    var model = {
        BuildingId: ko.observable(),
        Name: ko.observable(),
        LotNo: ko.observable(),
        Size: ko.observable(),
        Status: ko.observable(),
        Floors: ko.observable(),
        Note: ko.observable(),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        FloorCollection: ko.observableArray(),
        Height: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.BuildingPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.BuildingPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.LatLng = function (webId) {

    var model = {
        Lat: ko.observable(),
        Lng: ko.observable(),
        Elevation: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.LatLngPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.LatLngPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Address = function (webId) {

    var model = {
        UnitNo: ko.observable(),
        Floor: ko.observable(),
        Block: ko.observable(),
        Street: ko.observable(),
        City: ko.observable(),
        Postcode: ko.observable(),
        State: ko.observable(),
        Country: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.AddressPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.AddressPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Floor = function (webId) {

    var model = {
        Name: ko.observable(),
        Size: ko.observable(),
        Number: ko.observable(),
        Note: ko.observable(),
        LotCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.FloorPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.FloorPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Lot = function (webId) {

    var model = {
        Name: ko.observable(),
        Size: ko.observable(),
        FloorNo: ko.observable(),
        IsCommercialSpace: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.LotPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.LotPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.CommercialSpace = function (webId) {

    var model = {
        CommercialSpaceId: ko.observable(),
        BuildingId: ko.observable(),
        LotName: ko.observable(),
        FloorName: ko.observable(),
        Size: ko.observable(),
        Category: ko.observable(),
        RentalType: ko.observable(),
        IsOnline: ko.observable(),
        RegistrationNo: ko.observable(),
        Status: ko.observable(),
        ContactPerson: ko.observable(),
        ContactNo: ko.observable(),
        State: ko.observable(),
        City: ko.observable(),
        BuildingName: ko.observable(),
        BuildingLot: ko.observable(),
        RentalRate: ko.observable(),
        LotCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.CommercialSpacePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.CommercialSpacePartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.RentalApplication = function (webId) {

    var model = {
        RentalApplicationId: ko.observable(),
        CompanyName: ko.observable(),
        CompanyRegistrationNo: ko.observable(),
        DateStart: ko.observable(),
        DateEnd: ko.observable(),
        Purpose: ko.observable(),
        CompanyType: ko.observable(),
        CommercialSpaceId: ko.observable(),
        Status: ko.observable(),
        Experience: ko.observable(),
        IsRecordExist: ko.observable(),
        PreviousAddress: ko.observable(),
        IsCompany: ko.observable(),
        Type: ko.observable(),
        Remarks: ko.observable(),
        RegistrationNo: ko.observable(),
        ApplicationDate: ko.observable(),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        BankCollection: ko.observableArray(),
        Contact: ko.observable(new bespoke.sphcommercialspace.domain.Contact()),
        CurrentYearSales: ko.observable(),
        LastYearSales: ko.observable(),
        PreviousYearSales: ko.observable(),
        AttachmentCollection: ko.observableArray(),
        Offer: ko.observable(new bespoke.sphcommercialspace.domain.Offer()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.RentalApplicationPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.RentalApplicationPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Attachment = function (webId) {

    var model = {
        Type: ko.observable(),
        Name: ko.observable(),
        IsRequired: ko.observable(),
        IsReceived: ko.observable(),
        StoreId: ko.observable(),
        IsCompleted: ko.observable(),
        ReceivedDateTime: ko.observable(),
        ReceivedBy: ko.observable(),
        Note: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.AttachmentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.AttachmentPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Bank = function (webId) {

    var model = {
        Name: ko.observable(),
        Location: ko.observable(),
        AccountNo: ko.observable(),
        AccountType: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.BankPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.BankPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Contact = function (webId) {

    var model = {
        ContractId: ko.observable(),
        Name: ko.observable(),
        Title: ko.observable(),
        IcNo: ko.observable(),
        Role: ko.observable(),
        MobileNo: ko.observable(),
        OfficeNo: ko.observable(),
        Email: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContactPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContactPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.ContractTemplate = function (webId) {

    var model = {
        ContractTemplateId: ko.observable(),
        Type: ko.observable(),
        Description: ko.observable(),
        Status: ko.observable(),
        DocumentTemplateCollection: ko.observableArray(),
        TopicCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContractTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContractTemplatePartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.DocumentTemplate = function (webId) {

    var model = {
        Name: ko.observable(),
        StoreId: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DocumentTemplatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DocumentTemplatePartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Contract = function (webId) {

    var model = {
        ContractId: ko.observable(),
        ReferenceNo: ko.observable(),
        Type: ko.observable(),
        Date: ko.observable(),
        Value: ko.observable(),
        Title: ko.observable(),
        Remarks: ko.observable(),
        Period: ko.observable(),
        PeriodUnit: ko.observable(),
        StartDate: ko.observable(),
        EndDate: ko.observable(),
        RentalApplicationId: ko.observable(),
        Status: ko.observable(),
        RentType: ko.observable(),
        DocumentCollection: ko.observableArray(),
        Owner: ko.observable(new bespoke.sphcommercialspace.domain.Owner()),
        ContractingParty: ko.observable(new bespoke.sphcommercialspace.domain.ContractingParty()),
        Option: ko.observable(),
        Tenant: ko.observable(new bespoke.sphcommercialspace.domain.Tenant()),
        CommercialSpace: ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpace()),
        TopicCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContractPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContractPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Document = function (webId) {

    var model = {
        Title: ko.observable(),
        Extension: ko.observable(),
        DocumentVersionCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DocumentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DocumentPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.DocumentVersion = function (webId) {

    var model = {
        StoreId: ko.observable(),
        Date: ko.observable(),
        CommitedBy: ko.observable(),
        No: ko.observable(),
        Note: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DocumentVersionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DocumentVersionPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Owner = function (webId) {

    var model = {
        Name: ko.observable(),
        TelephoneNo: ko.observable(),
        FaxNo: ko.observable(),
        Email: ko.observable(),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.OwnerPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.OwnerPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.AuditTrail = function (webId) {

    var model = {
        User: ko.observable(),
        DateTime: ko.observable(),
        Operation: ko.observable(),
        Type: ko.observable(),
        EntityId: ko.observable(),
        ChangeCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.AuditTrailPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.AuditTrailPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Change = function (webId) {

    var model = {
        PropertyName: ko.observable(),
        OldValue: ko.observable(),
        NewValue: ko.observable(),
        Action: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ChangePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ChangePartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Organization = function (webId) {

    var model = {
        Name: ko.observable(),
        RegistrationNo: ko.observable(),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.OrganizationPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.OrganizationPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Offer = function (webId) {

    var model = {
        BusinessPlan: ko.observable(),
        CommercialSpaceId: ko.observable(),
        Size: ko.observable(),
        Building: ko.observable(),
        Floor: ko.observable(),
        Deposit: ko.observable(),
        Rent: ko.observable(),
        Date: ko.observable(),
        ExpiryDate: ko.observable(),
        Period: ko.observable(),
        PeriodUnit: ko.observable(),
        Option: ko.observable(),
        OfferConditionCollection: ko.observableArray(),
        BusinessPlanText: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.OfferPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.OfferPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.OfferCondition = function (webId) {

    var model = {
        Title: ko.observable(),
        Description: ko.observable(),
        Note: ko.observable(),
        IsRequired: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.OfferConditionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.OfferConditionPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Topic = function (webId) {

    var model = {
        Title: ko.observable(),
        Description: ko.observable(),
        Text: ko.observable(),
        ClauseCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.TopicPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.TopicPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Clause = function (webId) {

    var model = {
        Title: ko.observable(),
        Description: ko.observable(),
        No: ko.observable(),
        Text: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ClausePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ClausePartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.ContractingParty = function (webId) {

    var model = {
        Name: ko.observable(),
        RegistrationNo: ko.observable(),
        Contact: ko.observable(new bespoke.sphcommercialspace.domain.Contact()),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ContractingPartyPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ContractingPartyPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.DepositPayment = function (webId) {

    var model = {
        ReceiptNo: ko.observable(),
        Amount: ko.observable(),
        Date: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DepositPaymentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DepositPaymentPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Payment = function (webId) {

    var model = {
        PaymentId: ko.observable(),
        Amount: ko.observable(),
        Date: ko.observable(),
        ContractNo: ko.observable(),
        TenantIdSsmNo: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.PaymentPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.PaymentPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.User = function (webId) {

    var model = {
        UserId: ko.observable(),
        OrganizationId: ko.observable(),
        UserName: ko.observable(),
        Email: ko.observable(),
        FullName: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.UserPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.UserPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Deposit = function (webId) {

    var model = {
        DepositId: ko.observable(),
        DateTime: ko.observable(),
        Amount: ko.observable(),
        IsPaid: ko.observable(),
        IsRefund: ko.observable(),
        IsVoid: ko.observable(),
        PaymentDateTime: ko.observable(),
        RefundDateTime: ko.observable(),
        DueDate: ko.observable(),
        DepositPaymentCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.DepositPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.DepositPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Role = function (webId) {

    var model = {
        RoleId: ko.observable(),
        Name: ko.observable(),
        PermissionCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.RolePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.RolePartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Permission = function (webId) {

    var model = {
        IsAuthorized: ko.observable(),
        Name: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.PermissionPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.PermissionPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Complaint = function (webId) {

    var model = {
        ComplaintId: ko.observable(),
        No: ko.observable(),
        Status: ko.observable(),
        Title: ko.observable(),
        Text: ko.observable(),
        Category: ko.observable(),
        Contact: ko.observable(new bespoke.sphcommercialspace.domain.Contact()),
        Address: ko.observable(new bespoke.sphcommercialspace.domain.Address()),
        ReplyCollection: ko.observableArray(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ComplaintPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ComplaintPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Reply = function (webId) {

    var model = {
        Title: ko.observable(),
        Text: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.ReplyPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.ReplyPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Setting = function (webId) {

    var model = {
        SettingId: ko.observable(),
        Username: ko.observable(),
        Key: ko.observable(),
        Value: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.SettingPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.SettingPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Rebate = function (webId) {

    var model = {
        Building: ko.observable(),
        Floor: ko.observable(),
        Amount: ko.observable(),
        StartDate: ko.observable(),
        EndDate: ko.observable(),
        CommercialSpaceCategory: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.RebatePartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.RebatePartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.Interest = function (webId) {

    var model = {
        Building: ko.observable(),
        CommercialSpaceCategory: ko.observable(),
        Percentage: ko.observable(),
        Period: ko.observable(),
        PeriodType: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.InterestPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.InterestPartial());
    }
    return model;
};



bespoke.sphcommercialspace.domain.AdhocInvoice = function (webId) {

    var v = new bespoke.sphcommercialspace.domain.Invoice(webId);

    v.Category = ko.observable();
    v.F2 = ko.observable();
    v.InvoiceItemCollection = ko.observableArray();
    v.DocumentCollection = ko.observableArray();
    v.SentDate = ko.observable();//nillable
    v.Tenant = ko.observable(new bespoke.sphcommercialspace.domain.Tenant());
    v.Type2 = ko.observable();//type but not nillable
    v.Note2 = ko.observable();//type but not nillable
    v.Address = ko.observable(new bespoke.sphcommercialspace.domain.Address());
    if (bespoke.sphcommercialspace.domain.AdhocInvoicePartial) {
        return _(v).extend(new bespoke.sphcommercialspace.domain.AdhocInvoicePartial());
    }
    return v;
};



bespoke.sphcommercialspace.domain.InvoiceItem = function (webId) {

    var model = {
        Amount: ko.observable(),
        Category: ko.observable(),
        Note: ko.observable(),
        isBusy: ko.observable(false),
        WebId: ko.observable(webId)
    };
    if (bespoke.sphcommercialspace.domain.InvoiceItemPartial) {
        return _(model).extend(new bespoke.sphcommercialspace.domain.InvoiceItemPartial());
    }
    return model;
};


bespoke.sphcommercialspace.domain.Invoice = function (webId) {

    return {
        InvoiceId: ko.observable(),
        Date: ko.observable(),
        Amount: ko.observable(),
        No: ko.observable(),
        Type: ko.observable(),
        ContractNo: ko.observable(),
        TenantIdSsmNo: ko.observable(),
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
