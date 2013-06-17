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
            icon : 'icon-desktop',
            caption: 'Papan Tugas'
        },
        {
            url: 'organization.detail',
            moduleId: 'viewmodels/organization.detail',
            name: 'Setting Org',
            visible: true,
            icon : 'icon-desktop',
            caption: 'org'
        },
        {
            url: 'offerdetails/:rentalId/:csId',
            moduleId: 'viewmodels/offerdetails',
            name: 'Tawaran',
            visible: false,
            icon: 'icon-laptop',
            caption: 'Tawaran'
        },
        {
            url: 'contractlist',
            moduleId: 'viewmodels/contractlist',
            name: 'Kontrak',
            visible: true,
            icon: 'icon-briefcase',
            caption: 'Kontraks',
            settings: { caption: 'Senarai Kontraks' }
        },
        {
            url: 'contractdetails/:id',
            moduleId: 'viewmodels/contractdetails',
            name: 'contractdetails',
            visible: false,
            icon: 'icon-laptop',
            caption: 'Kontraks',
            settings: { caption: 'Maklumat Kontraks' }
        },
        {
            url: 'building',
            moduleId: 'viewmodels/building',
            name: 'Bangunan',
            visible: true,
            icon: 'icon-building',
            caption: 'Building',
            settings: { caption: 'Senarai Bangunan' }
        },
        {
            url: 'setting',
            moduleId: 'viewmodels/setting',
            name: 'Setting',
            visible: true,
            caption: 'Setting',
            icon: 'icon-laptop',
            settings: { caption: 'Settings' }
        },
        {
            url: 'createcontract/:rentalApplicationId',
            moduleId: 'viewmodels/createcontract',
            name: 'create contract',
            visible: false,
            icon: 'icon-laptop',
            caption: 'Buat kontrak'
        },
        {
            url: 'contracttype',
            moduleId: 'viewmodels/contracttype',
            name: 'contract type',
            visible: true,
            icon: 'icon-laptop',
            caption: 'Buat jenis kontrak'
        },
        {
            url: 'contracttypetemplate/:id',
            moduleId: 'viewmodels/contracttypetemplate',
            name: 'contract type',
            visible: false,
            icon: 'icon-laptop',
            caption: 'Buat jenis kontrak'
        },
        {
            url: 'commercialspace',
            moduleId: 'viewmodels/commercialspace',
            name: 'Ruang Komersial',
            visible: true,
            icon: 'icon-laptop',
            caption: 'Senarai Ruang Komersial'
        },
        {
            url: 'buildingbound',
            moduleId: 'viewmodels/buildingbound',
            name: 'Peta Kawasan',
            visible: true,
            icon: 'icon-globe',
            caption: 'Peta Kawasan'
        }, {
            url: 'buildingdetail/:id',
            moduleId: 'viewmodels/buildingdetail',
            name: 'BuildingDetail',
            caption: 'Building Details',
            icon: 'icon-laptop',
            visible: false
        }, {
            url: 'lotdetail/:buildingId/:floorname',
            moduleId: 'viewmodels/lotdetail',
            name: 'LotDetail',
            caption: 'Lot Details',
            icon: 'icon-tablet',
            visible: false
        }, {
            url: 'floorplan/:buildingId/:floorname',
            moduleId: 'viewmodels/floorplan',
            name: 'FloorPlan',
            caption: 'Floor plan',
            icon: 'icon-laptop',
            visible: false
        },
        {
            url: 'commercialspacedetail/:buildingId/:floorname/:commercialspaceid',
            moduleId: 'viewmodels/commercialspacedetail',
            name: 'ruang komersial',
            visible: false,
            icon: 'icon-laptop',
            caption: 'commercial space'
        },
        {
            url: 'rentalapplication/:id',
            moduleId: 'viewmodels/rentalapplication',
            name: 'rental application',
            visible: false,
            icon: 'icon-laptop',
            caption: 'rentalapplication',
            settings: { caption: 'Permohonan' }
        },
        {
            url: 'buildingforrental',
            moduleId: 'viewmodels/buildingforrental',
            name: 'Permohonan',
            visible: true,
            caption: 'permohonan',
            icon: 'icon-envelope',
            settings: { caption: 'Permohonan' }
        },
        {
            url: 'applicationlist/:status',
            moduleId: 'viewmodels/applicationlist',
            name: 'applicationlist',
            visible: false,
            caption: 'Senarai Permohonan',
            icon: 'icon-laptop',
            settings: { caption: 'Senarai permohonan' }
        },
        {
            url: 'verifyapplication/:applicationId',
            moduleId: 'viewmodels/verifyapplication',
            name: 'applicationdetailforverification',
            visible: false,
            caption: 'Senarai Permohonan',
            icon: 'icon-laptop',
            settings: { caption: 'Maklumat permohonan' }
        },
        {
            url: 'returnedapplication/:id',
            moduleId: 'viewmodels/returnedapplication',
            name: 'rental application',
            visible: false,
            caption: 'returnedapplication',
            icon: 'icon-laptop',
            settings: { caption: 'Kembalikan Permohonan' }
        },
        {
            url: 'deposit',
            moduleId: 'viewmodels/deposit',
            name: 'deposit',
            visible: true,
            caption: 'deposit',
            icon: 'icon-laptop',
            settings: { caption: 'Senarai deposit' }
        },
        {
            url: 'tenant',
            moduleId: 'viewmodels/tenant',
            name: 'Penyewa',
            visible: true,
            caption: 'Penyewa',
            icon: 'icon-laptop',
            settings: { caption: 'Senarai tenant' }
        },
        {
            url: 'tenant.detail/:tenantId',
            moduleId: 'viewmodels/tenant.detail',
            name: 'tenant.detail',
            visible: false,
            caption: 'penyewa',
            icon: 'icon-laptop',
            settings: { caption: 'Penyewa' }
        },
        {
            url: 'payment',
            moduleId: 'viewmodels/payment',
            name: 'Bayaran',
            visible: true,
            caption: 'Bayaran',
            icon: 'icon-laptop',
            settings: { caption: 'Bayaran' }
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