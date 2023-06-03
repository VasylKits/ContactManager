$(document).ready(function () {
    // Sort table by column
    $('th').click(function () {
        var table = $('#contact-table');
        var index = $(this).index();
        var rows = table.find('tbody tr').toArray().sort(comparer(index));

        $(this).siblings().removeClass('asc desc');
        if ($(this).hasClass('asc')) {
            $(this).removeClass('asc').addClass('desc');
            rows = rows.reverse();
        } else {
            $(this).removeClass('desc').addClass('asc');
        }

        for (var i = 0; i < rows.length; i++) {
            table.append(rows[i]);
        }
    });

    // Filter table by column
    $('.filter-input').on('input', function () {
        var table = $(this).closest('table');
        var index = $(this).closest('th').index();
        var filterText = $(this).val().toLowerCase();

        table.find('tbody tr').each(function () {
            var cellText = $(this).find('td').eq(index).text().toLowerCase();
            $(this).toggle(cellText.includes(filterText));
        });
    });
});

function comparer(index) {
    return function (a, b) {
        var valA = getCellValue(a, index), valB = getCellValue(b, index);
        return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : valA.toString().localeCompare(valB);
    };
}

function getCellValue(row, index) {
    return $(row).children('td').eq(index).text();
}
