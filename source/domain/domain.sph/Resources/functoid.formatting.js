
define(['services/datacontext', 'services/logger', 'plugins/dialog'],
    function (context, logger, dialog) {
        var functoid = ko.observable(),
            okClick = function (data, ev) {
                dialog.close(this, 'OK');
            },
            cancelClick = function () {
                dialog.close(this, 'Cancel');
            };
        var vm = {
            functoid: functoid,
            okClick: okClick,
            cancelClick: cancelClick
        };
        return vm;
    });