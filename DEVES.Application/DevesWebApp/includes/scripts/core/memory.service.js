(function() {
    'use strict';
    angular
        .module('app.core')
        .service('Memory', Memory);

    Memory.$inject = ['LStorage'];

    // memory service
    function Memory(LStorage) {
        var _memorydb = {};
        console.log("start Memory");
        var func =  {

            set:function (key,value,state) {
                if(!state){
                    state = 'global';
                }
                if( typeof _memorydb[state] == 'undefined'){
                    _memorydb[state] = {};
                }

                _memorydb[state][key]=value;
                LStorage.setParam(state+"-"+key,value);
            },
            get:function (key,state) {

                if(!state){
                    state = 'global';
                }
                console.log("----------LStorage--------------")
                console.log(LStorage.getParam(state+"-"+key))
                if(LStorage.getParam(state+"-"+key)){
                    return LStorage.getParam(state+"-"+key);
                }


                if( typeof _memorydb[state] == 'undefined'){
                    _memorydb[state] = {};
                }

                return  _memorydb[state][key]  || {};

            }
        }
        return func;
    }

})();