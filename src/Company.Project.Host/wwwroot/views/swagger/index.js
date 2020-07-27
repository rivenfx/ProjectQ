var customRequestInterceptorInterval = setInterval(() => {

    if (window.ui && window.ui.getConfigs()) {
        clearInterval(customRequestInterceptorInterval);

        window.ui.getConfigs().requestInterceptor = function (request) {

            // 获取租户
            var tenant = app.multiTenancy.getTenant();
            if (tenant && tenant !== '') {
                request.headers['Tenant'] = tenant;
            }

            return request;
        }
    }

}, 1000);



