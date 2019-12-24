var cName, cEmail, cPhone, cComment, cdate; post_cnt = 0;

$(document).on('click', '.apply-now-btn', function () {
    $('#driverAppModal').modal('show');
});

$('.custom-close').on('click', function () {
    $('.form_error_box').addClass('hide');
})

$('#btn-close-sending-modal').on('click', function () {
    $("input[type=text]").val('');
    $("input[type=email]").val('');
    $("input[type=tel]").val('');
    $('#contact-message').val('');
    $('#messageSending_Modal').modal('hide');
    $('#driverAppModal').modal('hide');
})

