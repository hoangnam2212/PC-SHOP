var currentImg = "";
$(document).ready(function () {
    $(".select2").select2();
});
function addCate() {
    //$('#cateName').val()
    if ($('#cateName').val().trim() == "") {
        showNotifyError("Nhập thiếu dữ liệu", "Phải nhập tên loại sản phẩm", 2500);
    } else {
        var name = $('#cateName').val().trim();
        var pId = $('#cateParent option:selected').val();

        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Product/AddCategory",
            data: { cateName: name, parentId: pId, currentImg: currentImg },
            dataType: "html",
            success: function (content) {
                $('#div-content').html(content);
                $.ajax({
                    type: "POST",
                    url: location.protocol + '//' + location.host + "/CMS_Product/GetParentCategory",
                    dataType: "html",
                    success: function (content) {
                        $('#div-select').html(content);
                        $(".select2").select2();
                        currentImg = "";
                        $('#cateName').val("");
                        $('#Imagecontainer1').html("");
                    }
                });
                $('#cateName').val("");
                showNotifySuccess("Thêm thành công", "Đã lưu vào cơ sở dữ liệu");
            },
            error: function () {
                alert("Loi");
            }
        });
    }
}

function editCate(id, name, pID, img) {
    $("#idCateEdit").val(id);
    $("#actionTitle").html("Sửa danh mục");
    $('#cateName').val(name);
    $('#cateParent').val(pID).change();
    $("#but-add").hide(200);
    $("#but-save").show(200);
    $("#but-cancel").show(200);
    currentImg = img;
    $('#Imagecontainer1').html("<img alt='Không có ảnh trang chủ' src='" + img + "' class='thumb' />");

}


function cancelCate() {
    $("#but-add").show(200);
    $("#but-save").hide(200);
    $("#but-cancel").hide(200);
    $('#cateName').val("");
    $("#actionTitle").html("Thêm danh mục mới");
    $('#Imagecontainer1').html('');
    currentImg = "";
}

function saveEdit() {
    var name = $('#cateName').val().trim();
    var id = $('#cateParent option:selected').val();

    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Product/EditCateById",
        data: { cId: $("#idCateEdit").val(), cateName: name, pId: id, currentImg: currentImg },
        dataType: "html",
        success: function (content) {
            if (content != "Error") {
                $('#div-content').html(content);
                $.ajax({
                    type: "POST",
                    url: location.protocol + '//' + location.host + "/CMS_Product/GetParentCategory",
                    dataType: "html",
                    success: function (content) {
                        $('#div-select').html(content);
                        $('#Imagecontainer1').html('');
                        $(".select2").select2();
                        $('#cateName').val("");
                        currentImg = "";
                    }
                });
                showNotifySuccess("Sửa thành công", "Đã lưu vào cơ sở dữ liệu");
            }
            cancelCate();
        },
    });
}



function uploadImage(id) {
    $("#Files" + id).trigger('click');
}


$("#Files1").change(function (evt) {
    singleFileSelected(evt, 1)
    UploadFile(1);
});


function singleFileSelected(evt, id) {
    //var selectedFile = evt.target.files can use this  or select input file element and access it's files object
    var selectedFile = ($("#Files" + id))[0].files[0];//FileControl.files[0];
    if (selectedFile) {
        var imageType = /image.*/;
        if (selectedFile.type.match(imageType)) {
            if (selectedFile.size < 2097152) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#Imagecontainer" + id).empty();
                    var dataURL = reader.result;
                    var img = new Image()
                    img.src = dataURL;
                    img.className = "thumb";
                    $("#Imagecontainer" + id).append(img);
                };
                reader.readAsDataURL(selectedFile);
            } else {
                alert("Ảnh không được quá 2 MB");
            }
        }
    }
}

function UploadFile(id) {
    if ($("#Files" + id)[0].files[0].size > 2097152) {
        return;
    }
    var xhr = new XMLHttpRequest();
    var fd = new FormData();

    fd.append("file", $("#Files" + id)[0].files[0]);
    xhr.open("POST", "/CMS_File/UploadProducts", true);
    xhr.send(fd);
    imgFace = $("#Files" + id)[0].files[0];

    xhr.addEventListener("load", function (event) {
        var f = JSON.parse(xhr.responseText);
        currentImg = f["nameFile"];

    }, false);
}


function deleteCate(id) {
    var r = confirm("Bạn có chắc chắn muốn xóa danh mục này ? ");
    if (r == true) {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Product/DeleteCategory",
            data: { cateId: id },
            success: function (content) {
                $('#div-content').html(content);
                $.ajax({
                    type: "POST",
                    url: location.protocol + '//' + location.host + "/CMS_Product/GetParentCategory",
                    dataType: "html",
                    success: function (content) {
                        $('#div-select').html(content);
                        $(".select2").select2();
                    }
                });
                $('#cateName').val("");
                cancelCate();
                showNotifySuccess("Xóa thành công", "Đã lưu vào cơ sở dữ liệu");
            },
            error: function () {
                alert("error");
            }
        });
    }
}