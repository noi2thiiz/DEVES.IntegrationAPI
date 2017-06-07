
app.run(['$window', '$location', '$loading', '$http', function ($window, $location, $loading, $http) {
    //ป้องกันการเปิด link โดยตร


    // $loading.start('main');
    //calibrate ui size
    var resize = function () {
         $("#main-container").height($(window).innerHeight()-60);
         $("#main-container").css("overflow","scroll");
    }
    $(window).resize(resize);
    resize();
    $("#main-container").show();

}]);
app.controller('mainController', ['$scope', 'dialog', '$loading', '$http','$q', function ($scope, dialog, $loading, $http,$q) {
    var vm = this;
    vm.deferred = $q.defer();
    vm.deferred.resolve();

    $scope.questions = [
        {
            title:" ข้อ 1/3",
            group:1,
            subQuestions: [
                {id:1, group:2,title: "1) การบริการของเจ้าหน้าที่รับแจ้งอุบัติเหตุ มีความพึงพอใจระดับใด",value:0}

           ]
        },
        {
            title:" ข้อ 2/3",
            group:2,
            subQuestions: [
                {id:1, group:2,title: "2)  การเดินทางถึงที่เกิดเหตุของเจ้าหน้าที่สำรวจภัย มีความพึงพอใจระดับใด",value:0}

            ]
        },
        {
            title:"ข้อ 3/3",
            group:3,
            subQuestions: [
                {id:1,group:3,title: "3) การบริการและความรวดเร็วในการช่วยเหลือท่านของเจ้าหน้าที่สำรวจภัย มีความพึงพอใจระดับใด" ,value:0}
            ]
        },


    ];


    $("#main-page").attr('disabled',true);






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

        var main = $scope.currentQuestionId ;
        var sub = $scope.currentSubQuestionId + 1;
        if(sub >= $scope.questions[main].subQuestions.length ){
            main+=1;
            sub = 0;
        }else{

        }


        if(main >=  $scope.questions.length){
            $("#question-header").hide();
            $("#question-mobile-form").hide();
            $("#question-mobile-form").css("display","none");
            $("#question-remark-form").show();

        }else{

            $scope.selectQuestion( main,sub);
        }


    }

    $scope.startQuestion = function () {

        //$("#button-start-panel").hide();
        $scope.selectQuestion(0,0);
    }
    $scope.startQuestion();

}]);