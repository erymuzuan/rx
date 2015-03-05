/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['plugins/dialog', objectbuilders.datacontext],
    function (dialog, context) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            form = ko.observable(new bespoke.sph.domain.EntityForm()),
            entityOptions = ko.observableArray(),
            operationsOption = ko.observableArray(),
            collectionMemberOptions = ko.observableArray(),
            entityId = ko.observable(),
            okClick = function(data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            },
            activate = function (formid) {
                
                var query = String.format("Id eq '{0}'", ko.unwrap(entityId)),
                    tcs = new $.Deferred();


                context.getListAsync("EntityDefinition", null, "Name")
                    .then(function (entities) {
                        var list = _(entities).map(function (v) {
                            return {
                                text: v,
                                value: v
                            };
                        });

                        list.push({ text: "UserProfile*", value: "UserProfile" });
                        list.push({ text: "Designation*", value: "Designation" });
                        list.push({ text: "Department*", value: "Department" });
                        entityOptions(list);
                    });

                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        entity(b);
                        if (formid === "0") {
                            tcs.resolve(true);
                        }
                        var operations = (b.EntityOperationCollection()).map(function (v) {
                            return v.Name();
                        });
                        operations.push("save");
                        operationsOption(operations);

                        var collectionMembers = [],
                            findCollectionMembers = function (list) {
                                _(list).each(function (v) { console.log(ko.unwrap(v.Name) + "->" + ko.unwrap(v.TypeName)); });
                                var temp = _(list).chain()
                                    .filter(function (v) {
                                        return ko.unwrap(v.TypeName) === "System.Array, mscorlib";
                                    })
                                    .map(function (v) {
                                        return {
                                            "text": ko.unwrap(v.Name).replace("Collection", ""),
                                            "value": "bespoke." + config.applicationName + "_" + entity().Id() + ".domain." + ko.unwrap(v.Name).replace("Collection", "")
                                        }
                                    })
                                    .value();
                                _(temp).each(function (v) { collectionMembers.push(v); });
                                _(list).each(function (v) {
                                    findCollectionMembers(v.MemberCollection());
                                });
                            };
                        findCollectionMembers(b.MemberCollection());
                        collectionMemberOptions(collectionMembers);
                    });
                form().EntityDefinitionId(entityId());
            };

        var vm = {
            entity: entity,
            form: form,
            okClick: okClick,
            cancelClick: cancelClick,
            activate: activate,
            entityOptions: entityOptions,
            operationsOption: operationsOption,
            collectionMemberOptions: collectionMemberOptions,
            entityId: entityId
        };


        return vm;

    });
