
var gulp = require('gulp'),
  markdown = require('gulp-markdown');


var sources = ['./source/web/web.sph/docs/*.md'],
  options = {
    breaks:true, 
    gfm:true,
    highlight: function (code, lang, callback) {
    require('pygmentize-bundled')({ lang: lang, format: 'csharp' }, code, function (err, result) {
      callback(err, result);
    });
  }
};


watchList.push({name:'md',sources: sources, tasks : ['md']});

gulp.task('md', function () {
    return gulp.src(['./source/web/web.sph/docs/*.md'])
        .pipe(markdown(options))
        .pipe(gulp.dest('./source/web/web.sph/docs/'));
});
