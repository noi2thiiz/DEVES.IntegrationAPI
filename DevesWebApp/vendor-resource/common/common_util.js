function AppCommonUtil(endpoint) {
    return {
        openWindow:function (opt) {
            var cfg  =  $.extend({
                // data sources
                iframe: true,
                inline: false,
                href: "",
                title:"",
                open:true,
                // callbacks
                onOpen: false,
                onLoad: false,
                onComplete: false,
                onCleanup: false,
                onClosed: false,
            },opt);

            $.colorbox(cfg);
        },
        alert:function (opt) {
            var $ = window.top.$;
            console.log($("body").html);
            var option = {};
            setTimeout(function () {
                if(typeof opt =="string"){
                    option = {
                        content:opt,
                        msgBoxImagePath:endpoint+"/msgbox"
                    }
                }
                var cfg = $.extend({
                    title: "Alert box",
                    content: "message",
                    type: "alert",
                    msgBoxImagePath:endpoint+"/msgbox"
                }, option);


                $(".msgBox").remove();
                $(".msgBoxBackGround").remove();


                $.msgBox(cfg);


            }, 10);
        },
        error:function (opt) {
            var $ = window.top.$;
           if(typeof  opt == undefined){
               opt = "เกิดข้อผิดพลาดในการรับส่งข้อมูล กรุณาลองใหม่อีกครั้ง หรือติดต่อผู้ดูแลระบบ";
           }
            var option = {};
            setTimeout(function () {
                if(typeof opt =="string"){
                    option = {
                        content:opt,
                        msgBoxImagePath:endpoint+"/msgbox",
                        type: "error",
                    }
                }
                var cfg = $.extend({
                    title: "Error",
                    content: "message",
                    type: "error",
                    msgBoxImagePath:endpoint+"/msgbox"
                }, option);


                $(".msgBox").remove();
                $(".msgBoxBackGround").remove();


                $.msgBox(cfg);


            }, 10);
        },
        ajax:function (opt) {

            var cfg  =  $.extend({
                crossDomain: true,
                processData: false,
                method: "post",
                headers: {
                    Accept: "application/json",
                    "Content-Type": "application/json; charset=utf-8",
                    "Access-Control-Allow-Origin": '*'
                }
            },opt);

            if(cfg.data  && typeof cfg.data != "string"){
                cfg.data = JSON.stringify(cfg.data);
            }



            return $.ajax(cfg)
        }
    }
};