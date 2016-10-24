
var gulp = require('gulp'),
  markdown = require('gulp-markdown');


var sources = ['./source/web/web.sph/docs/*.md'],
  options = {
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



var sources2 = ['./deployment/*.md'],
  options2 = {
    gfm:true,
    highlight: function (code, lang, callback) {
    require('pygmentize-bundled')({ lang: lang, format: 'csharp', python :"D:\project\tools\Python\Python35-32\python.exe" }, code, function (err, result) {
      callback(err, result);
    });
  }
};


watchList.push({name:'release-note-md',sources: sources2, tasks : ['release-note-md']});

gulp.task('release-note-md', function () {
    return gulp.src(['./deployment/*.md'])
        .pipe(markdown(options))
        .pipe(gulp.dest('./deployment/'));
});
