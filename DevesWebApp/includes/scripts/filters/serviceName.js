app.filter('serviceName', function () {
    return function (defaultValue, item) {

        try{
            var tm = item.data.RequestUri.split("/");
            var name = tm.pop();
            var tm2 = name.split("?");
            return tm2.shift();
        }catch (e){
            return defaultValue;
        }

    }
});