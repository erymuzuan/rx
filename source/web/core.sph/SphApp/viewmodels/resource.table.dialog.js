/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["plugins/dialog", objectbuilders.datacontext],
    function (dialog, context) {

        var items = ko.observableArray(),
            resource = ko.observable(),
            selectedLanguage = ko.observable(),
            keys = ko.observableArray(),
            languageOptions = ko.observableArray(),
            activate = function () {
                items(_(keys()).map(function (v) {
                    return { "Key": ko.observable(v), "Value": ko.observable(v) };
                }));

                return $.getJSON("/i18n/options").done(function (ret) {
                    var options = [];
                    for (var code in ret) {
                        if (ret.hasOwnProperty(code)) {
                            options.push({ code: code, display: ret[code] });
                        }
                    }
                    languageOptions(options);
                });
            },
            attached = function () {
                selectedLanguage("");
                //scrollable tbody
                $.getScript("Scripts/jquery.floatThead.min.js", function () {
                    $("#types-table").floatThead();
                });
            },
            addChildItem = function () {
                items.push({ "Key": ko.observable(""), "Value": ko.observable("") });
            },
            removeChildItem = function (row) {
                return function () {
                    items.remove(row);
                };
            },
            okClick = function () {
                var o = {};
                _(items()).each(function (v) {
                    o[ko.unwrap(v.Key)] = v.Value;
                });

                return context.post(ko.toJSON(o), "/i18n/" + selectedLanguage() + "/" + resource());

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };


        selectedLanguage.subscribe(function (lang) {
            if (!lang) {
                return;
            }
            $.getJSON("/i18n/" + lang + "/" + resource())
                .done(function (n) {
                    for (var k in n) {
                        if (n.hasOwnProperty(k)) {
                            var k1 = k,
                                key = _(items()).find(function (v) {
                                    return ko.unwrap(v.Key) === k1;
                                });
                            if (key) {
                                key.Value(n[k1]);
                            } else {
                                items.push({ "Key": ko.observable(k1), "Value": ko.observable(n[k1]) });
                            }
                        }
                    }

                });
        });
        var vm = {
            activate: activate,
            attached: attached,
            resource: resource,
            lang: selectedLanguage,
            languageOptions: languageOptions,
            items: items,
            keys: keys,
            addChildItem: addChildItem,
            removeChildItem: removeChildItem,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
