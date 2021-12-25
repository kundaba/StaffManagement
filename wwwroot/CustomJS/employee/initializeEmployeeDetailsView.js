$(function () {
    demo.initMaterialWizard();
    checkStatus();

    setTimeout(function () {
        $('.card.card-wizard').addClass('active');
    }, 300);
})

let checkStatus = function () {
    let status = $.trim($('.status').val());
    
    if (status === 'Terminated') {
        $('.amendmentButton').css('display', 'none');
        $('.transferBtn').css('display', 'none');
        $('.terminationBtn').css('display', 'none');
        $('#basicPayHistoryBtn').css('display', 'none');
        $('#basicPay').val(0.0);
        $('.linkEmployeeToPositionBtn').css('display', 'none');
        $('.reinstateBtn').css('display', 'block');
    } else {

        let positionCode = $.trim($('#position').val());
        if (positionCode === '' || positionCode === null) {
            $('.linkEmployeeToPositionBtn').css('display', 'block');
            $('.transferBtn').css('display', 'none');
        } else {
            $('.linkEmployeeToPositionBtn').css('display', 'none');
            $('.transferBtn').css('display', 'block');
        }
        $('.amendmentButton').css('display', 'block');
        $('.terminationBtn').css('display', 'block');
        $('.reinstateBtn').css('display', 'none');
        $('#basicPayHistoryBtn').css('display', 'block');
    }
}

        