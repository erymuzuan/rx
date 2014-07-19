/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var operation = ko.observable(),
            isBusy = ko.observable(false),
            adapterId = ko.observable(),
            requestSchema = ko.observable(),
            responseSchema = ko.observable(),
            member = ko.observable(),
            activate = function (id, uuid) {
                adapterId(parseInt(id));
                var tcs = new $.Deferred();

                $.get("/httpadapter/operation/" + id + "/" + uuid)
                    .done(function (op) {
                        operation(context.toObservable(op));

                        _(operation().RequestMemberCollection()).each(function(v){
                            if(!ko.unwrap(v.TypeName)){
                                v.TypeName('System.String, mscorlib');
                            }
                        });

                        requestSchema({
                            Name : ko.observable('Request'),
                            MemberCollection: ko.observableArray(operation().RequestMemberCollection())
                        });
                        responseSchema({
                            Name : ko.observable('Response'),
                            MemberCollection: ko.observableArray(operation().ResponseMemberCollection())
                        });


                        tcs.resolve(op);
                    });

                return tcs.promise();

            },
            attached = function (view) {

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
            };

        var vm = {
            requestSchema: requestSchema,
            responseSchema: responseSchema,
            member: member,
            operation: operation,
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            toolbar: {
                saveCommand: save,
                commands: ko.observableArray([

                ])

            }
        };

        return vm;

    });
