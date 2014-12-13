
var gulp = require('gulp'),
  markdown = require('gulp-markdown'),
  sources = [
    './source/web/web.sph/docs/EmailAction.md',
    './source/web/web.sph/docs/RazorTemplate.md'
];

watchList.push({name:'md.email.action',sources: sources, tasks : ['md.email.action']});
gulp.task('md.email.action', function () {
    return gulp.src(sources)
        .pipe(markdown({breaks:true}))
        .pipe(gulp.dest('./source/web/web.sph/docs/'));
});
