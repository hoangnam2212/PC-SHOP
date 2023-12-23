var optionDatatable = {
    "language": {
        "url": "/font-awesome/Vietnamese.json"
    }
};
var optionDatatableOrder = {
    "language": {
        "url": "/font-awesome/Vietnamese.json"
    },
    "pageLength": 100
};
var simpleDataTable = {
    "searching": false,
    "paging": false,
    "language": {
        "url": "/font-awesome/Vietnamese.json"
    },
    "info": false
}
var noneOptionDataTable = {
    "paging": true,
    "searching": false,
    "ordering": false,
    "info": false,
    "language": {
        "url": "/font-awesome/Vietnamese.json"
    }
}
var lstEmpDataTable = {
    "paging": false,
    "language": {
        "url": "/font-awesome/Vietnamese.json"
    },
    "ordering": false,
    "lengthChange": false,
}
var normalOptionDataTable = {
    "paging": true,
    "searching": true,
    "order": false,
    "info": true,
    "pageLength": 25,
    "language": {
        "url": "/font-awesome/Vietnamese.json"
    }
}

function init_daterangepicker_byId(id, align = 'right') {

    if (typeof ($.fn.daterangepicker) === 'undefined') { return; }

    var cb = function (start, end, label) {
        $('#' + id + ' span').html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));
    };

    var optionSet1 = {
        startDate: moment(),
        endDate: moment(),
        minDate: '01/01/2007',
        maxDate: '12/31/2025',
        dateLimit: {
            days: 60
        },
        showDropdowns: true,
        showWeekNumbers: true,
        timePicker: false,
        timePickerIncrement: 1,
        timePicker12Hour: true,
        ranges: {
            'Hôm nay': [moment(), moment()],
            'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Ngày mai': [moment().add(1, 'days'), moment().add(1, 'days')],
            '7 ngày trước': [moment().subtract(6, 'days'), moment()],
            'Tháng này': [moment().startOf('month'), moment().endOf('month')],
            //'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        opens: align,
        buttonClasses: ['btn btn-default'],
        applyClass: 'btn-small btn-primary',
        cancelClass: 'btn-small',
        format: 'DD/MM/YYYY',
        separator: ' to ',
        locale: {
            applyLabel: 'Chọn',
            cancelLabel: 'Xóa',
            fromLabel: 'Từ',
            toLabel: 'Đến',
            customRangeLabel: 'Ngày khác',
            daysOfWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
            monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
            firstDay: 1
        }
    };

    $('#' + id + ' span').html(moment().format('DD/MM/YYYY') + ' - ' + moment().format('DD/MM/YYYY'));

    $('#' + id).daterangepicker(optionSet1, cb);

    $('#options1').click(function () {
        $('#' + id).data('daterangepicker').setOptions(optionSet1, cb);
    });

    $('#options2').click(function () {
        $('#' + id).data('daterangepicker').setOptions(optionSet2, cb);
    });

    $('#destroy').click(function () {
        $('#' + id).data('daterangepicker').remove();
    });

}