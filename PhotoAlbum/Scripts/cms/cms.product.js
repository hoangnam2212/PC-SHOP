var currentImg = "";
var currentImg2 = "";
var currentImgEdit = "";
var currentImgEdit2 = "";
var currentGallery = "";
var currentGalleryEdit = "";
$(document).ready(function () {


});

$('#slluCate').change(function () {
    var cate = $(this).val();
    var onSale = $("#slluOnSale").val();
    var FilterProduct = {};
    FilterProduct.Cate = cate;
    FilterProduct.OnSale = onSale;
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + '/CMS_Product/FilterProducts',
        data: JSON.stringify({ FilterProduct: FilterProduct }),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            $('#tblPro').html(response);
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại !", 3000);
        }
    });
});

$('#slRate').change(function () {
    var rate = $(this).val();
    document.documentElement.style.setProperty('--per', rate);
});


$('#slluOnSale').change(function () {
    var onSale = $(this).val();
    var cate = $("#slluCate").val();
    var FilterProduct = {};
    FilterProduct.Cate = cate;
    FilterProduct.OnSale = onSale;
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + '/CMS_Product/FilterProducts',
        data: JSON.stringify({ FilterProduct: FilterProduct }),
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            $('#tblPro').html(response);
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại !", 3000);
        }
    });
});

function showModelNew() {
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Product/_ModalNewProduct",
        beforeSend: function () {
            $('#loading-all').show();
        },
        success: function (response) {
            $('#modalProductNew').html(response);
            $('#modalProductNew').modal("show");
        },
        error: function () {
            showNotifyError('Xảy ra lỗi', 'Mời thử lại!', 3000);
        }, complete: function () {
            $('#loading-all').hide();
        }
    });
    
}


function saveNewCate() {
    var cate = $('#txtCatePro').val();
    if (cate.trim() == "") {
        showNotifyError("Nhập loại hàng", "Mời thử lại");
    } else {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Product/SaveNewCate",
            data: { cate: cate },
            beforeSend: function () {
                $('#loading-all').show();
            },
            success: function (response) {
                $('#txtCatePro').val("");
                $('#add-new-cate').hide(200);
                $('#sl-box-cate').html(response);
            },
            error: function () {
                showNotifyError('Xảy ra lỗi', 'Mời thử lại!', 3000);
            }, complete: function () {
                $('#loading-all').hide();
            }
        });
    }
}

function saveProduct() {

    var ProductCode = $('#txtProductCode').val();
    var ProductName = $('#txtProductName').val();
    var ProductCate = $('#sleCate').val();

    if (ProductCode == "" || ProductName == "" || ProductCate == null) {
        showNotifyError("Nhập thiếu dữ liệu", "Những mục (*) bắt buộc nhập", 3000);
    } else {
        var datas = CKEDITOR.instances.editor1.getData();
        var promodel = {};
        promodel.ProductCode = ProductCode;
        promodel.ProductName = ProductName;
        promodel.HidImg = $("#txtHidImg").val();
        promodel.HidImg2 = $("#txtHidImg2").val();
        promodel.Gallery = $("#txtHidImgGallery").val();
        promodel.Category = $('#sleCate').val().toString();
        promodel.ProductDescription = datas;
        promodel.ProductSummary = CKEDITOR.instances.editor2.getData();
        promodel.PriceRetailEdit = $("#txtPriceRetail").val();
        promodel.PriceOri = $("#txtPriceOri").val();
        promodel.IsSlider = 0;
        promodel.IsFeature = 0;
        if ($('#chk-slider').is(':checked')) {
            // Checkbox được chọn
            promodel.IsSlider = 1;
        }

        if ($('#chk-feature').is(':checked')) {
            // Checkbox được chọn
            promodel.IsFeature = 1;
        }

        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + '/CMS_Product/SaveNewProducts',
            data: JSON.stringify({ pro: promodel }),
            contentType: 'application/json; charset=utf-8',
            dataType: "html",
            cache: false,
            success: function (response) {
                if (response.indexOf("!") == -1) {
                    $('#modalProductNew').modal("hide");
                    $('#tblPro').html(response);
                    showNotifySuccess("Thêm sản phẩm thành công", "Đã lưu vào hệ thống", 3000);
                } else {
                    showNotifyError("Xảy ra lỗi", response, 3000);
                }
            },
            error: function () {
                showNotifyError("Mất kết nối mạng", "Mời thử lại !", 3000);
            }
        });
    }
}

