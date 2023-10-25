// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

//This code is used for hiding and showing the answer
$(document).ready(function () {
    $('.answer').hide();
    $('.question').click(function () {
        $(this).next('.answer').slideToggle();
    });
    $('.hidden-answer').css('filter', 'blur(5px)');

    $('.toggle-answer-btn').click(function () {
        let associatedCardBody = $(this).prev('.card-body');

        if (associatedCardBody.css('filter') === 'blur(5px)') {
            associatedCardBody.css('filter', 'none');
            $(this).text('Hide Answer');
        } else {
            associatedCardBody.css('filter', 'blur(5px)');
            $(this).text('Show Answer');
        }
    });
});

