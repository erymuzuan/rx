bespoke.sph.Theme = function() {
    return {
        "rentalapplication.selectspace-spaceitem": function (element, space) {
            $(element).find("div.space-nav a").addClass("btn");
            if (space.Category() === "Kedai Runcit") {
                
            }
        },
        "public-index":function(element) {
            $(element).find("div.col-lg-6>div").first().after('<div><div><a class="box-label" href="/workflow_3_2/SkrinBorangPermohonan">Permohonan Wakaf Baru</a></div></div>');
        }
        //,
        //"admindashboard": function (element) {
        //    var div = $('<div class="row"></div>'),
        //        startWakaf = $('<a class="btn btn-warning" href="/workflow_1_21/ApplyScreen">Permohonan Wakaf Baru</a>');

        //    startWakaf.appendTo(div);
        //    div.appendTo($(element));
        //}
    };
};