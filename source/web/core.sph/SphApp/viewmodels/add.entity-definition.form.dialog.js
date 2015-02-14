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
            okClick = function (data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target)) {
                                    dialog.close(this, "OK");
                }
            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            },
            activate = function() {
                var query = String.format("Id eq '{0}'", entityid);
                /*
                context.loadOneAsync("EntityDefinition", query)
                    .done(function(b) {
                        entity(b);
                    });
                */
                form().EntityDefinitionId(entityid);
            };

        var vm = {
            entity: entity,
            form: form,
            okClick: okClick,
            cancelClick: cancelClick,
            activate: activate
        };


        return vm;

    });
