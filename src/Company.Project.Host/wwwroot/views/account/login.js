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
            success: function (response) {
                console.log(response);//打印服务端返回的数据(调试用)
                var result = response.result;

                if (result.accessToken) {
                    alert("登录成功,即将跳转到页面");
                    window.location.href = "/Home/Index";
                } else {
                    alert("登录失败");
                }

            },
            error: function (e) {
                debugger
                if (e.responseJSON && e.responseJSON.result) {
                    var result = e.responseJSON.result;
                    alert("登录失败,错误消息: " + result.message + "  详情: " + result.details);
                } else {
                    alert("登录异常!");
                }

            }
        });


        e.preventDefault();
        return false;
    });
});