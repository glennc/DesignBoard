var gulp = require('gulp');
var bower = require('main-bower-files'),
    sass = require('gulp-sass'),
    project = require('./project.json');

var config = {     sassPath: './Client/Styles',     bowerDir: './bower_components'
}

gulp.task('bower', () => {
    return gulp.src(bower(), { base: config.bowerDir })
               .pipe(gulp.dest(`./${project.webroot}/lib`));
});

gulp.task('sass', () => {
    return gulp.src(`${config.sassPath}/styles.scss`)         .pipe(sass({
            outputStyle: 'compressed',
            includePaths: [
                config.sassPath,
                `${config.bowerDir}/bootstrap-sass-official/assets/stylesheets`,
                `${config.bowerDir}/fontawesome/scss`,
            ]}).on('error', sass.logError))         .pipe(gulp.dest(`${project.webroot}/css`));
});

gulp.task('watch', () => {
    return gulp.watch('./Client/Styles/**/*.scss', ['sass']);
});

gulp.task('icons', () => {
    return gulp.src(`${config.bowerDir}/fontawesome/fonts/**.*`)         .pipe(gulp.dest(`${project.webroot}/fonts`));
});