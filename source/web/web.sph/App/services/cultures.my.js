/// <reference path="../durandal/amd/require.js" />


define([],
    function() {
        var cultures = {
            building: {
                SAVE_BUILDING_MESSAGE: "Bangunan sudah di simpan",
                MAP_TITLE: "Peta bangunan"
            },
            space: {
                title: "Senarai ruang",
                ADD_NEW_BUILDING : "[Tiada dalam senarai]",
                toolbar : {
                    ADD_NEW_SPACE : "Tambah ruang baru"
                }
            },
            spacetemplate : {
                title : "Templat ruang"
            },
            messages : {
                SAVE_SUCCESS: "{0} sudah beraya di simpan",
                SAVE_ERROR: "Ada masalah untuk menyimpan data anda",
                FORM_IS_NOT_VALID: "Sila pastikan input anda betul"
            },
            lots: {
                LOT_LIST_TITLE: "Senarai lot di block : {0}, tingkat : {1}"
            }            
        };

        return cultures;

    });
