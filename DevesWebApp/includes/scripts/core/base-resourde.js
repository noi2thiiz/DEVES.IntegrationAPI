(function() {
    'use strict';
    angular
        .module('app.core')
        .service('$baseResource', BaseResourceProvider);

   BaseResourceProvider.$inject = ['$resource', '$q', '$http'];

    // memory service
    //https://github.com/johndgiese/angular-hyper-resource/blob/master/angular-hyper-resource.js
    function BaseResourceProvider($resource, $q, $http) {


        var resources = {};

            function ResourceFactory(CFG) {
                var headers = {
                    'Content-Type': 'application/json'}
                var Resource  = $resource(window.top.CRMStoreServiceAPI, {}, {
                    query: {method: 'POST' ,isArray: false ,headers: headers,
                        transformResponse: function(rawSata) {
                            console.log( '--------------angular.fromJson(data)[0]------------------------' );
                            console.log( angular.fromJson(rawSata));

                            if(CFG.prop) {

                                    var data = angular.fromJson(rawSata);
                                    var transformedData = [];
                                    angular.forEach(data.data, function (item, key) {
                                        angular.forEach(CFG.prop, function (pop,key) {
                                            item[key] = item[pop.name];
                                        });

                                        transformedData.push(item);
                                    });
                                    console.log( '--------------transformedData------------------------' );
                                    console.log(transformedData);
                                     data.data = transformedData
                                    return data;


                            }
                            return angular.fromJson(rawSata);



                        }

                    },
                    get: {method: 'POST' ,isArray: false,headers: headers,
                        transformResponse: function(data) {

                            if(data){
                                console.log( '--------------angular.fromJson(data)[0]------------------------' );
                                console.log( angular.fromJson(data));
                                return angular.fromJson(data);
                            }
                            return null;
                        }},
                });


               // angular.extend(Resource.prototype, HyperObject.prototype);
                return Resource;
            }

            return ResourceFactory;





        //}];



    }

})();