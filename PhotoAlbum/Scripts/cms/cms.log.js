$(document).ready(function () {
    init_daterangepicker_byId("txtdatefilter");
    $('#slLookup').select2();
});

function filterByAcc() {
    var search = $('#slLookup').val();
    if (search == 0) {
        showNotifyError("Chọn người dùng", "Mời thử lại", 3000);
        return;
    }
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/FilterLogByAcc",
        data: { search: search },
        beforeSend: function () {
            $('#loading-all').show();
        },
        success: function (response) {
            $('#tblLogs').html(response);
        },
        error: function () {
        }
        , complete: function () {
            $('#loading-all').hide();
        }
    });
}

function filterByDate() {
    var date = $('#txtdatefilter span').html();
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/FilterLogByDate",
        data: { date: date },
        beforeSend: function () {
            $('#loading-all').show();
        },
        success: function (response) {
            $('#tblLogs').html(response);
        },
        error: function () {
        }
        , complete: function () {
            $('#loading-all').hide();
        }
    });
}