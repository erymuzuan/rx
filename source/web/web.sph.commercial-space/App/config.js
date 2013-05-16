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
        }, {
            url: 'buildingdetail/:id',
            moduleId: 'viewmodels/buildingdetail',
            name: 'BuildingDetail',
            caption: '<i class="icon-user"></i> Building Details',
            visible: false
        }, {
            url: 'lotdetail/:buildingId/:floorname',
            moduleId: 'viewmodels/lotdetail',
            name: 'LotDetail',
            caption: '<i class="icon-user"></i> Lot Details',
            visible: false
        },
        {
            url: 'commercialspace/:buildingId/:floorname/:lotname',
            moduleId: 'viewmodels/commercialspace',
            name: 'commercial space',
            visible: true,
            caption: '<i class="icon-user"></i> commercial space'
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