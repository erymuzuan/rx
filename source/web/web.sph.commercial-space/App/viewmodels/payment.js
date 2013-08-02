/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../services/Contract.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger'],
	function (context, logger) {

	    var isBusy = ko.observable(false),
	        payment = payment,
            activate = function () { return true; },
	        showDetails = function (data) {
	            vm.payment().ContractNo(data.ReferenceNo());
	            vm.payment().TenantIdSsmNo(data.Tenant.IdSsmNo());
	            $('#set-payment-modal').modal({});
	        },
	        
	        exportList = function (){},

            save = function () {
                var tcs = new $.Deferred();
                var payment = ko.mapping.toJS(vm.payment());
                var json = JSON.stringify({payment: payment });
                context.post(json, "/Payment/Save")
					.done(function (e) {
					    var query = String.format("ContractId gt 0");
					    context.loadAsync("Contract", query)
					        .then(function(lo) {
					            isBusy(false);
					            vm.contractCollection(lo.itemCollection);
					        });
					    logger.log("Payment received", e, "payment", true);
                        tcs.resolve(true);
                });
                return tcs.promise();
            };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        contractNo: ko.observable(),
	        idSsmNo: ko.observable(),
	        contractCollection: ko.observableArray([]),
	        showDetailsCommand: showDetails,
	        payment : ko.observable (new bespoke.sphcommercialspace.domain.Payment()),
	        saveCommand: save,
	        toolbar: ko.observable({
	            reloadCommand: function () {
	                return activate();
	            },
	            printCommand: ko.observable({
	                entity: ko.observable("Payment"),
	                id: ko.observable(0),
	                item: payment,
	            }),
	            exportCommand: exportList,
	        })

	    };

	    return vm;


	});
