$(function () {
    $('span').click(function () {
        $('#commentsList li:hidden').slice(0, 2).show();
        if ($('#commentsList li').length == $('#commentsList li:visible').length) {
            $('span ').hide();
        }
    });
});


