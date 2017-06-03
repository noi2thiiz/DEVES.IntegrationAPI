(function() {
  'use strict';
  angular
      .module('app.core')
      .service('LStorage', LStorage);

  LStorage.$inject = ["$window", "$rootScope"];

  // memory service
  function LStorage($window, $rootScope) {
      var MyStorage = [];
    angular.element($window).on('storage', function (event) {
      if (event.key === 'my-storage') {
        $rootScope.$apply();
      }
    });
    //localStorage.setItem('testObject', JSON.stringify(testObject));

// Retrieve the object from storage
    // var retrievedObject = localStorage.getItem('testObject');

    ////console.log('retrievedObject: ', JSON.parse(retrievedObject));
    var ls ={};

    ls.setData = function (val, key) {
      console.log(' ls.setData');
      if (typeof key == 'undefined') {
        key = 'my-storage';
      }

        $window.localStorage && $window.localStorage.setItem(key, JSON.stringify(val));
        MyStorage[key] = JSON.stringify(val);
        return this;
    };


    ls.getData = function (key) {
      console.log(' ls.getData');
      if (typeof key == 'undefined') {
        key = 'my-storage';
      }


      try {
          var val=null;
        if(MyStorage[key]){
            val = MyStorage[key]
        }else{
            val = $window.localStorage && $window.localStorage.getItem(key);
        }

        var json = JSON.parse(val);
      }
      catch (e) {
        json = {};
      }
      return json;

    };

    return {
      setData: function (val, key) {
        return ls.setData(val, key);
      },

      getData: function (key) {
        return ls.getData(key);
      },

      setParam: function ( id,val ) {

        var data = ls.getData();

        if( !data  ){
          data = {};
        }

        if( !data['params']){
          data['params'] = {};
        }

        data['params'][id] = val;
        ls.setData(data);

        return this;

      },

      getParam: function ( key ) {

        var data = ls.getData();

        if( !data || typeof data['params'] === 'undefined' ){
          return null;
        }
        console.log(data);
        return data['params'][key];

      },
      clearParam: function ( key ) {
        var data = ls.getData();

        if( !data  ){
          data = {};
        }

        if( !data['params']){
          data['params'] = {};
        }

        data['params'][key] = null;
        ls.setData(data);

        return this;
      },

      resetData: function () {
        try{
            MyStorage=[];
            $window.localStorage && $window.localStorage.setItem('my-storage', '{}');
            localStorage.removeItem('my-storage');
        }catch (e){

        }
        //

          //#

        return this;
      },
      clearData: function (key) {

        //#localStorage.removeItem(key);
          try{

              MyStorage=[key] =false;
              $window.localStorage && $window.localStorage.setItem(key, '{}');
              localStorage.removeItem(key);

          }catch (e){

          }


        return this;
      }
    };
  }

})();


