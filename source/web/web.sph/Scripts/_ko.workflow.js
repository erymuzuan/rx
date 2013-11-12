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
        name = /Bespoke\.Sph\.Domain\.(.*?),/.exec(fullName)[1];


        div.popover({
            title: name + ' : ' + act.Name(),
            html: true,
            content: function () {
                $('a.edit-activity').popover('hide');
                return div.find("div.context-menu").html();
            } 
        });
        $(document).on('click', 'a.edit-activity', function (e) {
            e.preventDefault();
            console.log(act);
        });
    }
};