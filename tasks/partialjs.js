var gulp = require('gulp');
var path = require('path');
var sourcemaps = require('gulp-sourcemaps'),
    concat = require('gulp-concat'),
    useref = require('gulp-useref'),
    gulpif = require('gulp-if'),
    uglify = require('gulp-uglify');
// ----------------------------------------------------------------------


// partial js
var sources = ['./source/web/core.sph/SphApp/partial/*.js',
      '!./source/web/core.sph/SphApp/partial/__partial.js',
      '!./source/web/core.sph/SphApp/partial/*.min.js'];


watchList.push({name:'partial.js',sources: sources, tasks : ['partial.js']});

gulp.task('partial.min.js', function(){

    return gulp.src(sources)
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(concat('__partial.min.js'))
        .pipe(sourcemaps.write('./'))
        .pipe(useref())
        .pipe(gulp.dest('./source/web/core.sph/SphApp/partial'));
});

gulp.task('partial.js',['partial.min.js'], function(){
    return gulp.src(sources)
        .pipe(concat('__partial.js'))
        .pipe(gulp.dest('./source/web/core.sph/SphApp/partial'));

});
