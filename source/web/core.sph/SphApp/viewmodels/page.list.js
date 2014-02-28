/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../objectbuilders.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define([objectbuilders.datacontext, objectbuilders.app],
    function (context, app) {

        var isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function () {

            },
            editPage = function (page) {
                var w = window.open("/sph/editor/page/" + page.PageId() + "?mode=html", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes'),
                    init = function () {
                        w.saved = function (code, close) {
                            page.Code(code);
                            if (close) {
                                w.close();
                            }
                            var tcs = new $.Deferred();
                            var data = ko.mapping.toJSON(page);

                            context.post(data, "/Page/Save")
                                .then(tcs.resolve);
                            return tcs.promise();
                        };
                        w.code = page.Code();

                    };
                if (w.attachEvent) { // for ie
                    w.attachEvent('onload', init);
                } else {
                    init();
                }


            },
            editDetail = function (page) {

                var tcs = new $.Deferred(),
                    clone = context.clone(page);

                require(['viewmodels/page.detail.dialog', 'durandal/app'], function (dialog, app2) {
                    dialog.page(clone);

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {

                                context.commit(clone, page);
                                var data = ko.mapping.toJSON(page);
                                context.post(data, "/Sph/Page/Save")
                                    .then(tcs.resolve);

                            }
                        });

                });
                return tcs.promise();
            },
            remove = function (page) {

                return function () {
                    var tcs = new $.Deferred();
                    app.showMessage("Are you sure to permanently delete " + page.Name(), "Delete Page", ["Yes", "No"])
                        .done(function (dialogResult) {
                            if (dialogResult === "Yes") {

                                context.post(ko.mapping.toJSON(page), "/Sph/Page/Remove")
                                    .done(tcs.resolve)
                                    .then(function () {
                                        vm.pages.remove(page);
                                    });
                            } else {
                                tcs.resolve(false);
                            }

                        });
                    return tcs.promise();
                };

            };

        var vm = {
            remove: remove,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            pages: ko.observableArray(),
            editPage: editPage,
            editDetail: editDetail
        };

        return vm;

    });
