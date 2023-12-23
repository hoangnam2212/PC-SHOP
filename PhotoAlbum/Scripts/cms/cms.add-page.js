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

function addPage() {
    var title = $("#postTitle").val().trim();
    
    if (title == "") {
        showNotifyError("Bạn cần nhập đầy đủ thông tin", "Mời thử lại", 3000);
    } else {
        var datas = CKEDITOR.instances.editor1.getData();
        var postmodel = {};
        postmodel.titlePost = title;
        postmodel.contentPost = datas;
        postmodel.seoDescPost = $("#seoDesc").val();
        postmodel.seoKeywordPost = $("#seoKeyword").val();


        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Page/AddPage",
            data: JSON.stringify({ models: postmodel }),
            contentType: 'application/json; charset=utf-8',
            dataType: "html",
            cache: false,
            success: function (content) {
                if (content.indexOf("OK") != -1) {
                    window.open(location.protocol + '//' + location.host + '/CMS_Page/AllPage', "_self");
                } else {
                    showNotifyError("Sai định dạng nhập vào", "Mời thử lại", 3000);
                }
            },
            error: function () {
                showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
            }
        });
    }
}
