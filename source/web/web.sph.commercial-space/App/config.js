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
            caption: 'Building',
            settings: { caption: '<i class="icon-user"></i> Building' }
        },
        {
            url: 'commercialspace',
            moduleId: 'viewmodels/commercialspace',
            name: 'Commercial Space',
            visible: true,
            caption: '<i class="icon-user"></i> Commercial Space'
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
            url: 'commercialspacedetail/:buildingId/:floorname/:lotname',
            moduleId: 'viewmodels/commercialspacedetail',
            name: 'commercial space',
            visible: false,
            caption: '<i class="icon-user"></i> commercial space'
        },
        {
            url: 'rentalapplication',
            moduleId: 'viewmodels/rentalapplication',
            name: 'rental application',
            visible: true,
            caption: 'rentalapplication',
            settings: { caption: '<i class="icon-user"></i> Permohonan' }
        },
        {
            url: 'buildingforrental',
            moduleId: 'viewmodels/buildingforrental',
            name: 'buildingforrental',
            visible: true,
            caption: 'buildingforrental',
            settings: { caption: '<i class="icon-user"></i> buildingforrental' }
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