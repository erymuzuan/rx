/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', 'ko/_ko.adapter.http'],
    function (context, logger, router) {

        var operation = ko.observable(),
            isBusy = ko.observable(false),
            timeoutInterval = ko.observable(1),
            adapterId = ko.observable(),
            requestSchema = ko.observable(),
            responseSchema = ko.observable(),
            member = ko.observable(),
            responseMember = ko.observable(),
            activate = function (id, uuid) {
                adapterId(parseInt(id));
                var tcs = new $.Deferred();

                $.get("/httpadapter/operation/" + id + "/" + uuid)
                    .done(function (op) {

                        op.RequestHeadersDictionary = ko.observableArray();
                        for (var key in op.RequestHeaders) {
                            if (op.RequestHeaders.hasOwnProperty(key)) {
                                op.RequestHeadersDictionary.push({ key: key, value: op.RequestHeaders[key] });
                            }
                        }

                        operation(context.toObservable(op));

                        _(operation().RequestMemberCollection()).each(function (v) {
                            if (!ko.unwrap(v.TypeName)) {
                                v.TypeName('System.String, mscorlib');
                            }
                        });

                        requestSchema({
                            Name: ko.observable('Request'),
                            MemberCollection: ko.observableArray(operation().RequestMemberCollection())
                        });
                        responseSchema({
                            Name: ko.observable('Response'),
                            MemberCollection: ko.observableArray(operation().ResponseMemberCollection())
                        });


                        tcs.resolve(op);
                    });

                return tcs.promise();

            },
            attached = function (view) {

            },
            pickRegex = function (m) {
                var w = window.open("/regex.picker.html", '_blank', 'height=600px,width=600px,toolbar=0,location=0');
                if (typeof w.window === "object") {

                    w.window.member = m;
                    w.window.operation = operation();
                    w.window.adapterId = adapterId();
                    w.window.saved = function (pattern, group) {
                        m.Pattern(pattern);
                        m.Group(group);
                        w.close();
                    };
                } else {

                    w.member = m;
                    w.adapterId = adapterId();
                    w.saved = function (pattern, group) {
                        m.Pattern(pattern);
                        m.Group(group);
                        w.close();
                    };
                }

            },
            save = function () {
                var tcs = new $.Deferred();
                $.ajax({
                    type: "PATCH",
                    data: ko.mapping.toJSON(operation),
                    url: '/httpadapter/' + adapterId(),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: tcs.reject,
                    success: tcs.resolve
                });


                return tcs.promise();
            },
            goBack = function() {
                window.location = "#adapter.http/" + adapterId();
                return Task.fromResult(0);
            };

        var vm = {
            timeoutInterval: timeoutInterval,
            pickRegex: pickRegex,
            requestSchema: requestSchema,
            responseSchema: responseSchema,
            responseMember: responseMember,
            member: member,
            operation: operation,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([
                    {
                        command: goBack,
                        caption: "Back",
                        icon: "fa fa-history"
                    }
                ])

            }
        };

        return vm;

    });
