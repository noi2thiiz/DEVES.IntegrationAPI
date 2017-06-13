function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

var app = angular.module('crmApp', ["app.core", "darthwade.dwLoading","ngResource","ngSanitize","ngCookies"]);

app.run(['$window', '$location', '$loading', '$http', '$cookies',function ($window, $location, $loading, $http,$cookies) {

    $("body").css("display","block");
    $("#main-container").css("display","block");


    $( "#home-page" ).animate({
        opacity: "show"
    }, {
        duration: 500
    });

}]);
app.controller('mainController', ['$scope', 'dialog', '$loading', '$http','$q','$cookies','$location', function ($scope, dialog, $loading, $http,$q,$cookies,$location) {
   //debugger;
    //var vm = this;
    //vm.deferred = $q.defer();
    //vm.deferred.resolve();

    // $("#main-page").attr('disabled',true);


    var refCode = $.trim(getParameterByName("ref"));
    var openMode = $.trim(getParameterByName("mode"));


    if(refCode.length!=11){
        dialog.alert({
            title: "Error",
            content: "ขออภัยรหัสอ้างอิงแบบประเมินนี้ไม่ถูกต้อง"
        });

    }

    var usuerGid = $.trim(getParameterByName("uid"));
    if(usuerGid==""){
        usuerGid = $.trim(getParameterByName("guid"));
    }
    if(usuerGid==""){
        usuerGid = $.trim(getParameterByName("userid"));
    }
    if(usuerGid==""){
        usuerGid = $.trim(getParameterByName("userGuid"));
    }


    if(openMode && openMode=="reset"){
        $cookies.remove('assessmentStatus_'+refCode);
        var url1 = (window.location.href).split("&");

        location.href=""+url1[0];

    }


    var ref = refCode.substr(0,10);
    var assessmentQuestionnaireId =""; //hard AdHoc

    var assessmentType = Number(refCode.substr(10,1));
    //set default
    if($.trim(assessmentType)==""){
        assessmentType = 1;

    }
    assessmentQuestionnaireId = window.appConfig.assessmentQuestionnaire[assessmentType];
    var assessmentSurveyByUserid = "";
    var assessmentGarageByUserid = "";

    //1 : Survey
    //2 : Garage

    if(assessmentType==1){

        assessmentSurveyByUserid = usuerGid;

    }else if(assessmentType==2){

        assessmentGarageByUserid=usuerGid;

    }



    var queryString =  $location.search();
    console.log(queryString);
    var scoreData = {
        "assessmentType": 0,
        "assessmentQuestionnaireId":0,
        "assessmentRefCode": "",
        "assessmentComment": ""
    };
    $scope.questions=[];

    if(assessmentType==2) {

        $scope.questions = [
            {
                id: 1,
                title: " ข้อ 1/3",
                group: 1,

                subQuestions: [
                    {
                        id: 1, group: 2,
                        ref: 'assessmentScore1',
                        title: "การให้บริการและการต้อนรับของอู่มีความพึงพอใจในระดับใด",
                        value: 0
                    }

                ]
            },
            {
                id: 2,
                title: " ข้อ 2/3",
                group: 2,

                subQuestions: [
                    {
                        id: 1, ref: 'assessmentScore2',
                        group: 2, title: "ระยะเวลาที่ใช้ในการจัดซ่อมมีความพึงพอใจในระดับใด", value: 0
                    }

                ]
            },
            {
                id: 3,
                title: "ข้อ 3/3",
                group: 3,

                subQuestions: [
                    {
                        id: 1, group: 3,
                        ref: 'assessmentScore3',
                        title: "ผลงานการจัดซ่อมของอู่มีความพึงพอใจในระดับใด", value: 0
                    }
                ]
            }


        ];

    }else {
        $scope.questions = [
            {
                id: 1,
                title: " ข้อ 1/3",
                group: 1,

                subQuestions: [
                    {
                        id: 1, group: 2,
                        ref: 'assessmentScore1',
                        title: "การบริการของเจ้าหน้าที่รับแจ้งอุบัติเหตุ มีความพึงพอใจระดับใด",
                        value: 0
                    }

                ]
            },
            {
                id: 2,
                title: " ข้อ 2/3",
                group: 2,

                subQuestions: [
                    {
                        id: 1, ref: 'assessmentScore2',
                        group: 2, title: "การเดินทางถึงที่เกิดเหตุของเจ้าหน้าที่สำรวจภัย มีความพึงพอใจระดับใด", value: 0
                    }

                ]
            },
            {
                id: 3,
                title: "ข้อ 3/3",
                group: 3,

                subQuestions: [
                    {
                        id: 1, group: 3,
                        ref: 'assessmentScore3',
                        title: "การบริการและความรวดเร็วในการทำงานของเจ้าหน้าที่สำรวจภัย มีความพึงพอใจระดับใด", value: 0
                    }
                ]
            }


        ];
    }


    var currentScreenWidth = "";
    var currentScreenHeight = "";
    function resize() {
        $("body").css("visibility","hidden");
    if(    currentScreenWidth !=  screen.width
        || currentScreenHeight != screen.height
        ){
        currentScreenWidth=screen.width
        currentScreenHeight=screen.height
        //alert("resize"+currentScreenWidth+":"+currentScreenHeight);


        if($("body").width()>400){
            $("#home-img-full").show();
            $("#home-img-mobile").hide();

            $("#home-page .page-header").hide();
            $("#page-footer").hide();
            //$("#thanks-page-body").height(screen.height-200);

            //  $("#thanks-page-body-link").css("margin-top",$("#thanks-page-body").height()-100);



        }else{
            $("#home-img-full").hide();
            $("#home-img-mobile").show();

            $("home-page .page-header").show();
            $("#page-footer").show();
            //  $("#thanks-page-body").height($("body").innerHeight()-250);

        }

        $("#thanks-page-body,#thanks-page-body-left-panel,#thanks-page-body-right-panel").css({
            "height": "95%"
        });




        if(window.innerHeight<=500){
            $("#home-page .page-header").hide();
            $("#textarea-comment").attr("rows",5);
            $("#page-footer").hide();
        }
        var bodyHeight = window.innerHeight;
        var bodywidth = $("#thanks-page-body-left-panel").innerWidth();

        var spaceHeight =  bodyHeight-$(".vertical-center").innerHeight();
        $(".vertical-center").css("margin-top",spaceHeight/4);

        $(".deves-icon").height((bodywidth*0.9)/3);



        setTimeout(function() {

            var rightWidth = $("#thanks-page-body-right-panel").width();

            $("#img-deves-line").css("margin-top",bodyHeight/5);
            $("#img-deves-line").height("auto");
            //$("#img-deves-line").width(rightWidth*0.95);

            if(window.innerHeight<=150) {
                $("#img-thanks-img").height("50px").width("auto");
                $(".deves-icon").height((bodywidth*0.8)/3.8);

            }else if(window.innerHeight<=300){




                $("#img-deves-logo").height("10px");

                $("#img-thanks-img").height("50px");
                $(".deves-icon").height((bodywidth*0.8)/3);


                $("#img-thanks-img").height("90px").width("auto");
                $(".deves-icon").height((bodywidth*0.6)/3);

                $("#img-deves-line").css("margin-top","30px");
                $("#img-deves-line").width(rightWidth*0.7);
                $("#img-deves-logo").height("20px");
                $("#img-deves-socail").height("20px").width("auto").css("bottom","2px").css("right","2px");
                $("#deves-icon-container").css("margin-top","0px");

            }
            else if(window.innerHeight<=500){
                $("#img-thanks-img").height("120px").width("auto");
                $("#img-deves-socail").height("50px").width("auto").css("bottom","2px").css("right","2px");

            }
            else{
                $("#img-deves-socail").height("auto").width("90%").css("bottom","2px").css("right","2px");




                if(window.innerHeight < window.innerWidth){
                    $("#img-thanks-img").width(bodywidth*0.6).height("auto");
                }else{
                    $("#img-thanks-img").width(bodywidth*0.9).height("auto");
                }
                var spaceHeight =  bodyHeight-$(".vertical-center").innerHeight();
                $(".vertical-center").css("margin-top",spaceHeight/4);

                $(".deves-icon").height((bodywidth*0.9)/3);
                if(bodyHeight-$(".vertical-center").innerHeight()>150){


                    $("#deves-icon-container").css("margin-top","50px");
                }

            }
        }, 200);



    }
        $("body").css("visibility","visible");
    }

    function showPage(pageId) {

        $("section").hide();
        $( "#" +pageId).animate({
            opacity: "show"
        }, {
            duration: 500
        });

        if(history.pushState) {
            history.pushState(null, null, "#"+pageId);
        }
        else {
            location.hash = '#'+pageId;
        }

        if(pageId=="thanks-page"){
           $(".page-header").remove();
            $("#page-footer").remove();
            $("body").css("background",'url("img/deves/left_panel_bg.png")');


        }
        resize();
    }


    $scope.currentQuestionId = 0;
    $scope.currentSubQuestionId = 0;

    $scope.currentQuestion=null;
    $scope.currentSubQuestion=null;

    $scope.selectQuestion = function (main,sub) {
        $loading.start('options');
        var time = 300;
        if(main ==0){
            time = 10;
        }

        $scope.currentQuestionId = main;
        $scope.currentSubQuestionId = sub;


        setTimeout(function(){

            $scope.currentQuestion = $scope.questions[main];
            $scope.currentSubQuestion = $scope.questions[main].subQuestions[sub];
            $scope.$apply();



            $("#question-header").hide();
            $("#question-mobile-form").show();
            $loading.finish("options");

        }, time);



    }

    $scope.selectQuestionValue = function (currentSubQuestion,value) {

        currentSubQuestion.value = value;
        var refscore = currentSubQuestion.ref;
        scoreData[refscore] =  currentSubQuestion.value


        var main = $scope.currentQuestionId ;
        var sub = $scope.currentSubQuestionId + 1;
        if(sub >= $scope.questions[main].subQuestions.length ){
            main+=1;
            sub = 0;
        }else{

        }


        $(".circleBase[data-id='circle-"+main+"']").removeClass("type2").addClass("type1");
        if(main >=  $scope.questions.length){
            $("#question-page").hide();
            showPage("question-remark-form");


        }else{

            $scope.selectQuestion( main,sub);
        }


    };

    $scope.startQuestion = function () {
        if(refCode.length!=11){
            dialog.alert({
                title: "Error",
                content: "ขออภัยรหัสอ้างอิงแบบประเมินนี้ไม่ถูกต้อง"
            });

        }else{
            try{
                $("#home-page").hide();
                showPage("question-page");

                if(window.innerHeight<=500){
                    $("#home-page .page-header").hide();
                    $("#textarea-comment").attr("rows",5);
                    $("#page-footer").hide();
                }else{
                    $("#page-footer").show();
                }

                $scope.selectQuestion(0,0);
            }catch (e){
                console.warn(e)

            }
        }


    };


    $scope.comment = "";
    $scope.submitQuestionResult  = function(){
         if(ref=="xxxxxxxxxx"){
             $cookies.put('assessmentStatus_'+refCode,"complete");
             showPage("thanks-page");
             location.href = "";
         }


        if(assessmentType==1){
            assessmentComment = $scope.comment;
            assessmentComment = $scope.comment;
        }else if(assessmentType==2){
            assessmentComment = $scope.comment;;
        }

        var submitData = {
            assessmentType: assessmentType,
            assessmentQuestionnaireId:assessmentQuestionnaireId,
            assessmentRefCode: ""+refCode,
            assessmentComment: ""+$scope.comment,

        };
       var  finalSubmitData = angular.merge(scoreData,submitData);


        var apiEndpoint =  "/csat-service/api/SubmitSurveyAssessmentResult";
        var valid = true;
        if (!valid) {
            dialog.alert({
                title: "Alert",
                content: "กรุณาตอบคำถามให้ครบทุกข้อก่อนครับ"
            });
        } else {
            console.log(JSON.stringify(finalSubmitData));
            $loading.start('main');
            $http({
                method: 'POST',
                data: finalSubmitData,
                url: apiEndpoint
            }).then(function successCallback(response) {
                $loading.finish('main');
                // this callback will be called asynchronously
                // when the response is available
                if (response.data.code == '200') {

                    //$loading.finish("main");

                    //success
                    $cookies.put('assessmentStatus_'+refCode,"complete");
                    if($.trim(openMode)=="user" && window.opener){
                        window.close();

                    }else{

                        showPage("thanks-page");
                        location.href = "";
                    }



                } else {
                    $loading.finish("main");
                    // {"code":"500","message":"Invalid Input(s)","description":"Some of your input is invalid. Please recheck again.","transactionId":"25b558af-0d26-47ab-b0a2-57ea00e6685a","transactionDateTime":"4/29/2017 10:24:42 PM","data":{"fieldErrors":[{"name":"profileInfo.salutation","message":"Required field must not be null"}]}}

                    var error = response.data ; //JSON.parse()

                    console.log(error.data);
                    var message = "Server Error";
                    var title = "ผลการส่งคะแนนประเมิน";

                        if($.trim(error.description) != "" && $.trim(error.message) != ""){
                            title = error.message;
                            message = error.description;
                        }else
                        if($.trim(error.message) != ""){
                            message = error.message;
                         }
                         else{
                            message = "มีข้อผิดพลาดเกิดขึ้น โปรดลองใหม่อีกครั้งในภายหลัง";
                        }



                    dialog.alert({
                        title: title,
                        content: message
                    });
                    //showPage("home-page");
                }
                //console.log(response)
            }, function errorCallback(response) {
                $loading.finish("main");
                dialog.alert({
                    title: "Server Error",
                    content: "มีข้อผิดพลาดเกิดขึ้น โปรดลองใหม่อีกครั้งในภายหลัง"
                });
                //showPage("home-page");
            });
        }



    }
    $("textarea").on("focus",function () {
        //if(window.innerHeight<500) {
            $("#page-footer").hide();
            //$(".rcorners1").height(50);
        //}

    });

    $("textarea").on("blur",function () {
        if(window.innerHeight>=500){
            $("#page-footer").show();
            //$(".rcorners1").height(100);
        }

    });




/*
    if(window.location.hash) {
        var hash = window.location.hash.substr(1);
        hash=hash.replace("#","");
        hash=hash.replace("!","");

            showPage(hash);

    }
*/
    if($cookies.get('assessmentStatus_'+refCode) == "complete"){
        showPage("thanks-page");
    };


    $( document ).ready(function() {
        console.log( "ready!" );
        $(window).on("load", function() {
            $(window).resize(resize);
            resize();
        });


    });

}]);