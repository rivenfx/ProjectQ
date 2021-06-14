$(function () {
    var $loginForm = $('#login');
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

    var $tenant = $('#tenant');
    if ($tenant) {
        var tenant = app.multiTenancy.getTenant();
        if (tenant && tenant !== '') {
            $tenant.val(tenant);
        }
    }


    $loginForm.submit(function (e) {

        var tenant = $loginForm.serializeObject().tenant;

        $.ajax({
            type: 'POST',// 方法类型
            dataType: 'json',// 预期服务器返回的数据类型
            contentType: 'application/json; charset=utf-8',
            url: 'api/TokenAuth/Authenticate',//url
            data: $loginForm.serializeObjectStr(),
            beforeSend: function (request) {
                if (tenant && tenant !== '') {
                    request.setRequestHeader('Tenant', tenant);
                }
            },
            success: function (response) {
                console.log(response);//打印服务端返回的数据(调试用)
                var result = response.result;

                if (result.accessToken) {
                    alert('登录成功,即将跳转页面');

                    // 设置租户
                    app.multiTenancy.setTenant(tenant);

                    // 跳转到首页
                    window.location.href = 'Home/Index';
                } else {
                    alert('登录失败');
                }

            },
            error: function (e) {
                if (e.responseJSON && e.responseJSON.result) {
                    var result = e.responseJSON.result;

                    console.error('登录失败,错误消息: ' + result.message + '  详情: ' + result.details);
                    alert('登录失败,错误消息: ' + result.message + '  详情: ' + result.details);
                } else {
                    alert('登录异常!');
                }

            }
        });


        e.preventDefault();
        return false;
    });
});