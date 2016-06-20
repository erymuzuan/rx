/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['plugins/dialog', "services/datacontext"],
    function(dialog, context) {

        var tables = ko.observableArray(),
            isBusy = ko.observable(false),
            schema = ko.observable(),
            name = ko.observable(),
            adapter = ko.observable(),
            selectedTables = ko.observableArray(),
            activate = function () {

                selectedTables([]);
                var adp = ko.unwrap(adapter),
                    server = ko.unwrap(adp.Server),
                    database = ko.unwrap(adp.Database),
                    trusted = ko.unwrap(adp.TrustedConnection),
                    userid = ko.unwrap(adp.UserId),
                    password = ko.unwrap(adp.Password),
                    url = trusted ? "" : "&trusted=false&userid=" + userid+ "&password=" + password;
                return $.getJSON("/sqlserver-adapter/table-options?server=" + server + "&database=" + database + url)
                .done(function(result){
                    var list = _(result).filter(function(v){
                            var f = _(adapter().TableDefinitionCollection())
                                .find(function(x){
                                    return ko.unwrap(x.Name) === ko.unwrap(v.Name) && ko.unwrap(v.Schema) === ko.unwrap(x.Schema);

                                });
                            return !f;
                    });
                    tables(list);

                });
            },
            attached = function(view){
                $(view).on("click","input[type=checkbox].table-checkbox", function(e){
                    var table = ko.dataFor(this);

                var adp = ko.toJS(adapter),
                    server = ko.unwrap(adp.Server),
                    database = ko.unwrap(adp.Database),
                    trusted = ko.unwrap(adp.TrustedConnection),
                    userid = ko.unwrap(adp.UserId),
                    password = ko.unwrap(adp.Password),
                    strategy = ko.unwrap(adp.ColumnDisplayNameStrategy),
                    url = trusted ? "" : "&trusted=false&userid=" + userid+ "&password=" + password;
                    if($(this).is(":checked")){
                        isBusy();
                        $.getJSON("/sqlserver-adapter/table-options/" +  table.Schema + "/" + table.Name +"/?server=" + server + "&database=" + database + "&strategy=" + strategy + url)
                                .done(function(result){
                                    var tr = context.toObservable(result);
                                    tr.IsSelected(true);
                                    selectedTables.push(tr);
                                    isBusy(false);
                                });

                    }else{
                        var tr = _(selectedTables()).find(function(v){
                            return ko.unwrap(v.Name) === ko.unwrap(table.Name) && ko.unwrap(v.Schema) === ko.unwrap(table.Schema);
                        });
                        if(tr)
                            selectedTables.remove(tr);
                    }
                });
            },
            okClick = function(data, ev) {
                dialog.close(this, "OK");                
            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            };

        var vm = {
            activate : activate,
            attached : attached,
            isBusy : isBusy,
            tables : tables,
            selectedTables : selectedTables,
            adapter : adapter,
            name : name,
            schema : schema,
            okClick: okClick,
            activate: activate,
            cancelClick: cancelClick
        };


        return vm;

    });
