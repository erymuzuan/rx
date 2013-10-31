bespoke.sph.Theme = function() {
    return {
        "rentalapplication.selectspace-spaceitem": function (element, space) {
            $(element).find("div.space-nav a").addClass("btn");
            if (space.Category() === "Kedai Runcit") {
                
            }
        },
        "public-index":function(element) {
            $(element).find("div.carousel-inner>div.item").first().after('<a class="btn btn-warning" href="/workflow/start/1">Mohon Wakaf</a>');
        }
    };
};