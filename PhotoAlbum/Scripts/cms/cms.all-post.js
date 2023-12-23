$(document).ready(function () {
    $('#example1').DataTable(normalOptionDataTable);
});


function deletePost(id) {
    var r = confirm("Bạn có chắc chắn muốn xóa bài viết này ? ");
    if (r == true) {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Post/DeletePost",
            data: { postId: id },
            success: function (content) {
                if (content != "Error") {
                    window.open(location.protocol + '//' + location.host + '/CMS_Post/AllPost', "_self");
                }
            },
            error: function () {
                alert("error");
            }
        });
    }
}