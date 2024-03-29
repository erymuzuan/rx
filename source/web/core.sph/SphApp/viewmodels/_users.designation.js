﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'plugins/router'],
	function (context,router) {

	    var isBusy = ko.observable(false),
	        activate = function (roles) {
	            vm.roles.removeAll();
	            _.each(roles, function(v, i) {
	                var r = { RoleName: ko.observable(''), Index: ko.observable() };
	                r.Index(i + 1);
	                r.RoleName(v);
	                vm.roles.push(r);
	            });
	            
	            var query = String.format("Id ne '0'");
	            var tcs = new $.Deferred();
	            context.loadAsync("Designation", query)
                    .done(function (lo) {
                        vm.designationCollection(lo.itemCollection);
                        tcs.resolve(true);
                    });

	            return tcs.promise();
	        },
	        add = function() {
	            var designation = {
	                Name: ko.observable(),
	                Roles: ko.observableArray()
	            };
	            vm.designationCollection.push(designation);
	        },
	        save = function() {
	            var tcs = new $.Deferred();
	            var data = JSON.stringify({
	                settings: [{
	                    Key: "Designation.Role",
	                    Value: ko.mapping.toJSON(vm.designationCollection())
	                }]
	            });

	            isBusy(true);
	            context.post(data, "/Setting/Save")
	                .then(function(result) {
	                    isBusy(false);
	                    tcs.resolve(result);
	                });
	            return tcs.promise();
	        };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        designation : {
	            Name: ko.observable(),
	            Roles: ko.observableArray()
	        },
	        designationCollection: ko.observableArray(),
	        roles: ko.observableArray(),
	        saveCommand: save,
	        addCommand : add
	    };

	    return vm;

	});
