function UIController(resourceUrls, functionHandle) {
    if (!resourceUrls) {
        resourceUrls = new Array();
    }
    //  var resourceEndpoint = "https://crmdev.deves.co.th/resources";
    var resourceEndpoint = "";
    //resourceUrls.unshift('/WebResources/pfc_Function_Appconfig.js');
    var commonResourceUrls = new Array();

    commonResourceUrls.push(resourceEndpoint + '/jquery/dist/jquery.min.js');


    resourceUrls.unshift(resourceEndpoint + '/colorbox/example1/colorbox.css');
    resourceUrls.unshift(resourceEndpoint + '/colorbox/example1/colorbox.css');

    //MsgBox
    resourceUrls.unshift(resourceEndpoint + '/msgbox/styles/msgBoxLight.css');
    resourceUrls.unshift(resourceEndpoint + '/msgbox/scripts/jquery.msgBox.js');


    resourceUrls.push(resourceEndpoint + '/common/common_util.js');


    if (!functionHandle) {
        functionHandle = function (cfg, util) {

        }
    }

    var cfg = {};

    /**
     * Used  to inject a CSS link into the head element based on the url
     * @param url
     */
    function loadCss(url, callback) {
        var document = window.top.document;

        //  var file = location.pathname.split( "/" ).pop();

        var link = document.createElement("link");
        link.href = url;
        link.type = "text/css";
        link.rel = "stylesheet";
        link.media = "screen,print";

        link.onreadystatechange = callback;
        link.onload = callback;

        document.getElementsByTagName("head")[0].appendChild(link);
    }

    function loadScript() {

        function loadScript(url, callback) {
            var document = window.top.document;

            // Adding the script tag to the head as suggested before
            var head = document.getElementsByTagName('head')[0];
            var script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = url;

            // Then bind the event to the callback function.
            // There are several events for cross browser compatibility.
            script.onreadystatechange = callback;
            script.onload = callback;

            // Fire the loading
            head.appendChild(script);
        }

    }

    function loadExternalResource(url, type) {
        return new Promise(function (resolve, reject) {
            var tag;
            var ltag;
            if (!type) {
                var match = url.match(/\.([^.]+)$/);
                if (match) {
                    type = match[1];
                }
            }

            if (!type) {
                type = "js";       // default to js
            }

            if (type === 'css') {
                tag = window.top.document.createElement("link");
                tag.type = 'text/css';
                tag.rel = 'stylesheet';
                tag.href = url;

                ltag = window.document.createElement("link");
                ltag.type = 'text/css';
                ltag.rel = 'stylesheet';
                ltag.href = url;
            }
            else if (type === "js") {
                tag = window.top.document.createElement("script");
                tag.type = "text/javascript";
                tag.src = url;

                ltag = window.document.createElement("script");
                ltag.type = "text/javascript";
                ltag.src = url;
            }
            if (tag) {
                tag.onload = function () {
                    resolve(url);
                };
                tag.onerror = function () {
                    reject(url);
                };
                window.top.document.getElementsByTagName("head")[0].appendChild(tag);

                ltag.onload = function () {
                    resolve(url);
                };
                ltag.onerror = function () {
                    reject(url);
                };
                window.document.getElementsByTagName("head")[0].appendChild(ltag);
            }
        });
    }

    function loadMultipleExternalResources(itemsToLoad) {
        var promises = itemsToLoad.map(function (url) {
            return loadExternalResource(url);
        });
        return Promise.all(promises);
    }

    loadMultipleExternalResources(commonResourceUrls).then(function () {

        loadMultipleExternalResources(resourceUrls).then(function () {
            var util = new window.top.AppCommonUtil();
            functionHandle($, cfg, util);

        });

    });


}