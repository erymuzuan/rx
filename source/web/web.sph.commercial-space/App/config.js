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
        name: 'Papan Tugas',
        visible: true,
        caption: '<i class="icon-user"></i> Papan Tugas'
    },
        {
            url: 'building',
            moduleId: 'viewmodels/building',
            name: 'Bangunan',
            visible: true,
            caption: 'Building',
            settings: { caption: '<i class="icon-user"></i> Building' }
        },
        {
            url: 'commercialspace',
            moduleId: 'viewmodels/commercialspace',
            name: 'Ruang Komersial',
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
        }, {
            url: 'floorplan/:buildingId/:floorname',
            moduleId: 'viewmodels/floorplan',
            name: 'FloorPlan',
            caption: 'Floor plan',
            visible: false
        },
        {
            url: 'commercialspacedetail/:buildingId/:floorname/:lotname',
            moduleId: 'viewmodels/commercialspacedetail',
            name: 'ruang komersial',
            visible: false,
            caption: '<i class="icon-user"></i> commercial space'
        },
        {
            url: 'rentalapplication/:id',
            moduleId: 'viewmodels/rentalapplication',
            name: 'rental application',
            visible: false,
            caption: 'rentalapplication',
            settings: { caption: '<i class="icon-user"></i> Permohonan' }
        },
        {
            url: 'buildingforrental',
            moduleId: 'viewmodels/buildingforrental',
            name: 'permohonan',
            visible: true,
            caption: 'permohonan',
            settings: { caption: '<i class="icon-user"></i> permohonan' }
        },
        {
            url: 'commercialspaceforrental/:buildingId',
            moduleId: 'viewmodels/commercialspaceforrental',
            name: 'commercialspaceforrental',
            visible: false,
            caption: 'commercialspaceforrental',
            settings: { caption: '<i class="icon-user"></i> permohonan' }
        }
    ];
    
    var startModule = 'building';

    return {
        debugEnabled: ko.observable(true),
        imageSettings: imageSettings,
        remoteServiceName: remoteServiceName,
        routes: routes,
        startModule: startModule
    };
});