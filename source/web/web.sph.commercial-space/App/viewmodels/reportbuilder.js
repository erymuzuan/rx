define(['services/datacontext'], function(context) {
    var title = ko.observable('Report Builder'),
        isBusy = ko.observable(false),
        activate = function() {
            return true;
        };

    var vm = {
        title: title,
        isBusy: isBusy,
        activate: activate,
        toolbar: {
            
        }
    };

    return vm;
    
});