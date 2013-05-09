define(function () {
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';

    var imageSettings = {
        imageBasePath: '../content/images/photos/',
        unknownPersonImageSource: 'unknown_person.jpg'
    };

    var remoteServiceName = 'api/Breeze';

    var routes = [{
        url: 'shiftschedule',
        moduleId: 'viewmodels/shiftschedule',
        name: 'Shift Schedule',
        visible: true,
        caption: '<i class="icon-book"></i> Shift Schedule'
    }, {
        url: 'details',
        moduleId: 'viewmodels/details',
        name: 'Details',
        visible: true,
        caption: '<i class="icon-user"></i> Details'
    },/* {
        url: 'sessiondetail/:id',
        moduleId: 'viewmodels/sessiondetail',
        name: 'Edit Session',
        visible: false
    },*/ {
        url: 'leave',
        moduleId: 'viewmodels/leave',
        name: 'Leave Application',
        visible: true,
        caption: '<i class="icon-plus"></i> Leave application',
        settings: {admin: true}
    }
    ];
    
    var startModule = 'shiftschedule';

    return {
        debugEnabled: ko.observable(true),
        imageSettings: imageSettings,
        remoteServiceName: remoteServiceName,
        routes: routes,
        startModule: startModule
    };
});