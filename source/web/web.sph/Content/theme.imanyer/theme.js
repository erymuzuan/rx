bespoke.sph.Theme = function() {
    return {
        "rentalapplication.selectspace-spaceitem": function (element, space) {
            $(element).find("div.space-nav a").addClass("btn");
            if (space.Category() === "Kedai Runcit") {
                
            }
            
        }
    };
};