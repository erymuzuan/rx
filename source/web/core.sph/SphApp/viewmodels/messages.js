define([objectbuilders.datacontext, objectbuilders.config, objectbuilders.logger],
  function (context, config, logger) {

      var isBusy = ko.observable(false),
        messages = ko.observableArray(),
        unread = ko.observable(),
        query = String.format("UserName eq '{0}' and IsRead eq 0", config.userName),
        activate = function () {

            return context.loadAsync({
                entity: "Message",
                includeTotal: true
            }, query)
              .then(function (lo) {
                  messages(lo.itemCollection);
                  unread(lo.rows);

                  if (typeof $.connection !== "function") {
                      return $.getScript("/scripts/jquery.signalR-2.1.2.min.js");
                  }
                  return Task.fromResult(true);
              });

        },
        getTimeSpan = function (m) {
            var date = moment(ko.unwrap(m.CreatedDate), "YYYY-MM-DDTHH:mm.ss");
            var ms = moment().diff(date);
            var d = moment.duration(ms);
            return d.humanize();
        },
        attached = function () {

            return activate()
            .done(function () {
                var connection = $.connection("/signalr_message");

                connection.received(function (data) {
                    if (typeof data.unread === "number") {
                        messages(data.messages);
                        unread(data.unread);
                    }
                    if (typeof data === "string") {
                        logger.info(data);
                    }
                });

                connection.start().done(function (e) {
                    console.log("started...connection to message connection");
                    console.log(e);
                });

            });
        };


      var vm = {
          isBusy: isBusy,
          getTimeSpan: getTimeSpan,
          unread: unread,
          activate: activate,
          attached: attached,
          messages: messages
      };

      return vm;

  });
