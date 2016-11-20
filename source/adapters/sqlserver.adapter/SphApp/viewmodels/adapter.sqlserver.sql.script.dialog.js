/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />

var bespoke = bespoke || {};
bespoke.sph = bespoke.sph || {};
bespoke.sph.domain = bespoke.sph.domain || {};
bespoke.sph.domain.Adapters = bespoke.sph.domain.Adapters || {};
bespoke.sph.domain.Adapters.SqlScriptOperationDefinition = function() {
    const sqlStatement = ko.observable(),
        name = ko.observable(),
        methodName = ko.observable(),
        requestMemberCollection = ko.observableArray(),
        responseMemberCollection = ko.observableArray(),
        schema = ko.observable(),
        errorRetry = ko.observable(new bespoke.sph.domain.api.ErrorRetry()),
        useHttpGet = ko.observable(),
        uuid = ko.observable();

    return {
        $type: "Bespoke.Sph.Integrations.Adapters.SqlScriptOperationDefinition, sqlserver.adapter",
        ObjectType: "SqlScript",
        Name : name,
        SqlStatement: sqlStatement,
        MethodName: methodName,
        ResponseMemberCollection: responseMemberCollection,
        RequestMemberCollection : requestMemberCollection,
        UseHttpGet: useHttpGet,
        Schema : schema,
        ErrorRetry: errorRetry,
        WebId: ko.observable(),
        Uuid : uuid
    };

};

define(["plugins/dialog"],
    function (dialog) {

        const script = ko.observable(new bespoke.sph.domain.Adapters.SqlScriptOperationDefinition()),
            adapter = ko.observable(),
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        const vm = {
            adapter : adapter,
            script: script,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