function deleteProduct(proid) {
    var r = confirm("Bạn có chắc chắn muốn ngừng kinh doanh mặt hàng này ? ");
    if (r == true) {
        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + "/CMS_Product/DeleteProductByID",
            data: { productID: proid },
            beforeSend: function () {
                $('#loading-all').show();
            },
            success: function (response) {
                $('#tblPro').html(response);
                showNotifySuccess("Ngừng kinh doanh mặt hàng thành công", "Đã lưu vào hệ thống", 3000);
            },
            error: function () {
                showNotifyError("Mất kết nối mạng", "Mời thử lại !", 3000);
            }, complete: function () {
                $('#loading-all').hide();
            }
        });
    }
}

function editProduct(proid) {
    $.ajax({
        type: "POST",
        url: location.protocol + '//' + location.host + "/CMS_Product/GetProductByID",
        data: { productID: proid },
        beforeSend: function () {
            $('#loading-all').show();
        },
        success: function (response) {
            $('#modalProductEdit').html(response);
            $('#modalProductEdit').modal("show");
        },
        error: function () {
            showNotifyError("Mất kết nối mạng", "Mời thử lại !", 3000);
        }, complete: function () {
            $('#loading-all').hide();
        }
    });
}

function saveEditProduct() {

    var ProductCode = $('#txtProductCodeEdit').val();
    var ProductName = $('#txtProductNameEdit').val();
    var ProductCate = $('#sleCateEdit').val();

    if (ProductCode == "" || ProductName == "" || ProductCate == null) {
        showNotifyError("Nhập thiếu dữ liệu", "Những mục (*) bắt buộc nhập", 3000);
    } else {

        var datas = CKEDITOR.instances.editor2.getData();
        var promodel = {};
        promodel.ProductID = $("#txtProEditID").val();
        promodel.ProductCode = ProductCode;
        promodel.ProductName = ProductName;
        promodel.HidImg = $("#txtHidImgEdit").val();
        promodel.HidImg2 = $("#txtHidImgEdit2").val();
        promodel.PriceOri = $("#txtPriceOriEdit").val();
        promodel.Gallery = $("#txtHidImgEditGallery").val();
        promodel.Category = $('#sleCateEdit').val().toString();
        promodel.ProductDescription = datas;
        promodel.ProductSummary = CKEDITOR.instances.editor1.getData();
        promodel.PriceRetailEdit = $("#txtPriceRetailEdit").val();
        promodel.IsSlider = 0;
        promodel.IsFeature = 0;
        if ($('#chk-slider').is(':checked')) {
            // Checkbox được chọn
            promodel.IsSlider = 1;
        }

        if ($('#chk-feature').is(':checked')) {
            // Checkbox được chọn
            promodel.IsFeature = 1;
        }

        $.ajax({
            type: "POST",
            url: location.protocol + '//' + location.host + '/CMS_Product/SaveEditProducts',
            data: JSON.stringify({ pro: promodel }),
            contentType: 'application/json; charset=utf-8',
            dataType: "html",
            cache: false,
            success: function (response) {
                if (response.indexOf("!") == -1) {
                    $('#modalProductEdit').modal("hide");
                    $('#tblPro').html(response);
                    showNotifySuccess("Cập nhật sản phẩm thành công", "Đã lưu vào hệ thống", 3000);
                } else {
                    showNotifyError("Xảy ra lỗi", response, 3000);
                }
            },
            error: function () {
                showNotifyError("Mất kết nối mạng", "Mời thử lại !", 3000);
            }
        });
    }
}


function uploadFileExcel() {
    $("#excelFile").trigger('click');
    $("#excelFile").change(function () {
        submitFileImport();
    });
}

function showDetailNew() {
    var check = $('#lnkDetail').html();
    if (check == "Nội dung sản phẩm") {
        $('#detailProductNew').show(200);
        $('#lnkDetail').html('Ẩn');
    } else {
        $('#detailProductNew').hide(200);
        $('#lnkDetail').html('Nội dung sản phẩm');
    }
}


function showDetailEdit() {
    var check = $('#lnkDetailEdit').html();
    if (check == "Nội dung sản phẩm") {
        $('#detailProductEdit').show(200);
        $('#lnkDetailEdit').html('Ẩn');
    } else {
        $('#detailProductEdit').hide(200);
        $('#lnkDetailEdit').html('Nội dung sản phẩm');
    }
}



function showSumNew() {
    var check = $('#lnkSumNew').html();
    if (check == "Tóm tắt sản phẩm") {
        $('#sumaryProductNew').show(200);
        $('#lnkSumNew').html('Ẩn');
    } else {
        $('#sumaryProductNew').hide(200);
        $('#lnkSumNew').html('Tóm tắt sản phẩm');
    }
}


function showSumEdit() {
    var check = $('#lnkSumEdit').html();
    if (check == "Tóm tắt sản phẩm") {
        $('#sumaryProductEdit').show(200);
        $('#lnkSumEdit').html('Ẩn');
    } else {
        $('#sumaryProductEdit').hide(200);
        $('#lnkSumEdit').html('Tóm tắt sản phẩm');
    }
}

