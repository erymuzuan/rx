/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />


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
            attached = function () {

                $('#script-help-buttton').popover({
                    title: 'C# scripting help',
                    content: $('#script-help-content').html(),
                    html: true
                });


            },
            edit = function () {
                var w = window.open("/sph/editor/ace", '_blank', 'height=600px,width=600px,toolbar=0,location=0');
                if (typeof w.window === "object") {

                    w.window.field = vm.field();
                    w.window.saved = function (script) {
                        vm.field().Script(script);
                        w.close();
                    };
                }
                w.field = vm.field();
                w.saved = function (script) {
                    vm.field().Script(script);
                    w.close();
                };
            };

        var vm = {
            field: ko.observable(new bespoke.sph.domain.FunctionField()),
            okClick: okClick,
            cancelClick: cancelClick,
            attached: attached,
            edit: edit
        };


        return vm;

    });
