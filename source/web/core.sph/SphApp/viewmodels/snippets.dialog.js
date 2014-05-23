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

        var lang = ko.observable(''),
            activate = function () {

                var tcs = new $.Deferred();
                lang.subscribe(function (lg) {
                    $.getJSON('/sph/editor/snippets/' + lg)
                        .done(function (snippets) {
                            var list = _(snippets).map(function (v) {
                                var t = {};
                                for (var name in v) {
                                    t[name] = ko.observable(v[name]);
                                }
                                return t;
                            });
                            vm.snippets(list);
                        });
                });
                setTimeout(tcs.resolve, 500);
                return tcs.promise();
            },
            saveItem = function () {

                var tcs = new $.Deferred(),
                    json = ko.mapping.toJSON(vm.snippet);
                context.post(json, "/sph/editor/SaveSnippet")
                    .then(tcs.resolve);
                return tcs.promise();

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            attached = function () {

                $('#snippets-list-ul').on('click', 'li>a', function () {
                    vm.snippet(ko.dataFor(this));
                });
            },
            add = function () {
                var snippet = { title: ko.observable(), code: ko.observable(), note: ko.observable(), lang: lang(), };
                vm.snippets.push(snippet);
                vm.snippet(snippet);
            },
            deleteItem = function (item) {
                vm.snippets.remove(item);
            };


        var vm = {
            snippet: ko.observable({ title: ko.observable(), code: ko.observable(), note: ko.observable(), lang: ko.observable() }),
            snippets: ko.observableArray(),
            lang: lang,
            attached: attached,
            activate: activate,
            add: add,
            saveItem: saveItem,
            deleteItem: deleteItem,
            cancelClick: cancelClick
        };


        return vm;

    });
