/// <reference path="../durandal/amd/require.js" />


define([],
    function() {
        var cultures = {
            space: {
                title : "Senarai ruang",
                toolbar : {
                    ADD_NEW_SPACE : "Tambah ruang baru"
                }
            },
            spacetemplate : {
                title : "Templat ruang"
            },
            messages : {
                SAVE_SUCCESS: "{0} sudah beraya di simpan",
                SAVE_ERROR : "Ada masalah untuk menyimpan data anda"
            },
            lots: {
                LOT_LIST_TITLE: "Senarai lot di block : {0}, tingkat : {1}"
            }            
        };

        return cultures;

    });
