app.controller('mainController', ['$scope', '$rootScope', 'dialog', '$loading', '$http', function ($scope, $rootScope, dialog, $loading, $http) {

    $scope.questions = [
        {
            id:1,
            title:"กริยามารยาทและการให้คำแนะนำคำปรึกษาของเจ้าหน้าที่รับแจ้งอุบัติเหตุทางโทรศัพท์",
            value:0

        },
        {
            id:2,
            title:"กริยามารยาทและการให้คำแนะนำคำปรึกษาของเจ้าหน้าที่สำรวจภัย",
            value:0

        },
        {
            id:3,
            title:"ระยะเวลาที่เจ้าหน้าที่สำรวจภัยเดินทางถึงที่เกิดเหตุ",
            value:0

        }


    ];
    $scope.garageQuestions = [
        {
            id:"1",
            title:"การให้บริการและการต้อนรับของอู่",
            value:0,
            sub:[
                {
                    id:"1.1",
                    title:"เจ้าหน้าที่อู่ให้บริการอย่างมืออาชีพ",
                    value:0
                },
                {
                    id:"1.2",
                    title:"สถานที่ของอู่ได้มาตรฐาน",
                    value:0
                },
            ]
        },
        {
            id:"2",
            title:"การนัดหมายรถเสร็จ",
            value:0,
            sub:[
                {
                    id:"2.1",
                    title:"ระยะเวลาที่ใช้ในการจัดซ่อมเหมาะสม",
                    value:0
                },
                {
                    id:"2.2",
                    title:"การนัดหมายและรักษาเวลาอย่างมืออาชีพ",
                    value:0
                }
            ]
        },
        {
            id:"3",
            title:"ผลงานการจัดซ่อม",
            value:0,
            sub:[
                {
                    id:"3.1",
                    title:"ความเรียบร้อยของงานซ่อมและความสะอาดตอนส่งมอบได้มาตรฐาน",
                    value:0
                },
                {
                    id:"3.2",
                    title:"คุณภาพของการซ่อมสีได้มาตรฐาน",
                    value:0
                }
            ]
        },

        {
            id:"4",
            title:"การจัดซ่อม",
            value:0,
            sub:[
                {
                    id:"4.1",
                    title:"ผลการซ่อมตัวถังและการทำสี",
                    value:0
                },
                {
                    id:"4.2",
                    title:"ผลการซ่อมช่วงล่างและเครื่องยนต์",
                    value:0
                },
                {
                    id:"4.3",
                    title:"การติดตามและดูแลการซ่อมของเจ้าหน้าที่",
                    value:0
                }
            ]
        }


    ];
    $scope.remarkOption=[
        {title:"ชมเชยพนักงานรับแจ้งอุบัติเหตุ",value:0},
        {title:"ชมเชยเจ้าหน้าที่สำรวจภัย",value:0},
        {title:"ชมเชยอู่นี้เทเวศเชียร์",value:0},
        {title:"ปัญหาเจ้าหน้าที่รับแจ้งอุบัติเหตุ",value:0},
        {title:"ปัญหาเจ้าหน้าที่สำรวจภัย",value:0},
        {title:"ปัญหาอู่นี้เทเวศเชียร์",value:0},
        {title:"ปัญหาสาขา",value:0},
        {title:"ประเมินประเภท 2 ประเภท 3",value:0},
        {title:"เคลม พรบ.",value:0},
        {title:"ติดต่อลูกค้าไม่ได้",value:0},
        {title:"อื่น ๆ",value:0}
    ]
    $scope.saveOfficer = function () {
        dialog.confirm({
            title: "ยืนยันบันทึก",
            content: "คุณต้องการบันทึกหรือไม่"
        });
    }
    $scope.saveGarage = function () {
        dialog.confirm({
            title: "ยืนยันบันทึก",
            content: "คุณต้องการบันทึกหรือไม่"
        });
    }

}]);