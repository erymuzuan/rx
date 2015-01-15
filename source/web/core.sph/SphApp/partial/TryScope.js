bespoke.sph.domain.TryScopePartial = function () {


    var addCatchScope = function (a, b) {
        console.log(a);
        console.log(b);

        var self2 = this;
        var correlationSet = new bespoke.sph.domain.CorrelationSet("123213213213");

        require(['viewmodels/correlation.set.dialog', 'durandal/app'], function (dialog, app2) {
            dialog.correlationSet(correlationSet);
            if (typeof dialog.wd === "function") {
                dialog.wd(self2);
            }
            console.log("dialog");
            console.log(app2);
            console.log("app2");
            console.log(app2);
            app2.showDialog(dialog)
                .done(function (result) {
                    if (!result) return;
                    if (result === "OK") {
                        //console.log("Adding new stuff in CorrelationSetCollection");
                        //console.log(self2.CorrelationSetCollection);
                        //self2.CorrelationSetCollection.push(correlationSet);
                    }

                });

        });
    };

    return {
        addCatchScope: addCatchScope
    };
};