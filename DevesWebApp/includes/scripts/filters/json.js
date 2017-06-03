app.filter('jsonHighlight', function () {
    return function jsonHighlight(json) {
        if(!json){
            return "null";
        }
      //  var str = JSON.stringify(obj, null, 2);
        if (typeof json != 'string') {
            json = JSON.stringify(json, undefined, 2);
        }
        json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        var hide = "";
        return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
            var cls = 'number';
            var key = "";

            if (/^"/.test(match)) {
                if (/:$/.test(match)) {
                    cls = 'key';
                    key = match;
                } else {
                    cls = 'string';
                }
            } else if (/true|false/.test(match)) {
                cls = 'boolean';
            } else if (/null/.test(match)) {
                cls = 'null';
            }
            if(key == '"data":'){
               hide =  "hide"
            }
            return '<span class="' + cls +'">' + match + '</span>';
        });
    };
});

