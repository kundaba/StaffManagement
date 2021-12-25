
let allPositionCodes = $('.allPositionCodes');
allPositionCodes.val('');
allPositionCodes.autocomplete({
    source: function (request, response) {
        $.ajax({
            url: '/PositionCodes/SearchPositionCodes',
            dataType: "json",
            method: "POST",
            data: {
                searchTerm: allPositionCodes.val(),
                status: 'ALL'
            },
            success: function (data) {
                if (data.length > 0) {
                    response(data);
                }
                else {
                    id.val('');
                    response([{ label: 'position code not found.' }]);
                }
            },
            error: function (xhr) {
                console.log(xhr);
            },
        });
    },
    min_length: 2,
    appendTo: ('.searchDiv')
});