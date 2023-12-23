$(document).ready(function () {
    $('#txtBirthdayNew').daterangepicker(optionSingleDate);
});


function editUser(uID) {
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Account/GetUserByUserID",
        data: { userID:uID},
        success: function (result) {
            $('#fullNameEdit').val(result.FullName);
            $('#userNameEdit').val(result.UserName);
            $('#slRoleEdit').val(result.RoleID);
            $('#passEdit').val("");
            $('#phoneEdit').val(result.Phone);
            $('#addEdit').val(result.Address);
            $('#emailEdit').val(result.Email);
            $('#txtBirthdayEdit').val(result.BirthdayStr);
     
            if (result.Banned) {
                $('#slBannedEdit').val(1);
            } else {
                $('#slBannedEdit').val(0);
            }
            if (result.Gender) {
                $('#genderEditM').iCheck('check');
            } else {
                $('#genderEditF').iCheck('check');
            }
            $('#txtBirthdayEdit').daterangepicker(optionSingleDate);
            $('#modalUserEdit').modal('show');
        },
        error: function () {
            showNotifyError('Xảy ra lỗi', 'Mời thử lại!');
        }
    });
}

function saveEditUser() {
    var role = $('#slRoleEdit').val();
    var name = $('#fullNameEdit').val();

    if (role == 0 || name.trim() == "" ) {
        showNotifyError("Nhập thiếu dữ liệu", "Mời thử lại", 3000);
    } else {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Account/SaveUserInfo",
            data: $('#formUsersEdit').serialize(),
            beforeSend: function () {
                $('#loading-all').show();
            },
            success: function (response) {
                $('#tblUsers').html(response);

                $('#modalUserEdit').modal('hide');
                showNotifySuccess('Cập nhật thành công', 'Thông tin người dùng đã được lưu lại!');
            },
            error: function () {
                showNotifyError('Cập nhật không thành công', 'Nhập lại thông tin!');
            }, complete: function () {
                $('#loading-all').hide();
            }
        });
    }
}

function showModalRole() {
    $.ajax({
        async: false,
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Account/GetListInitRole",
        success: function (response) {
            $('#div-init-role').html(response);
            $('#modalRoleNew').modal("show");
        },
        error: function () {
            showNotifyError('Xảy ra lỗi', 'Mời thử lại!');
        }
    });
}

function addNewRole() {
    var lstRole = "";

    $('input.chkRole').each(function () {
        var val = this.checked ? this.value : '';
        if (this.checked) {
            lstRole += this.value + "-" + $(this).attr("alt") + ",";
        }
    });
    var titleRole = $('#txtTitleNewRole').val();
    if (titleRole.trim() == "") {
        showNotifyError('Phải nhập tên quyền', 'Mời thử lại!');
    } else {
        $.ajax({
            async: false,
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Account/AddNewRole",
            data: { lstRole: lstRole, titleRole: titleRole },
            beforeSend: function () {
                $('#loading-all').show();
            },
            success: function (response) {
                if (response != "ERROR") {
                    showNotifySuccess('Thêm quyền thành công', 'Đã tạo quyền mới!');
                    $('#tblRoles').html(response);
                    $('#modalRoleNew').modal("hide");
                }
            },
            error: function () {
                showNotifyError('Xảy ra lỗi', 'Mời thử lại!');
            }, complete: function () {
                $('#loading-all').hide();
            }
        });
    }
}

function saveEditRole() {
    var lstRole = "";
    var roleID = $('#txtRoleIdEdit').val();
    $('input.chkRoleEdit').each(function () {
        var val = this.checked ? this.value : '';
        if (this.checked) {
            lstRole += this.value + "-" + $(this).attr("alt") + ",";
        }
    });
   
    var titleRole = $('#txtTitleEditRole').val();
    if (titleRole.trim() == "") {
        showNotifyError('Phải nhập tên quyền', 'Mời thử lại!');
    } else {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Account/SaveEditRole",
            data: { lstRole: lstRole, titleRole: titleRole, roleID: roleID },
            beforeSend: function () {
                $('#loading-all').show();
            },
            success: function (response) {
                if (response != "ERROR") {
                    showNotifySuccess('Chỉnh sửa thành công', 'Đã lưu vào hệ thống!');
                    $('#tblRoles').html(response);
                    $('#modalRoleEdit').modal("hide");
                }
            },
            error: function () {
                showNotifyError('Xảy ra lỗi', 'Mời thử lại!');
            }, complete: function () {
                $('#loading-all').hide();
            }
        });
    }
}

function showModalNewUser() {
    $('#modalUserNew').modal("show");
}

function saveNewUser() {
    var role = $('#slRoleNew').val();
    var name = $('#fullNameNew').val();
    var username = $('#userNameNew').val();
    var pass = $('#passNew').val();
    var rePass = $('#rePassNew').val();
    if (role == 0 || name.trim() == "" || username.trim() == "" || pass.trim() == "" || rePass.trim() == "") {
        showNotifyError("Nhập thiếu dữ liệu", "Mời thử lại", 3000);
    } else {
        if (pass == rePass) {
            $.ajax({
                type: "POST",
                url: location.protocol + '//' + location.host + "/CMS_Account/AddNewUser",
                data: $('#formUsersNew').serialize(),
                beforeSend: function () {
                    $('#loading-all').show();
                },
                success: function (response) {
                    if (response.indexOf("ERROR") == -1) {
                        showNotifySuccess("Tạo người dùng mới thành công", "Đã cập nhập vào hệ thống !");
                        $('#tblUsers').html(response);
                        $('#modalUserNew').modal("hide");
                    } else {
                        showNotifyError(response, "Xin thử lại !");
                    }
                },
                error: function () {
                    showNotifyError('Xảy ra lỗi', 'Mời thử lại!');
                },
                complete: function () {
                    $('#loading-all').hide();
                }
            });
        }
        else {
            showNotifyError("Mật khẩu không khớp", "Mời thử lại", 3000);
        }
    }
}

function editRole(roleID, roleName) {
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Account/GetEditRole",
        data: { roleID: roleID },
        beforeSend: function () {
            $('#loading-all').show();
        },
        success: function (response) {
            $('#txtTitleEditRole').val(roleName);
            $('#txtRoleIdEdit').val(roleID);
            $('#div-init-role-edit').html(response);
            $('#modalRoleEdit').modal("show");
        },
        error: function () {
            showNotifyError('Xảy ra lỗi', 'Mời thử lại!');
        }, complete: function () {
            $('#loading-all').hide();
        }
    });
}
function searchUser(character) {
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Account/SearchUserName",
        data: { character: character },
        beforeSend: function () {
            $('#loading-all').show();
        },
        success: function (response) {
            $('#tblUsers').html(response);
        },
        error: function () {
            showNotifyError('Xảy ra lỗi', 'Mời thử lại!');
        }, complete: function () {
            $('#loading-all').hide();
        }
    });
}