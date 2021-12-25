
let documentUploadMethods = {
    
    getQualificationTypes :function (){

        let qualificationType = ['School_Certificate','Trade_Certificate','Diploma','Bsc','Beng','B.Art','Msc','MBA','MEng', 'PhD'];
        $.each(qualificationType, function (i, item) {
            $('#qualificationType').append(`<option value = ${item} > ${item}</option >`).selectpicker('refresh');
        });
    } ,
    
    getEmployeeQualifications : function (employeeCode){

        $.post('/EmployeeQualifications/GetEmployeeQualifications', {employeeCode: employeeCode})
            .done(function (response) {
                let data = response;

                if (data !== null) {

                    let tableBody = $('.qualificationDocumentsTbody');
                    tableBody.empty();
                    
                    $.each(data, function (index, item) {
                        
                        let guid = item.guId;
                        
                        tableBody.append(`<tr class=''>
                        <td class="d-none">${guid} </td>
                        <td>${item.qualificationType} </td>
                        <td>${item.fieldOfStudy} </td>
                        <td>${new Date(item.startDate).toDateString()} </td>
                        <td>${new Date(item.endDate).toDateString()} </td>
                        <td>${item.expiryStatus}</td>
                        <td>
                        <a href="/EmployeeQualifications/DownloadQualificationDocument?documentGuid=${guid}"><i class="fa fa-download"></i></a>                           
                        </td>
                        </tr>`);
                    });
                }
            })
            .fail(function (error) {
                console.log(error);
            });
    },
    
    uploadQualifications : function (formData){

        $('.btn').prop('disabled', true);

        $.ajax({
            url: '/EmployeeQualifications/UploadQualifications',
            method: 'POST',
            contentType: false,
            processData: false,
            data: formData,

            success: function (response) {

                if(!response.success){
                    $('.btn').prop('disabled', false);
                    swal({title: 'ERROR', text: response.message, type: 'error'});
                    return;
                }
                swAlert(response);
            },
            error: function (xhr, i, exception) {
                $('.btn').prop('disabled', false);
                console.log(exception);
                swal({
                    title: 'ERROR',
                    text: exception,
                    type: 'error',
                    buttonsStyling: false,
                    confirmButtonClass: 'btn btn-info okBtn'
                });
            },
        });
    }
  
};

$(function (){

    initialiseDatePickerByClass('docDate');
    documentUploadMethods.getQualificationTypes();
    documentUploadMethods.getEmployeeQualifications($.trim($('.employeeCode').val()));

    $('.addBtn').on('click', function () {

        $('.qualificationUploadTable').append("<tr class='qualificationUploadTableRow'>" +
            "<td>" +
            "<label>Document Type <span class='text-danger'>*</span></label>"+
            "<select class='form-control documentType' id='documentType' data-live-search='true'>"+
            "<option value='' selected disabled>Select Doc Type </option>"+
            "<option value='Academic'>Academic </option>"+
            "<option value='Non-Academic'>Non-Academic </option>"+
            "</select>"+
            "</td>"+
            "<td class='qualificationType' style='display: none'>" +
            "<label></label>"+
            "<select class='form-control qualificationType' id='qualificationType' data-live-search='true'>"+
            "<option value='' selected disabled>Select type </option>"+
            "<option value='School Certificate'>School Certificate </option>"+
            "<option value='Trade Certificate'>Trade Certificate </option>"+
            "<option value='Diploma'>Diploma </option>"+
            "<option value='Bsc'>Bsc </option>"+
            "<option value='Beng'>Beng </option>"+
            "<option value='B.Art'>B.Art </option>"+
            "<option value='Msc'>Msc </option>"+
            "<option value='MBA'>MBA </option>"+
            "<option value='MEng'>MEng </option>"+
            "<option value='PhD'>PhD </option>"+
            "</select>"+
            "</td>" +
            "<td class='fieldOfStudy' style='display: none'>" +
            "<label for='fieldOfStudy' class='block'>" +
            "</label>" +
            "<input id='fieldOfStudy' type='text' class='form-control fieldOfStudy'" +
            " minlength='5' maxlength='20' placeholder='Enter field of study'/>" +
            "</td>" +
            "<td class='startDate'>" +
            "<label for='startDate' class='block'>" +
            "<span class='text-danger'></span>" +
            "</label>" +
            "<input id='startDate' type='text' class='form-control docDate'" +
            " placeholder='Enter date of issue'" +
            " required/>" +
            "</td>" +
            "<td class='endDate'>" +
            "<label for='endDate' class='block'>" +
            "<span class='text-danger'></span>" +
            "</label>" +
            "<input id='endDate' type='text' class='form-control docDate'" +
            " placeholder='Enter expiry date' " +
            "required/>" +
            "</td>" +
            "<td class='document'>" +
            "<label for='document' class='block'>" +
            "<span class='text-danger'></span>" +
            "</label>" +
            "<input id='document' type='file' name='document' class='form-control document'" +
            " placeholder='select file'" +
            " required/>" +
            "</td>" +
            "<td>" +
            "<button class='btn btn-danger btn-sm removeBtn' style='margin-top: 30px'><i class='material-icons' title='Remove'>close</i>Remove</button>" +
            "</td>" +
            "</tr>");
        initialiseDatePickerByClass('docDate');
    });
    
    $(document).on('click', '.removeBtn', function () {
        let currentRow = $(this).closest('tr');
        currentRow.remove();
    });
    
    $(document).on('change', '.documentType', function () {
        if($(this).val() ==='Academic'){
            $('.qualificationType').css('display','block');
            $('.fieldOfStudy').css('display','block');
        }
        else {
            $('.qualificationType').css('display','none');
            $('.fieldOfStudy').css('display','none');
        }
    });
    
})

