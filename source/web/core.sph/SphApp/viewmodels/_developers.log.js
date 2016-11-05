/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../../Scripts/_task.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};

bespoke.sph.domain.LogEntry = function (optionOrWebid) {
    const model = {
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
};

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.config],
	function (context, logger, router, config) {
	    "use strict";

	    let ws = null;
	    const outputLogsSetting = "output.logs.setting",
            isBusy = ko.observable(false),
            mute = ko.observable(false),
		    logs = ko.observableArray().extend({ rateLimit: 250 }),
		    list = ko.observableArray(),
		    subscribers = ko.observableArray(),
		    outputFiles = ko.observableArray(),
		    connected = ko.observable(true),
		    setting = ko.observable({
		        port: ko.observable(50238),
		        max: ko.observable(200),
		        host: ko.observable("localhost"),
		        iis: {
		            excludeStatusCodes: ko.observableArray(["101"]),
		            excludeWhenContains: ko.observable("/signalr_;Field.png;IncrementMessages;WriteFragment;BufferType;CompletionExpected;BufferType;No of Buffers;WriteCompletedInline;Action = 0;")
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
		    scroll = function () {
		        var elem = document.getElementById("developers-log-footer");
		        elem.scrollTop = elem.scrollHeight;
		    },
		    clear = function () {
		        logs([]);
		        list([]);
		        console.clear();
		    },
            cleanup = function () {
                setInterval(function () {
                    var temp = logs(),
                        count = temp.length,
                        max = setting().max();
                    if (count <= max) {
                        return;
                    }
                    if (!mute())
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
		        const tcs = new $.Deferred(),
				    support = "MozWebSocket" in window ? "MozWebSocket" : ("WebSocket" in window ? "WebSocket" : null);

		        if (support === null) {
		            logger.error("No WebSocket support for console notification");
		        }

		        logs.push(new bespoke.sph.domain.LogEntry({ message: `* Connecting to server on port ${setting().port()}....`, time: `[${moment().format("HH:mm:ss")}]`, severity: "Info" }));
		        if (isNaN(setting().port())) {
		            logs.push(new bespoke.sph.domain.LogEntry({ message: `!Port no. is not set : ${setting().port()}....`, time: `[${moment().format("HH:mm:ss")}]`, severity: "Info" }));
		            return Task.fromResult(0);
		        }
		        // create a new websocket and connect
		        ws = new window[support](`ws://${setting().host()}:${setting().port()}/`);

		        ws.onmessage = function (evt) {
		            const model = JSON.parse(evt.data),
                        message = model.message;
		            let severity = model.severity;


		            model.message = model.message.replace(/\r\n/g, "<br/>");
		            model.time = `[${moment(model.time).format("HH:mm:ss")}]`;
		            if (model.severity === "info") {
		                model.severity = "Info";
		                severity = "Info";
		            }
		            if (typeof model.outputFile === "string") {

		                const existing = _(outputFiles()).find(function (v) { return v.outputFile === model.outputFile; });
		                if (existing) {
		                    outputFiles.remove(existing);
		                }

		                model.deployed = ko.observable(false);
		                outputFiles.push(model);
		                return;
		            }

		            if (!runFilter(model)) {
		                return;
		            }

		            logs.push(new bespoke.sph.domain.LogEntry(model));
		            scroll();

		            if (mute()) {
		                return;
		            }
		            switch (severity) {
		                case "Verbose":
		                    console.log(message);
		                    break;
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

		        ws.onopen = function () {

		            if (!mute())
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

		            if (!mute())
		                console.error("* Connection closed");
		            logs.push(new bespoke.sph.domain.LogEntry({ message: "* Connection closed", time: "[" + moment().format("HH:mm:ss") + "]", severity: "Warning" }));
		            scroll();
		            connected(false);
		            ws.onmessage = null;
		            ws.onopen = null;
		            ws.onclose = null;
		            ws = null;

		            // restart
		            var refresh = setInterval(function () {
		                context.loadAsync("Setting", "")
                            .fail(function () {

                            })
                            .done(function () {
                                clearInterval(refresh);
                                if (tcs.state() !== "resolved") {
                                    start().done(function () {

                                        if (!mute())
                                            console.log("Web socket re-connect!");
                                    });
                                }
                                tcs.resolve(true);
                            });

		            }, 5000);

		        };

		        return tcs.promise();
		    },
            logDisplaySetting = function (prop, propName) {
                var key = "developers.log." + propName;
                prop(localStorage.getItem(key) === "true");
                prop.subscribe(function (m) {
                    localStorage.setItem(key, m);
                });
            },
		    activate = function () {

		        logDisplaySetting(mute, "mute");
		        logDisplaySetting(info, "info");
		        logDisplaySetting(debug, "debug");
		        logDisplaySetting(warning, "warning");
		        logDisplaySetting(error, "error");
		        logDisplaySetting(critical, "critical");
		        logDisplaySetting(verbose, "verbose");
		        const sj = localStorage.getItem(outputLogsSetting);
		        if (sj) {
		            setting(ko.mapping.fromJSON(sj));
		        }

		        return $.get("developer-service/environment-variables")
                    .done(function (result) {
                        const port = result[`RX_${config.applicationName.toUpperCase()}_LoggerWebSocketPort`];
                        setting().port(parseInt(port));
                    });
		    },
            attached = function (view) {
                if (config.roles.indexOf("developers") < 0) {
                    $("#developers-log-panel").hide();
                    return;
                }
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
                    $("#developers-log-panel").css("bottom", "0");
                    $("#developers-log-panel").css("left", "0");
                    $("#developers-log-panel").css("position", "fixed");
                    $("#developers-log-panel").css("width", "100%");
                    $("#developers-log-panel").css("background-color", "transparent");

                    $("#content").css({
                        "margin-left": "",
                        "height": "",
                        "overflow-y": "",
                        "min-height": "",
                        "max-height": ""
                    });

                });
                var expand = function () {
                    var dev = $("#developers-log-panel").height(),
                        top = $(window).height(),
                        height = top - dev - 90; //TODO: WHERE's the magic no coming from
                    $("#content").css({
                        "margin-left": 0,
                        "height": "100%",
                        "overflow-y": "scroll",
                        "max-height": height,
                        "min-height": height
                    });
                };
                $("#developers-log-panel-expand", view).on("click", function (e) {
                    e.preventDefault();
                    $("#developers-log-panel > div.tabbable").show();
                    //$("#developers-log-panel").css("bottom", "0px");
                    $("#developers-log-panel").css("left", "");
                    $("#developers-log-panel").css("position", "");
                    $("#developers-log-panel").css("width", "");
                    $("#developers-log-panel").css("background-color", "whitesmoke");
                    $("html, body").animate({ scrollTop: $(document).height() }, 1000);

                    expand();

                }).trigger("click");
                $(window).resize(function () {
                    if ($("#developers-log-panel > div.tabbable:visible").length > 0) {
                        expand();
                    }
                });

                $(document).on("keyup", function (e) {
                    if (e.shiftKey & e.ctrlKey && (e.keyCode === 76)) {
                        clear();
                    }
                });


            },
            stop = function () {
                ws.close();
            },
            showSettingDialog = function () {
                require(["viewmodels/output.setting.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.setting(setting());

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                setting(dialog.setting());
                                localStorage.setItem(outputLogsSetting, ko.toJSON(dialog.setting));
                            }
                        });

                });
            },
            viewFile = function (e) {
                const file = e.FileName || e;
                const line = e.Line || 1;
                const params = [
                    `height=${screen.height}`,
                    `width=${screen.width}`,
                    "toolbar=0",
                    "location=0",
                    "fullscreen=yes"
                ].join(",");
                const editor = window.open(`/sph/editor/file?id=${file.replace(/\\/g, "/")}&line=${line}`, "_blank", params);
                editor.moveTo(0, 0);
            },
            viewStackTrace = function (logEntry) {
                if (typeof logEntry.buildError === "object") {
                    viewFile(logEntry.buildError);
                    return;
                }
                require(["viewmodels/error.log.detail.dialog", "durandal/app"], function (dialog, app2) {
                    dialog.log(logEntry);

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                            }
                        });

                });
            },
            clearLogsFilter = function () {

            },
	        showControlCenter = function () {
	            ws.send("POST /bring-to-view:");
	        },
	        deployOutputFiles = function () {
	            var files = _(ko.unwrap(outputFiles)).map(function (v) { return v.outputFile; });
	            ws.send("POST /deploy:" + files.join(";"));
	            outputFiles([]);

	            // once deployed, wait few seconds and issue a web request to restart the server
	            var query = "Key eq 'Departments'",
                    tcs = new $.Deferred();
	            var refresh = setInterval(function () {
	                context.loadAsync("Setting", query)
                        .fail(function () {

                        })
                        .done(function () {
                            clearInterval(refresh);
                            if (tcs.state() !== "resolved") {
                                start().done(tcs.resolve);
                            }
                        });

	            }, 2000);
	            return tcs.promise();

	        },
	        selectAllOutputFiles = function () {

	        },
	        clearOutputFiles = function () {
	            outputFiles([]);
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
	    const vm = {
	        isBusy: isBusy,
	        mute: mute,
	        viewFile: viewFile,
	        visible: config.roles.indexOf("developers") > -1,
	        list: list,
	        subscribers: subscribers,
	        outputFiles: outputFiles,
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
	        attached: attached,
	        selectAllOutputFiles: selectAllOutputFiles,
	        showControlCenter: showControlCenter,
	        deployOutputFiles: deployOutputFiles,
	        clearOutputFiles: clearOutputFiles
	    };

	    return vm;

	});
