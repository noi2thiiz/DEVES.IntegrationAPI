<!--Created by patiw on 29/4/2560.
- layout.pug-->
<!--Created by patiw on 29/4/2560.--><!-- - var cfg = {"id":"","label":"ประเภทความสัมพันธ์ของผุ้นำชุมชนกฟผ.","vm":"lvm","placeholder":"เลือกทั้งหมด" ,"defaultText":"ทั้งหมด","service":"LeaderLutRelNetworkTypeService","param":"PI_REL_NET_TYPE"}*/--><!--///////////////////////////////////////////////////////////////////-->
<html>
<head>
    <title>CRM-KM</title>
    <meta charset="UTF-8" />
    <link rel="stylesheet" href="/vendor-resource/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="/vendor-resource/bootstrap/css/bootstrap.min.css" crossorigin="anonymous" />
    <link rel="stylesheet" href="/vendor-resource/bootstrap/css/bootstrap-theme.min.css" crossorigin="anonymous" />
    <link rel="stylesheet" href="/vendor-resource/WebResources/pfc_angular.css" crossorigin="anonymous" />
    <script src="/vendor-resource/WebResources/pfc_jquery.js" type="text/javascript" crossorigin="anonymous"></script>
    <script src="/vendor-resource/WebResources/pfc_angular.js" type="text/javascript"></script>
    <script src="/vendor-resource/WebResources/pfc_angular_modules.js" type="text/javascript"></script>
    <script src="/vendor-resource/WebResources/pfc_bootstrap.js" type="text/javascript"></script>
    <script src="/vendor-resource/WebResources/pfc_variables_constructor.js" type="text/javascript"></script><!--Created by patiw on 29/4/2560.-->
    <style>
        body {
            padding-top: 60px;
            font-size: 12px !important;
        }

        .image-icon {
            width: 30px;
            height: 30px;
            background-color: transparent;
            background-repeat: no-repeat;
            background-position: left top;
            background-size: 30px 30px;
        }

        .navbar-brand {
            padding-top: 5px;
        }

        input.form-control {
            width: 15px;
            webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0) !important;
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0) !important;
        }

        td {
            vertical-align: middle !important;
            font-size: 12px !important;
        }

        .action-bar {
            margin-left: 0px;
            padding-top: 2px;
            margin-right: 2px;
        }

        #page-nav-header {
            background-repeat: repeat-x;
            background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAgAAAB+CAIAAACWFscAAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAJOgAACToAYJjBRwAAACiSURBVEhLbcexCQAhFECxP4ojOaw43xUHrwhCmsw992lm7TcfHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx//W/sDH7DGkBRJg0cAAAAASUVORK5CYII=');
        }

        .click-able {
            cursor: pointer;
        }

            .click-able:hover {
                background-color: #DFDFDF;
            }

        input.form-control {
            width: auto;
        }

        .container {
            width: 95% !important;
        }

        .input-sm {
            height: 25px !important;
            width: 95% !important;
        }

        .label {
            font-weight: 200;
        }

        .page-info-value {
            color: blue;
            border-bottom: 1px solid rgba(133, 144, 136, 0.42);
            min-height: 25px;
        }

        legend {
            font-size: 13px;
            font-weight: 200;
        }

        .form-horizontal .col-sm-9 {
            padding-left: 0 !important;
        }

        .form-horizontal .col-sm-3 {
            width: 23.5% !important;
        }

        .form-horizontal .col-sm-2 {
            width: 14% !important;
        }

        input, select, option {
            color: blue !important;
        }

        pre.json {
            outline: 1px solid #ccc;
            padding: 5px;
            margin: 5px;
        }

        .string {
            color: green;
        }

        .number {
            color: darkorange;
        }

        .boolean {
            color: blue;
        }

        .null {
            color: magenta;
        }

        .key {
            color: red;
        }

        .hide {
            display: none;
        }

        .navbar-header {
            max-width: 200px;
        }

        a {
            cursor: pointer;
        }

        .page-header {
            padding-bottom: 0px;
            margin: 10px 0 10px;
            border-bottom: 0px solid #eee;
        }


        .close {
            font-size: 12px;
        }
    </style>
    <style>
        body {
            overflow: hidden;
        }

        #wrap {
            position: fixed;
            left: 0;
            width: 100%;
            top: 0;
            height: 100%;
        }

        #tabContents {
            display: block;
            width: 100%;
            height: 100%;
        }

        .iframeContent {
            display: block;
            width: 100%;
            height: 100%;
        }

        #tabs-action-bar {
            padding-top: 5px;
            padding-right: 5px;
            min-height: 30px;
            margin-bottom: 0px;
            margin-top: 0px;
            border-top: none;
        }

        #menuContents {
            overflow-x: hidden;
        }

        .page-header {
            margin-bottom: 0px;
        }

        #custom-search-input {
            padding: 3px;
            border: solid 1px #E4E4E4;
            border-radius: 6px;
            background-color: #fff;
        }

            #custom-search-input input {
                border: 0;
                box-shadow: none;
            }

            #custom-search-input button {
                margin: 2px 0 0 0;
                background: none;
                box-shadow: none;
                border: 0;
                color: #666666;
                padding: 0 8px 0 10px;
                border-left: solid 1px #ccc;
            }

                #custom-search-input button:hover {
                    border: 0;
                    box-shadow: none;
                    border-left: solid 1px #ccc;
                }

            #custom-search-input .glyphicon-search {
                font-size: 16px;
            }

        .iframeMenu {
            max-width: 0px;
            max-height: 0px;
        }

        a {
            font-size: smaller;
        }
    </style>
