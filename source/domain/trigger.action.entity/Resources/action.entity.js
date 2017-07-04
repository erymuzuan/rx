/// <reference path="../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../Scripts/require.js" />
/// <reference path="../Scripts/underscore.js" />
/// <reference path="../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../Scripts/trigger.workflow.g.js" />



define(["services/datacontext", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function (context, logger, dialog, system) {

        bespoke.sph.domain.EntityAction = function (optionOrWebid) {

            const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
            v.OutboundMap = ko.observable();
            v.OutboundEntity = ko.observable();
            v.Operation = ko.observable();
            v["$type"] = "Bespoke.Sph.Messaging.EntityAction, trigger.action.entity";
            v.Title.subscribe(function(title) {
                if (!ko.unwrap(v.Operation)) {
                    v.Operation(title);
                }
            });
            if (optionOrWebid && typeof optionOrWebid === "object") {
                for (let n in optionOrWebid) {
                    if (optionOrWebid.hasOwnProperty(n)) {
                        if (typeof v[n] === "function") {
                            v[n](optionOrWebid[n]);
                        }
                    }
                }
            }
            if (optionOrWebid && typeof optionOrWebid === "string") {
                v.WebId(optionOrWebid);
            }


            if (bespoke.sph.domain.EntityActionPartial) {
                return _(v).extend(new bespoke.sph.domain.EntityActionPartial(v));
            }
            return v;
        };



        const action = ko.observable(new bespoke.sph.domain.EntityAction(system.guid())),
            trigger = ko.observable(),
            mappingOptions = ko.observableArray(),
            entityOptions = ko.observableArray(),
            activate = function () {
                const query = (`InputTypeName eq '${trigger().TypeOf()}'`);

                context.loadAsync("TransformDefinition", query)
                    .then(function (lo) {
                        mappingOptions(lo.itemCollection);
                    });

                return context.loadAsync("EntityDefinition", "Id ne '0'")
                    .then(function (lo) {
                        entityOptions(lo.itemCollection);
                    });

            },
            attached = function (view) {
                setTimeout(() => $(view).find("input#title-option").focus(), 500);
            },
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        const vm = {
            trigger: trigger,
            action: action,
            mappingOptions: mappingOptions,
            entityOptions: entityOptions,
            activate: activate,
            attached: attached,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
