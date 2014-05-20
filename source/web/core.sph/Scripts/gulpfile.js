var gulp = require('gulp');
jsmin = require('gulp-jsmin');
concat = require('gulp-concat');


gulp.task('css', function(){
    return gulp.src(".js")
        .pipe(concat())
});