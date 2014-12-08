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
  console.log("Starting the watchers");
});



// ---------------------------------------------------------------------------
// domain.js
var domainSources =['./source/web/core.sph/SphApp/schemas/form.designer.g.js',
      './source/web/core.sph/SphApp/schemas/report.builder.g.js',
      './source/web/core.sph/SphApp/schemas/sph.domain.g.js',
      './source/web/core.sph/SphApp/schemas/trigger.workflow.g.js'];

gulp.task('domain.min.js', function(){

    return gulp.src(domainSources)
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(concat('__domain.min.js'))
        .pipe(sourcemaps.write('./'))
        .pipe(useref())
        .pipe(gulp.dest('./source/web/core.sph/SphApp/schemas'));
});

gulp.task('domain.js',['domain.min.js'], function(){

    return gulp.src(domainSources)
        .pipe(concat('__domain.js'))
        .pipe(gulp.dest('./source/web/core.sph/SphApp/schemas'));


});
var domainWatcher = gulp.watch(domainSources, ['domain.js']);
domainWatcher.on('change', function(event) {
  console.log('File ' + event.path + ' was ' + event.type + ', running tasks...');
});


// ----------------------------------------------------------------------


// partial js
var partialSources = ['./source/web/core.sph/SphApp/partial/*.js',
      '!./source/web/core.sph/SphApp/partial/__partial.js',
      '!./source/web/core.sph/SphApp/partial/*.min.js'];

gulp.task('partial.min.js', function(){

    return gulp.src(partialSources)
        .pipe(sourcemaps.init())
        .pipe(uglify())
        .pipe(concat('__partial.min.js'))
        .pipe(sourcemaps.write('./'))
        .pipe(useref())
        .pipe(gulp.dest('./source/web/core.sph/SphApp/partial'));
});

gulp.task('partial.js',['partial.min.js'], function(){
    return gulp.src(partialSources)
        .pipe(concat('__partial.js'))
        .pipe(gulp.dest('./source/web/core.sph/SphApp/partial'));

});

var partialWatcher = gulp.watch(partialSources, ['partial.js']);
partialWatcher.on('change', function(event) {
  console.log('File ' + event.path + ' was ' + event.type + ', running tasks...');
});