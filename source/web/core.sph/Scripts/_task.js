(function (window, $) {
    window.Task = window.Task || {};
    window.Task.fromResult = function (ret, delay) {
        var tcs = new $.Deferred(),
            d = delay || 100;

        setTimeout(function () {
            tcs.resolve(returnValue);
        }, d);
        return tcs.promise();

    };

})(window, jQuery);
