
watchList = [];
var gulp = require('gulp');
var requireDir = require('require-dir');
var dir = requireDir('./tasks');



gulp.task('default', function() {
  console.log("Starting all the watchers...");
  var callback = function(event) {
    console.log('File ' + event.path + ' was ' + event.type + ', running tasks...');
  };

  watchList.forEach(function(w){
    console.log("Starting " + w.name);
    gulp.watch(w.sources, w.tasks).on('change', callback);
  });


  console.log("All the watchers started...");
});
