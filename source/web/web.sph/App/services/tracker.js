/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="./datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext'],
    function (context) {

        var getPendingTasks = function (id) {
            var tcs = new $.Deferred(),
                data = JSON.stringify({ id: id });
            context.post(data, "/Workflow/GetPendingTasks/" + id)
                .then(function (result) {
                    tcs.resolve(result);
                });
            return tcs.promise();

        };

        var tracker = {
            getPendingTasks: getPendingTasks
        };

        return tracker;

    });
