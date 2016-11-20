/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />


define(["plugins/dialog"],
    function (dialog) {

        const member = ko.observable({
            name: ko.observable(),
            type: ko.observable(),
            length: ko.observable(),
            nullable: ko.observable(true)
        }),
        selectedMembers = ko.observable(),
        adapter = ko.observable(),
            okClick = function (data, ev) {
                const dlg = this;

                if (bespoke.utils.form.checkValidity(ev.target)) {

                    const adp = ko.toJS(adapter),
                        m = ko.toJS(member);
                    context.get(`/sqlserver-adapter/operation-options/column-metadata?name=${m.name}&type=${m.type}&length=${m.length}&nullable=${m.nullable}&server=${adp.Server}&database=${adp.Database}&trusted=${adp.Trusted}&userid=${adp.UserId}&password=${adp.Password}`)
                    .done(function (result) {
                        selectedMembers(result);
                        dialog.close(this, "OK");
                    });
                }

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            typeOptions = ko.observableArray(["CHAR", "VARCHAR", "TEXT", "NCHAR", "NVARCHAR", "NTEXT", "BINARY", "VARBINARY", "IMAGE",
                "BIT", "TINYINT", "SMALLINT", "INT", "BIGINT", "DECIMAL", "DEC", "NUMERIC", "FLOAT", "REAL", "SMALLMONEY", "MONEY",
                "DATE", "DATETIME", "DATETIME2", "SMALLDATETIME", "TIME", "DATETIMEOFFSET"]);

        const vm = {
            member: member,
            okClick: okClick,
            cancelClick: cancelClick,
            typeOptions: typeOptions,
            selectedMembers: selectedMembers,
            adapter: adapter
        };


        return vm;

    });
