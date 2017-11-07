///<reference path="/Scripts/knockout-3.4.0.debug.js"/>
///<reference path="/SphApp/schemas/__domain.js"/>

define([objectbuilders.datacontext], function (context) {

    const view = ko.observable(),
        entity = ko.observable(),
        item = ko.observable(new bespoke.sph.domain.ViewColumn()),
        formsOptions = ko.observableArray(),
        activate = function (vw) {
            view(vw);
            item(new bespoke.sph.domain.ViewColumn());
            const tcs = new $.Deferred(),
                query = String.format("Id eq '{0}'", vw.EntityDefinitionId()),
                fquery = String.format("EntityDefinitionId eq '{0}'", vw.EntityDefinitionId()),
                entityTask = context.loadOneAsync("EntityDefinition", query),
                viewsTask = context.loadAsync("EntityView", `EntityDefinitionId eq '${ko.unwrap(vw.EntityDefinitionId)}'`),
                formsTask = context.getTuplesAsync("EntityForm", fquery, "Name", "Route"),
                customFormTask = $.get("/custom-forms/routes");

            $.when(entityTask, formsTask, viewsTask, customFormTask).done(function (b, flo, vlo, customForms) {
                entity(b);
                formsOptions(flo);

                formsOptions.push({ Name: " -- ", Route: "invalid" });
                formsOptions.push({ Name: "[Or select a view]", Route: "invalid" });
                _(vlo.itemCollection).each(function (v) {
                    if (v.RouteParameterCollection().length) {
                        formsOptions.push(v);
                    }
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
        attached = function (htmlview) {
            $("#column-design").sortable({
                items: ">li:not(:last)",
                placeholder: "ph",
                forcePlaceholderSize: true,
                forceHelperSize: true,
                helper: "original"
            });

            $(htmlview).on("click", "ul#column-design>li:not(:last)", function () {
                $("ul#column-design>li.selected-th").removeClass("selected-th");
                $(this).addClass("selected-th");
                item(ko.dataFor(this));
            });
        };

    return {
        activate: activate,
        attached: attached,
        formsOptions: formsOptions,
        view: view,
        item: item,
        entity: entity
    };
});