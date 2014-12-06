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



// domain.js
gulp.task('domain.js', function(){

    return gulp.src(['./source/web/core.sph/SphApp/schemas/form.designer.g.js',
      './source/web/core.sph/SphApp/schemas/report.builder.g.js',
      './source/web/core.sph/SphApp/schemas/sph.domain.g.js',
      './source/web/core.sph/SphApp/schemas/trigger.workflow.g.js'])
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(concat('__domain.js'))
        .pipe(sourcemaps.write())
        .pipe(useref())
        .pipe(gulp.dest('./source/web/core.sph/SphApp/schemas'));


});