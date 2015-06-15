
bespoke.sph.domain.EntityChartPartial = function (model) {
    var context = require(objectbuilders.datacontext),
        pin = function () {
            if (typeof model.IsDashboardItem === "function") {
                model.IsDashboardItem(true);
            } else {
                model.IsDashboardItem = ko.observable(true);
            }

            var tcs = new $.Deferred();

            context.post(ko.mapping.toJSON(model), '/sph/entitychart/save')
                .done(tcs.resolve);

            return tcs.promise();
        },
        unpin = function() {
            if (typeof model.IsDashboardItem === "function") {
                model.IsDashboardItem(false);
            } else {
                model.IsDashboardItem = ko.observable(false);
            }

            var tcs = new $.Deferred();

            context.post(ko.mapping.toJSON(model), '/sph/entitychart/save')
                .done(tcs.resolve);

            return tcs.promise();
        };
    return {
        pin: pin,
        unpin : unpin
    };
};