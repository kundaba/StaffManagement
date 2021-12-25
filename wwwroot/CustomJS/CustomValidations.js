
$(document).ready(function () {
    
    $('input[type = text]').keyup(function () {
        
        let regexValues = /^[A-Za-z,0-9 ]+$/;
        
        if ($(this).val().match(regexValues)) {
            return true;
        }
        else {
            $(this).val('');
            return false;
        }
    });
})