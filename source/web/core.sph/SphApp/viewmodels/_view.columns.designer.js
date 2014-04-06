/**
 * Created by bespoke1 on 28/03/2014.
 */
define([objectbuilders.datacontext], function (context) {

    var
        _view = ko.observable(),
        _entity = ko.observable(),
        _item = ko.observable(new bespoke.sph.domain.ViewColumn()),
        activate = function (vw) {
            // console.log(view);
            _view(vw);
            var tcs = new $.Deferred(),
                query = String.format("EntityDefinitionId eq {0}", vw.EntityDefinitionId());

            context.loadOneAsync("EntityDefinition", query)
                .done(function (b) {
                    _entity(b);
                    tcs.resolve(true);
                });
            return tcs.promise();
        },
        attached = function(view){
            $('#column-design').sortable({
                items: '>li:not(:first)',
                placeholder: 'ph',
                forcePlaceholderSize: true,
                forceHelperSize: true,
                helper: 'original'
            });

            $(view).on('click', 'ul#column-design>li:not(:first)', function(){
                $('ul#column-design>li.selected-th').removeClass('selected-th');
                $(this).addClass('selected-th');
                _item(ko.dataFor(this));
            });
        };

    return {
        activate: activate,
        attached : attached,
        view: _view,
        item: _item,
        entity: _entity
    };
});