</head>
<body ng-app="crmApp" ng-controller="mainController as vm" dw-loading="main" dw-loading-options="{text:'Processing...', spinner: true}">
    <nav class="navbar navbar-default navbar-fixed-top" id="page-nav-header"><div class="container"><div class="navbar-header"><a class="navbar-brand" href="#"><img src="/vendor-resource/images/theme_navbarlogo.png" /></a></div><div class="pull-right" style="font-size: larger; margin-top: 15px; color: white">รวม Link</div></div></nav><div id="main-container" style="display: none">
        <div id="wrapper">
            <div class="col-md-3 col-lg-2" id="sidebar-wrapper">
                <div id="sidebar">
                    <div class="page-header" id="page-menu-header"><ul class="nav nav-tabs"><li role="presentation" data-ng-click="activeCategory='product'" ng-class="{'active':activeCategory=='product'}"><a>Products</a></li><li role="presentation" data-ng-click="activeCategory='information'" ng-class="{'active':activeCategory=='information'}"><a>Information</a></li></ul><div id="custom-search-input"><div class="input-group col-md-12"><input class="form-control input-sm" type="text" placeholder="search menu" ng-model="searchMenu" /><span class="input-group-btn"><a class="btn btn-sm" type="button" data-ng-click="searchMenu=''"><i class="glyphicon glyphicon-refresh"></i></a></span></div></div></div><div class="page-body" id="menuContents"><ul class="nav list-group"><li ng-repeat="item in menuItems |filter:{title:searchMenu,category:activeCategory}"><a class="list-group-item" ng-if="item.openType=='page'" target="iframeContent" data-ng-click="openMenuItem(item)">{{item.title}}<img ng-if="item.imgSrc!=''" ng-src="{{item.imgSrc}}" /></a><a class="list-group-item" ng-if="item.openType=='download'" ng-href="{{item.link}}">{{item.title}}<img ng-if="item.imgSrc!=''" ng-src="{{item.imgSrc}}" /><i class="glyphicon glyphicon-paperclip pull-right" ng-if="item.openType=='download'"></i></a></li></ul></div><iframe class="iframeMenu" id="iframeMenu1" frameborder="0"></iframe>
                    <iframe class="iframeMenu" id="iframeMenu2" frameborder="0"></iframe>
                </div>
            </div><div class="col-md-9 col-lg-10 pull-right" id="main-wrapper">
                <div id="main">
                    <div class="page-header"><ul class="nav nav-tabs" id="tabs"><li ng-repeat="itemTab in menuTabs" ng-class="{'active':(menuSelectedItem.id==itemTab.id)}"><a data-ng-click="openLink(itemTab)">{{itemTab.title}}<i class="glyphicon glyphicon-remove pull-right close" data-ng-click="removeTab(itemTab)"></i></a></li><li class="pull-right" id="lastTab" ng-show="moreTabs.length&gt;0"><a class="btn dropdown-toggle" data-toggle="dropdown" href="#">More<span class="caret"></span></a><ul class="dropdown-menu" id="collapsed"><li ng-repeat="itemMoreTab in moreTabs "><a data-ng-click="openLinkInFirstTab(itemMoreTab)">{{itemMoreTab.title}}</a></li></ul></li></ul></div><div class="page-body" dw-loading="iframe" dw-loading-options="{text:'Loading...', spinner: true}">
                        <div class="row" ng-show="menuTabs.length&lt;=0">
                            <h5 class="text-center" style="padding-top:100px">
                                หากท่านเปิดหน้ารวมลิงก์ทิ้งไว้นานเกินไปโดยไม่ใช้งาน ระบบอาจจะตัดการเชื่อมต่อและจะแสดงหน้าจอให้ล็อกอิน ซึ่งท่านสามารถ refresh(F5) หน้านี้ใหม่เพื่อเข้าใช้งานระบบได้อีกครั้ง
                            </h5>
                        </div><div class="row" ng-show="menuTabs.length&gt;0"><div class="col-sm-12"><nav class="navbar navbar-default" id="tabs-action-bar"><a class="btn btn-xs btn-default pull-right" data-ng-click="removeTab(menuSelectedItem)"><i class="glyphicon glyphicon-remove"></i></a><a class="btn btn-xs btn-default pull-right"><i class="glyphicon glyphicon-refresh" data-ng-click="refreshTab(menuSelectedItem)"></i></a><a class="btn btn-xs btn-default pull-right"><i class="glyphicon glyphicon-new-window" data-ng-click="detachTab(menuSelectedItem)"></i></a></nav></div></div><div class="row"><div class="col-sm-12" id="tabContents"></div></div>
                    </div>
                </div><div class="col-md-12 footer"></div>
            </div>
        </div>
    </div>
    <script type="text/javascript">initvariable();</script>
    <script type="text/javascript">
