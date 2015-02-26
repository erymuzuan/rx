/// <reference path="../../Scripts/jquery-2.1.1.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schema/sph.domain.g.js" />


define(['plugins/dialog', objectbuilders.datacontext,objectbuilders.system, objectbuilders.config],
    function (dialog, context, system, config) {

        var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
            operation = ko.observable(new bespoke.sph.domain.EntityOperation()),
            errors = ko.observableArray(),
            roles = ko.observableArray(),
            okClick = function(data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                    dialog.close(this, "OK");
                }
            },
            cancelClick = function() {
                dialog.close(this, "Cancel");
            },
            activate = function (eid, name) {
                eid = "blog";
                console.log(eid);
                if (!roles().length) {
                    roles(config.allRoles);
                    roles.splice(0, 0, 'Anonymous');
                    roles.splice(0, 0, 'Everybody');
                }

                var query = String.format("Id eq '{0}'", eid),
                    tcs = new $.Deferred();
                console.log(query);
                context.loadOneAsync("EntityDefinition", query)
                    .done(function (b) {
                        entity(b);
                        var o = _(b.EntityOperationCollection()).find(function (v) {
                            return v.Name() === name;
                        });
                        if (!o) {
                            o = new bespoke.sph.domain.EntityOperation({
                                WebId: system.guid(),
                                Name: name
                            });
                            entity().EntityOperationCollection.push(o);
                        }
                        operation(o);
                        tcs.resolve(true);
                        console.log(b.EntityOperationCollection());
                    });
                
                return tcs.promise();
            };

        var vm = {
            entity: entity,
            operation: operation,
            errors: errors,
            roles: roles,
            okClick: okClick,
            cancelClick: cancelClick,
            activate: activate,
            config: config
        };


        return vm;

    });
