var gulp = require('gulp');
var bower = require('main-bower-files'),
    less = require('gulp-less'),
    minifyCSS = require('gulp-minify-css'),
    project = require('./project.json');

var config = {
    lessPath: './Client/Styles',
    bowerDir: './bower_components'
}

gulp.task('default', ['bower', 'icons', 'styles']);

gulp.task('bower', function() {
    return gulp.src(bower(), { base: config.bowerDir })
               .pipe(gulp.dest(project.webroot + '/lib'));
});

gulp.task('styles', function () {
    return gulp.src(config.lessPath + '/styles.less')
               .pipe(less({
                    paths: [
                        config.lessPath,
                        config.bowerDir + '/bootstrap/less',
                        config.bowerDir + '/fontawesome/less'
                        ]}))
               .pipe(minifyCSS())
               .pipe(gulp.dest(project.webroot + '/css'));
});

gulp.task('watch', function() {
    return gulp.watch('./Client/Styles/**/*.less', ['styles']);
});

gulp.task('icons', function() {
    return gulp.src(config.bowerDir + '/fontawesome/fonts/**.*')
        .pipe(gulp.dest(project.webroot + '/fonts'));
});