var app = angular.module('crmApp', ["app.core", "darthwade.dwLoading",'ui.bootstrap',,"ngSanitize"]);app.run(['$window', '$location', '$loading', '$http', function ($window, $location, $loading, $http) {



    $("#main-container").show();
}]);app.controller('mainController', ['$scope', '$rootScope', 'dialog', '$loading', '$http', function ($scope, $rootScope, dialog, $loading, $http) {
    $scope.clients = [];
    $scope.pageInfo = {
        currentPage: 1, totalPage: 1,
        totalItem: 0, Offset: 0, Limit: 100
    };

    $scope.selectedItem = {};
    // init ui
    $scope.props = {};
    $scope.model = "";

    var linkProductUrl = window.top.callCenterLinkNewProductPart;
    var linkInformationUrl = window.top.callCenterLinkInformationPart;

    var linkHostname = window.top.callCenterLinkHostname;
    var InfomationlinkHostname = window.top.callCenterLinkHostname; //= "http://192.168.10.33";

    var linkProxy = window.top.callCenterLinkProxyPart;
    var linkProxyToken = window.top.callCenterLinkToken;

    var env = window.top.environmentName;


    console.log(linkProductUrl);
    console.log(linkHostname);
    console.log(linkProxy);

    var resize = function () {
        var pageNavHeaderHeight = $("#page-nav-header").height();
        var pageMenuHeaderHeight = $("#page-menu-header").height();

        $("#tabContents").height($(window).innerHeight() - (pageNavHeaderHeight + pageMenuHeaderHeight + 70));
        $("#menuContents").height($(window).innerHeight() - (pageNavHeaderHeight + pageMenuHeaderHeight + 70));

        $("#tabContents").css("background-color","yellow");
        $('.iframe-link').height($("#tabContents").innerHeight());
        $('.iframe-link').width($("#tabContents").innerWidth() - 20)

        $("#menuContents").css("overflow-y", "auto");
    }
    $(window).resize(resize);
    resize();


    $scope.menuTabs = [];
    $scope.moreTabs = [];
    $scope.allTabs = [];
    $scope.menuItems = [];
    $scope.activeCategory = "product";

    $scope.removeTab = function (item) {


        var index = $scope.menuTabs.indexOf(item);
        $scope.menuTabs.splice(index, 1);
        $('iframe#tabContent_' + item.id).remove();
        if (item.id == $scope.menuSelectedItem.id) {

            if ($scope.menuTabs.length > 0) {
                var openItem = $scope.menuTabs[0];
                $scope.openLink(openItem)
            }

        }

        if($scope.menuTabs.length <= 4 && $scope.moreTabs.length>0){

            $firstMoreTab =$scope.moreTabs[0];
            $scope.moreTabs.splice(0, 1);
            $scope.menuTabs.push($firstMoreTab);
        }

    }
    $scope.openLinkInFirstTab= function (item) {


        var index = $scope.moreTabs.indexOf(item);
        $scope.moreTabs.splice(index, 1);
        $('iframe#tabContent_' + item.id).remove();
        $scope.openLink(item);


    };

    $scope.openMenuItem = function (item) {
        var index = $scope.moreTabs.indexOf(item);

        if(index >0){

            $scope.moreTabs.splice(index, 1);
            $scope.openLinkInFirstTab(item);

        }else{
            $scope.openLink(item);
        }


    };

    $scope.openLink = function (item) {
        $scope.menuSelectedItem = item;


        $(".iframe-link").css("display", "none");
        $(".iframe-link").hide();

        if ($('iframe#tabContent_' + item.id).length) {
            $('iframe#tabContent_' + item.id).show();
            resize();
            return;
        }

        try {


            $loading.start('main');


            if ($scope.menuTabs.length > 4) {

                var lastItem = $scope.menuTabs.pop();
                $scope.moreTabs.unshift(lastItem);

            }

            $scope.menuTabs.unshift(item);
            $scope.allTabs.unshift(item);

             var iframeId ="tabContent_"+item.id;
             var $iframe = $('<iframe class="iframe-link" id="' + iframeId + '" src="" frameborder="0" ></iframe>');
            $("#tabContents").append($iframe);
            resize();
            $iframe.attr('src', item.link);
            $iframe.show();
            $iframe.load(function () {
                resize();
                $loading.finish("main");
            });

            setTimeout(function () {
                resize();

            }, 1000 );

        } catch (e) {

        }

    };


    $scope.refreshTab = function (item) {
        $loading.start('main');
        var iframeId ="tabContent_"+item.id;
        var $iframe = $('iframe#' + iframeId);

        $iframe.attr("src",item.link);
    };
    $scope.detachTab = function (item) {
        var iframeId ="tabContent_"+item.id;
        var $iframe = $('iframe#' + iframeId);

        var url = $iframe.attr("src");
        window.open(url, '_blank', 'location=yes,height=' + (screen.height - 200) + ',width=800,scrollbars=yes,top=10,status=yes');
    };


    setTimeout(function () {
        $loading.finish("main");

    }, 10000);

    function getFileType(filename) {
        var ext = "";
        if(filename){
            ext =  filename.split('.').pop();
        }

        switch (ext){
            case 'xls' : return 'download';
            case 'csv' : return 'download';
            default : return 'page'
        }

    }
    $scope.loadProductMenu = function () {
        $loading.start('main');


        $("#iframeMenu1").attr("src",linkProductUrl);
        $("#iframeMenu1").load(function () {

            $http.get(linkProxy+"?token="+linkProxyToken+"&env="+env+"&linkType=NewProduct", {}).
            then(function successCallback(response) {
                    $loading.finish('main');
                   // console.log(response.data);


                     $(response.data).find("a[href!='#'][target]").each(function (i,a) {

                         var $img = $(a).parent("td").find("img");
                         var openType = getFileType($(a).attr("href"));
                         var sourceUrl = $(a).attr("href");
                         var href =(sourceUrl.indexOf("http") != -1)? sourceUrl : linkHostname+"/"+sourceUrl;
                        $scope.menuItems.push({
                            id: 100+i,
                            title: $.trim($(a).text()),
                            imgSrc: $.trim($img.attr("src")),
                            link: href,
                            category:"product",
                            openType:openType,
                            active: false
                        });
                    });

console.log($scope.menuItems);

                    setTimeout(function () {
                        $scope.$apply();

                    }, 100);

                },
                function errorCallback(response) {
                    $loading.finish("main");
                    dialog.alert({
                        title: "Server Error",
                        content: response.statusText
                    });
                });


        });


    };
    $scope.loadInformationMenu = function () {
        $loading.start('main');


        $("#iframeMenu2").attr("src",linkInformationUrl);
        $("#iframeMenu2").load(function () {

            $http.get(linkProxy+"?token="+linkProxyToken+"&env="+env+"&linkType=Information", {}).
            then(function successCallback(response) {
                    $loading.finish('main');
                    // console.log(response.data);

                    $(response.data).find("a[href!='#'][target]").each(function (i,a) {

                        var $img = $(a).parent("td").find("img");
                        var openType = getFileType($(a).attr("href"));
                        var sourceUrl = $(a).attr("href");
                        var href =(sourceUrl.indexOf("http") != -1)? sourceUrl : InfomationlinkHostname+"/"+sourceUrl;
                        $scope.menuItems.push({
                            id: 200+i,
                            title: (i+1)+". "+$.trim($(a).text()),
                            imgSrc: $.trim($img.attr("src")),
                            link: href,
                            category:"information",
                            openType:openType,
                            active: false
                        });
                    });

                    setTimeout(function () {
                        $scope.$apply();

                    }, 100);

                },
                function errorCallback(response) {
                    $loading.finish("main");
                    dialog.alert({
                        title: "Server Error",
                        content: response.statusText
                    });
                });


        });


    };

    $scope.loadAllMenu = function(){
        $scope.loadProductMenu();
        $scope.loadInformationMenu();
    }
    $scope.loadAllMenu();
    setTimeout(function () {
        $scope.loadAllMenu();
        $scope.loadAllMenu();

    }, 500000);




}]);</script>
</body>
</html>