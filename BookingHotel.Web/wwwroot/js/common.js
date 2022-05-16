function open_popup_product_QuickView() {
    $.magnificPopup.open({
        items: {
            src: '#product_popup'
        },
        type: 'inline',
        preloader: false,
        // When elemened is focused, some mobile browsers in some cases zoom in
        // It looks not nice, so we disable it:
        callbacks: {
            beforeOpen: function () {
                $('.fotorama').fotorama();
            },
        }
    });
}
function getview(parameter) {
    //var url = "@Html.Raw(Url.Action("PartialEditCompany", "Controller", new { parameterID = " - parameter" }))";
    //url = url.replace("-parameter", parameter);
    //$('#partial').load(url);


    var url = "/Product/QuickView?id=" + parameter;
     $.ajax({
         url: url, 
         type: "GET", 
         dataType: "html",
         success: function (result) {
             $('#product_popup').empty();
             $('#product_popup').html(result);
             open_popup_product_QuickView();
         },
         error: function(){
             //handle your error here
         }
     });
    
}
function Wishlist(Id) {
    $.ajax({
        url: '/Product/ProductWishlist',
        data: { productID: Id },
        dataType: 'Json',
        type: 'POST',
        success: function (res) {

            if (res.status === true) {
                window.location = "/yeu-thich";
            }
            else {
                window.location = "/dang-nhap";
            }
        }
    });
}
function DeleteWishlist(Id) {
    //check login
    $.ajax({
        url: '/Product/DeteleProductWishlist',
        data: { productID: Id },
        dataType: 'Json',
        type: 'POST',
        success: function (res) {
            if (res.status === true) {
                alert("Bạn đã bỏ yêu thích sản phẩm này!")
                window.location.reload();
            }
            else {
                window.location = "/dang-nhap";
            }
        }
    });
}
function Search() {
    var key = $("#txtkey").val();
    var category = $("#category-select").val() || 0;
    if (key !== "" && key !== "undefined") {
        let newurl = `${window.location.protocol}//${window.location.host}/tim-kiem/1/${key}`;
        const encoded = encodeURI(newurl);
        window.location.href = `/tim-kiem/1/${key}`;
        //$.get(newurl, function (data) {
        //});
      
    }
}
function UpdateCart() {
    var Productid = $(this).attr("data-productid");
    var Quantity = $("#Quantity-" + Productid).val();
    var formData = new FormData();
    formData.append('productId', Productid);
    formData.append('quantity', Quantity);
    $.ajax({
        type: "POST",
        url: "/product/UpdateCart",
    dataType: 'Json',
        data: formData,
            success: function (result) {
                window.location.href = "/gio-hang";
            }
});
}
function AddToCartInDetail() {
    var Productid = $("#qty").attr("data-productid");
    var Quantity = $("#qty").val();
    var formData = new FormData();
    formData.append('productId', Productid);
    formData.append('quantity', Quantity);
    $.ajax({
        type: "POST",
        dataType: 'Json',
        url: `/them-gio-hang/${Productid}/${Quantity}`,
        data: {},
        success: function (result) {
            if (result.status) {
            window.location.href = "/gio-hang";
            }
        }
    });
}
function UpdateMiniCart() {
    var Productid = $(this).attr("data-productid");
    var Quantity = $("#Quantity-" + Productid).val();
    var formData = new FormData();
    formData.append('productId', Productid);
    formData.append('quantity', Quantity);
    $.ajax({
        type: "POST",
        url: "/product/UpdateCart",
        dataType: 'Json',
        data: formData,
        success: function (result) {
            window.location.href = "/gio-hang";
        }
    });
}

$(document).ready(function () {
 
});
