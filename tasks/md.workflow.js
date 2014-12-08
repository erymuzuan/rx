
var gulp = require('gulp');
var less = require('gulp-less');
var path = require('path');
var sourcemaps = require('gulp-sourcemaps');

var concat = require('gulp-concat');

var markdown = require('gulp-markdown')
    useref = require('gulp-useref'),
    gulpif = require('gulp-if'),
    uglify = require('gulp-uglify'),
    minifyCss = require('gulp-minify-css');

gulp.task('md', function () {
    return gulp.src(['./source/web/web.sph/docs/*Adapter.md',
      './source/web/web.sph/docs/adapter.*.md',
      './source/web/web.sph/docs/*Activity.md',
      './source/web/web.sph/docs/ExceptionFilter.md',
      './source/web/web.sph/docs/*-activity-dialog.md'])
        .pipe(markdown())
        .pipe(gulp.dest('./source/web/web.sph/docs/'));
});
