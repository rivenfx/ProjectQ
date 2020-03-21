$(function () {
    var $loginForm = $("#login");
    $loginForm.validate({
        rules: {
            account: {
                required: true
            },
            password: {
                required: true
            },
        }
    });

    $loginForm.submit(function (e) {
        $.ajax({
            type: "POST",// 方法类型
            dataType: "json",// 预期服务器返回的数据类型
            contentType: "application/json; charset=utf-8",
            url: "/api/TokenAuth/Authenticate",//url
            data: $loginForm.serializeObjectStr(),
            success: function (result) {
                debugger
                console.log(result);//打印服务端返回的数据(调试用)
                alert("登录成功,即将跳转到页面");

                window.location.href = "/Home/Index";
            },
            error: function (e) {
                debugger
                alert("异常！");
            }
        });


        e.preventDefault();
        return false;
    });
});