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
});