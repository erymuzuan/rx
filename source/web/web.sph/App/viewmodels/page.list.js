﻿/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../durandal/re" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger'],
    function (context) {

        var isBusy = ko.observable(false),
            activate = function () {

            },
            attached = function () {

            },
            editPage = function (page) {
                var w = window.open("/editor/page/" + page.PageId() + "?mode=html", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes'),
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

                var tcs = new $.Deferred();
                require(['viewmodels/page.detail.dialog', 'durandal/app'], function (dialog, app2) {
                    dialog.page(page);

                    app2.showModal(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {

                                var data = ko.mapping.toJSON(page);

                                context.post(data, "/Page/Save")
                                    .then(tcs.resolve);

                            }
                        });

                });
                return tcs.promise();
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            pages: ko.observableArray(),
            editPage: editPage,
            editDetail: editDetail
        };

        return vm;

    });
