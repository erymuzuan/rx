/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.3.0.debug.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define([],
    function () {

        var editor = null,
            okClick = function (data, ev) {
                if (ev.target.form.checkValidity()) {

                    vm.field().Script(editor.getValue());
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


            };

        var vm = {
            field: ko.observable(new bespoke.sph.domain.FunctionField()),
            okClick: okClick,
            cancelClick: cancelClick,
            viewAttached: viewAttached
        };

        vm.field.subscribe(function (field) {
            var createEditor = function () {
                setTimeout(function () { // wait fo the modal
                    editor = ace.edit("function-field-script");
                    editor.setTheme("ace/theme/textmate");
                    editor.getSession().setMode("ace/mode/csharp");

                    editor.setValue(field.Script());

                }, 1000);

            };
            if (typeof ace === "undefined") {
                $.getScript('/scripts/ace/ace.js').done(createEditor);
            } else {
                createEditor();
            }
        });

        return vm;

    });
