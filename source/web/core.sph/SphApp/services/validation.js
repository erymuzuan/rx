/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define([],
    function () {

        var m_form, m_template,
            init = function (form, template) {
                m_form = form;
                m_template = template;

                var validation = { debug: true, rules: {}, messages: {} };
                _(template.FormDesign().FormElementCollection()).each(function (f) {
                    var path = f.Path(), v = f.FieldValidation();
                    if (!f.FieldValidation()) {
                        return;
                    }
                    if (v.Mode() == "digit") {
                        v.Mode("digits");
                    }

                    validation.rules[path] = { required: v.IsRequired() };
                    if (v.Message()) {
                        validation.messages[path] = v.Message();
                    }

                    if (v.MaxLength()) {
                        validation.rules[path].maxlength = v.MaxLength();
                    }
                    if (v.MinLength()) {
                        validation.rules[path].minlength = v.MinLength();
                    }
                    if (v.Mode()) {
                        validation.rules[path][v.Mode()] = true;
                    }

                    // TODO : for ListView which contains child FormElementCollection
                    //#1812
                    if (typeof f.ListViewColumnCollection === "function") {
                        var cols = ko.unwrap(f.ListViewColumnCollection);
                        _(cols).each(function(c) {
                            console.log(typeof ko.unwrap(c.Input));
                        });
                    }


                });
                vm.validationOptions(validation);
                m_form.validate(validation);
            };

        var vm = {
            init: init,
            valid: function () {

                if (typeof m_form[0].checkValidity === "function") {
                    return m_form.valid() && m_form[0].checkValidity();
                }

                return m_form.valid();
            },
            validationOptions: ko.observable()
        };

        return vm;

    });
