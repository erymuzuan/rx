define(function () {
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';

    var imageSettings = {
        imageBasePath: '../content/images/photos/',
        unknownPersonImageSource: 'unknown_person.jpg'
    };

    var remoteServiceName = 'api/Breeze';

    var routes = [{
        url: 'admindashboard',
        moduleId: 'viewmodels/admindashboard',
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
            settings: { caption: 'Senaria Bangunan' }
        },
        {
            url: 'commercialspace',
            moduleId: 'viewmodels/commercialspace',
            name: 'Ruang Komersial',
            visible: true,
            caption: 'Senarai Ruang Komersial'
        },
        {
            url: 'buildingbound',
            moduleId: 'viewmodels/buildingbound',
            name: 'Peta Kawasan',
            visible: true,
            caption: 'Peta Kawasan'
        }, {
            url: 'buildingdetail/:id',
            moduleId: 'viewmodels/buildingdetail',
            name: 'BuildingDetail',
            caption: 'Building Details',
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
            caption: 'commercial space'
        },
        {
            url: 'rentalapplication/:id',
            moduleId: 'viewmodels/rentalapplication',
            name: 'rental application',
            visible: false,
            caption: 'rentalapplication',
            settings: { caption: 'Permohonan' }
        },
        {
            url: 'buildingforrental',
            moduleId: 'viewmodels/buildingforrental',
            name: 'permohonan',
            visible: true,
            caption: 'permohonan',
            settings: { caption: 'Permohonan' }
        },
        {
            url: 'commercialspaceforrental/:buildingId',
            moduleId: 'viewmodels/commercialspaceforrental',
            name: 'commercialspaceforrental',
            visible: false,
            caption: 'commercialspaceforrental',
            settings: { caption: 'Permohonan' }
        },
        {
            url: 'applicationlist/:status',
            moduleId: 'viewmodels/applicationlist',
            name: 'applicationlist',
            visible: false,
            caption: 'Senarai Permohonan',
            settings: { caption: '<i class="icon-user"></i> Senarai permohonan' }
        },
        {
            url: 'verifyapplication/:applicationId',
            moduleId: 'viewmodels/verifyapplication',
            name: 'applicationdetailforverification',
            visible: false,
            caption: 'Senarai Permohonan',
            settings: { caption: '<i class="icon-user"></i> Maklumat permohonan' }
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