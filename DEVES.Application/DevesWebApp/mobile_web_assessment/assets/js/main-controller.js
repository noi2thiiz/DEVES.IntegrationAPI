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
    var vm = this;
    vm.deferred = $q.defer();
    vm.deferred.resolve();

    // $("#main-page").attr('disabled',true);


    var refCode = $.trim(getParameterByName("ref"));
    if(!refCode){
        refCode="";
    }

    var usuerGid = $.trim(getParameterByName("ref"));
    if(!usuerGid){
        usuerGid="";
    }


    var ref = refCode.substr(0,10);

    var assessmentType = Number(refCode.substr(10,1));

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
    var scoreData = {};
    $scope.questions = [
        {
            id:1,
            title:" ข้อ 1/3",
            group:1,

            subQuestions: [
                {id:1, group:2,
                    ref:'assessmentClaimNotiScore',
                    title: "การบริการของเจ้าหน้าที่รับแจ้งอุบัติเหตุ มีความพึงพอใจระดับใด",
                    value:0}

           ]
        },
        {
            id:2,
            title:" ข้อ 2/3",
            group:2,

            subQuestions: [
                {id:1, ref:'assessmentSurveyScore',
                    group:2,title: "การเดินทางถึงที่เกิดเหตุของเจ้าหน้าที่สำรวจภัย มีความพึงพอใจระดับใด",value:0}

            ]
        },
        {
            id:3,
            title:"ข้อ 3/3",
            group:3,

            subQuestions: [
                {id:1,group:3,
                    ref:'assessmentSurveySpeedScore',
                    title: "การบริการและความรวดเร็วในการทำงานของเจ้าหน้าที่สำรวจภัย มีความพึงพอใจระดับใด" ,value:0}
            ]
        },


    ];



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
   }






    $scope.currentQuestionId = 0;
    $scope.currentSubQuestionId = 0;

    $scope.currentQuestion;
    $scope.currentSubQuestion;

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


    }

    $scope.startQuestion = function () {

        $("#home-page").hide();
        showPage("question-page");
        $("#page-footer").show();
        $scope.selectQuestion(0,0);
    }

    $scope.comment = "";
    $scope.submitQuestionResult  = function(){

        var assessmentClaimNotiScore = scoreData['assessmentClaimNotiScore'];

        var assessmentSurveyScore = scoreData['assessmentSurveyScore'];
        var assessmentSurveySpeedScore = scoreData['assessmentSurveySpeedScore'];
        var assessmentSurveyTimeUsageScore = scoreData['assessmentSurveyTimeUsageScore'];

        var assessmentGarageServiceScore = scoreData['assessmentGarageServiceScore'];
        var assessmentGarageCommitScore = scoreData['assessmentGarageCommitScore'];
        var assessmentGarageRepairScore = scoreData['assessmentGarageRepairScore'];

        var assessmentGarageComment = "";


        if(assessmentType==1){
            var assessmentSurveyComment = $scope.comment;
            var assessmentClaimNotiComment = $scope.comment;
        }else if(assessmentType==2){
            var assessmentGarageComment = $scope.comment;;
        }

        var submitData = {
            "assessmentType": assessmentType,
            "assessmentrefcode": ""+ref,
            "assessmentClaimNotiScore": assessmentClaimNotiScore,
            "assessmentClaimNotiComment": ""+assessmentClaimNotiComment,
            "assessmentSurveyScore": assessmentSurveyScore,
            "assessmentSurveySpeedScore": assessmentSurveySpeedScore,
            "assessmentSurveyTimeUsageScore": assessmentSurveyTimeUsageScore,
            "assessmentSurveyComment": ""+assessmentSurveyComment,
            "assessmentSurveyByUserid": $.trim(assessmentSurveyByUserid),
            "assessmentGarageServiceScore": assessmentGarageServiceScore,
            "assessmentGarageCommitScore": assessmentGarageCommitScore,
            "assessmentGarageRepairScore": assessmentGarageRepairScore,
            "assessmentGarageComment": ""+assessmentGarageComment,
            "assessmentGarageByUserid": $.trim(assessmentGarageByUserid)
        };

        var apiEndpoint =  "https://crmappqa.deves.co.th/xrmapi/api/SubmitSurveyAssessmentResult";
        var valid = true;
        if (!valid) {
            dialog.alert({
                title: "Alert",
                content: "กรุณาตอบคำถามให้ครบทุกข้อก่อนครับ"
            });
        } else {
            console.log(JSON.stringify(submitData));
            $loading.start('main');
            $http({
                method: 'POST',
                data: submitData,
                url: apiEndpoint
            }).then(function successCallback(response) {
                $loading.finish('main');
                // this callback will be called asynchronously
                // when the response is available
                if (response.data.code == '200') {

                    $loading.finish("main");
                    var message = response.data.message;
                    if($.trim(message)==""){
                        message = "เพิ่มข้อมูลเรียบร้อยแล้ว"

                    }
                    //success
                    $cookies.put('assessmentStatus_'+refCode,"complete");
                    showPage("thanks-page");


                } else {
                    $loading.finish("main");
                    // {"code":"500","message":"Invalid Input(s)","description":"Some of your input is invalid. Please recheck again.","transactionId":"25b558af-0d26-47ab-b0a2-57ea00e6685a","transactionDateTime":"4/29/2017 10:24:42 PM","data":{"fieldErrors":[{"name":"profileInfo.salutation","message":"Required field must not be null"}]}}

                    var error = response.data ; //JSON.parse()

                    console.log(error.data);
                    var message = "Server Error";
                    var title = "Server Error";
                    if(error.code=="500"){
                        if(error.data.message){
                            message +=error.data.data.message;
                        }

                    }else {
                        title += ":"+ error.code

                        if(error.message){
                            message = error.message;

                        }
                        if(error.description){
                            message = error.message+":"+error.description;
                        }

                    }

                    dialog.alert({
                        title: title,
                        content: message
                    });
                    showPage("home-page");
                }
                console.log(response)
            }, function errorCallback(response) {
                $loading.finish("main");
                dialog.alert({
                    title: "Server Error",
                    content: response.statusText
                });
                showPage("home-page");
            });
        }



    }
    $("textarea").on("focus",function () {
        $("#page-footer").hide();
        $(".rcorners1").height(50);

    })
    $("textarea").on("blur",function () {
        $("#page-footer").show();
        $(".rcorners1").height(100);
    })


    function resize() {

         $("body").append("<div id='screenInfo' class='screenInfo'></div>");
         $("#screenInfo").text(window.innerWidth+"x"+window.innerHeight);
        //alert(($("body").height()));

       // $("#thanks-page-body").height(screen.height-(screen.height*0.4));
        //$("#thanks-page-body-left-panel").height($("#thanks-page-body").height());
        //$("#thanks-page-body-right-panel").height($("#thanks-page-body").height());


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

        if(window.innerHeight < window.innerWidth){

            //$("#thanks-page-body-link").css("margin-top",$("#thanks-page-body").height()-60);

            $("#thanks-page-body,#thanks-page-body-left-panel,#thanks-page-body-right-panel").css({
                "height": "95%"
            });
            $("#img-thanks-img").height(100);
            $("#img-thanks-img").width(100);
            document.getElementById("img-deves-line").style.marginTop = "10px !important";
            $("#img-deves-line").css("cssText", "margin-top: 100px !important;");
            //$("#img-deves-line").css('margin-top',"10px","!important");


           // $("#thanks-page-body-link img").height(80);
            //$("#thanks-page-body-link img").width(80);

        }else{
            //$("#thanks-page-body-link").css("margin-top",$("#thanks-page-body").height()-100);

            //$("#thanks-page-body-link img").height(80);
            //$("#thanks-page-body-link img").width(80);
        }




    }
    $(window).resize(resize);
    resize();
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




}]);