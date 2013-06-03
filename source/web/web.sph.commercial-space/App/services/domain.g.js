﻿
/// <reference path="../../scripts/knockout-2.2.1.debug.js" />


var bespoke = bespoke || {};
bespoke.sphcommercialspace = {};
bespoke.sphcommercialspace.domain = {};




bespoke.sphcommercialspace.domain.Land = function () {


    return {
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
        Owner: new bespoke.sphcommercialspace.domain.Owner(),
        Address: new bespoke.sphcommercialspace.domain.Address()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Building = function () {


    return {
        BuildingId: ko.observable(),
        Name: ko.observable(),
        LotNo: ko.observable(),
        Size: ko.observable(),
        Status: ko.observable(),
        Floors: ko.observable(),
        Note: ko.observable(),
        Address: new bespoke.sphcommercialspace.domain.Address(),
        FloorCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Address = function () {


    return {
        State: ko.observable(),
        City: ko.observable(),
        Postcode: ko.observable(),
        Street: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Floor = function () {


    return {
        Name: ko.observable(),
        Size: ko.observable(),
        Number: ko.observable(),
        Note: ko.observable(),
        LotCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Lot = function () {


    return {
        Name: ko.observable(),
        Size: ko.observable(),
        FloorNo: ko.observable(),
        IsCommercialSpace: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.CommercialSpace = function () {


    return {
        CommercialSpaceId: ko.observable(),
        BuildingId: ko.observable(),
        LotName: ko.observable(),
        FloorName: ko.observable(),
        Size: ko.observable(),
        Category: ko.observable(),
        RentalRate: ko.observable(),
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
        LotCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.RentalApplication = function () {


    return {
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
        Address: new bespoke.sphcommercialspace.domain.Address(),
        BankCollection: ko.observableArray(),
        Contact: new bespoke.sphcommercialspace.domain.Contact(),
        AttachmentCollection: ko.observableArray(),
        Offer: new bespoke.sphcommercialspace.domain.Offer()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Attachment = function () {


    return {
        Type: ko.observable(),
        Name: ko.observable(),
        IsRequired: ko.observable(),
        IsReceived: ko.observable(),
        StoreId: ko.observable(),
        IsCompleted: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Bank = function () {


    return {
        Name: ko.observable(),
        Location: ko.observable(),
        AccountNo: ko.observable(),
        AccountType: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Contact = function () {


    return {
        ContractId: ko.observable(),
        Name: ko.observable(),
        Title: ko.observable(),
        IcNo: ko.observable(),
        Role: ko.observable(),
        MobileNo: ko.observable(),
        OfficeNo: ko.observable(),
        Email: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.ContractTemplate = function () {


    return {
        ContractTemplateId: ko.observable(),
        Type: ko.observable(),
        Description: ko.observable(),
        Status: ko.observable(),
        DocumentTemplateCollection: ko.observableArray(),
        TopicCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.DocumentTemplate = function () {


    return {
        Name: ko.observable(),
        StoreId: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Contract = function () {


    return {
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
        DocumentCollection: ko.observableArray(),
        Owner: new bespoke.sphcommercialspace.domain.Owner(),
        ContractingParty: new bespoke.sphcommercialspace.domain.ContractingParty(),
        Tenant: new bespoke.sphcommercialspace.domain.Tenant(),
        CommercialSpace: new bespoke.sphcommercialspace.domain.CommercialSpace(),
        TopicCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Document = function () {


    return {
        Title: ko.observable(),
        Extension: ko.observable(),
        DocumentVersionCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.DocumentVersion = function () {


    return {
        StoreId: ko.observable(),
        Date: ko.observable(),
        CommitedBy: ko.observable(),
        No: ko.observable(),
        Note: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Owner = function () {


    return {
        Name: ko.observable(),
        TelephoneNo: ko.observable(),
        FaxNo: ko.observable(),
        Email: ko.observable(),
        Address: new bespoke.sphcommercialspace.domain.Address()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.AuditTrail = function () {


    return {
        User: ko.observable(),
        DateTime: ko.observable(),
        Operation: ko.observable(),
        Type: ko.observable(),
        EntityId: ko.observable(),
        ChangeCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Change = function () {


    return {
        PropertyName: ko.observable(),
        Before: ko.observable(),
        After: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Organization = function () {


    return {
        Name: ko.observable(),
        RegistrationNo: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Offer = function () {


    return {
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
        DepositPaymentCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.OfferCondition = function () {


    return {
        Title: ko.observable(),
        Description: ko.observable(),
        Note: ko.observable(),
        IsRequired: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Topic = function () {


    return {
        Title: ko.observable(),
        Description: ko.observable(),
        Text: ko.observable(),
        ClauseCollection: ko.observableArray()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Clause = function () {


    return {
        Title: ko.observable(),
        Description: ko.observable(),
        No: ko.observable(),
        Text: ko.observable()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.ContractingParty = function () {


    return {
        Name: ko.observable(),
        RegistrationNo: ko.observable(),
        Contact: new bespoke.sphcommercialspace.domain.Contact(),
        Address: new bespoke.sphcommercialspace.domain.Address()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.Tenant = function () {


    return {
        Name: ko.observable(),
        RegistrationNo: ko.observable(),
        Id: ko.observable(),
        Address: new bespoke.sphcommercialspace.domain.Address()
    };//remove-last-comma
};




bespoke.sphcommercialspace.domain.DepositPayment = function () {


    return {
        ReceiptNo: ko.observable(),
        Amount: ko.observable(),
        Date: ko.observable()
    };//remove-last-comma
};

