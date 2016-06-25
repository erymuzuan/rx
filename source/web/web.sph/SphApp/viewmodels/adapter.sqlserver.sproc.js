﻿/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/respond.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../../web/web.sph/scripts/prism.js" />

define(["services/datacontext", "services/logger", "plugins/router", objectbuilders.system, "ko/_ko.adapter.sqlserver", "sqlserver-adapter/resource/_sql.server.adapter.domain.js"],
    function (context, logger, router, system) {

        var operation = ko.observable({ObjectType: ko.observable()}),
            isBusy = ko.observable(false),
            adapterId = ko.observable(),
            text = ko.observable(),
            requestSchema = ko.observable(),
            responseSchema = ko.observable(),
            member = ko.observable(),
            selected = ko.observable(),
            objectType = ko.computed(function(){
                var type = operation().ObjectType();
                switch(type){
                    case "P":
                    case "P ": return "Stored Procedure";
                    case "FN": return "Scalar Valued Function";
                    case "IF": return "Inline Table Valued Function";
                    case "TF": return "Table Valued Function";

                }
                return type;
            }),
        activate = function (id, uuid) {
            adapterId(id);
            var tcs = new $.Deferred();

            $.get("/sqlserver-adapter/sproc/" + id + "/" + uuid.replace(".", "/"))
                .done(function (op) {

                    var op2 = context.toObservable(op);
                    console.log(op2);

                    operation(op2);

                    _(operation().RequestMemberCollection()).each(function (v) {
                        if (!ko.isObservable(v.TypeName)) {
                            v.TypeName = ko.observable("System.String, mscorlib");
                        }
                        if (!ko.unwrap(v.TypeName)) {
                            v.TypeName("System.String, mscorlib");
                        }
                    });

                    requestSchema({
                        Name: ko.observable("Request"),
                        MemberCollection: ko.observableArray(operation().RequestMemberCollection())
                    });
                    responseSchema({
                        Name: ko.observable("Response"),
                        MemberCollection: ko.observableArray(operation().ResponseMemberCollection())
                    });


                    tcs.resolve(op);
                });

            $.get("/sqlserver-adapter/sproc-text/" + id + "/" + uuid.replace(".", "/"))
                .done(function (st) {
                    text(st);
                });

            setTimeout(function () {
                $.getScript("/scripts/prism.js", function () {
                    logger.info("loading syntax highlighter");
                });
            }, 5000);

            return tcs.promise();

        },
        attached = function (view) {

        },
        save = function () {
            var tcs = new $.Deferred();
            $.ajax({
                type: "PATCH",
                data: ko.mapping.toJSON(operation),
                url: "/sqlserver-adapter/sproc/" + adapterId(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: tcs.reject,
                success: tcs.resolve
            });


            return tcs.promise();
        },
        goBack = function () {
            window.location = "#adapter.sqlserver/" + adapterId();
            return Task.fromResult(0);
        };

        var vm = {
            text: text,
            requestSchema: requestSchema,
            responseSchema: responseSchema,
            selected: selected,
            member: member,
            objectType : objectType,
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
