
    /// <reference path="../../scripts/knockout-2.2.1.debug.js" />


    var bespoke = bespoke || {};
    bespoke.sphcommercialspace =  {};
    bespoke.sphcommercialspace.domain = {};
    
    

          bespoke.sphcommercialspace.domain.ContractHistory = function(webId) {
        

return {
          ContractNo : ko.observable(),
              DateFrom : ko.observable(),
              TenantName : ko.observable(),
              DateEnd : ko.observable(),
              DateStart : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.PaymentDistribution = function(webId) {
        

return {
          PaymentId : ko.observable(),
              ReceiptNo : ko.observable(),
              DateTime : ko.observable(),
              Amount : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Rent = function(webId) {
        

return {
          RentId : ko.observable(),
              Type : ko.observable(),
              InvoiceNo : ko.observable(),
              Date : ko.observable(),
              Amount : ko.observable(),
              ContractId : ko.observable(),
              TenantId : ko.observable(),
              Half : ko.observable(),
              Quarter : ko.observable(),
              IsPaid : ko.observable(),
              PaymentDistributionCollection : ko.observableArray(),
		Month : ko.observable(),
		Year : ko.observable(),
		Tenant : ko.observable(new bespoke.sphcommercialspace.domain.Tenant()),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Tenant = function(webId) {
        

return {
          TenantId : ko.observable(),
              IdSsmNo : ko.observable(),
              Name : ko.observable(),
              BussinessType : ko.observable(),
              PhoneNo : ko.observable(),
              FaksNo : ko.observable(),
              MobilePhoneNo : ko.observable(),
              Email : ko.observable(),
              RegistrationNo : ko.observable(),
              ContractHistoryCollection : ko.observableArray(),
		Address : ko.observable(new bespoke.sphcommercialspace.domain.Address()),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Land = function(webId) {
        

return {
          Lot : ko.observable(),
              Title : ko.observable(),
              Location : ko.observable(),
              Size : ko.observable(),
              SizeUnit : ko.observable(),
              CurrentMarketValue : ko.observable(),
              RezabNo : ko.observable(),
              SheetNo : ko.observable(),
              ApprovedPlanNo : ko.observable(),
              LandOffice : ko.observable(),
              Usage : ko.observable(),
              Note : ko.observable(),
              OwnLevel : ko.observable(),
              PlanNo : ko.observable(),
              Status : ko.observable(),
              IsApproved : ko.observable(),
              Owner : ko.observable(new bespoke.sphcommercialspace.domain.Owner()),
		Address : ko.observable(new bespoke.sphcommercialspace.domain.Address()),
		LeaseExpiryDate : ko.observable(),
		LeasePeriod : ko.observable(),
		ApprovedDateTime : ko.observable(),
		ApprovedBy : ko.observable(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Building = function(webId) {
        

return {
          BuildingId : ko.observable(),
              Name : ko.observable(),
              LotNo : ko.observable(),
              Size : ko.observable(),
              Status : ko.observable(),
              Floors : ko.observable(),
              Note : ko.observable(),
              Address : ko.observable(new bespoke.sphcommercialspace.domain.Address()),
		FloorCollection : ko.observableArray(),
		Height : ko.observable(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.LatLng = function(webId) {
        

return {
          Lat : ko.observable(),
              Lng : ko.observable(),
              Elevation : ko.observable(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Address = function(webId) {
        

return {
          UnitNo : ko.observable(),
              Floor : ko.observable(),
              Block : ko.observable(),
              Street : ko.observable(),
              City : ko.observable(),
              Postcode : ko.observable(),
              State : ko.observable(),
              Country : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Floor = function(webId) {
        

return {
          Name : ko.observable(),
              Size : ko.observable(),
              Number : ko.observable(),
              Note : ko.observable(),
              LotCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Lot = function(webId) {
        

return {
          Name : ko.observable(),
              Size : ko.observable(),
              FloorNo : ko.observable(),
              IsCommercialSpace : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.CommercialSpace = function(webId) {
        

return {
          CommercialSpaceId : ko.observable(),
              BuildingId : ko.observable(),
              LotName : ko.observable(),
              FloorName : ko.observable(),
              Size : ko.observable(),
              Category : ko.observable(),
              RentalRate : ko.observable(),
              RentalType : ko.observable(),
              IsOnline : ko.observable(),
              RegistrationNo : ko.observable(),
              Status : ko.observable(),
              ContactPerson : ko.observable(),
              ContactNo : ko.observable(),
              State : ko.observable(),
              City : ko.observable(),
              BuildingName : ko.observable(),
              BuildingLot : ko.observable(),
              LotCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.RentalApplication = function(webId) {
        

return {
          RentalApplicationId : ko.observable(),
              CompanyName : ko.observable(),
              CompanyRegistrationNo : ko.observable(),
              DateStart : ko.observable(),
              DateEnd : ko.observable(),
              Purpose : ko.observable(),
              CompanyType : ko.observable(),
              CommercialSpaceId : ko.observable(),
              Status : ko.observable(),
              Experience : ko.observable(),
              IsRecordExist : ko.observable(),
              PreviousAddress : ko.observable(),
              IsCompany : ko.observable(),
              Type : ko.observable(),
              Remarks : ko.observable(),
              RegistrationNo : ko.observable(),
              ApplicationDate : ko.observable(),
              Address : ko.observable(new bespoke.sphcommercialspace.domain.Address()),
		BankCollection : ko.observableArray(),
		Contact : ko.observable(new bespoke.sphcommercialspace.domain.Contact()),
		CurrentYearSales : ko.observable(),
		LastYearSales : ko.observable(),
		PreviousYearSales : ko.observable(),
		AttachmentCollection : ko.observableArray(),
		Offer : ko.observable(new bespoke.sphcommercialspace.domain.Offer()),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Attachment = function(webId) {
        

return {
          Type : ko.observable(),
              Name : ko.observable(),
              IsRequired : ko.observable(),
              IsReceived : ko.observable(),
              StoreId : ko.observable(),
              IsCompleted : ko.observable(),
              ReceivedDateTime : ko.observable(),
		ReceivedBy : ko.observable(),
		Note : ko.observable(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Bank = function(webId) {
        

return {
          Name : ko.observable(),
              Location : ko.observable(),
              AccountNo : ko.observable(),
              AccountType : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Contact = function(webId) {
        

return {
          ContractId : ko.observable(),
              Name : ko.observable(),
              Title : ko.observable(),
              IcNo : ko.observable(),
              Role : ko.observable(),
              MobileNo : ko.observable(),
              OfficeNo : ko.observable(),
              Email : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.ContractTemplate = function(webId) {
        

return {
          ContractTemplateId : ko.observable(),
              Type : ko.observable(),
              Description : ko.observable(),
              Status : ko.observable(),
              DocumentTemplateCollection : ko.observableArray(),
		TopicCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.DocumentTemplate = function(webId) {
        

return {
          Name : ko.observable(),
              StoreId : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Contract = function(webId) {
        

return {
          ContractId : ko.observable(),
              ReferenceNo : ko.observable(),
              Type : ko.observable(),
              Date : ko.observable(),
              Value : ko.observable(),
              Title : ko.observable(),
              Remarks : ko.observable(),
              Period : ko.observable(),
              PeriodUnit : ko.observable(),
              StartDate : ko.observable(),
              EndDate : ko.observable(),
              RentalApplicationId : ko.observable(),
              Status : ko.observable(),
              RentType : ko.observable(),
              DocumentCollection : ko.observableArray(),
		Owner : ko.observable(new bespoke.sphcommercialspace.domain.Owner()),
		ContractingParty : ko.observable(new bespoke.sphcommercialspace.domain.ContractingParty()),
		Option : ko.observable(),
		Tenant : ko.observable(new bespoke.sphcommercialspace.domain.Tenant()),
		CommercialSpace : ko.observable(new bespoke.sphcommercialspace.domain.CommercialSpace()),
		TopicCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Document = function(webId) {
        

return {
          Title : ko.observable(),
              Extension : ko.observable(),
              DocumentVersionCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.DocumentVersion = function(webId) {
        

return {
          StoreId : ko.observable(),
              Date : ko.observable(),
              CommitedBy : ko.observable(),
              No : ko.observable(),
              Note : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Owner = function(webId) {
        

return {
          Name : ko.observable(),
              TelephoneNo : ko.observable(),
              FaxNo : ko.observable(),
              Email : ko.observable(),
              Address : ko.observable(new bespoke.sphcommercialspace.domain.Address()),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.AuditTrail = function(webId) {
        

return {
          User : ko.observable(),
              DateTime : ko.observable(),
              Operation : ko.observable(),
              Type : ko.observable(),
              EntityId : ko.observable(),
              ChangeCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Change = function(webId) {
        

return {
          PropertyName : ko.observable(),
              OldValue : ko.observable(),
              NewValue : ko.observable(),
              Action : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Organization = function(webId) {
        

return {
          Name : ko.observable(),
              RegistrationNo : ko.observable(),
              Address : ko.observable(new bespoke.sphcommercialspace.domain.Address()),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Offer = function(webId) {
        

return {
          BusinessPlan : ko.observable(),
              CommercialSpaceId : ko.observable(),
              Size : ko.observable(),
              Building : ko.observable(),
              Floor : ko.observable(),
              Deposit : ko.observable(),
              Rent : ko.observable(),
              Date : ko.observable(),
              ExpiryDate : ko.observable(),
              Period : ko.observable(),
              PeriodUnit : ko.observable(),
              Option : ko.observable(),
              OfferConditionCollection : ko.observableArray(),
		BusinessPlanText : ko.observable(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.OfferCondition = function(webId) {
        

return {
          Title : ko.observable(),
              Description : ko.observable(),
              Note : ko.observable(),
              IsRequired : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Topic = function(webId) {
        

return {
          Title : ko.observable(),
              Description : ko.observable(),
              Text : ko.observable(),
              ClauseCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Clause = function(webId) {
        

return {
          Title : ko.observable(),
              Description : ko.observable(),
              No : ko.observable(),
              Text : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.ContractingParty = function(webId) {
        

return {
          Name : ko.observable(),
              RegistrationNo : ko.observable(),
              Contact : ko.observable(new bespoke.sphcommercialspace.domain.Contact()),
		Address : ko.observable(new bespoke.sphcommercialspace.domain.Address()),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.DepositPayment = function(webId) {
        

return {
          ReceiptNo : ko.observable(),
              Amount : ko.observable(),
              Date : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Payment = function(webId) {
        

return {
          PaymentId : ko.observable(),
              Amount : ko.observable(),
              ReceiptNo : ko.observable(),
              Date : ko.observable(),
              ParentId : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Invoice = function(webId) {
        

return {
          InvoiceId : ko.observable(),
              Date : ko.observable(),
              Amount : ko.observable(),
              No : ko.observable(),
              Type : ko.observable(),
              ParentId : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.User = function(webId) {
        

return {
          UserId : ko.observable(),
              OrganizationId : ko.observable(),
              UserName : ko.observable(),
              Email : ko.observable(),
              FullName : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Deposit = function(webId) {
        

return {
          DepositId : ko.observable(),
              DateTime : ko.observable(),
              Amount : ko.observable(),
              IsPaid : ko.observable(),
              IsRefund : ko.observable(),
              IsVoid : ko.observable(),
              PaymentDateTime : ko.observable(),
		RefundDateTime : ko.observable(),
		DueDate : ko.observable(),
		DepositPaymentCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Role = function(webId) {
        

return {
          RoleId : ko.observable(),
              Name : ko.observable(),
              PermissionCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Permission = function(webId) {
        

return {
          IsAuthorized : ko.observable(),
              Name : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Complaint = function(webId) {
        

return {
          ComplaintId : ko.observable(),
              No : ko.observable(),
              Status : ko.observable(),
              Title : ko.observable(),
              Text : ko.observable(),
              Category : ko.observable(),
              Contact : ko.observable(new bespoke.sphcommercialspace.domain.Contact()),
		Address : ko.observable(new bespoke.sphcommercialspace.domain.Address()),
		ReplyCollection : ko.observableArray(),
		isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        

          bespoke.sphcommercialspace.domain.Reply = function(webId) {
        

return {
          Title : ko.observable(),
              Text : ko.observable(),
              isBusy : ko.observable(false),
		  WebId : ko.observable(webId)
          };
          };
          
        