let loadQualificationUploadModal = function (){
    $('#qualificationUploadModal').modal({backdrop: 'static', keyboard: false});
}

let uploadQualifications = function(){

    let documentType = [];
    let qualificationType = [];
    let fieldOfStudy = [];
    let startDate = [];
    let endDate = [];

    let employeeCode = $.trim($('.employeeCode').val());
    $('#qualificationUploadModal').modal('hide');

    if (employeeCode === null || employeeCode === '') {
        swal({
            title: 'ERROR',
            type: 'error',
            text: 'Employee Code not provided'
        }).then(function () {
            $('#qualificationUploadModal').modal('show');
        });
        return
    }

    $('.qualificationUploadTable tr.qualificationUploadTableRow').each(function () {

        let docType = $.trim($(this).closest('tr').find('td:eq(0)').find(':input').val());
        let qualifType = $.trim($(this).closest('tr').find('td:eq(1)').find(':input').val());
        let areaOfStudy = $.trim($(this).closest('tr').find('td:eq(2)').find(':input').val());
        let dateOfIssue = $(this).closest('tr').find('td:eq(3)').find(':input').val();
        let expiryDate = $(this).closest('tr').find('td:eq(4)').find(':input').val();
        let doc = $(this).closest('tr').find('td:eq(5)').find(':input').val();

        if (docType !== '' && dateOfIssue !== '' && areaOfStudy !== '' && doc !== null) {

            expiryDate = expiryDate === null || expiryDate === '' ? expiryDate : new Date(expiryDate).toDateString();
            documentType.push(docType);
            qualificationType.push(qualifType);
            fieldOfStudy.push(areaOfStudy);
            startDate.push(new Date(dateOfIssue).toDateString());
            endDate.push(expiryDate);
        }
    });

    if (documentType.length === 0 || qualificationType.length === 0 || fieldOfStudy.length === 0 || startDate.length === 0 || document.length === 0) {
        swal({
            title: 'ERROR',
            type: 'error',
            text: 'Please provide data for all the fields'
        }).then(function () {
            $('#qualificationUploadModal').modal('show');
        });
        return;
    }
    
    let formData = new FormData();
    
    $('input[name="document"]').each(function (a, b) {
        let fileInput = $('input[name="document"]')[a];
        let files = fileInput.files[a];
        formData.append("Document", files);
    });

    formData.append("DocumentType", documentType);
    formData.append("QualificationType", qualificationType);
    formData.append("FieldOfStudy", fieldOfStudy);
    formData.append("StartDate", startDate);
    formData.append("EndDate", endDate);
    formData.append("EmployeeCode", employeeCode);
    formData.append("ActionType", 'Update');
    
    swal({
        title: 'Are you sure you want to upload this document(s)?',
        text: 'Note that this action is irreversible',
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: 'btn btn-success confirmationBtn',
        cancelButtonClass: 'btn btn-danger',
        confirmButtonText: "Yes",
    });
    $('.confirmationBtn').click(function () {
        documentUploadMethods.uploadQualifications(formData);
    });
  
}
