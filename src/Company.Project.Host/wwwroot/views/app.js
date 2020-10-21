var app = {};

app.appPath = app.appPath || '/';
app.domain = null;

app.multiTenancy = {
    key: 'TenantName',
    setTenant: function (tenant) {

        if (tenant) {
            app.utils.setCookie(
                app.multiTenancy.key,
                tenant,
                new Date(new Date().getTime() + 5 * 365 * 86400000), //5 years
                app.appPath,
                app.domain
            );
        } else {
            app.utils.deleteCookie(app.multiTenancy.key, app.appPath);
        }
    },
    getTenant: function () {
        var value = app.utils.getCookie(app.multiTenancy.key);
        if (!value) {
            return null;
        }

        return value;
    }
}


app.utils = {
    setCookie: function (key, value, expireDate, path, domain) {
        var cookieValue = encodeURIComponent(key) + '=';

        if (value) {
            cookieValue = cookieValue + encodeURIComponent(value);
        }

        if (expireDate) {
            cookieValue = cookieValue + "; expires=" + expireDate.toUTCString();
        }

        if (path) {
            cookieValue = cookieValue + "; path=" + path;
        }

        if (domain) {
            cookieValue = cookieValue + "; domain=" + domain;
        }

        document.cookie = cookieValue;
    },
    getCookie: function (key) {
        var equalities = document.cookie.split('; ');
        for (var i = 0; i < equalities.length; i++) {
            if (!equalities[i]) {
                continue;
            }

            var splitted = equalities[i].split('=');
            if (splitted.length != 2) {
                continue;
            }

            if (decodeURIComponent(splitted[0]) === key) {
                return decodeURIComponent(splitted[1] || '');
            }
        }

        return null;
    },
    deleteCookie: function (key, path) {
        var cookieValue = encodeURIComponent(key) + '=';

        cookieValue = cookieValue + "; expires=" + (new Date(new Date().getTime() - 86400000)).toUTCString();

        if (path) {
            cookieValue = cookieValue + "; path=" + path;
        }

        document.cookie = cookieValue;
    }
}
