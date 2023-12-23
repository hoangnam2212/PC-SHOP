
function pressEnter(e) {
    if (e.keyCode == 13) {
        loginProcess();
    }
}

// Xử lý đăng nhập
function loginProcess() {
    $('#messLogin').show();
    $('#loadding').show();
    var username = $('#txtUserName').val();
    var pass = $('#txtPass').val();
    if (username.trim() == "" || pass.trim() == "") {
        showNotifyError("Phải nhập đầy đủ tên đăng nhập hoặc mật khẩu", "Mời thử lại !");
        $('#messLogin').hide();
        $('#loadding').hide();
    } else {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Login/LoginProcess",
            data: { user: username,pass:pass },
            success: function (response) {
                if (response == "OK") {
                    window.open(location.protocol + '//' + location.host + '/CMS_Home/Dashboard', "_self");
                } else {
                    showNotifyError(response, "Mời thử lại");
                    $('#messLogin').hide();
                    $('#loadding').hide();
                }
            },
            error: function () {
                $('#messLogin').hide();
                $('#loadding').hide();
            }
        });
    }
}