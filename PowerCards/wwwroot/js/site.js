// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification


//This code is used for hiding and showing the answer
$(document).ready(function () {
    // Initially, hide all answers
    $('.answer').hide();

    // Toggle answer display on question click
    $('.question').on('click', function () {
        let associatedAnswer = $(this).next('.answer');

        // Slide the answer view up or down
        associatedAnswer.slideToggle();
    });

    // Blur hidden answers for better user experience
    $('.hidden-answer').css('filter', 'blur(5px)');

    // Button to toggle showing and hiding answers
    $('.toggle-answer-btn').on('click', function () {
        let associatedCardBody = $(this).prev('.card-body');

        if (associatedCardBody.css('filter') === 'blur(5px)') {
            // If currently blurred, remove blur
            associatedCardBody.css('filter', 'none');

            // Update button text
            $(this).text('Hide Answer');
        } else {
            // If not blurred, add blur
            associatedCardBody.css('filter', 'blur(5px)');

            // Update button text again
            $(this).text('Show Answer');
        }
    });
});


