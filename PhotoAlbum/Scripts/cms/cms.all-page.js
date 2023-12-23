$(document).ready(function () {
    $('#example1').DataTable(normalOptionDataTable);
});


function deletePost(id) {
    var r = confirm("Bạn có chắc chắn muốn xóa trang này ? ");
    if (r == true) {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Page/DeletePage",
            data: { pageID: id },
            success: function (content) {
                if (content != "Error") {
                    //window.open(location.protocol + '//' + location.host + '/CMS_Page/AllPage', "_self");
                    showNotifySuccess("Xóa thành công", "Đã lưu vào hệ thống", 3000);
                    setTimeout(function () {
                        location.reload();
                    }, 2200);

                }
            },
            error: function () {
                alert("error");
            }
        });
    }
}