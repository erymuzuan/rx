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


gulp.task('less', function () {
  gulp.src('./source/web/core.sph/Content/theme.matyie/*.less')
  	.pipe(sourcemaps.init())
    .pipe(less({
      paths: [ path.join(__dirname, 'less', 'includes') ]
    }))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('./source/web/core.sph/Content/theme.matyie/'));
});