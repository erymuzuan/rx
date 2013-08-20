/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define([],
	function () {

	    var
        isBusy = ko.observable(false),
        activate = function () {
            return true;
        },
	        viewAttached = function (view) {
	            $(view).find('.carousel').carousel({
	                interval: 2000
	            });
	        };

	    var vm = {
	        viewAttached: viewAttached,
	        isBusy: isBusy,
	        activate: activate
	    };

	    return vm;

	});
