/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};

bespoke.sph.domain.LogEntry = function (optionOrWebid) {
    var model = {
        severity: ko.observable(""),
        log: ko.observable(""),
        source: ko.observable(""),
        operation: ko.observable(""),
        user: ko.observable(""),
        computer: ko.observable(""),
        time: ko.observable(""),
        message: ko.observable(""),
        id: ko.observable(""),
        keywords: ko.observableArray([]),
        details: ko.observable(""),
        callerFilePath: ko.observable(""),
        callerMemberName: ko.observable(""),
        callerLineNumber: ko.observable(""),
        WebId: ko.observable()
    };
    if (optionOrWebid && typeof optionOrWebid === "object") {
        for (var n in optionOrWebid) {
            if (optionOrWebid.hasOwnProperty(n)) {
                if (typeof model[n] === "function") {
                    model[n](optionOrWebid[n]);
                }
            }
        }
    }
    if (optionOrWebid && typeof optionOrWebid === "string") {
        model.WebId(optionOrWebid);
    }


    return model;
}

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.config],
	function (context, logger, router, config) {
	    "use strict";
	    var OUTPUT_LOGS_SETTING = "output.logs.setting";
	    var isBusy = ko.observable(false),
		logs = ko.observableArray().extend({ rateLimit: 250 }),
		list = ko.observableArray(),
		subscribers = ko.observableArray(),
		connected = ko.observable(true),
		setting = ko.observable({
		    port: ko.observable(5030),
		    max: ko.observable(200),
		    host: ko.observable("localhost"),
		    iis: {
		        excludeStatusCodes: ko.observableArray(["101"]),
		        excludeWhenContains: ko.observable("/signalr_")
		    }
		}),
		filterText = ko.observable("").extend({ rateLimit: { method: "notifyWhenChangesStop", timeout: 300 } }),
        logFilter = {
            application: ko.observable(true),
            workflow: ko.observable(true),
            schedulers: ko.observable(true),
            subscribers: ko.observable(true),
            webServer: ko.observable(true),
            elasticsearch: ko.observable(true),
            sqlRepository: ko.observable(true),
            sqlPersistence: ko.observable(true),
            logger: ko.observable(true),
            objectBuilder: ko.observable(true),
            security: ko.observable(true)
        },
		verbose = ko.observable(true),
		info = ko.observable(true),
		log = ko.observable(true),
		debug = ko.observable(true),
		warning = ko.observable(true),
		error = ko.observable(true),
		critical = ko.observable(true),
		filter = function () {
		    var temp = _(logs()).filter(function (v) {
		        var sv = ko.unwrap(v.severity);

		        switch (sv) {
		            case "Verbose": return verbose();
		            case "Info": return info();
		            case "Log": return log();
		            case "Debug": return debug();
		            case "Warning": return warning();
		            case "Error": return error();
		            case "Critical": return critical();

		            default:
		                return false;
		        }
		    });
		    list(temp);
		},
		ws = null,
		scroll = function () {
		    var elem = document.getElementById("developers-log-footer");
		    elem.scrollTop = elem.scrollHeight;
		},
        cleanup = function () {
            setInterval(function () {
                var temp = logs(),
                    count = temp.length,
                    max = setting().max();
                if (count <= max) {
                    return;
                }
                console.log("Running cleaning up..");
                temp.splice(0, count - max);
                list(temp);
                scroll();
            }, 10000);
        },
	    runFilter = function (entry) {

	        var message = ko.unwrap(entry.message);
	        if (entry.log === "WebServer") {
	            var any = _(setting().iis.excludeWhenContains().split(";")).any(function (v) {
	                return (message || "<>").indexOf(v) > -1;
	            });
	            if (any) {
	                return false;
	            }

	            var codes = /HTTP status ([0-9]{3})/.exec(message);
	            var code = _(setting().iis.excludeStatusCodes()).any(function (v) {
	                if (!_.isArray(codes)) {
	                    return false;
	                }
	                if (codes.length < 2) {
	                    return false;
	                }
	                if (codes[1] === v) {
	                    return true;
	                }

	                return false;
	            });
	            if (code) {
	                return false;
	            }
	        }

	        return true;
	    },
		start = function () {
		    var tcs = new $.Deferred(),
				support = "MozWebSocket" in window ? "MozWebSocket" : ("WebSocket" in window ? "WebSocket" : null);

		    if (support == null) {
		        logger.error("No WebSocket support for console notification");
		    }

		    logs.push(new bespoke.sph.domain.LogEntry({ message: "* Connecting to server ...", time: "[" + moment().format("HH:mm:ss") + "]", severity: "Info" }));
		    // create a new websocket and connect
		    ws = new window[support]("ws://" + setting().host() + ":" + setting().port() + "/");

		    ws.onmessage = function (evt) {
		        var model = JSON.parse(evt.data),
		            severity = model.severity,
		            message = model.message;

		        model.message = model.message.replace(/\r\n/g, "<br/>");
		        model.time = "[" + moment(model.time).format("HH:mm:ss") + "]";
		        if (model.severity === "info") {
		            model.severity = "Info";
		            severity = "Info";
		        }

		        if (!runFilter(model)) {
		            return;
		        }

		        logs.push(new bespoke.sph.domain.LogEntry(model));
		        scroll();
		        switch (severity) {
		            case "Verbose":
		                console.log(message);
		            case "Info":
		            case "Log":
		            case "Debug":
		                console.info(message);
		                break;
		            case "Warning":
		                console.warn(message);
		                break;
		            case "Error":
		            case "Critical":
		                console.error(message);
		                break;
		            default:
		                console.log(severity, model.message);
		        }


		    };

		    // when the connection is established, this method is called
		    ws.onopen = function () {
		        console.log("* Connection open");
		        logs.push(new bespoke.sph.domain.LogEntry({ message: "* Connection open", time: "[" + moment().format("HH:mm:ss") + "]", severity: "Info" }));

		        ws.send("web logger listener is listening");
		        tcs.resolve(true);
		        scroll();
		        connected(true);

		        ws.send("GET subscribers");
		    };

		    // when the connection is closed, this method is called
		    ws.onclose = function () {
		        logger.error("* Connection closed");
		        logs.push(new bespoke.sph.domain.LogEntry({ message: "* Connection closed", time: "[" + moment().format("HH:mm:ss") + "]", severity: "Warning" }));
		        scroll();
		        connected(false);
		        tcs.resolve(false);
		    };

		    return tcs.promise();
		},
		activate = function () {
		    var sj = localStorage.getItem(OUTPUT_LOGS_SETTING);
		    if (sj) {
		        setting(ko.mapping.fromJSON(sj));
		    }
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
		    verbose.subscribe(filter);
		    critical.subscribe(filter);
		    log.subscribe(filter);
		    debug.subscribe(filter);
		    var self = this;
		    logs.subscribe(function (changes) {

		        // For this example, we'll just print out the change info
		        _(changes).each(function (v) {


		            if (v.status !== "added") {
		                return;
		            }
		            var entry = v.value,
		                severity = ko.unwrap(entry.severity);
		            if (typeof self[severity.toLowerCase()] === "function" && self[severity.toLocaleLowerCase()]()) {
		                list.push(entry);
		            }
		        });

		    }, null, "arrayChange");

		    $("#developers-log-panel-collapse", view).on("click", function (e) {
		        e.preventDefault();
		        $("#developers-log-panel > div.tabbable").hide();

		    });
		    $("#developers-log-panel-expand", view).on("click", function (e) {
		        e.preventDefault();
		        $("#developers-log-panel > div.tabbable").show();

		    });


		},
		stop = function () {
		    ws.close();
		},
		clear = function () {
		    logs([]);
		    list([]);
		},
		showSettingDialog = function () {
		    require(["viewmodels/output.setting.dialog", "durandal/app"], function (dialog, app2) {
		        dialog.setting(setting());

		        app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            setting(dialog.setting());
                            localStorage.setItem(OUTPUT_LOGS_SETTING, ko.toJSON(dialog.setting));
                        }
                    });

		    });
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
	    },
	    clearLogsFilter = function () {

	    };

	    filterText.subscribe(function (text) {
	        if (!text) {
	            list(logs());
	            return;
	        }
	        var temp = _(logs()).filter(function (v) {
	            return ko.unwrap(v.message).toLowerCase().indexOf(text.toLowerCase()) > -1;
	        });
	        list(temp);
	    });

	    setTimeout(cleanup, 500);
	    var vm = {
	        isBusy: isBusy,
	        visible: config.roles.indexOf("developers") > -1,
	        list: list,
	        subscribers: subscribers,
	        connected: connected,
	        start: start,
	        stop: stop,
	        clear: clear,
	        verbose: verbose,
	        info: info,
	        debug: debug,
	        log: log,
	        warning: warning,
	        error: error,
	        critical: critical,
	        logFilter: logFilter,
	        clearLogsFilter: clearLogsFilter,
	        filterText: filterText,
	        showSettingDialog: showSettingDialog,
	        viewStackTrace: viewStackTrace,
	        activate: activate,
	        attached: attached
	    };

	    return vm;

	});
