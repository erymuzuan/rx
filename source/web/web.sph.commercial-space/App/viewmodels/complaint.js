/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../services/mockComplainTemplateContext.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
	function (context) {

	    var
        isBusy = ko.observable(false),
        activate = function () {
            var query = String.format("ComplaintTemplateId gt 0");
            var tcs = new $.Deferred();
            context.loadAsync("ComplaintTemplate", query)
                .done(function (lo) {
                    isBusy(false);
                    var categoryOptions = ko.observableArray();
                    _.each(lo.itemCollection, function (t) {
                        categoryOptions.push(t.Category());
                    });
                    var distinctCategories = ko.observableArray();
                    distinctCategories(_.uniq(categoryOptions()));
                    var items = _(distinctCategories()).map(function (t) {
                        var filtered = _(lo.itemCollection).filter(function (c) {
                            return c.Category() === t;
                        });
                        return {
                            name: t,
                            templates: filtered
                        };
                    });
                    vm.complaintTemplates(items);

                    tcs.resolve(true);
                });
            return tcs.promise();
        };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        complaintTemplates: ko.observableArray()
	    };

	    return vm;

	});
