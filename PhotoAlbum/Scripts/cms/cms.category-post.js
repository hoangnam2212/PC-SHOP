
$(document).ready(function () {
	$(".select2").select2();
});


function addCate() {
	if ($('#cateName').val().trim() == "") {
		showNotifyError("Nhập thiếu dữ liệu", "Phải nhập tên loại sản phẩm", 2500);
	} else {
		var name = $('#cateName').val().trim();
		var pId = $('#cateParent option:selected').val();

		$.ajax({
			type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Post/AddCategory",
			data: { cateName: name, parentId: pId },
			dataType: "html",
			success: function (content) {
				$('#div-content').html(content);
				$.ajax({
					type: "POST",
                    url: location.protocol + '//' + location.host + "/CMS_Post/GetParentCategory",
					dataType: "html",
					success: function (content) {
						$('#div-select').html(content);
						$(".select2").select2();
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


function editCate(id, name, pID) {
	$("#idCateEdit").val(id);
	$("#actionTitle").html("Sửa danh mục");
	$('#cateName').val(name);
	$('#cateParent').val(pID).change();
	$("#but-add").hide(200);
	$("#but-save").show(200);
	$("#but-cancel").show(200);
}

function cancelCate() {
	$("#but-add").show(200);
	$("#but-save").hide(200);
	$("#but-cancel").hide(200);
	$('#cateName').val("");
	$("#actionTitle").html("Danh mục bài viết");
}

function saveEdit() {
	var name = $('#cateName').val().trim();
	var id = $('#cateParent option:selected').val();

	$.ajax({
		type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Post/EditCateById",
		data: { cId: $("#idCateEdit").val(), cateName: name, pId: id },
		dataType: "html",
		success: function (content) {
			if (content != "Error") {
				$('#div-content').html(content);
				$.ajax({
					type: "POST",
                    url: location.protocol + '//' + location.host + "/CMS_Post/GetParentCategory",
					dataType: "html",
					success: function (content) {
						$('#div-select').html(content);
						$(".select2").select2();
					}
				});
				showNotifySuccess("Sửa thành công", "Đã lưu vào cơ sở dữ liệu");
			}
			cancelCate();
		},
	});
}

function deleteCate(id) {
	var r = confirm("Bạn có chắc chắn muốn xóa danh mục này ? ");
	if (r == true) {
		$.ajax({
			type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Post/DeleteCategory",
			data: { cateId: id },
			success: function (content) {
				$('#div-content').html(content);
				$.ajax({
					type: "POST",
                    url: location.protocol + '//' + location.host + "/CMS_Post/GetParentCategory",
					dataType: "html",
					success: function (content) {
						$('#div-select').html(content);
						$(".select2").select2();
					}
				});
				$('#cateName').val("");
				showNotifySuccess("Xóa thành công", "Đã lưu vào cơ sở dữ liệu");
			},
			error: function () {
				alert("error");
			}
		});
	}
}