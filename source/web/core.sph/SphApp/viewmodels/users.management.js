/// <reference path="~\Scripts/jquery-2.2.0.intellisense.js" />
/// <reference path="~\Scripts/knockout-3.4.0.debug.js" />
/// <reference path="~\Scripts/r.js" />
/// <reference path="~\Scripts/require.js" />
/// <reference path="~\Scripts/_task.js" />
/// <reference path="~\SphApp\schemas/sph.domain.g.js" />
/// <reference path="~/SphApp/objectbuilders.js" />

define([objectbuilders.datacontext, "services/logger", objectbuilders.dialog, objectbuilders.app], function (context, logger, app) {
    const isBusy = ko.observable(false),
        printprofile = ko.observable(new bespoke.sph.domain.Profile()),
        profiles = ko.observableArray(),
        selectedUsers = ko.observableArray(),
        importedSecuritySettingStoreId = ko.observable(""),
        map = function (item) {
            item.IsNew = ko.observable(false);
            return item;
        },
        activate = function () {
            const query = String.format("Id ne '0'");
            return context.loadAsync("UserProfile", query)
                .then(function (p) {
                    isBusy(false);

                    const list = _(p.itemCollection).map(map);
                    profiles(list);

                });
        },
        searchUsers = function (text) {
            return context.loadAsync("UserProfile", `substringof('${text}', UserName) eq true OR substringof('${text}', FullName) eq true`)
            .done(function (lo) {
                const list = lo.itemCollection.map(map);
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
                    const existing = _(profiles()).find(function (v) {
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
        savePassword = function (profile, password1) {
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
        lockUsers = function () {
            const tcs = $.Deferred();
            app.showMessage(`Are you sure you want to lock  ${selectedUsers().length} user(s), this action cannot be undone`, "Rx Developer", ["Yes", "No"])
                .done(function (dialogResult) {
                    if (dialogResult === "Yes") {

                        const tasks = selectedUsers().map(v => context.post(ko.mapping.toJSON(v), `/admin/lock/${ko.unwrap(v.UserName)}`));
                        $.when(tasks).done(function (result) {
                            if (result.success) {
                                logger.info(`Selected users has been successfully locked`);
                                selectedUsers.forEach(v => profiles.remove(v));
                                tcs.resolve(true);
                            }
                        });
                    } else {
                        tcs.resolve(false);
                    }
                });

            return tcs.promise();
        },
        unlockUsers = function () {
            const tcs = $.Deferred();
            app.showMessage(`Are you sure you want to unlock ${selectedUsers().length} user(s), this action cannot be undone`, "Rx Developer", ["Yes", "No"])
                .done(function (dialogResult) {
                    if (dialogResult === "Yes") {

                        const tasks = selectedUsers().map(v => context.post(ko.mapping.toJSON(v), `/admin/unlock/${ko.unwrap(v.UserName)}`));
                        $.when(tasks)
                            .done(function (result) {
                                if (result.success) {
                                    logger.info(`Selected users has been successfully unlocked`);
                                    tcs.resolve(true);
                                }
                            });
                    } else {
                        tcs.resolve(false);
                    }
                });

            return tcs.promise();
        },
        removeUsers = function () {
            const tcs = $.Deferred();
            app.showMessage(`Are you sure you want to remove ${selectedUsers().length} user(s), this action cannot be undone`, "Rx Developer", ["Yes", "No"])
                .done(function (dialogResult) {
                    if (dialogResult === "Yes") {

                        const tasks = selectedUsers()
                            .map(v => context.sendDelete(`/admin/RemoveUser/${ko.unwrap(v.UserName)}`));
                        $.when(tasks)
                            .done(function (result) {
                                if (result.success) {
                                    logger.info(`Selected users has been successfully removed`);
                                    selectedUsers.forEach(v => profiles.remove(v));
                                    tcs.resolve(true);
                                }
                            });
                    } else {
                        tcs.resolve(false);
                    }
                });

            return tcs.promise();
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
        searchUsers: searchUsers,
        profiles: profiles,
        selectedUsers: selectedUsers,
        printprofile: printprofile,
        editCommand: edit,
        lockUsers: lockUsers,
        unlockUsers: unlockUsers,
        removeUsers: removeUsers,
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