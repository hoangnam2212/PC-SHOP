

function updateConfig() {

    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/UpdateConfigGeneral",
        data: $('#frmSetting').serialize(),
        success: function (content) {
            if (content.indexOf("OK") != -1) {
                showNotifySuccess("Cập nhật thông tin thành công", "Đã lưu vào hệ thống", 3000);
            } else {
                showNotifyError("Sai định dạng nhập vào", "Mời thử lại", 3000);
            }
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
        }
    });
} 

function updateConfigAdvance() {
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/UpdateConfigAdvance",
        data: $('#frmSettingAdvance').serialize(),
        success: function (content) {
            if (content.indexOf("OK") != -1) {
                showNotifySuccess("Cập nhật thông tin thành công", "Đã lưu vào hệ thống", 3000);
            } else {
                showNotifyError("Sai định dạng nhập vào", "Mời thử lại", 3000);
            }
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
        }
    });
}