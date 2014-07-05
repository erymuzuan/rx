/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

define(['plugins/dialog'],
    function (dialog) {

        var okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            edit = function () {
                var w = window.open("/sph/editor/ace?mode=csharp", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes');
                w.window.code = vm.activity().Expression();
                w.window.saved = function (code, close) {
                    vm.activity().Expression(code);
                    if (close) {
                        w.close();
                    }
                };
            };

        var vm = {
            activity: ko.observable(new bespoke.sph.domain.ExpressionActivity()),
            wd: ko.observable(new bespoke.sph.domain.WorkflowDefinition()),
            edit: edit,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
