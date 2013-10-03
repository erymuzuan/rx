(function (window, $) {
    window.Task = window.Task || {};
    window.Task.fromResult = function (returnValue, delay) {
        var tcs = new $.Deferred(),
            ret = returnValue || true,
            d = delay || 100;

        setTimeout(function () {
            tcs.resolve(ret);
        }, d);
        return tcs.promise();

    };

})(window, jQuery);
