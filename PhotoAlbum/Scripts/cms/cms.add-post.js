var currentImg = "";
var pathImg = "/Images/photo/Articleimages";
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

function addPost() {
    var title = $("#postTitle").val().trim();
    var lstCateId = "";
    $('#lstCate :checked').each(function () {
        lstCateId += $(this).val() + ",";
    });
    if (title == "" || lstCateId == "") {
        showNotifyError("Bạn cần nhập đầy đủ thông tin", "Mời thử lại", 3000);
    } else {
        var datas = CKEDITOR.instances.editor1.getData();
        var datas2 = $("#editor2").val();
        var postmodel = {};
        postmodel.titlePost = title;
        postmodel.contentPost = datas;
        postmodel.catePost = lstCateId;
        postmodel.seoDescPost = $("#seoDesc").val();
        postmodel.seoKeywordPost = $("#seoKeyword").val();
        postmodel.imagePost = currentImg;
        postmodel.description = datas2;

        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Post/AddPost",
            data: JSON.stringify({ models: postmodel }),
            contentType: 'application/json; charset=utf-8',
            dataType: "html",
            cache: false,
            success: function (content) {
                if (content.indexOf("OK") != -1) {
                    window.open(location.protocol + '//' + location.host + '/CMS_Post/AllPost', "_self");
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

function uploadImage() {
    $("#Files").trigger('click');
    $('input[name=Files]').change(function (evt) { singleFileSelected(evt); });

    $("#Files").change(function () {
        UploadFile();
    });
}

function singleFileSelected(evt) {
    //var selectedFile = evt.target.files can use this  or select input file element and access it's files object
    var selectedFile = ($("#Files"))[0].files[0];//FileControl.files[0];
    if (selectedFile) {
        var imageType = /image.*/;
        if (selectedFile.type.match(imageType)) {
            if (selectedFile.size < 2097152) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#Imagecontainer").empty();
                    var dataURL = reader.result;
                    alert(dataURL);
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

function showMedia() {
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Media/GetAllImage",
        beforeSend: function () {
            $('#loading-all').show();
        },
        success: function (content) {
            $('#lstGallery').html(content);
            $('#modalMedia').modal("show");
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
        }, complete: function () {
            $('#loading-all').hide();
        }
    });
}

function chooseIMG(img,id) {
    currentImg = img;
    $('#modalMedia').modal("hide");
    $("#Imagecontainer").empty();
    var divImg = "<img class='thumb' src='" + img + "' />"
    $("#Imagecontainer").append(divImg);
}