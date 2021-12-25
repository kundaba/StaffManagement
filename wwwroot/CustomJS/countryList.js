
let methods = {
    
    getNationalities : function (){

        let ajaxCallSettings = {
            type: 'GET',
            url: 'https://www.universal-tutorial.com/api/getaccesstoken',
            headers: {
                Accept: "application/json",
                'api-token': '_IrP7GRlufJR-v_bLDb06btc9oSrThOakwsGNbhlWBpliJxIq03rbwkRx0XU1fptmkM',
                'user-email': 'barnabas.kunda2018@gmail.com'
            }
        };
        
        let apiToken = localStorage.getItem('nationalityApiAuthToken');     //get api-token from local storage

        if (apiToken !== '') {
            methods.getCountries(apiToken, ajaxCallSettings);
        }
        methods.generateAuthToken(ajaxCallSettings);
    },
    
    generateAuthToken : function (ajaxCallSettings){

        $.ajax(ajaxCallSettings)
            .done(function (response) {

                if (response.hasOwnProperty('auth_token')) {
                    localStorage.setItem('nationalityApiAuthToken', response.auth_token);
                    methods.getCountries(response.auth_token, ajaxCallSettings);
                }
            })
            .fail(function (err) {
                console.log(err);
            })
    },
    
    getCountries: function (apiToken, apiTokenAjaxCallSettings){

        let ajaxCallSettings = {
            type: 'GET',
            url: 'https://www.universal-tutorial.com/api/countries/',
            headers: {
                'Authorization': "Bearer " + apiToken,
                Accept: 'application/json'
            }
        };

        $.ajax(ajaxCallSettings)
            .done(function (response) {

                if (response.hasOwnProperty('status') && response.status === 500 || response.hasOwnProperty('error')) {
                    methods.generateAuthToken(apiTokenAjaxCallSettings);
                }
                let countriesList = [];
                
                $.each(response, function (index, item){
                    
                    let countryName = item.country_name;
                    let countryCode = item.country_short_name;
                    let countryPhoneCode = item.country_phone_code;
                    
                    let object = {
                        CountryCode: $.trim(countryCode),
                        CountryName : $.trim(countryName),
                        CountryPhoneCode : Number($.trim(countryPhoneCode))
                    };
                    countriesList.push(object);
                });
                
               if(countriesList.length > 0){
                   console.log('---VALUES FROM THE ARRAY---');
                   console.log(countriesList);
               }
            })
            .fail(function (err) {
                console.log(err)
                let msg = err.hasOwnProperty('responseJSON') ? err.responseJSON.message.body : 'failed to retrieve countries';
                    swal({
                        type: 'error',
                        title: 'ERROR',
                        text: msg
                    })
            })
    },
    
    
}

$(document).ready(function (){
    methods.getNationalities();
})