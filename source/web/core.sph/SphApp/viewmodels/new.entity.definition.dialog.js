/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="../../Scripts/_task.js" />

define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        const entity = ko.observable(),
            id = ko.observable("0"),
            activate = function() {
                const ed = new bespoke.sph.domain.EntityDefinition();
                ed.Name.subscribe(function (name) {
                    if (!entity().Plural()) {
                        $.get(`/entity-definition/plural/${name}`, function (v) {
                            entity().Plural(v);
                        });
                    }
                    window.typeaheadEntity = name;
                });
                entity(ed);
            },
            attached = function () {
                setTimeout(function () {
                    $("#ent-name").focus();
                }, 500);
            },
            okClick = function (data, ev) {
                if (!bespoke.utils.form.checkValidity(ev.target)) {
                    return Task.fromResult(0);
                }
                const record = new bespoke.sph.domain.SimpleMember({
                        "Name": entity().RecordName(),
                        "TypeName": "System.String, mscorlib",
                        "IsNullable": false,
                        "IsNotIndexed": false,
                        "IsAnalyzed": false,
                        "IsFilterable": true,
                        "IsExcludeInAll": false,
                        "Boost": 5,
                        "MemberCollection": [],
                        "FieldPermissionCollection": [],
                        "isBusy": false,
                        "WebId": system.guid()
                    });

                entity().MemberCollection([record]);
                
                const ent = ko.mapping.toJSON({ item: entity, attachedProperties: [] });
                return context.post(ent, "/entity-definition")
                    .done(function (result) {
                        if (result.success) {
                            id(result.id);
                            dialog.close(data, "OK");
                        }
                    });

            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };


        const vm = {
            activate: activate,
            attached : attached,
            id: id,
            entity: entity,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
