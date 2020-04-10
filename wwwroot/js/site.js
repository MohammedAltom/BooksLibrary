$('#Sfield').on('input', function () {
    var val = $(this).val();
    if (val == "" || val == null) {
        $('tr td.Suser').each(function () {
            $(this).parent().show();
        });
    } else {
        $('tr td.Suser').each(function () {
            $(this).parent().hide();
        });
    }
    $('tr td.Suser').each(function () {
        var user = $(this).text();
        if (user.indexOf(val) >= 0) {
            $(this).parent().show();
        }

    });

})