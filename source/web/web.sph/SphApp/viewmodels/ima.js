/**
 * Created by itpro on 2/5/14.
 */
/// <reference path="../../Scripts/jquery-2.0.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.0.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../schemas/trigger.workflow.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router'],
    function (context, logger, router) {

        var isBusy = ko.observable(false),
            activate = function () {
                return true;
            },
            attached = function (view) {

            },
            generateDocument = function(){
                var json = ko.mapping.toJSON({
                    data : customer,
                    templateName:'customer.invite.docx',
                    format : ['pdf', 'docx']

                });
                context.post('/Shp/Document/Generate', json)
                    .done(function(result){
                        window.open(result.url);
                    });
            };

        var vm = {
            isBusy: isBusy,
            activate: activate,
            attached: attached,
            generateDocument :generateDocument
        };

        return vm;

    });
