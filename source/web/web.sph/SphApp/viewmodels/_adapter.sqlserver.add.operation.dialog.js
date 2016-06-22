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

        var operations = ko.observableArray(),
            isBusy = ko.observable(false),
            schema = ko.observable(),
            name = ko.observable(),
            adapter = ko.observable(),
            selectedOperations = ko.observableArray(),
            getType = function(op){

                var typeName = ko.unwrap(op.$type),
                    type = "";
                switch(typeName){
                    case  "Bespoke.Sph.Integrations.Adapters.TableValuedFunction, sqlserver.adapter":
                        type = "table-valued-functions";
                        break;
                    case "Bespoke.Sph.Integrations.Adapters.ScalarValuedFunction, sqlserver.adapter":
                        type = "scalar-valued-functions";
                        break;
                    default:
                        type = "sprocs";
                        break;
                }

                return type;
            },
            activate = function () {

                selectedOperations([]);
                var adp = ko.unwrap(adapter),
                    server = ko.unwrap(adp.Server),
                    database = ko.unwrap(adp.Database),
                    trusted = ko.unwrap(adp.TrustedConnection),
                    userid = ko.unwrap(adp.UserId),
                    password = ko.unwrap(adp.Password),
                    url = trusted ? "" : "&trusted=false&userid=" + userid+ "&password=" + password;
                return $.getJSON("/sqlserver-adapter/operation-options?server=" + server + "&database=" + database + url)
                .done(function(result){
                    var list = _(result).filter(function(v){
                            var f = _(adapter().OperationDefinitionCollection())
                                .find(function(x){
                                    return ko.unwrap(x.Name) === ko.unwrap(v.Name) && ko.unwrap(v.Schema) === ko.unwrap(x.Schema);

                                });
                            return !f;
                    });
                    operations(list);

                });
            },
            attached = function(view){
                $(view).on("click","input[type=checkbox].operation-checkbox", function(e){
                    var operation = ko.dataFor(this),
                        adp = ko.unwrap(adapter),
                        server = ko.unwrap(adp.Server),
                        database = ko.unwrap(adp.Database),
                        trusted = ko.unwrap(adp.TrustedConnection),
                        userid = ko.unwrap(adp.UserId),
                        password = ko.unwrap(adp.Password),
                        url = trusted ? "" : "&trusted=false&userid=" + userid+ "&password=" + password,
                        type = ko.unwrap(operation.ObjectType);


                    if($(this).is(":checked")){
                        isBusy();
                        $.getJSON("/sqlserver-adapter/operation-options/" +  type + "/" +  operation.Schema + "/" + operation.Name +"?server=" + server + "&database=" + database + url)
                                .done(function(result){
                                    var tr = context.toObservable(result);
                                    selectedOperations.push(tr);
                                    isBusy(false);
                                });

                    }else{
                        var tr = _(selectedOperations()).find(function(v){
                            return ko.unwrap(v.Name) === operation.Name 
                                && ko.unwrap(v.Schema) === operation.Schema;
                        });
                        if(tr)
                            selectedOperations.remove(tr);
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
            getType : getType,
            operations : operations,
            selectedOperations : selectedOperations,
            adapter : adapter,
            name : name,
            schema : schema,
            okClick: okClick,
            activate: activate,
            cancelClick: cancelClick
        };


        return vm;

    });
