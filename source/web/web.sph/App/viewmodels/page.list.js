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
                var w = window.open("/editor/ace?mode=html", '_blank', 'height=800,width=800,toolbar=0,location=0');
                w.code = page.Code();
                w.saved = function (code) {
                    page.Code(code);
                    w.close();
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
