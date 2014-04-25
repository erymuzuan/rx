/**
 * Created by bespoke1 on 28/03/2014.
 */
define([objectbuilders.datacontext], function (context) {

    var
        _view = ko.observable(),
        _entity = ko.observable(),
        _item = ko.observable(new bespoke.sph.domain.ViewColumn()),
        formsOptions = ko.observableArray(),
        activate = function (vw) {
            // console.log(view);
            _view(vw);
            var tcs = new $.Deferred(),
                query = String.format("EntityDefinitionId eq {0}", vw.EntityDefinitionId()),
                entityTask = context.loadOneAsync("EntityDefinition", query),
                formsTask = context.loadAsync("EntityForm", query);

            $.when(entityTask, formsTask).done(function (b, flo) {
                _entity(b);
                formsOptions(flo.itemCollection);
                tcs.resolve(true);
            });
            return tcs.promise();
        },
        attached = function (view) {
            $('#column-design').sortable({
                items: '>li:not(:last)',
                placeholder: 'ph',
                forcePlaceholderSize: true,
                forceHelperSize: true,
                helper: 'original'
            });

            $(view).on('click', 'ul#column-design>li:not(:last)', function () {
                $('ul#column-design>li.selected-th').removeClass('selected-th');
                $(this).addClass('selected-th');
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