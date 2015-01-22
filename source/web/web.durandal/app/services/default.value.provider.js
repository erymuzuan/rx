/// <reference path="../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="../../Scripts/knockout.mapping-latest.debug.js" />
/// <reference path="../../Scripts/require.js" />
/// <reference path="../../Scripts/underscore.js" />
/// <reference path="../../Scripts/moment.js" />
/// <reference path="../services/datacontext.js" />
/// <reference path="../services/domain.g.js" />
/// <reference path="../../Scripts/bootstrap.js" />


define(['services/datacontext', 'services/logger', 'plugins/router', objectbuilders.system],
	function (context, logger, router, system) {

	    var
        isBusy = ko.observable(false),
        loadAsync = function (type, template) {
            var tcs = new $.Deferred();
            var data = JSON;

            context.post(data, "/Template/PropertyPath/" + type)
                .then(function (result) {

                    var fields = _(result).map(function (f) {
                        var field = new bespoke.sph.domain.DefaultValue(system.guid());
                        field.PropertyName(f.Name);
                        field.TypeName(f.TypeName);
                        field.IsNullable(f.IsNullable);
                        // TODO :look for existing values
                        var ef = _(template.DefaultValueCollection()).find(function (e) {
                            return e.PropertyName() === f.Name;
                        });
                        if (ef) {
                            field.Value(ef.Value);
                        }
                        return field;
                    });
                    template.DefaultValueCollection(fields);
                    tcs.resolve(result);
                });
            return tcs.promise();


        },
        setDefaultValues = function (item, template) {
            // default values
            _(template.DefaultValueCollection()).each(function (v) {
                if (v.Value()) {
                    var props = v.PropertyName().split(".");
                    if (props.length === 1) {
                        item[props[0]](v.Value());
                        return;
                    }
                    var k = null;
                    for (var i = 0; i < props.length - 1; i++) {
                        if (typeof k === "function") {
                            k = item[props[i]]();
                        } else {
                            k = item[props[i]];
                        }

                    }
                    if (typeof k === "function") {
                        k()[props[props.length - 1]](v.Value());
                    } else {
                        throw "What the fuck is wrong with k,, !! ima kata : astrairllah";
                    }
                }
            });

        };

	    var vm = {
	        loadAsync: loadAsync,
	        setDefaultValues: setDefaultValues
	    };

	    return vm;

	});
