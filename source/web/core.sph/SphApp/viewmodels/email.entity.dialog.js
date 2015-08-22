/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(["services/datacontext", "services/logger", "plugins/dialog"],
    function (context, logger, dialog) {

        var entity = ko.observable(),
            template = ko.observable(),
            templateOptions = ko.observableArray(),
            isBusy = ko.observable(false),
            id = ko.observable(),
            to = ko.observable(),
            subject = ko.observable(),
            body = ko.observable(),
            message = ko.observable(),
            activate = function () {
                to("");
                subject("");
                body("");
                message("");
                template(0);

                var query = String.format("IsPublished eq true and Entity eq '{0}'", entity()),
                    tcs = new $.Deferred();

                context.loadAsync("EmailTemplate", query)
                    .then(function (lo) {
                        templateOptions(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();

            },
            send = function () {
                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON({
                        to: to,
                        subject: subject,
                        body: body
                    }),
                    self = this;

                context.post(data, "/email-template/send")
                    .then(function (result) {
                        tcs.resolve(result);
                        dialog.close(self, "OK");
                    message(String.format("The email has been sucessfully sent to {0}", to()));
                });
                return tcs.promise();
            },
            canSend = ko.computed(function () {
                return to() && subject() && body();
            }),
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        template.subscribe(function (emailTemplateId) {
            if (!emailTemplateId) {
                return false;
            }
             isBusy(true);

           return $.get( "/email-template/generate/" + ko.unwrap(entity) + "/" + ko.unwrap(id) + "/" + emailTemplateId)
                .then(function (result) {
                    isBusy(false);
                    subject(result.subject);
                    body(result.body);
                });
        });
        var vm = {
            message: message,
            isBusy: isBusy,
            activate: activate,
            templateOptions: templateOptions,
            template: template,
            entity: entity,
            id: id,
            to: to,
            subject: subject,
            body: body,
            send: send,
            canSend: canSend,
            cancelClick: cancelClick
        };


        return vm;

    });
