﻿
define([objectbuilders.datacontext, "services/logger", objectbuilders.dialog, objectbuilders.app], function (context, logger, app) {
    const isBusy = ko.observable(false),
        printprofile = ko.observable(new bespoke.sph.domain.Profile()),
        profiles = ko.observableArray(),
        importedSecuritySettingStoreId = ko.observable(""),
        map = function (item) {
            const p = new bespoke.sph.domain.Profile();
            p.IsNew(false);
            p.FullName(item.FullName());
            p.UserName(item.UserName());
            p.Email(item.Email());
            p.Designation(item.Designation());
            p.Department(item.Department());
            p.Telephone(item.Telephone());
            return p;
        },
        activate = function () {
            const query = String.format("Id ne '0'");
            return context.loadAsync("UserProfile", query)
                .then(function (p) {
                    isBusy(false);

                    var list = _(p.itemCollection).map(map);
                    profiles(list);

                });
        },
        save = function (profile) {
            const data = ko.mapping.toJSON({ profile: profile });
            isBusy(true);

            return context.post(data, "/sph/Admin/AddUser")
                .then(function (result) {
                    isBusy(false);
                    if (!result.success) {
                        logger.error(result.message);
                        return;
                    }
                    var usp = result.profile;
                    var existing = _(profiles()).find(function (v) {
                        return ko.unwrap(v.UserName) === ko.unwrap(usp.UserName);
                    });
                    if (existing) {
                        profiles.replace(existing, usp);
                    } else {
                        profiles.push(usp);
                    }
                });
        },
        add = function () {
            var user = new bespoke.sph.domain.Profile();
            user.IsNew = ko.observable(true);

            require(["viewmodels/user.dialog", "durandal/app"], function (dialog, app2) {
                dialog.profile(user);
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (result === "OK") {
                            return save(dialog.profile());
                        }
                        return Task.fromResult(0);
                    });
            });
        },
        edit = function (user) {
            var clone = ko.mapping.fromJSON(ko.mapping.toJSON(user));
            clone.IsNew = ko.observable(false);

            require(["viewmodels/user.dialog", "durandal/app"], function (dialog, app2) {
                dialog.profile(clone);
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (result === "OK") {
                            return save(dialog.profile());
                        }
                        return Task.fromResult(0);
                    });
            });



        },
        savePassword = function (profile, password1, password2) {
            const data = ko.mapping.toJSON({ userName: profile.UserName(), password: password1 });
            isBusy(true);

            return context.post(data, "/sph/Admin/ResetPassword")
                .then(function (result) {
                    isBusy(false);
                    if (result.OK) {
                        logger.info(result.messages);
                    } else {
                        logger.logError(result.messages, this, this, true);
                    }
                });

        },
        resetPassword = function (user) {
            require(["viewmodels/user.reset.password.dialog", "durandal/app"], function (dialog, app2) {
                app2.showDialog(dialog)
                    .done(function (result) {
                        if (result === "OK") {
                            return savePassword(user, dialog.password1, dialog.password2);
                        }
                        return Task.fromResult(0);
                    });
            });
        },
        exportCommand = function () {
            window.open("/sph/admin/ExportSecuritySettings");
            return Task.fromResult(true);

        },
        removeUser = function(profile) {
            app.showMessage(`Are you sure you want to remove ${profile.UserName()}, this action cannot be undone`, "Rx Developer", ["Yes", "No"])
                .done(function(dialogResult) {
                    if (dialogResult === "Yes") {
                        context.sendDelete(`/admin/RemoveUser/${ko.unwrap(profile.UserName)}`).done(function(result) {
                            if (result.success) {
                                logger.info(`${profile.UserName()} has been successfully removed`);
                                profiles.remove(profile);
                            }
                        });
                    }
                });
        };

    importedSecuritySettingStoreId.subscribe(function (id) {
        context.post(JSON.stringify({ "id": id }), "/sph/admin/import/" + id)
        .done(function () {
            logger.info("All the settings has been imported");
            activate();
        });
    });

    const vm = {
        importedSecuritySettingStoreId: importedSecuritySettingStoreId,
        activate: activate,
        profiles: profiles,
        printprofile: printprofile,
        editCommand: edit,
        removeUser: removeUser,
        add: add,
        resetPasswordCommand: resetPassword,
        map: map,
        searchTerm: {
            department: ko.observable(),
            keyword: ko.observable()
        },
        toolbar: ko.observable({
            exportCommand: exportCommand,
            reloadCommand: function () {
                return activate();
            },
            printCommand: ko.observable({
                entity: ko.observable("UserProfile"),
                id: ko.observable(0),
                item: printprofile
            })
        })
    };


    return vm;


});