
let initialiseDatePicker = function (fieldId){
    
    $('#'+fieldId).datetimepicker({
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
    
}

let initialiseDatePickerByClass = function (classId){

    $('.'+classId).datetimepicker({
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

}