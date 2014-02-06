define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog],
    function (context, logger, router, system, validation, eximp, dialog) {

        var entity = ko.observable(new bespoke.dev_1.domain.Customer({WebId: system.guid()})),
            form = ko.observable(new bespoke.sph.domain.EntityForm()),
            activate = function (id) {

                var query = String.format("CustomerId eq {0}", id),
                    tcs = new $.Deferred(),
                    itemTask = context.loadOneAsync("Customer", query),
                    formTask = context.loadOneAsync("EntityForm", "Route eq 'add-customer-form'");

                $.when(itemTask, formTask).done(function (b, f) {
                    if (b) {
                        var item = context.toObservable(b, /Bespoke\.Dev_1\.Domain\.(.*?),/);
                        entity(item);


                        vm.toolbar.printCommand.id(parseInt(id));

                    }
                    else {
                        entity(new bespoke.dev_1.domain.Customer({WebId: system.guid()}));
                    }
                    form(f);

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
                emailCommand: function () {
                    console.log("Sending email");
                    return Task.fromResult(true);
                },
                emailCommand: {},
                printCommand: {
                    entity: 'Customer',
                    id: ko.observable()
                },

                saveCommand: save,
                commands: ko.observableArray([
                    { caption: "Do this well", command: doThis, icon: "fa fa-user" }
                ])
            }
        };

        return vm;
    });
