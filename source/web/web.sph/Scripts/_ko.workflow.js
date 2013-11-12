ko.bindingHandlers.activityClass = {
    init: function (element,valueAccessor) {

        var div = $(element),
            act = ko.unwrap(valueAccessor()),
        fullName = typeof act.$type === "function" ? act.$type() : act.$type,
        name = /Bespoke\.Sph\.Domain\.(.*?),/.exec(fullName)[1];

        div.addClass("activity32").addClass("activity32-" + name);
    }
};

ko.bindingHandlers.activityPopover = {
    init: function (element,valueAccessor) {

        var div = $(element),
            act = ko.unwrap(valueAccessor()),
            fullName = typeof act.$type === "function" ? act.$type() : act.$type,
            name = /Bespoke\.Sph\.Domain\.(.*?),/.exec(fullName)[1],
            wd = ko.dataFor(div.parent()[0]).wd();


        div.popover({
            title: name + ' : ' + act.Name(),
            html: true,
            content: function () {
                $('a.edit-activity').popover('hide');
                div.find("a.edit-activity").addClass(act.WebId());
                return div.find("div.context-menu").html();
            } 
        });
        $(document).on('click', 'a.' + act.WebId(), function (e) {
            e.preventDefault();
            wd.editActivity(act)();
        });
    }
};