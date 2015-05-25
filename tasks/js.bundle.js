var gulp = require("gulp");
var path = require("path");
var sourcemaps = require("gulp-sourcemaps"),
    concat = require("gulp-concat"),
    useref = require("gulp-useref"),
    gulpif = require("gulp-if"),
    uglify = require("gulp-uglify");
// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
// core.js
var sources = [
"./source/web/core.sph/SphApp/objectbuilders.js",
"./source/web/core.sph/Scripts/__vendor.js",
"./source/web/core.sph/Scripts/__core.js",
"./source/web/core.sph/SphApp/schemas/__domain.js",
"./source/web/core.sph/SphApp/prototypes/prototypes.js",
"./source/web/core.sph/SphApp/partial/__partial.js",
"./source/web/core.sph/kendo/js/kendo.custom.min.js"
];

watchList.push({name:"rx.js",sources: sources, tasks : ["rx.js"]});

gulp.task("rx.min.js", function(){

    return gulp.src(sources)
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(concat("__rx.min.js"))
        .pipe(sourcemaps.write("./"))
        .pipe(useref())
        .pipe(gulp.dest("./source/web/core.sph/Scripts"));
});

gulp.task("rx.js",["rx.min.js"], function(){
    return gulp.src(sources)
        .pipe(concat("__rx.js"))
        .pipe(gulp.dest("./source/web/core.sph/Scripts"));

});