app.controller('mainController', ['$scope', '$rootScope', 'dialog', '$loading', '$http', function ($scope, $rootScope, dialog, $loading, $http) {
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

        $("#tabContents").height($(window).innerHeight() - (pageNavHeaderHeight + pageMenuHeaderHeight + 50));
        $("#menuContents").height($(window).innerHeight() - (pageNavHeaderHeight + pageMenuHeaderHeight + 50));

        $('iframe').height($("#tabContents").innerHeight());
        $('iframe').width($("#tabContents").innerWidth() - 20)

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

                $loading.finish("main");
            });

            setTimeout(function () {
                //$scope.$apply();

            }, 100 + dl);

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




}]);