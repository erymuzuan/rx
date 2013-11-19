/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />



define([],
    function () {

        var okClick = function (data, ev) {
            if (bespoke.utils.form.checkValidity(ev.target)) {
                this.modal.close("OK");
            }
        },
            cancelClick = function () {
                this.modal.close("Cancel");
            }, edit = function () {
                var w = window.open("/editor/ace?mode=csharp", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes');
                w.code = vm.activity().Expression();
                w.saved = function (code, close) {
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
