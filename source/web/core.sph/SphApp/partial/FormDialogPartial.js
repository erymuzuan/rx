/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />
/// <reference path="../schemas/form.designer.g.js" />

bespoke.sph.domain.FormDialogPartial = function (model) {

    var system = require("durandal/system"),
        init = function() {

            var buttons = ko.unwrap(model.DialogButtonCollection);
            _(buttons).each(function (v) {
                v.canMoveDown(true);
                v.canMoveUp(true);
            });
            var first = _(buttons).first(),
                last = _(buttons).last();
            if (first) {
                first.canMoveUp(false);
            }
            if (last) {
                last.canMoveDown(false);
            }
        },
        addDialogButton = function () {
            model.DialogButtonCollection.push(new bespoke.sph.domain.DialogButton({ WebId: system.guid(), canMoveDown: false }));
            init();
        },
        removeDialogButton = function (btn) {
            model.DialogButtonCollection.remove(btn);
            init();
        },
        move = function (array, from, to) {
            if (to === from) return;

            var target = array[from];
            var increment = to < from ? -1 : 1;

            for (var k = from; k !== to; k += increment) {
                array[k] = array[k + increment];
            }
            array[to] = target;
        },
        arrange = function (btn, step) {

            var index = model.DialogButtonCollection().indexOf(btn);
            var temps = ko.unwrap(model.DialogButtonCollection);
            move(temps, index, index + step);

            _(temps).each(function (v) {
                v.canMoveDown(true);
                v.canMoveUp(true);
            });
            _(temps).first().canMoveUp(false);
            _(temps).last().canMoveDown(false);
            model.DialogButtonCollection(temps);
        },
        moveDown = function (btn) {
            arrange(btn, 1);
        },
        moveUp = function (btn) {
            arrange(btn, -1);
        };

    init();

    return {
        removeDialogButton: removeDialogButton,
        addDialogButton: addDialogButton,
        moveDown: moveDown,
        moveUp: moveUp
    };
};
bespoke.sph.domain.DialogButtonPartial = function (model) {

    var canMoveUp = ko.observable(true),
        canMoveDown = ko.observable(true);

    return {
        canMoveDown: canMoveDown,
        canMoveUp: canMoveUp
    };
};