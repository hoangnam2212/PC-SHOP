var currentImg = "";

$(document).ready(function () {
    $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%' // optional
    });
    CKEDITOR.replace('editor1', {
        filebrowserBrowseUrl: location.protocol + '//' + location.host + '/Scripts/plugins/ckfinder/ckfinder.html',
        filebrowserImageBrowseUrl: location.protocol + '//' + location.host + '/Scripts/plugins/ckfinder/ckfinder.html?type=Images',
        filebrowserFlashBrowseUrl: location.protocol + '//' + location.host + '/Scripts/plugins/ckfinder/ckfinder.html?type=Flash',
        height: '500px',
    });

});

function editPage() {
    var title = $("#postTitle").val().trim();
   
    if (title == " ") {
        showNotifyError("Bạn cần nhập đầy đủ thông tin","Mời thử lại",3000);
    } else {
        var datas = CKEDITOR.instances.editor1.getData();
        var postmodel = {};
        postmodel.postId = $("#editId").val();
        postmodel.titlePost = title;
        postmodel.contentPost = datas;
        
        postmodel.seoDescPost = $("#seoDesc").val();
        postmodel.seoKeywordPost = $("#seoKeyword").val();

        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Page/EditPage",
            data: JSON.stringify({ models: postmodel }),
            contentType: 'application/json; charset=utf-8',
            dataType: "html",
            success: function (content) {
                if (content.indexOf("OK") != -1) {
                    showNotifySuccess("Chỉnh sửa thành công","Đã lưu vào hệ thống",3000);
                } else {
                    showNotifyError("Xảy ra lỗi khi chỉnh sửa", "Thử lại sau ít phút");
                }
            },
            error: function () {
                showNotifyError("Xảy ra lỗi khi chỉnh sửa", "Thử lại sau ít phút");
            }
        });
    }
}

