bespoke.sph.domain.TryScopePartial = function () {


    var
        addCatchScope = function (wd) {
            return function () {
                console.log("addCatchScope in bespoke.sph.domain.TryScopePartial");
                var system = require('durandal/system');


                var self2 = this;
                var catchScope = new bespoke.sph.domain.CatchScope(system.guid());

                require(['viewmodels/catch.scope.dialog', 'durandal/app'], function (dialog, app2) {
                    console.log("inside dialog");
                    console.log(dialog);
                    dialog.catchScope(catchScope);

                    if (typeof dialog.wd === "function") {
                        dialog.wd(wd());
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                console.log("Adding new stuff in CatchScopeCollection");
                                console.log(self2.CatchScopeCollection);
                                self2.CatchScopeCollection.push(catchScope);
                            }

                        });

                });
            };
        };

    return {
        addCatchScope: addCatchScope
    };
};