/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(["services/datacontext", "services/logger", "plugins/router"],
	function (context, logger) {
	    var departments = ko.observableArray([]),
            isBusy = ko.observable(false),
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
                        logger.log("All the setting has been saved", result, this, true);
                        tcs.resolve(result);
                    });
                return tcs.promise();
            },
            activate = function () {
                var query = String.format("Key eq 'Departments'");
                return context.loadOneAsync("Setting", query)
                    .done(function (s) {
                        if (s) {
                            var list = _(JSON.parse(ko.mapping.toJS(s.Value)))
                            .filter(function (v) {
                                return v.Name;
                            });
                            departments(list);
                        }
                    });

            },
	        attached = function (view) {
	            $(view).on("blur", "input.department-field", function (e) {
	                if ($(this).val()) {
	                    saveDepartment();
	                }
	            });
	        },
            addDepartment = function () {
                var d = { Name: ko.observable() };
                departments.push(d);
            },
            removeDepartment = function (department) {
                departments.remove(department);
                saveDepartment();
            };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        attached: attached,
	        departments:departments,
	        addCommand: addDepartment,
	        removeCommand: removeDepartment,
	        saveCommand: saveDepartment
	    };

	    return vm;

	});
