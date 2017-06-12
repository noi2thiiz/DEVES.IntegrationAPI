﻿<html lang="en" data-ng-app="crmApp"><head><meta charset="UTF-8"/><meta http-equiv="X-UA-Compatible" content="IE=edge"/><meta name="viewport" content="width=device-width, initial-scale=1"/><!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags--><meta name="description" content=""/><meta name="author" content=""/><title>DEVES-แบบสอบถามความพึงพอใจ</title><link rel="apple-touch-icon" sizes="57x57" href="img/apple-icon-57x57.png"/><link rel="apple-touch-icon" sizes="60x60" href="img/apple-icon-60x60.png"/><link rel="apple-touch-icon" sizes="72x72" href="img/apple-icon-72x72.png"/><link rel="apple-touch-icon" sizes="76x76" href="img/apple-icon-76x76.png"/><link rel="apple-touch-icon" sizes="114x114" href="img/apple-icon-114x114.png"/><link rel="apple-touch-icon" sizes="120x120" href="img/apple-icon-120x120.png"/><link rel="apple-touch-icon" sizes="144x144" href="img/apple-icon-144x144.png"/><link rel="apple-touch-icon" sizes="152x152" href="img/apple-icon-152x152.png"/><link rel="apple-touch-icon" sizes="180x180" href="img/apple-icon-180x180.png"/><link rel="icon" type="image/png" sizes="192x192" href="img/android-icon-192x192.png"/><link rel="icon" type="image/png" sizes="32x32" href="img/favicon-32x32.png"/><link rel="icon" type="image/png" sizes="96x96" href="img/favicon-96x96.png"/><link rel="icon" type="image/png" sizes="16x16" href="img/favicon-16x16.png"/><link rel="manifest" href="img/manifest.json"/><meta name="msapplication-TileColor" content="#ffffff"/><meta name="msapplication-TileImage" content="img/ms-icon-144x144.png"/><meta name="theme-color" content="#ffffff"/><!-- Bootstrap core CSS--><link href="assets/bootstrap/css/bootstrap.css" rel="stylesheet"/><link href="assets/bootstrap/css/bootstrap-theme.css" rel="stylesheet"/><!-- IE10 viewport hack for Surface/desktop Windows 8 bug--><link href="assets/css/ie10-viewport-bug-workaround.css" rel="stylesheet"/><!-- Custom styles for this template--><link href="assets/css/sticky-footer.css" rel="stylesheet"/><link rel="stylesheet" href="assets/resource/pfc_angular.css" crossorigin="anonymous"/><link href="assets/css/style.css" rel="stylesheet"/><!-- Just for debugging purposes. Don't actually copy these 2 lines!--><!--if lt IE 9script(src='../../assets/js/ie8-responsive-file-warning.js')--><script src="assets/js/ie-emulation-modes-warning.js"></script><!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries--><!--if lt IE 9script(src='https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js')
    script(src='https://oss.maxcdn.com/respond/1.4.2/respond.min.js')--></head><body ng-controller="mainController as vm" dw-loading="main" dw-loading-options="{text:'Processing...', spinner: false}" width="100%" style="margin: 0; mso-line-height-rule: exactly; "><!-- Begin page content--><div class="container rcorners0"><div class="container header-container" id="main-container" style="display:none"><section id="home-page" style="display:none"><div class="page-header" style="height:50px"></div><div class="page-body"><a data-ng-click="startQuestion()"><img id="home-img-mobile" src="img/deves/home.jpg" style="width:100%;background-color: white"/><img id="home-img-full" src="img/deves/Questionaire1_Greeting.jpg" style="width:100%;background-color: white;display:none"/></a></div></section><section id="question-page" style="display:none"><div class="page-header text-center"><div class="circleBase type2" ng-repeat="q in questions" data-id="circle-{{q.id}}"></div><div class="clearfix"></div></div><div class="page-body"><div class="row rcorners1"><div class="col-sm-12"><h4>{{currentQuestion.title}}</h4></div><div class="col-sm-12"><h4>{{currentSubQuestion.title}}</h4></div></div><div class="row rcorners2"><div class="col-sm-12 text-left sub-question-title"><h5>ระดับความพึงพอใจ</h5></div><div class="col-sm-12" dw-loading="options" dw-loading-options="{text:'Loading...', spinner: false}"><div class="list-group"><button class="list-group-item text-left" data-option-id="{{currentSubQuestion.id}}" type="button" data-ng-click="selectQuestionValue(currentSubQuestion,1)" ng-class="{'list-group-item-warning':currentSubQuestion.value==1}"><i class="glyphicon glyphicon-unchecked" ng-if="currentSubQuestion.value!=1"></i><i class="glyphicon glyphicon-check" ng-if="currentSubQuestion.value==1"></i> มากที่สุด</button><button class="list-group-item text-left" data-option-id="{{currentSubQuestion.id}}" type="button" data-ng-click="selectQuestionValue(currentSubQuestion,2)" ng-class="{'list-group-item-warning':currentSubQuestion.value==2}"><i class="glyphicon glyphicon-unchecked" ng-if="currentSubQuestion.value!=2"></i><i class="glyphicon glyphicon-check" ng-if="currentSubQuestion.value==2"></i> มาก</button><button class="list-group-item text-left" data-option-id="{{currentSubQuestion.id}}" type="button" data-ng-click="selectQuestionValue(currentSubQuestion,3)" ng-class="{'list-group-item-warning':currentSubQuestion.value==3}"><i class="glyphicon glyphicon-unchecked" ng-if="currentSubQuestion.value!=3"></i><i class="glyphicon glyphicon-check" ng-if="currentSubQuestion.value==3"></i> ปานกลาง</button><button class="list-group-item text-left" data-option-id="{{currentSubQuestion.id}}" type="button" data-ng-click="selectQuestionValue(currentSubQuestion,4)" ng-class="{'list-group-item-warning':currentSubQuestion.value==4}"><i class="glyphicon glyphicon-unchecked" ng-if="currentSubQuestion.value!=4"></i><i class="glyphicon glyphicon-check" ng-if="currentSubQuestion.value==4"></i> น้อย</button><button class="list-group-item text-left" data-option-id="{{currentSubQuestion.id}}" type="button" data-ng-click="selectQuestionValue(currentSubQuestion,5)" ng-class="{'list-group-item-warning':currentSubQuestion.value==5}"><i class="glyphicon glyphicon-unchecked" ng-if="currentSubQuestion.value!=5"></i><i class="glyphicon glyphicon-check" ng-if="currentSubQuestion.value==5"></i> น้อยที่สุด</button></div></div></div></div></section><section id="question-remark-form" style="display:none"><div class="page-header text-center"><div class="circleBase type1" ng-repeat="q in questions"></div><div class="clearfix"></div></div><div class="page-body"><div class="row rcorners1"><div class="col-sm-12"><h4>ข้อแนะนำเพิ่มเติม</h4></div></div><div class="row rcorners2"><div class="col-sm-12 text-left"><h5></h5><textarea id="textarea-comment" rows="10" style="width:100%" autofocus="autofocus" ng-model="comment"></textarea><div class="clearfix"></div></div><div class="col-sm-12 text-center" style="margin-top:20px"><a class="btn btn-black btn-lg" id="btn-submitform" ng-click="submitQuestionResult()"><span class="button-link" style="color:#ffffff">ส่งคำตอบ </span></a></div></div></div></section><section id="thanks-page" style="display:none"><div class="page-header"></div><div class="page-body" id="thanks-page-body"><div class="row"><div class="col-xs-8 text-center" id="thanks-page-body-left-panel"><div class="row"><div class="vertical-center"><img id="img-thanks-img" src="img/deves/thanks-img.png" style="width:95%"/><div id="deves-icon-container"><a id="btn-thanks1" href="http://www.deves.co.th/service/GarageCheer.aspx"><img class="deves-icon" src="img/deves/btn1_2.png" style="width:auto;height:150px"/></a><a id="btn-thanks2" href="http://www.deves.co.th/service/Cgarage.aspx"><img class="deves-icon" src="img/deves/btn2_2.png" style="width:auto;height:150px"/></a><a id="btn-thanks3" href="http://www.deves.co.th/Content/View/303"><img class="deves-icon" src="img/deves/btn3_2.png" style="width:7auto;height:150px"/></a></div></div></div></div><div class="col-xs-4" id="thanks-page-body-right-panel"><img id="img-deves-logo" src="img/deves/deves_logo.png" style="height:70px"/><img id="img-deves-line" src="img/deves/deves_line.png" style="width:98%;height:auto"/><a href="https://goo.gl/nrTbtA"><img id="img-deves-line-add-friends" src="img/deves/icon-add-friends.jpg" style="width:90%;height:auto"/></a><img id="img-deves-socail" src="img/deves/social.png" style="height:100px"/><!--#thanks-page-body-social
    img(src="img/deves/social.png",style="width:150px; margin-right:50px;")
    --></div></div></div></section></div></div><footer class="footer" id="page-footer"><div class="container"><div class="row" style="max-height:100px"><div class="col-xs-6"><img class="float-left" id="footer-deves-logo" src="img/deves-logo.png"/></div><div class="col-xs-6"><img class="float-right" id="footer-deves-line-qr" src="img/deves-line-qr.png" style="float: right !important"/></div></div></div></footer><!-- IE10 viewport hack for Surface/desktop Windows 8 bug--><script src="assets/js/ie10-viewport-bug-workaround.js"></script><script src="assets/resource/pfc_jquery.js"></script><script src="assets/resource/pfc_angular.js"></script><script src="assets/resource/pfc_angular_modules.js"></script><script src="assets/js/config.js?v=12-6-1560"></script><script src="assets/js/main-controller.js?v=12-6-1560"></script></body></html>