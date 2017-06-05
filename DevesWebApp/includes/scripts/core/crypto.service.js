(function() {

    String.prototype.replaceAll = function(search, replacement) {
        var target = this;
        return target.replace(new RegExp(search, 'g'), replacement);
    };

    'use strict';
    angular
        .module('app.core')
        .service('crypto', crypto);

    crypto.$inject = [];

    // memory service
    function crypto() {
        return{
            getHash:function (str) {
                var hash = 0, i, chr, len;
                if (str.length === 0) return hash;
                for (i = 0, len = str.length; i < len; i++) {
                    chr   = str.charCodeAt(i);
                    hash  = ((hash << 5) - hash) + chr;
                    hash |= 0; // Convert to 32bit integer
                }
                return hash;
            }
        }
    }

})();