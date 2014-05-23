/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />



define(['services/datacontext', 'plugins/dialog'],
    function (context, dialog) {

        var activate = function () {
                var tcs = new $.Deferred();
                $.getJSON('/code.snippets.js')
                    .done(function (snippets) {
                        var list = _(snippets).map(function (v) {
                            var t = {};
                            for (var name in v) {
                                t[name] = ko.observable(v[name]);
                            }
                            return t;
                        });
                        vm.snippets(list);
                        tcs.resolve(true);
                    });
                return tcs.promise();
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
                var tcs = new $.Deferred();
                var json = ko.mapping.toJSON(vm.snippets);
                context.post(json, "/Editor/SaveSnippets")
                    .then(function (result) {
                        tcs.resolve(result);
                    });
                return tcs.promise();

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function (view) {

                $('#snippets-list-ul').on('click', 'li>a', function () {
                    vm.snippet(ko.dataFor(this));
                });
            },
            add = function () {
                var snippet = { title: ko.observable(), code: ko.observable(), lang: ko.observable() };
                vm.snippets.push(snippet);
                vm.snippet(snippet);
            },
            deleteItem = function (item) {
                vm.snippets.remove(item);
            };


        var vm = {
            snippet: ko.observable({ title: ko.observable(), code: ko.observable(), lang: ko.observable() }),
            snippets: ko.observableArray(),
            attached: attached,
            activate: activate,
            add: add,
            deleteItem: deleteItem,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
