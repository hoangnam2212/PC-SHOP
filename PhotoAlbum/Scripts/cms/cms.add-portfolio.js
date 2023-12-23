var currentImg = "";
var ImgGallery = "";
var pathImg = "/Images/photo/Articleimages";
var typeMedia = 1;

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

function addPortfolio() {
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
        var portfoliomodel = {};
        portfoliomodel.Title = title;
        portfoliomodel.ContentPortfolio = datas;
        portfoliomodel.CatePortfolio = lstCateId;
        portfoliomodel.SeoDesc = $("#seoDesc").val();
        portfoliomodel.SeoKeyword = $("#seoKeyword").val();
        portfoliomodel.Image = currentImg;
        portfoliomodel.Description = datas2;
        portfoliomodel.ImageGallery = ImgGallery;

        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Portfolio/AddNewPortfolio",
            data: JSON.stringify({ models: portfoliomodel }),
            contentType: 'application/json; charset=utf-8',
            dataType: "html",
            cache: false,
            success: function (content) {
                if (content.indexOf("OK") != -1) {
                    window.open(location.protocol + '//' + location.host + '/CMS_Portfolio/AllPortfolio', "_self");
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

function showMedia(type) {
    typeMedia = type;
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
    if (typeMedia == 1) {
        currentImg = img;
        $('#modalMedia').modal("hide");
        $("#Imagecontainer").empty();
        var divImg = "<img class='thumb' src='" + img + "' />";
        $("#Imagecontainer").append(divImg);
    } else {
        ImgGallery += img + ";";
        $('#modalMedia').modal("hide");
        var divImg = "<li id='li-" + id + "' ><img src='" + img + "' class='thumb-detail'/><a class='btn btn-danger btn-xs' onclick='delImgGallery(" + id+")'>X</a><input type='hidden' id='img-hid-"+id+"' value='"+img+"' /></li>";
        $("#lst-show-gallery").append(divImg);
    }
}

function delImgGallery(id) {
    var img = $('#img-hid-' + id).val();
    
    $('#li-' + id).remove();
    ImgGallery = ImgGallery.replace(img + ";", "");

}

function uploadImageMultiple() {
    $("#MultiFiles").trigger('click');
}

$("#MultiFiles").change(function () {
    UploadMultipleFiles();
});

function multiFileSelected() {
    $('#lstGallery').empty();
    //var selectedFile = evt.target.files can use this  or select input file element and access it's files object
    var selectedFile = $("#MultiFiles")[0].files;//FileControl.files[0];
    var imageType = /image.*/;
    for (var i = 0; i < selectedFile.length; i++) {
        if (!selectedFile[i].type.match(imageType)) {
            continue;
        }
        DataURLFileReader.read(selectedFile[i], function (err, fileInfo) {
            var image = '<li><img width="60" src="' + fileInfo.fileContent + '"/></li>';
            $('#lstGallery').append(image);
        });
    }
}

function UploadMultipleFiles() {
    var selectedFiles = $("#MultiFiles")[0].files;//FileControl.files[0];

    //var files = document.getElementById("UploadedFiles").files;
    for (var i = 0; i < selectedFiles.length; i++) {
        if (!selectedFiles[i].type.match('image.*')) {
            continue;
        }
        if (selectedFiles[i].size > 2097152) {
            continue;
        }

        var formData = new FormData();

        formData.append("file", $("#MultiFiles")[0].files[i]);
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_File/UploadImgWeb",
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,
            success: function (content) {
                ImgGallery += content.nameFile + ";";
                var id = Math.floor((Math.random() * 2000) + 100)
                var divImg = "<li id='li-" + id + "' ><img src='" + content.nameFile + "' class='thumb-detail'/><a class='btn btn-danger btn-xs' onclick='delImgGallery(" + id + ")'>X</a><input type='hidden' id='img-hid-" + id + "' value='" + content.nameFile + "' /></li>";
                $("#lst-show-gallery").append(divImg);
            },
            error: function () {
            }
        });
    }
}