function delImg(img,id) {
    var r = confirm("Bạn có chắc chắn muốn xóa ảnh này ? ");
    if (r == true) {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Media/DelImage",
            data: { path: img },
            success: function (response) {
                $('#box-media-'+id).remove();
                showNotifySuccess('Xóa thành công', 'Đã xóa trên hệ thống!');
            },
            error: function () {
                showNotifyError('Cập nhật không thành công', 'Nhập lại thông tin!');
            }
        });
    }
}

function uploadImgMul() {
    $("#MultiFiles").trigger('click');
}

$("#MultiFiles").change(function () {
    //multiFileSelected();
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

// Upload nhiều ảnh
var DataURLFileReader = {
    read: function (file, callback) {
        var reader = new FileReader();
        var fileInfo = {
            name: file.name,
            type: file.type,
            fileContent: null,
            size: function () {
                var FileSize = 0;
                if (file.size > 1048576) {
                    FileSize = Math.round(file.size * 100 / 1048576) / 100 + " MB";
                }
                else if (file.size > 1024) {
                    FileSize = Math.round(file.size * 100 / 1024) / 100 + " KB";
                }
                else {
                    FileSize = file.size + " bytes";
                }
                return FileSize;
            }
        };
        if (!file.type.match('image.*')) {
            callback("file type not allowed", fileInfo);
            return;
        }
        reader.onload = function () {
            fileInfo.fileContent = reader.result;
            callback(null, fileInfo);
        };
        reader.onerror = function () {
            callback(reader.error, fileInfo);
        };
        reader.readAsDataURL(file);
    }
};

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

            },
            error: function () {

            }
        });
    }
    showNotifySuccess("Thêm ảnh thành công","Đã lưu vào hệ thống",2000);
    setTimeout(function () {  location.reload();},5000);
   
}