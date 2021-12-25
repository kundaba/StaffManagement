
let showFormulaCreationModal = function (){
    $('#FormulaCreationModal').modal({backdrop: 'static', keyboard: false});
}

let createFormula = function ($this){
    
    let rate = Number($('.rate').val());
    let units = $('#units').val();
    let operation = $.trim($this);
    let constructedFormula = $('.constructedFormula'); 
    let formula = '';
    
    if(rate <= 0 ){
        swal({type: 'error', title: 'ERROR', text: 'Rate can not be less than 0'});
        return;
    }
    if(operation === ''){
        swal({type: 'error', title: 'ERROR', text: 'Please select the operation'});
        return;
    }
    
    if(units === '' || units === null){
        constructedFormula.val(rate +' '+'x'+ ' '+'BASIC PAY');
    }
    else {
        formula = formula +' '+ operation+ ' '+ units;
        constructedFormula.val(rate+' '+formula +' '+'x'+ ' '+'BASIC PAY');    
    }

}

let saveFormula = function (){
    let formula = $.trim($('.constructedFormula').val());
    
    if(formula === ''){
        swal({type: 'warning', title: 'There is nothing to save', text: ''});
        return;   
    }
    $('#FormulaCreationModal').modal('hide');
    let formulaField = $('.formula');
    formulaField.val('');
    formulaField.val(formula);
}