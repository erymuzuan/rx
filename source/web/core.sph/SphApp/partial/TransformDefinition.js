/// <reference path="../objectbuilders.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../durandal/system.js" />
/// <reference path="../durandal/amd/require.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />



bespoke.sph.domain.TransformDefinitionPartial = function () {

    var addReferencedAssembly = function () {
            var self = this;
            require(["viewmodels/assembly.dialog", "durandal/app"], function (dialog, app2) {
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (!result) return;
                        if (result === "OK") {
                            _(dialog.selectedAssemblies()).each(function (v) {
                                self.ReferencedAssemblyCollection.push(v);
                            });
                        }
                    });

            });


        },
        editReferencedAssembly = function (dll) {
            alert("not implemented" + dll);
        },
        removeReferencedAssembly = function (dll) {
            var self = this;
            return function () {
                self.ReferencedAssemblyCollection.remove(dll);
            };
        };

    var vm = {
        editReferencedAssembly: editReferencedAssembly,
        removeReferencedAssembly: removeReferencedAssembly,
        addReferencedAssembly: addReferencedAssembly
    };

    return vm;
};
