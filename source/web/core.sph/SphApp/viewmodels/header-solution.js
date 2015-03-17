define(['services/datacontext', 'services/logger', 'plugins/router'],
	function (context, logger, router) {

	    var isBusy = ko.observable(false),
			activate = function () {
			},
			attached = function (view) {

			},
	        addNewSolution = function () {
	            require(["viewmodels/add.solution", "durandal/app"], function (dialog, app2) {
	                app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                context.post(ko.toJSON(dialog.solution), "/Solution")
                                        .done(function (edr) {
                                            if (edr.success) {
                                                logger.info("Done!!!!");
                                            }
                                        });
                            }
                        });

	            });
	        };

	    var vm = {
	        isBusy: isBusy,
	        activate: activate,
	        attached: attached,
	        addNewSolution: addNewSolution
	    };

	    return vm;

	});
