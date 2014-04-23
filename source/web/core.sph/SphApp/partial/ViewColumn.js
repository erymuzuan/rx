/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.ViewColumnPartial = function () {

    // Filter
    var system = require('durandal/system'),
        addConditionalFormatting = function () {
            this.ConditionalFormattingCollection.push(new bespoke.sph.domain.ConditionalFormatting(system.guid()));
        },
        removeConditionalFormatting = function (cf) {
            var self = this;
            return function () {
                self.ConditionalFormattingCollection.remove(cf);
            };

        },
        removeIconCssClass = function () {
            this.IconCssClass('');
        },
        removeIconStoreId = function () {
            this.IconStoreId('');
        };
    return {
        addConditionalFormatting: addConditionalFormatting,
        removeConditionalFormatting: removeConditionalFormatting,
        removeIconStoreId: removeIconStoreId,
        removeIconCssClass: removeIconCssClass
    };
};