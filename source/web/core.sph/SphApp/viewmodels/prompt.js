/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog"],
    function(dialog) {

        var okClick = function() {
               dialog.close(this, "OK");
            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            title: ko.observable("Rx Developer"),
            label: ko.observable(""),
            value: ko.observable(""),
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
