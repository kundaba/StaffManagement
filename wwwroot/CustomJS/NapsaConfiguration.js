
let methods = {
    
    showModal : function (){
        $('#AddRecordModal').modal({backdrop: 'static', keyboard: false});
    },

    validateNumericValues(value,textField){

        let message = '';
        
        if(textField === 'percentage' && (value < 0 || value > 100)){
            message = 'Please enter a valid percentage value between 1 and 100';
           
            $('#AddRecordModal').modal('hide');
            $("."+textField).val("");

            swal({
                title: 'Error',
                text: message,
                type: 'error',
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info"
            }).then(function () {
                $('#AddRecordModal').modal('show');
            });

        }
        if(textField === 'ceiling' && (value <= 10 || value > 10000)){
            message = 'Please enter a valid ceiling value between 10 and 10000';
            
            $('#AddRecordModal').modal('hide');
            $("."+textField).val("");

            swal({
                title: 'Error',
                text: message,
                type: 'error',
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info"
            }).then(function () {
                $('#AddRecordModal').modal('show');
            });

        }
    },
    
    initializeDatePicker: function (){
        $('.datepicker').datetimepicker({
            format: "MM/DD/YYYY",
            icons: {
                time: 'fa fa-clock-o',
                date: 'fa fa-calendar',
                up: "fa fa-chevron-up",
                down: "fa fa-chevron-down",
                previous: "fa fa-chevron-left",
                today: 'fa fa-screenshot',
                next: "fa fa-chevron-right",
                clear: "fa fa-trash",
                close: "fa fa-remove",
            }
        })
    },
    
    submitRequest : function (data){
        $.post('/NapsaConfiguration/CreateNewLine',{request: data})
            .done(function (response){
                console.log(response);
                swAlert(response);
            })
            .fail(function (error){
                console.log(error);
                alert(error.responseText);
            })
    }
}


$(document).ready(function (){
    
    methods.initializeDatePicker();
    
    $('.addBtn').on('click', function (){
        methods.showModal();
    });

    $('.percentage').on('change',function (){
        methods.validateNumericValues($(this).val(),'percentage');
    });
    
    $('.ceiling').on('change',function (){
        methods.validateNumericValues($(this).val(),'ceiling');
    });

    $('.startDate').on('change', function(){

        let startDate = $(this).val();
        let today = new Date().toDateString();
        
        if((startDate !== '') && (new Date(startDate) < new Date(today))){
            $(this).val('');
            swal({
                title: 'Error',
                text: 'Please note that start date can not be in the past',
                type: "error",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info okBtn"
            });
        }
    });
    
    $('.SubmitBtn').on('click',function (){

        let percentage = Number($('.percentage').val());
        let ceiling = Number($('.ceiling').val());
        let startDate = $.trim($('.startDate').val());

        if (percentage <= 0 || percentage > 100) {
            swal({
                title: 'Error',
                text: 'Please enter a valid percentage value',
                type: "error",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info okBtn"
            });
            return;
        }
        if (ceiling === 0) {
            swal({
                title: 'Error',
                text: 'Please enter a valid ceiling value',
                type: "error",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info okBtn"
            });
            return;
        }
        if (startDate === '') {
            swal({
                title: 'Error',
                text: 'Please provide start date',
                type: "error",
                buttonsStyling: false,
                confirmButtonClass: "btn btn-info okBtn"
            });
        }
        
        else{
            let data = {
                Percentage : percentage,
                MaximumCeiling : ceiling,
                StartDate : new Date(startDate).toISOString()
            };
            methods.submitRequest(data);
        }
    })
})