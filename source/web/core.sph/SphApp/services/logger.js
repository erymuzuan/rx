define(["durandal/system"],
    function (system) {


        function logIt(message, data, source, showToast, toastType) {
            source = source ? "[" + source + "] " : "";
            if (data) {
                system.log(source, message, data);
            } else {
                system.log(source, message);
            }
            if (showToast) {
                if (toastType === "error") {
                    toastr.error(message);
                } else {
                    toastr.success(message);
                }

            }

        }

        function info(message) {
            logIt(message, null, null, true, "info");
        }

        function log(message, data, source, showToast) {
            logIt(message, data, source, showToast, "info");
        }

        function error(message) {
            logIt(message, null, null, true, "error");
        }
        function logError(message, data, source, showToast) {
            logIt(message, data, source, showToast, "error");
        }

        var port = ko.observable(5030),
            host = ko.observable("localhost"),
            logger = {
            log: log,
            info: info,
            error: error,
            logError: logError
        },
            ws = null,
            start = function () {
                var tcs = new $.Deferred(),
                    support = "MozWebSocket" in window ? "MozWebSocket" : ("WebSocket" in window ? "WebSocket" : null);

                if (support == null) {
                    logger.error("No WebSocket support for console notification");
                }

                logger.info("* Connecting to server ..<br/>");
                // create a new websocket and connect
                ws = new window[support]("ws://" + host() + ":" + port() + "/");

                ws.onmessage = function (evt) {
                    var model = JSON.parse(evt.data),
                        severity = model.severity || "info",
                        message = model.message || "<empty message>";
                    console.log(message, severity);

                };

                // when the connection is established, this method is called
                ws.onopen = function () {
                    logger.info("* Connection open");
                    ws.send("web logger listener is listening");
                    tcs.resolve(true);
                };

                // when the connection is closed, this method is called
                ws.onclose = function () {
                    logger.error("* Connection closed");
                };

                return tcs.promise();
            };

        start();

        return logger;

    });