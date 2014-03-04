define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog, objectbuilders.watcher],
    function (context, logger, router, system, validation, eximp, dialog, watcher) {

        var entity = ko.observable(new bespoke.dev_1.domain.Customer({WebId: system.guid()})),
            form = ko.observable(new bespoke.sph.domain.EntityForm()),
            watching = ko.observable(false),
            id = ko.observable(),
            activate = function (id2) {
                id(parseInt(id2));
                var query = String.format("CustomerId eq {0}", id2),
                    tcs = new $.Deferred(),
                    itemTask = context.loadOneAsync("Customer", query),
                    formTask = context.loadOneAsync("EntityForm", "Route eq 'add-customer-form'"),
                    watcherTask = watcher.getIsWatchingAsync("Customer", id2);

                $.when(itemTask, formTask, watcherTask).done(function (b, f, w) {
                    if (b) {
                        var item = context.toObservable(b, /Bespoke\.Dev_1\.Domain\.(.*?),/);
                        entity(item);


                        vm.toolbar.printCommand.id(parseInt(id2));

                    }
                    else {
                        entity(new bespoke.dev_1.domain.Customer({WebId: system.guid()}));
                    }
                    form(f);
                    watching(w);

                    tcs.resolve(true);
                });

                return tcs.promise();
            },
            attached = function (view) {
                // validation
                validation.init($('#add-customer-form-form'), form());

            },

            Customer;
        CheckTheRevenue = function () {

            var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(entity);

            context.post(data, "/Sph/BusinessRule/Validate?Customer;CheckTheRevenue")
                .then(function (result) {
                    tcs.resolve(result);
                });
            return tcs.promise();
        },
            Customer;
        VerifyTheGrade = function () {

            var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(entity);

            context.post(data, "/Sph/BusinessRule/Validate?Customer;VerifyTheGrade")
                .then(function (result) {
                    tcs.resolve(result);
                });
            return tcs.promise();
        },
            Customer;
        VerifyTheAge = function () {

            var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(entity);

            context.post(data, "/Sph/BusinessRule/Validate?Customer;VerifyTheAge")
                .then(function (result) {
                    tcs.resolve(result);
                });
            return tcs.promise();
        },
            Customer;
        MustBeMalaysian = function () {

            var tcs = new $.Deferred(),
                data = ko.mapping.toJSON(entity);

            context.post(data, "/Sph/BusinessRule/Validate?Customer;MustBeMalaysian")
                .then(function (result) {
                    tcs.resolve(result);
                });
            return tcs.promise();
        },
            doThis = function () {
                console.log("do this");
                return Task.fromResult(true);
            },
            save = function () {
                if (!validation.valid()) {
                    return Task.fromResult(false);
                }

                var tcs = new $.Deferred(),
                    data = ko.mapping.toJSON(entity);


                context.post(data, "/Sph/BusinessRule/Validate?Customer;CheckTheRevenue;VerifyTheGrade;VerifyTheAge;MustBeMalaysian")
                    .then(function (result) {
                        if (result.success) {
                            context.post(data, "/Customer/Save")
                                .then(function (result) {
                                    tcs.resolve(result);
                                });
                        } else {
                            tcs.resolve(result);
                        }
                    });


                return tcs.promise();
            };

        var vm = {
            CheckTheRevenue: CheckTheRevenue,
            VerifyTheGrade: VerifyTheGrade,
            VerifyTheAge: VerifyTheAge,
            MustBeMalaysian: MustBeMalaysian,
            activate: activate,
            attached: attached,
            entity: entity,
            save: save,
            toolbar: {
                emailCommand: {
                    entity: 'Customer',
                    id: id
                },

                watchCommand: function () {
                    return watcher.watch("Customer", entity().CustomerId())
                        .done(function () {
                            watching(true);
                        });
                },
                unwatchCommand: function () {
                    return watcher.unwatch("Customer", entity().CustomerId())
                        .done(function () {
                            watching(false);
                        });
                },
                watching: watching,
                printCommand: {
                    entity: 'Customer',
                    id: id
                },

                saveCommand: save,
                commands: ko.observableArray([
                    { caption: "Do this well", command: doThis, icon: "fa fa-user" }
                ])
            }
        };

        return vm;
    });
