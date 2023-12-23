
$(function () {
    $('.dd').nestable({
        maxDepth: 2,
    });
    
});


function addNewMenu(position) {
    var menuType = $('#slMenuType').val();
    if (menuType == 0) {
        showNotifyError("Chọn vị trí menu", "Mời thử lại !", 3000);
        return;
    }

    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/AddMenuHTML",
        data: { menuType: menuType },
        success: function (content) {
            $('#lst-sort').append(content);
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
        }
    });
}


function deleteMenu(mID) {
    var r = confirm("Bạn có chắc chắn muốn xóa menu này ? ");
    if (r == true) {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Setting/DeleteMenu",
            data: { mID: mID },
            success: function (content) {
                $('#menu-' + mID).remove();
                showNotifySuccess("Xóa thành công", "Đã lưu vào hệ thống", 3000);
            },
            error: function () {
                showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
            }
        });
    }

}


function saveMenu() {
    var order = window.JSON.stringify($('.dd').nestable('serialize'));
  
    $('#lstMenuID').val(order);
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/SaveMenu",
        data: $('#frmMenu').serialize(),
        success: function (content) {
            showNotifySuccess("Cập nhật menu thành công", "Đã lưu vào hệ thống", 3000);
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
        }
    });
}

$('#slMenuType').change(function () {
    var mID = $(this).val();
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/ChangeMenu",
        data: {mID:mID},
        success: function (content) {
            $('#frmMenu').html(content);
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
        }
    });
});