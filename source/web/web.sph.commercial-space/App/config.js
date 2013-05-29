define(function () {
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';

    var imageSettings = {
        imageBasePath: '../content/images/photos/',
        unknownPersonImageSource: 'unknown_person.jpg'
    };

    var remoteServiceName = 'api/Breeze';

    var routes = [
        {
            url: 'admindashboard',
            moduleId: 'viewmodels/admindashboard',
            name: 'Papan Tugas',
            visible: true,
            caption: 'Papan Tugas'
        }, {
            url: 'offerdetails/:rentalId/:csId',
            moduleId: 'viewmodels/offerdetails',
            name: 'Tawaran',
            visible: false,
            caption: 'Tawaran'
        },
        {
            url: 'building',
            moduleId: 'viewmodels/building',
            name: 'Bangunan',
            visible: true,
            caption: 'Building',
            settings: { caption: 'Senarai Bangunan' }
        },
        {
            url: 'setting',
            moduleId: 'viewmodels/setting',
            name: 'Setting',
            visible: true,
            caption: 'Setting',
            settings: { caption: 'Settings' }
        },
        {
            url: 'contractsetup',
            moduleId: 'viewmodels/contractsetup',
            name: 'contract setup',
            visible: true,
            caption: 'setup kontrak'
        },
        {
            url: 'createcontract/:rentalApplicationId',
            moduleId: 'viewmodels/createcontract',
            name: 'create contract',
            visible: false,
            caption: 'Buat kontrak'
        },
        {
            url: 'contracttype',
            moduleId: 'viewmodels/contracttype',
            name: 'contract type',
            visible: true,
            caption: 'Buat jenis kontrak'
        },
        {
            url: 'contracttypetemplate/:id',
            moduleId: 'viewmodels/contracttypetemplate',
            name: 'contract type',
            visible: false,
            caption: 'Buat jenis kontrak'
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
            caption: 'Lot Details',
            visible: false
        }, {
            url: 'floorplan/:buildingId/:floorname',
            moduleId: 'viewmodels/floorplan',
            name: 'FloorPlan',
            caption: 'Floor plan',
            visible: false
        },
        {
            url: 'commercialspacedetail/:buildingId/:floorname/:commercialspaceid',
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
            settings: { caption: 'Senarai permohonan' }
        },
        {
            url: 'verifyapplication/:applicationId',
            moduleId: 'viewmodels/verifyapplication',
            name: 'applicationdetailforverification',
            visible: false,
            caption: 'Senarai Permohonan',
            settings: { caption: 'Maklumat permohonan' }
        },
        {
            url: 'returnedapplication/:id',
            moduleId: 'viewmodels/returnedapplication',
            name: 'rental application',
            visible: false,
            caption: 'returnedapplication',
            settings: { caption: 'Kembalikan Permohonan' }
        }
    ];
    
    var startModule = 'admindashboard';

    return {
        debugEnabled: ko.observable(true),
        imageSettings: imageSettings,
        remoteServiceName: remoteServiceName,
        routes: routes,
        startModule: startModule
    };
});