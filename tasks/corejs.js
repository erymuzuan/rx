var gulp = require('gulp');
var path = require('path');
var sourcemaps = require('gulp-sourcemaps'),
    concat = require('gulp-concat'),
    useref = require('gulp-useref'),
    gulpif = require('gulp-if'),
    uglify = require('gulp-uglify');
// -*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*
// core.js
var sources = [
'./source/web/core.sph/Scripts/string.js',
'./source/web/core.sph/Scripts/_pager.js',
'./source/web/core.sph/Scripts/_ko.workflow.js',
'./source/web/core.sph/Scripts/_ko.kendo.js',
'./source/web/core.sph/Scripts/_ko.bootstrap.js',
'./source/web/core.sph/Scripts/_function.prototypes.js',
'./source/web/core.sph/Scripts/_constants.js',
'./source/web/core.sph/Scripts/_uiready.js',
'./source/web/core.sph/Scripts/_utils.js',
'./source/web/core.sph/Scripts/_task.js',
'./source/web/core.sph/Scripts/_theme.js',
'./source/web/core.sph/Scripts/_references.js'
];

watchList.push({name:'core.js',sources: sources, tasks : ['core.js']});

gulp.task('core.min.js', function(){

    return gulp.src(sources)
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(concat('__core.min.js'))
        .pipe(sourcemaps.write('./'))
        .pipe(useref())
        .pipe(gulp.dest('./source/web/core.sph/Scripts'));
});

gulp.task('core.js',['core.min.js'], function(){
    return gulp.src(sources)
        .pipe(concat('__core.js'))
        .pipe(gulp.dest('./source/web/core.sph/Scripts'));

});
