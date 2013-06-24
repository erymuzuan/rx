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
            var query = String.format("Key eq 'State'");
            var tcs = new $.Deferred();
            context.loadOneAsync("Setting", query)
                .done(function (s) {
                    if (s) {
                        var states =JSON.parse(ko.mapping.toJS(s.Value));
                        vm.states(states);
                    }
                    tcs.resolve(true);
                });

            return tcs.promise();
        },
        addState = function () {
            var state = new bespoke.sphcommercialspace.domain.State();
            vm.states.push(state);
        }, removeState= function(state) {
            vm.states.remove(state);
        },
            saveState = function () {
                var tcs = new $.Deferred();
                var data = JSON.stringify({
                    settings: [{
                        Key: "State",
                        Value: ko.mapping.toJSON(vm.states())
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
	        states: ko.observableArray([]),
	        addStateCommand: addState,
	        removeState: removeState,
	        saveStateCommand: saveState
	    };

	    return vm;

	});
