var gulp = require('gulp');
var less = require('gulp-less');
var path = require('path');
var sourcemaps = require('gulp-sourcemaps');
var markdown = require('gulp-markdown');

gulp.task('less', function () {
  gulp.src('./source/web/core.sph/Content/theme.matyie/*.less')
  	.pipe(sourcemaps.init())
    .pipe(less({
      paths: [ path.join(__dirname, 'less', 'includes') ]
    }))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest('./source/web/core.sph/Content/theme.matyie/'));
});
gulp.task('md', function () {
    return gulp.src(['./source/web/web.sph/docs/*Adapter.md',
      './source/web/web.sph/docs/adapter.*.md',
      './source/web/web.sph/docs/*Activity.md',
      './source/web/web.sph/docs/ExceptionFilter.md',
      './source/web/web.sph/docs/*-activity-dialog.md'])
        .pipe(markdown())
        .pipe(gulp.dest('./source/web/web.sph/docs/'));
});
gulp.task('default', function() {
  // place code for your default task here
});