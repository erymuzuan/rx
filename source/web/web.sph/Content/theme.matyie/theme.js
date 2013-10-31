bespoke.sph.Theme = function() {
    return {
        "rentalapplication.selectspace-spaceitem": function (element, space) {
            $(element).find("div.space-nav a").addClass("btn");
            if (space.Category() === "Kedai Runcit") {
                
            }
        },
        "public-index":function(element) {
            $(element).find("div.col-lg-6>div.home-complaint-tile").first().after('<a class="btn btn-warning" href="/workflow/start/2">Permohonan</a>');
        }
    };
};