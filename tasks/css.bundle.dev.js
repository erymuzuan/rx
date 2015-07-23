///<reference path="../node_modules/gulp/bin/gulp.js"/>
var gulp = require("gulp");
var path = require("path");
var sourcemaps = require("gulp-sourcemaps"),
    concat = require("gulp-concat"),
    useref = require("gulp-useref"),
    gulpif = require("gulp-if"),
    uglify = require("gulp-uglify"),
    _ = require("underscore"),
    mcss = require("gulp-minify-css");
// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
// core.js
var files = [
"/kendo/styles/kendo.common.css",
"/kendo/styles/kendo.metro.css",
"/kendo/styles/kendo.dataviz.css",
"/kendo/styles/kendo.dataviz.metro.css",

"/Content/durandal.css",
"/Content/font-awesome.css",
"/Content/nprogress.css",
"/Content/typeahead.css",
"/Content/toastr.min.css",
"/Content/daterangepicker-bs3.css",

"/Content/bootstrap.min.css",
"/Content/uniform/css/uniform.default.css",
"/Content/simple-line-icons/simple-line-icons.min.css",
"/Content/darkblue.css",
"/Content/layout.css",

"/Content/theme.matyie/style.css",
"/Content/theme.matyie/site.css",
"/Content/theme.matyie/header.css",
"/Content/theme.matyie/nav.css",
"/Content/theme.matyie/user.css",
"/Content/theme.matyie/report.css",
"/Content/theme.matyie/workflow.triggers.css",
"/Content/theme.matyie/dashboard.css"
],
    sources = _(files).map(function (v) {
        return "./source/web/core.sph" + v;
    });

watchList.push({ name: "css.bundle.dev", sources: sources, tasks: ["css.bundle.dev"] });

gulp.task("css.bundle.dev.min", function () {

    return gulp.src(sources)
        .pipe(mcss({ keepBreaks: true }))
        .pipe(concat("__css.bundle.dev.min.css"))
        .pipe(gulp.dest("./source/web/core.sph/Content"));
});

gulp.task("css.bundle.dev", ["css.bundle.dev.min"], function () {
    return gulp.src(sources)
        .pipe(concat("__css.bundle.dev.css"))
        .pipe(gulp.dest("./source/web/core.sph/Content"));

});
