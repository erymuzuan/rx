/// <reference path="../schemas/report.builder.g.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/jquery-2.1.0.intellisense.js" />
/// <reference path="../../App/durandal/amd/require.js" />
/// <reference path="../../App/durandal/system.js" />


bespoke.sph.domain.NotificationActivityPartial = function(model) {
    
    if (!model.From()) {
        model.From("admin@@sph.my");
    }
    return {
    };
};