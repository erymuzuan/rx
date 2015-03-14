/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.config],
	function (context, logger, router, config) {

	    var isBusy = ko.observable(false),
		logs = ko.observableArray().extend({ rateLimit: 250 }),
		list = ko.observableArray(),
		info = ko.observable(true),
		warning = ko.observable(true),
		error = ko.observable(true),
		filter = function () {
		    var temp = _(logs()).filter(function (v) {
		        if (info() && v.severity === "info") return true;
		        if (warning() && v.severity === "warning") return true;
		        if (error() && v.severity === "error") return true;
		        return false;
		    });
		    list(temp);
		},
		port = ko.observable(5030),
		host = ko.observable("localhost"),
		ws = null,
		scroll = function () {
		    var elem = document.getElementById("developers-log-footer");
		    elem.scrollTop = elem.scrollHeight;
		},
		start = function () {
		    var tcs = new $.Deferred(),
				support = "MozWebSocket" in window ? "MozWebSocket" : ("WebSocket" in window ? "WebSocket" : null);

		    if (support == null) {
		        logger.error("No WebSocket support for console notification");
		    }

		    logs.push({ message: "* Connecting to server ...", severity: "info" });
		    // create a new websocket and connect
		    ws = new window[support]("ws://" + host() + ":" + port() + "/");

		    ws.onmessage = function (evt) {
		        var model = JSON.parse(evt.data);
		        if (typeof model === "string") {
		            model = { message: model, severity: "info" };
		        }
		        var severity = model.severity || "info",
					message = model.message || "<empty message>";

		        logs.push(model);
		        scroll();
		        switch (severity) {
		            case "info":
		                console.info(message);
		                break;
		            case "warn":
		                console.warn(message);
		                toastr.options = {
		                    "closeButton": true,
		                    "debug": false,
		                    "newestOnTop": false,
		                    "progressBar": false,
		                    "positionClass": "toast-top-right",
		                    "preventDuplicates": false,
		                    "onclick": null,
		                    "showDuration": "300",
		                    "hideDuration": "0",
		                    "timeOut": "0",
		                    "extendedTimeOut": "0",
		                    "showEasing": "swing",
		                    "hideEasing": "swing",
		                    "showMethod": "fadeIn",
		                    "hideMethod": "fadeOut"
		                }
		                toastr["warning"](message, "Warning in your server");
		                break;
		            case "error":
		                console.error(message);
		                toastr.options = {
		                    "closeButton": true,
		                    "debug": false,
		                    "newestOnTop": false,
		                    "progressBar": false,
		                    "positionClass": "toast-top-right",
		                    "preventDuplicates": false,
		                    "onclick": null,
		                    "showDuration": "300",
		                    "hideDuration": "0",
		                    "timeOut": "0",
		                    "extendedTimeOut": "0",
		                    "showEasing": "swing",
		                    "hideEasing": "swing",
		                    "showMethod": "fadeIn",
		                    "hideMethod": "fadeOut"
		                }
		                toastr["error"](message, "Error in your server");
		                break;

		            default:
		                console.log(severity, message);
		        }
		        toastr.options.timeOut = 4000;
		        toastr.options.positionClass = "toast-bottom-full-width";
		        toastr.options.closeButton = true;

		    };

		    // when the connection is established, this method is called
		    ws.onopen = function () {
		        console.log("* Connection open");
		        logs.push({ message: "* Connection open", severity: "info" });
		        ws.send("web logger listener is listening");
		        tcs.resolve(true);
		        scroll();
		    };

		    // when the connection is closed, this method is called
		    ws.onclose = function () {
		        logger.error("* Connection closed");
		        logs.push({ message: "* Connection closed", severity: "warning" });
		        scroll();
		        tcs.resolve(false);
		    };

		    return tcs.promise();
		},
		activate = function () {

		},
		attached = function (view) {
		    if (config.roles.indexOf("developers") < 0) {
		        $("#developers-log-panel").hide();
		        return;
		    };
		    start();
		    error.subscribe(filter);
		    warning.subscribe(filter);
		    info.subscribe(filter);

		    logs.subscribe(function (changes) {

		        // For this example, we'll just print out the change info
		        _(changes).each(function (v) {

		            console.log(v);
		            if (v.status !== "added") {
		                return;
		            }
		            var entry = v.value;
		            if (entry.severity === "info" && info()) {
		                list.push(entry);
		            }
		            if (entry.severity === "warning" && warning()) {
		                list.push(entry);
		            }
		            if (entry.severity === "error" && error()) {
		                list.push(entry);
		            }
		        });

		    }, null, "arrayChange");
		},
		stop = function () {

		},
		    clear = function () {
		        logs([]);
		        list([]);
		    },
			setting = function () {

			},
	        viewStackTrace = function (log) {
	            require(["viewmodels/error.log.detail.dialog", "durandal/app"], function (dialog, app2) {
	                dialog.log(log);

	                app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                            }
                        });

	            });
	        };


	    var vm = {
	        isBusy: isBusy,
	        visible: config.roles.indexOf("developers") > -1,
	        list: list,
	        start: start,
	        stop: stop,
	        clear: clear,
	        error: error,
	        warning: warning,
	        info: info,
	        setting: setting,
	        viewStackTrace: viewStackTrace,
	        activate: activate,
	        attached: attached
	    };

	    return vm;

	});
