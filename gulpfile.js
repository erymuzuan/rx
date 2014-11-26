var gulp = require('gulp');
var less = require('gulp-less');
var path = require('path');
var sourcemaps = require('gulp-sourcemaps');

gulp.task('less', function () {
  gulp.src('./source/web/core.sph/Content/theme.matyie/*.less')
  	.pipe(sourcemaps.init())
    .pipe(less({
      paths: [ path.join(__dirname, 'less', 'includes') ]
    }))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('./source/web/core.sph/Content/theme.matyie/'));
});

gulp.task('default', function() {
  // place code for your default task here
});