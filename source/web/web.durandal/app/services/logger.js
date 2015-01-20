define(['durandal/system'],
    function (system) {


        var logger = {
            log: log,
            info: info,
            error: error,
            logError: logError
        };

        return logger;

        function info(message) {
            logIt(message, null, null, true, 'info');
        }

        function log(message, data, source, showToast) {
            logIt(message, data, source, showToast, 'info');
        }

        function error(message) {
            logIt(message, null, null, true, 'error');
        }
        function logError(message, data, source, showToast) {
            logIt(message, data, source, showToast, 'error');
        }

        function logIt(message, data, source, showToast, toastType) {
            source = source ? '[' + source + '] ' : '';
            if (data) {
                system.log(source, message, data);
            } else {
                system.log(source, message);
            }
            if (showToast) {
                if (toastType === 'error') {
                    toastr.error(message);
                } else {
                    toastr.success(message);
                }

            }

        }
    });