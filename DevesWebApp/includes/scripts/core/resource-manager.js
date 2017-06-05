/**
 * Created by patiw on 4/28/2016.
 */
var serviceApi = angular.module('app.core');
serviceApi.factory('resourceManager',['$resource','$rootScope','$q','$filter',"$window","crypto",
    function ($resource,$rootScope,$q,$filter,$window,crypto) {

    function formatDateObject(date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0'+minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return date.getMonth()+1 + "/" + date.getDate() + "/" + date.getFullYear(); // + "  " + strTime;
    }

    function formatDate(dateIn) {
        if(!dateIn){
            return dateIn;
        }
        if(typeof dateIn =='object'){
           return   formatDateObject(dateIn);
        }



        if(dateIn ==""){
            return "";
        }


        var text = '';
        if( !angular.isUndefined(dateIn) && dateIn !=''){
            if(dateIn=='0000-00-00 00:00:00'){
                return null;
            }

            var dateAndTime = dateIn.split("T");
            var dateOnly = dateAndTime[0].split("-");
            var timeOnly = dateAndTime[1].split(":");
            var y = dateOnly[0];
            var m = dateOnly[1];
            var d = dateOnly[2];


            text = m+"/"+d+"/"+y;
        }

        return text;
    }
    var resourceManager = function(cfg){
        console.log('resourceManager');
        var _resource = cfg.resource;
        var _fieldPk = cfg.proc.pk;
        var _saveSelectedRecord = null;
        var _procname =cfg.proc.procname;
        var _property =cfg.proc.prop;


        var _manager = {

            _pool: {},
            _retrieveInstance: function (id, record) {
                var instance = this._pool[id];

                if (instance) {
                    instance = record;
                } else if(this._pool[id]) {
                    instance = record;
                    this._pool[id] = record;
                }else
                {
                   var ls =  $window.localStorage;
                   var item = ls.getItem(_procname+id);
                   if(item){
                       instance = JSON.parse(item);
                   }
                }

                return instance;
            },
            _update:function(id, record){
                this._pool[id] = record;
            },
            _search: function (id) {
                return this._pool[id];
            },

            _getFilter:function (params) {
                if(!params){
                    var params = {};
                }
/*
                var request = {
                    "storeName": "sp_Query_TransactionLog",
                    "storeParams": [
                        {
                            "key": "Offset",
                            "value": $scope.pageInfo.Offset
                        },
                        {
                            "key": "Limit",
                            "value": $scope.pageInfo.Limit
                        }
                    ]
                };*/
                var data = {storeName:_procname,storeParams:[]};

                console.log("-------------------- _getFilter-----------------------");
                // console.log(JSON.stringify(_property));

                var isPI_LENGTH = false;

                angular.forEach(_property,function (val,key) {

                    if(val['default']){
                        var propName = val['name'];
                        params[propName] = val['default'];
                    }
                });

                return data;
            },
            _load: function (id, deferred) {
                console.log("------------_load form serve "+ _fieldPk+ " = "+id+"------------------");
                var scope = this;

                var qfilter = {};
                qfilter['PI_'+_fieldPk] = id;
                console.log(qfilter);
                var filter = scope._getFilter(qfilter);
                _resource.get(filter).$promise.then(function (result) {
                    console.log("------------result------------------");
                    console.log(result);
                    //scope._retrieveInstance(result[_fieldPk], result.data);

                    if(angular.isArray(result.data)){
                        deferred.resolve(result.data[0]);
                    }else{
                        deferred.resolve(null);
                    }


                }, function (error) {
                   // alert('debug ---- error');
                    console.error(error);
                    deferred.reject();

                });

                return deferred.promise;

            },

            getObject: function (id) {
                var deferred = $q.defer();
                var recode = this._search(id);
                console.log( '------------------getObject----------------------' );

                if (recode) {
                    console.log( '------------------found recode----------------------' );
                    console.log( recode );
                    deferred.resolve(recode);
                } else {

                    console.log('can not found');
                    this._load(id, deferred);
                }
                return deferred.promise;
            },
            getNullObject: function () {
                return new _resource;
            },


            query: function (id) {
                var deferred = $q.defer();
                _resource.get({id: id}).$promise.then(function (result) {


                    deferred.resolve(result);
                });
                return deferred.promise;
            },

            detail: function (id) {
                var deferred = $q.defer();
                _resource.detail({id: id}).$promise.then(function (result) {


                    deferred.resolve(result);
                });
                return deferred.promise;
            },
            getDetail: function (filter) {
                var deferred = $q.defer();
                _resource.getDetail(filter).$promise.then(function (result) {


                    deferred.resolve(result);
                });
                return deferred.promise;
            },

            fetchAll: function (filter) {
                var deferred = $q.defer();
                var scope = this;

                filter = this._getFilter(filter);
                var filterHash = crypto.getHash( _procname+JSON.stringify(filter));
                var tem = localStorage.getItem(filterHash);
                if(false){

                    var data = JSON.parse(tem);

                    deferred.resolve( data );

                }else{

                    _resource.query(filter).$promise.then(function (result) {

                        var records = [];
                        //console.log(_fieldPk);
                        //alert(_fieldPk);

                        angular.forEach(result.data,function(row,key){

                            var page = Math.ceil(result.total/result.data.length);
                            row['PAGE']= page;

                            records.push(row)
                            scope._retrieveInstance(row[_fieldPk],row);

                        });
                        var data = {data:records,  header:{total:result.total}};
                        //localStorage.setItem(filterHash, JSON.stringify(data));
                        deferred.resolve( data );
                    }, function(reason) {

                        deferred.reject(reason);

                    });


                }





                return deferred.promise;
            },

            persist:function(record){

                this._update(record[_fieldPk],record);
                var deferred = $q.defer();
                if(!angular.isUndefined(record[_fieldPk]) && $.trim(record[_fieldPk]) != ""){
                    record.$update(function(result) {
                        console.log('$update');
                        deferred.resolve(result);

                    },function(error){
                        deferred.resolve(error);
                        // deferred.reject(error);
                    });
                }else{
                    console.log('$save');
                    record.$save(function(result) {
                        deferred.resolve(result);

                    },function(error){
                        deferred.resolve(error);
                        // deferred.reject(error);
                    });
                }


                return deferred.promise;
            },

            saveSelectedRecord:function (record) {
                _saveSelectedRecord = record;
            },
            getSelectedRecord:function () {
                return _saveSelectedRecord;
            }
            ,
            setProcname:function (procname) {
                _procname = procname;
            }

        };
        return _manager;

    };

    return resourceManager;

}]);