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

function editPost() {
    var title = $("#postTitle").val().trim();
    var lstCateId = "";
    $('#lstCate :checked').each(function () {
        lstCateId += $(this).val() + ",";
    });
    if (title == " " || lstCateId == "") {
        alert("Bạn cần nhập đầy đủ thông tin");
    } else {
        var datas = CKEDITOR.instances.editor1.getData();
        var datas2 = $('#editor2').val();
        var postmodel = {};
        postmodel.postId = $("#editId").val();
        postmodel.titlePost = title;
        postmodel.contentPost = datas;
        postmodel.catePost = lstCateId;
        if ($("#Files").val() == "") {
            postmodel.imagePost = $('#imgFace').val();
        } else {
            postmodel.imagePost = currentImg;
        }
        postmodel.seoDescPost = $("#seoDesc").val();
        postmodel.seoKeywordPost = $("#seoKeyword").val();
        postmodel.description = datas2;
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Post/EditPost",
            data: JSON.stringify({ models: postmodel }),
            contentType: 'application/json; charset=utf-8',
            dataType: "html",
            cache: false,
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


function uploadImage() {
    $("#Files").trigger('click');
    $('input[name=Files]').change(function (evt) { singleFileSelected(evt); });

    $("#Files").change(function () {
        $('#imgEdit').hide();
        UploadFile();
    });
}

function singleFileSelected(evt) {
    //var selectedFile = evt.target.files can use this  or select input file element and access it's files object
    var selectedFile = ($("#Files"))[0].files[0];//FileControl.files[0];
    if (selectedFile) {
        var FileSize = 0;
        var imageType = /image.*/;

        if (selectedFile.type.match(imageType)) {
            if (selectedFile.size < 2097152) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#Imagecontainer").empty();
                    var dataURL = reader.result;
                    var img = new Image()
                    img.src = dataURL;
                    img.className = "thumb";
                    $("#Imagecontainer").append(img);
                };
                reader.readAsDataURL(selectedFile);
            } else {
                alert("Ảnh không được quá 2 MB");
            }
        }
    }
}


function UploadFile() {
    if ($("#Files")[0].files[0].size > 2097152) {
        return;
    }
    var xhr = new XMLHttpRequest();
    var fd = new FormData();

    fd.append("file", $("#Files")[0].files[0]);
    xhr.open("POST", "/CMS_File/UploadImgPost", true);
    xhr.send(fd);
    imgFace = $("#Files")[0].files[0];

    xhr.addEventListener("load", function (event) {
        var f = JSON.parse(xhr.responseText);
        currentImg = f["nameFile"];
    }, false);
}