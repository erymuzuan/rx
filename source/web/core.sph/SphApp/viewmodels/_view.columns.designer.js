define([objectbuilders.datacontext], function (context) {

    var
        _view = ko.observable(),
        _entity = ko.observable(),
        _item = ko.observable(new bespoke.sph.domain.ViewColumn()),
        formsOptions = ko.observableArray(),
        activate = function (vw) {
            _view(vw);

            var tcs = new $.Deferred(),
                viewFilter = {
                    "filter": {
                        "bool": {
                            "must": [
                                {
                                    "script": {
                                        "script": "_source.RouteParameterCollection.size() > 0"
                                    }
                                }
                            ]
                        }
                    }
                },
                query = String.format("Id eq '{0}'", vw.EntityDefinitionId()),
                fquery = String.format("EntityDefinitionId eq '{0}'", vw.EntityDefinitionId()),
                entityTask = context.loadOneAsync("EntityDefinition", query),
                viewsTask = context.searchAsync({ entity: "EntityView", size: 50 }, viewFilter),
                formsTask = context.getTuplesAsync("EntityForm", fquery, "Name", "Route"),
                customFormTask = $.get("/config/custom-routes");

            $.when(entityTask, formsTask, viewsTask, customFormTask).done(function (b, flo, vlo, customForms) {
                _entity(b);
                var forms = _(flo).map(function (v) {
                    return { Name: v.Item1, Route: v.Item2 };
                });
                formsOptions(forms);

                formsOptions.push({ Name: " -- ", Route: "invalid" });
                formsOptions.push({ Name: "[Or select a view]", Route: "invalid" });
                _(vlo.itemCollection).each(function (v) {
                        formsOptions.push(v);
                });

                formsOptions.push({ Name: " -- ", Route: "invalid" });
                formsOptions.push({ Name: "[Or select a custom form]", Route: "invalid" });
                _(customForms[0]).each(function (v) {
                    formsOptions.push({ Name: v.title, Route: v.route });
                });

                tcs.resolve(true);

            });
            return tcs.promise();
        },
        attached = function (view) {
            $("#column-design").sortable({
                items: ">li:not(:last)",
                placeholder: "ph",
                forcePlaceholderSize: true,
                forceHelperSize: true,
                helper: "original"
            });

            $(view).on("click", "ul#column-design>li:not(:last)", function () {
                $("ul#column-design>li.selected-th").removeClass("selected-th");
                $(this).addClass("selected-th");
                _item(ko.dataFor(this));
            });
        };

    return {
        activate: activate,
        attached: attached,
        formsOptions: formsOptions,
        view: _view,
        item: _item,
        entity: _entity
    };
});