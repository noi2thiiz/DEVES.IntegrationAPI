app.filter('findAttr', function () {
    return function (jsonTest,attrName) {
        try{
            var json = JSON.parse(jsonTest);
            if(json["content"]){
                if(json["content"][attrName]){
                    return json["content"][attrName] ;
                }else{
                    if(attrName=='code'){
                        attrName = 'responseCode'
                    }
                    else if(attrName=='message'){
                        attrName = 'responseMessage'
                    }
                    return json[attrName] ;
                }
                return json["content"][attrName];
            }else{
                return json[attrName];
            }
        }catch(e) {
            return "";
        }
    };
});