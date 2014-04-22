﻿/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.1.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />

bespoke.sph.domain.ButtonPartial = function () {

    var editCommand = function () {
        var self = this,
            w = window.open("/sph/editor/ace?mode=javascript", '_blank', 'height=' + screen.height + ',width=' + screen.width + ',toolbar=0,location=0,fullscreen=yes'),
            wdw = w.window || w,
            init = function () {
                wdw.code = ko.unwrap(self.Command);
                if (!w.code) {
                    w.code = "//insert your code here";
                }
                wdw.saved = function (code, close) {
                    self.Command(code);
                    if (close) {
                        w.close();
                    }
                };
            };
        if (wdw.attachEvent) { // for ie
            wdw.attachEvent('onload', init);
        } else {
            init();
        }
    };
    return {
        editCommand: editCommand
    };
};