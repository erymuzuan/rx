/// <reference path="../../Scripts/jquery-1.9.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-2.2.1.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'durandal/plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function () {

            },
            viewAttached = function (view) {

            },
            editPage = function (page) {
                var w = window.open("/editor/ace?mode=html", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes');
                w.code = page.Code();
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
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            viewAttached: viewAttached,
            pages: ko.observableArray(),
            editPage: editPage
        };

        return vm;

    });
