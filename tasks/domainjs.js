
///<reference path="../node_modules/gulp/bin/gulp.js"/>
///<reference path="../node_modules/require-dir/index.js"/>
var gulp = require("gulp");
var path = require("path");
var sourcemaps = require("gulp-sourcemaps"),
    concat = require("gulp-concat"),
    useref = require("gulp-useref"),
    gulpif = require("gulp-if"),
    uglify = require("gulp-uglify");
// ---------------------------------------------------------------------------
// domain.js
var sources =["./source/web/core.sph/SphApp/schemas/form.designer.g.js",
      "./source/web/core.sph/SphApp/schemas/report.builder.g.js",
      "./source/web/core.sph/SphApp/schemas/sph.domain.g.js",
      "./source/web/core.sph/SphApp/schemas/trigger.workflow.g.js"];


watchList.push({name:"domain.js",sources: sources, tasks : ["domain.js"]});

gulp.task("domain.min.js", function(){

    return gulp.src(sources)
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(concat("__domain.min.js"))
        .pipe(sourcemaps.write("./"))
        .pipe(useref())
        .pipe(gulp.dest("./source/web/core.sph/SphApp/schemas"));
});

gulp.task("domain.js",["domain.min.js"], function(){

    return gulp.src(sources)
        .pipe(concat("__domain.js"))
        .pipe(gulp.dest("./source/web/core.sph/SphApp/schemas"));


});