/// <reference path="../../Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.4.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../../../core.sph/SphApp/schemas/__domain.js" />
/// <reference path="../../../core.sph/SphApp/objectbuilders.js" />
/// <reference path="../../../core.sph/Scripts/_task.js" />
/// <reference path="../../../core.sph/Scripts/require.js" />


define(["plugins/dialog", objectbuilders.datacontext, objectbuilders.system],
    function (dialog, context, system) {

        const port = ko.observable(new bespoke.sph.domain.ReceivePort({
            TextFormatter: {
                Name: ko.observable(),
                SampleStoreId: ko.observable()
            }
        })),
            delimiterOptions = ko.observableArray([
            {
                text: "csv(,)",
                value:","
            }, {
                text: "[TAB]",
                value:"\t"
            }, {
                text: "|",
                value:"|"
            }, {
                text: ":",
                value : ":"
            }]),
            id = ko.observable(),
            sampleText = ko.observable(),
            page = ko.observable(1),
            isDelimiter = ko.observable(false),
            isPositional = ko.observable(false),
            activate = function () {
                return true;
            },
            backEnable = ko.computed(function () {
                return ko.unwrap(page) > 1;
            }),
            nextEnable = ko.computed(function () {
                return ko.unwrap(page) < 5;
            }),
            okEnable = ko.computed(function () {
                if (ko.isObservable(port().isWizardOk)) {
                    return port().isWizardOk;
                }
                return true;
            }),
            backClick = function () {
                page(ko.unwrap(page) - 1);
            },
            nextClick = function () {
                page(ko.unwrap(page) + 1);
            },
            okClick = function (data, ev) {
                console.log(ev);
                const self = this;
                context.post(ko.mapping.toJSON(port), "/receive-ports/")
                    .done(function (r) {
                        port().Id(r.id);
                        id(r.id);
                        dialog.close(self, "OK");
                    });


            },
            cancelClick = function () {
                dialog.close(this, "Cancel");
            };

        port().Formatter.subscribe(function (f) {
            if (!f) {
                return;
            }
            port().TextFormatter(new bespoke.sph.domain[f](system.guid()));
        });

        const vm = {
            delimiterOptions : delimiterOptions,
            activate: activate,
            port: port,
            id: id,
            backClick: backClick,
            nextClick: nextClick,
            okClick: okClick,
            backEnable: backEnable,
            nextEnable: nextEnable,
            okEnable: okEnable,
            isDelimiter: isDelimiter,
            isPositional: isPositional,
            page: page,
            cancelClick: cancelClick,
            sampleText: sampleText
        };


        return vm;

    });
