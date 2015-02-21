define(["services/datacontext", "services/logger", "plugins/dialog"],
    function (context, logger, dialog) {
        var functoid = ko.observable(),
            okClick = function (data, ev) {
                dialog.close(this, 'OK');

            },
            cancelClick = function () {
                dialog.close(this, 'Cancel');
            },
            edit = function () {
                var w = window.open("/sph/editor/ace", "_blank", "height=600px,width=600px,toolbar=0,location=0");
                if (typeof w.window === "object") {

                    w.window.code = functoid().Expression();
                    w.window.saved = function (script) {
                        functoid().Expression(script);
                        w.close();
                    };
                }
                w.code = functoid().Expression();
                w.saved = function (script) {
                    functoid().Expression(script);
                    w.close();
                };

            };
        var vm = {
            functoid: functoid,
            edit: edit,
            okClick: okClick,
            cancelClick: cancelClick
        };
        return vm;
    });