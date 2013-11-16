/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define([],
    function () {

        var okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    this.modal.close("OK");
                }

            },
            cancelClick = function () {
                this.modal.close("Cancel");
            },
            viewAttached = function () {

                $('#script-help-buttton').popover({
                    title: 'C# scripting help',
                    content: $('#script-help-content').html(),
                    html: true
                });


            },
            edit = function () {
                var w = window.open("/editor/ace", '_blank', 'height=600px,width=600px,toolbar=0,location=0');
                w.field = vm.field();
                w.saved= function(script) {
                    vm.field().Script(script);
                    w.close();
                };
            };

        var vm = {
            field: ko.observable(new bespoke.sph.domain.FunctionField()),
            okClick: okClick,
            cancelClick: cancelClick,
            viewAttached: viewAttached,
            edit: edit
        };


        return vm;

    });
