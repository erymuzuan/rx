﻿bespoke.sph.domain.TryScopePartial = function () {


    var
        addCatchScope = function(wd) {
            return function() {
                var system = require('durandal/system');


                var self2 = this;
                var catchScope = new bespoke.sph.domain.CatchScope(system.guid());

                require(['viewmodels/catch.scope.dialog', 'durandal/app'], function(dialog, app2) {
                    dialog.catchScope(catchScope);

                    if (typeof dialog.wd === "function") {
                        dialog.wd(wd());
                    }

                    app2.showDialog(dialog)
                        .done(function(result) {
                            if (!result) return;
                            if (result === "OK") {
                                self2.CatchScopeCollection.push(catchScope);
                            }

                        });

                });
            };
        },
        editCatchScope = function (catchScope, wdOutside) {
            var self = this, wd = wdOutside;
            var clone = ko.mapping.fromJS(ko.mapping.toJS(catchScope));

            return function () {
                require(['viewmodels/catch.scope.dialog', 'durandal/app'], function (dialog, app2) {
                    dialog.catchScope(clone);

                    if (typeof dialog.wd === "function") {
                        dialog.wd(wd());
                    }

                    app2.showDialog(dialog)
                        .done(function (result) {
                            if (!result) return;
                            if (result === "OK") {
                                for (var g in catchScope) {
                                    if (typeof catchScope[g] === "function" && catchScope[g].name === "observable") {
                                        catchScope[g](ko.unwrap(clone[g]));
                                    } else {
                                        catchScope[g] = clone[g];
                                    }
                                }
                            }
                        });

                });
            };

        },
        removeCatchScope = function (catchScope, wdOutside) {
            var self = this, wd = wdOutside;
            
            return function () {
                wd().ActivityCollection().forEach(function (act) {
                    if (act.CatchScope() === catchScope.Id()) {
                        act.CatchScope("");
                    }
                });

                self.CatchScopeCollection.remove(catchScope);
            };
            
        };

    return {
        addCatchScope: addCatchScope,
        editCatchScope: editCatchScope,
        removeCatchScope: removeCatchScope
    };
};