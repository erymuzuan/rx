/// <reference path="../schemas/sph.domain.g.js" />
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/underscore.js" />

var bespoke = bespoke || {};
bespoke.sphcommercialspace = bespoke.sphcommercialspace || {};
bespoke.sphcommercialspace.domain = bespoke.sphcommercialspace.domain || {};


bespoke.sphcommercialspace.domain.ComplaintPartial = function (model) {
    var getStatus = function (context) {
        var r = this;
        var query = String.format("ComplaintId eq {0}", model.ComplaintId());
        var tcs = new $.Deferred();
        context.loadOneAsync("Maintenance", query)
            .done(function (b) {
                r.MaintenanceStatus(b.Status());
                tcs.resolve(b.Status);
            });
        return tcs.promise();
    },
        getCustomField = function (name) {
            var cs = _(this.CustomFieldValueCollection()).find(function (v) {
                return v.Name() === name;
            });
            if (!cs) {
                throw "Cannot find custom field for " + name + " in Complaint";
            }
            return cs.Value;
        };
    return {
        CustomField: getCustomField,
        MaintenanceStatus: ko.observable(),
        getMaintenanceStatus: getStatus
    };
};