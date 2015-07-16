define(function () {
    var guid = function () {
        return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === "x" ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    },
        showDialog = function (dialog) {

            // get the view
            var moduleId = dialog.__moduleId__,
                tcs = new $.Deferred();

            if (dialog.__view__) {
                dialog.__view__.modal();
            } else {
                $.get(moduleId.replace("viewmodels", "/app/views") + ".html")
                    .done(function (html) {
                        var panel = $(html).appendTo("body");
                        panel.modal();
                        dialog.__view__ = panel;
                        dialog.modal = {
                            close: function (result) {
                                tcs.resolve(result);
                            }
                        };
                        if (dialog.attached) {
                            dialog.attached(panel[0]);
                        }
                        if (dialog.activate) {
                            dialog.activate()
                                .done(function () {

                                    ko.applyBindings(dialog, panel[0]);
                                });
                        } else {

                            ko.applyBindings(dialog, panel[0]);
                        }

                    });
            }


            return tcs.promise();
        },
        showMessage = function (message, title, options) {
            var id = guid(),
                tcs = new $.Deferred(),
                dialog = $(
                    "<div class=\"modal fade\" id=\"" + id + "\">" +
                    "   <div class=\"modal-dialog\">" +
                    "       <div class=\"modal-content\">" +
                    "           <div class=\"modal-header\">" +
                    "               <button type=\"button\" data-bind=\"click: cancelClick\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">&times;</button>" +
                    "               <h4 class=\"modal-title\" data-bind=\"html: title\"></h3>" +
                    "           </div>" +
                    "           <div class=\"modal-body\">" +
                    "               <p class=\"message\" data-bind=\"html: message\"></p>" +
                    "           </div>" +
                    "           <div class=\"modal-footer\" data-bind=\"foreach: options\">" +
                    "               <button class=\"btn btn-default\" data-bind=\"click: function () { $parent.selectOption($data); }, html: $data, css: { autofocus: $index() == 0 }\"></button>" +
                    "           </div>" +
                    "       </div>" +
                    "   </div>" +
                    "</div>");

            dialog.appendTo("body");
            var vm = {
                message: message,
                title: title,
                options: options,
                selectOption: function (result) {
                    tcs.resolve(result);
                    dialog.modal("hide");
                    setTimeout(function () { dialog.remove(); }, 500);
                },
                cancelClick: function () {
                    tcs.resolve(false);
                    setTimeout(function () { dialog.remove(); }, 500);
                }
            };
            ko.applyBindings(vm, document.getElementById(id));
            dialog.modal({ keyboard: true });

            return tcs.promise();

        };
    var app = {
        guid: guid,
        showModal: showDialog,
        showMessage: showMessage
    };
    return app;
});