define(function () {
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';

    var imageSettings = {
        imageBasePath: '../content/images/photos/',
        unknownPersonImageSource: 'unknown_person.jpg'
    };

    var remoteServiceName = 'api/Breeze';

    var routes = [{
        url: 'details',
        moduleId: 'viewmodels/details',
        name: 'Details',
        visible: true,
        caption: '<i class="icon-user"></i> Details'
    },
        {
            url: 'building',
            moduleId: 'viewmodels/building',
            name: 'Building',
            visible: true,
            caption: '<i class="icon-user"></i> Building'
        }
    ];
    
    var startModule = 'details';

    return {
        debugEnabled: ko.observable(true),
        imageSettings: imageSettings,
        remoteServiceName: remoteServiceName,
        routes: routes,
        startModule: startModule
    };
});