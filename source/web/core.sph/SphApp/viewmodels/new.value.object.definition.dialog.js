/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/form.designer.g.js" />
/// <reference path="~/Scripts/_task.js" />

define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        var vod = ko.observable(),
            id = ko.observable("0"),
            activate = function() {
                var ed = new bespoke.sph.domain.ValueObjectDefinition(system.guid());
                ed.Name.subscribe(function (name) {
                    context.loadOneAsync("ValueObjectDefinition", String.format("Name eq '{0}'", name))
                    .done(function(count) {
                            if (count) {
                                alert("Already exist");
                            }
                        });
                });

                vod(ed);
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

                return context.post(ko.mapping.toJSON(vod), "/value-object-definition")
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


        var vm = {
            activate: activate,
            attached : attached,
            id: id,
            vod: vod,
            okClick: okClick,
            cancelClick: cancelClick
        };


        return vm;

    });
