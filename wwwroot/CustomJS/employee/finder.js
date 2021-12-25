
let customMethods ={
    
    openModalOnLoad : function (){
        $("#finder-modal").modal('show');
    },
    
    getOperations : function (){
        return [
            {value: '=', name: 'Equals'},
            {value: '>', name: 'Greater Than'},
            {value: '>=', name: 'Greater or Equal'},
            {value: '<', name: 'Less Than '},
            {value: '<=', name: 'Less or Equal'},
        ];
    },
}

$(document).ready(function(){
    customMethods.openModalOnLoad();
})
