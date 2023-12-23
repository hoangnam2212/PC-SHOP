
$(function () {
    var group = $("ol.sort-banner").sortable({
        onDrop: function ($item, container, _super) {
            var order = "";
            $('#lst-sort li').each(function () {
                order += $(this).val().toString() + ",";
            });
            $.ajax({
                type: "POST",
                url: location.protocol + '//' + location.host + "/CMS_Setting/SortSlider",
                data: { order: order },
                success: function (content) {
                    location.reload();
                },
                error: function () {
                    showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
                }
            });

            _super($item, container);
        }
    });
});

function addSlider(position) {
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/AddSliderHTML",
        success: function (content) {
            $('#lst-sort').append(content);
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
        }
    });
}

function deleteSlider(bID) {
    var r = confirm("Bạn có chắc chắn muốn xóa slider này ? ");
    if (r == true) {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Setting/DeleteSlider",
            data: { bID: bID },
            success: function (content) {
                $('#slider-' + bID).remove();
            },
            error: function () {
                showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
            }
        });
    }

}

function uploadImage(id) {
    $("#Files-" + id).trigger('click');
    $('input[name=Files-' + id + ']').change(function (evt) { singleFileSelected(evt, id); });

    $("#Files-" + id).change(function () {
        UploadFile(id);
    });
}

function singleFileSelected(evt, id) {
    var selectedFile = ($("#Files-" + id))[0].files[0];//FileControl.files[0];
    if (selectedFile) {
        var imageType = /image.*/;
        if (selectedFile.type.match(imageType)) {
            if (selectedFile.size < 2097152) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#Imagecontainer-" + id).empty();
                    var dataURL = reader.result;
                    var img = new Image()
                    img.src = dataURL;
                    img.className = "img-thumb-banner";
                    $("#Imagecontainer-" + id).append(img);
                };
                reader.readAsDataURL(selectedFile);
            } else {
                alert("Ảnh không được quá 2 MB");
            }
        }
    }
}

function UploadFile(id) {
    if ($("#Files-" + id)[0].files[0].size > 2097152) {
        return;
    }
    var xhr = new XMLHttpRequest();
    var fd = new FormData();

    fd.append("file", $("#Files-" + id)[0].files[0]);
    xhr.open("POST", "/CMS_File/UploadImgWeb", true);
    xhr.send(fd);
    imgFace = $("#Files-" + id)[0].files[0];

    xhr.addEventListener("load", function (event) {
        var f = JSON.parse(xhr.responseText);
        currentImg = f["nameFile"];
        $('#link-' + id).val(currentImg);
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Setting/AddSliderImg",
            data: { currentImg: currentImg, bID: id },
            success: function (content) {
            },
            error: function () {
                showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
            }
        });
    }, false);
}

function updateTitle(bid) {
    var title = $('#title-' + bid).val();
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Setting/UpdateTitleSlider",
        data: { title: title, bID: bid },
        success: function (content) {
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại", 3000);
        }
    });
}
