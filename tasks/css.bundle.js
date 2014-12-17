var gulp = require('gulp');
var path = require('path');
var sourcemaps = require('gulp-sourcemaps'),
    concat = require('gulp-concat'),
    useref = require('gulp-useref'),
    gulpif = require('gulp-if'),
    uglify = require('gulp-uglify'),
    _ = require("underscore"),
    mcss = require('gulp-minify-css');
// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
// core.js
var files = [
'demo.css',
'sph.wf.16.css',
'sph.wf.32.css',
'sph.wf.64.css',
],
    sources = _(files).map(function(v){
        return './source/web/core.sph/Content/jsplumb/' + v;
    });

watchList.push({name:'jsplumb.css.bundle',sources: sources, tasks : ['jsplumb.css.bundle']});

gulp.task('jsplumb.css.bundle.min', function(){

    return gulp.src(sources)
        .pipe(mcss({keepBreaks:true}))        
        .pipe(concat('jsplumb.min.css'))
        .pipe(gulp.dest('./source/web/core.sph/Content/jsplumb'));
});

gulp.task('jsplumb.css.bundle',['jsplumb.css.bundle.min'], function(){
    return gulp.src(sources)
        .pipe(concat('jsplumb.css'))
        .pipe(gulp.dest('./source/web/core.sph/Content/jsplumb'));

});
