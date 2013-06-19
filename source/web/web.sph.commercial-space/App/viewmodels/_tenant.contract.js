/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'durandal/app'],
	function (context,app) {

	    var
        isBusy = ko.observable(false),
        activate = function () {
            return true;
        },
        generateLedger = function () {
            var tcs = new $.Deferred();
            var data = JSON.stringify({ id : this.ContractId()});
            isBusy(true);
            context.post(data, "/Contract/GenerateLedger")
                .then(function(result) {
                    isBusy(false);
                    var message = "lejer "+ result+" disimpan di desktop anda" ;
                    app.showMessage(message, "Maklumat");
                    tcs.resolve(result);
                });
            return tcs.promise();
        };

	    var vm = {
	        isBusy: isBusy,
	        init: function (tenant) {
	            var query = "TenantIdSsmNo eq '" + tenant.IdSsmNo() +"'";
	            var tcs = new $.Deferred();
	            context.loadAsync("Contract", query).done(function (lo) {
	                vm.contractCollection(lo.itemCollection);
	                tcs.resolve(true);
	            });
	            return tcs.promise();
	        },
	        activate: activate,
	        contractCollection: ko.observableArray(),
	        generateCommand: generateLedger
	    };

	    return vm;

	});
