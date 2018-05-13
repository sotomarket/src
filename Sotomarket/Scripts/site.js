$(function () {
    $('.delete-btn').click(function () {
        $.post($(this).prop('href'), function (result) {
            var message = "Неизвестная ошибка!";
            if (result) {
                if (result.success) {
                    message = "Удалено успешно!";
                } else {
                    message = "Ошибка во время удаления";
                    if (result.ex) {
                        console.log(result.ex);
                        message += '<span class="error">' + reult.ex.Message + '</span>';
                    }
                }
            }
            bootbox.alert(message);
        });
        return false;
    });
    initGoodsCategorySelects();

    $('input[data-val-date]').datepicker({
        format: "dd.mm.yyyy",
        weekStart: 1,
        language: "ru",
        todayBtn: "linked",
        autoclose: true
    });
});

function initGoodsCategorySelects() {

    var goodsSubCategoryId = $('#GoodsSubCategoryId');
    
    if (goodsSubCategoryId.length) {
        var goodsCategoryId = $('#GoodsCategoryId');
        goodsSubCategoryId.select2();
        Select2Cascade(goodsCategoryId, goodsSubCategoryId, '/GoodsSubCategory/ListJson?categoryId=:parentId:');
    }
}

var Select2Cascade = (function (window, $) {

    function Select2Cascade(parent, child, url, select2Options) {
        var afterActions = [];
        var options = select2Options || {};

        // Register functions to be called after cascading data loading done
        this.then = function (callback) {
            afterActions.push(callback);
            return this;
        };

        parent.select2(select2Options).on("change", function (e) {

            child.prop("disabled", true);
            var _this = this;

            $.getJSON(url.replace(':parentId:', $(this).val()), function (items) {
                var newOptions = '<option value=""> &nbsp;</option>';
                $.each(items, function () {
                    newOptions += '<option value="' + this.id + '">' + this.text + '</option>';
                });

                child.select2('destroy').html(newOptions).prop("disabled", false)
                    .select2(options);

                afterActions.forEach(function (callback) {
                    callback(parent, child, items);
                });
            });
        });
    }

    return Select2Cascade;

})(window, $);