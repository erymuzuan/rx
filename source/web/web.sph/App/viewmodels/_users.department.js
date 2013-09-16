/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
	function (context, logger) {
	    var
        isBusy = ko.observable(false),
        activate = function () {
            var query = String.format("Key eq 'Departments'");
            var tcs = new $.Deferred();
            context.loadOneAsync("Setting", query)
                .done(function (s) {
                    if (s) {
                        var departments = JSON.parse(ko.mapping.toJS(s.Value));
                        vm.departments(departments);
                    }
                    tcs.resolve(true);
                });

            return tcs.promise();
        },
        addDepartment = function () {
            var department ={Name : ko.observable()};
            vm.departments.push(department);
        },
        removeDepartment = function (department) {
            vm.departments.remove(department);
        },
            saveDepartment = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({
                    settings: [{
                        Key: "Departments",
                        Value: ko.mapping.toJSON(vm.departments())
                    }]
                });;
                isBusy(true);
                context.post(data, "/Setting/Save")
                    .then(function (result) {
                        isBusy(false);
                        logger.log('All the setting has been saved', result, this, true);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        departments: ko.observableArray([]),
	        addCommand: addDepartment,
	        removeCommand: removeDepartment,
	        saveCommand: saveDepartment
	    };

	    return vm;

	});
