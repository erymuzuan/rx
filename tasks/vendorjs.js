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
"./source/web/core.sph/Scripts/moment.js",
"./source/web/core.sph/Scripts/underscore.js",
"./source/web/core.sph/Scripts/jquery-2.1.3.js",
"./source/web/core.sph/Scripts/jquery.validate.js",
"./source/web/core.sph/Scripts/jquery.tablesorter.min.js",
"./source/web/core.sph/Scripts/jquery-ui-1.10.3.custom.min.js",
"./source/web/core.sph/Scripts/knockout-3.4.0.js",
"./source/web/core.sph/Scripts/knockout.mapping-latest.js",
"./source/web/core.sph/Scripts/bootstrap.js",
"./source/web/core.sph/Scripts/typeahead.bundle.js",
"./source/web/core.sph/Scripts/jquery.tablescroll.js",
"./source/web/core.sph/Scripts/daterangepicker.js",
"./source/web/core.sph/Scripts/complete.ly.1.0.1.js",
"./source/web/core.sph/Scripts/toastr.js",
"./source/web/core.sph/Scripts/nprogress.js",
"./source/web/core.sph/Scripts/jstree.min.js",
"./source/web/core.sph/Scripts/jquery.floatThead.min.js",
"./source/web/core.sph/Scripts/jquery.signalR-2.1.2.min"

];

watchList.push({name:"vendor.js",sources: sources, tasks : ["vendor.js"]});

gulp.task("vendor.min.js", function(){

    return gulp.src(sources)
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(concat("__vendor.min.js"))
        .pipe(sourcemaps.write("./"))
        .pipe(useref())
        .pipe(gulp.dest("./source/web/core.sph/Scripts"));
});

gulp.task("vendor.js",["core.min.js"], function(){
    return gulp.src(sources)
        .pipe(concat("__vendor.js"))
        .pipe(gulp.dest("./source/web/core.sph/Scripts"));

});
