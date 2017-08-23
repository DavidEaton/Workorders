$(function () {

    $('.jq-clear-kendo-filters').click(function (ev) {

        ev.preventDefault();

        var grid = $('#' + $(this).data('id')).data().kendoGrid;

        persistGridSettings.call(grid, true);

        loadGridStateFromCookie(grid);
    });


    $('.jq-search-kendo-grid').keyup(function (ev) {

        if (ev.which == 13) {

            var searchTerm = $(this).val();

            var grid = $('#' + $(this).data('id')).data().kendoGrid;
            var dataSource = grid.dataSource;
            var filter = dataSource.filter();

            if (typeof filter == "undefined" || typeof filter.filters == "undefined" || filter.filters.length == 0) {
                filter = {};
                filter.filters = [];

                if (searchTerm != '') {
                    filter.filters.push({ "field": "Details", "operator": "contains", "value": searchTerm });
                }


            } else {
                var result = $.grep(filter.filters, function (e) { return e.field == "Details" && e.operator == "contains"; });
                if (result.length != 0) {

                    $.each(filter.filters, function (i) {

                        if (filter.filters[i].field === 'Details' && filter.filters[i].operator == "contains") {
                            filter.filters.splice(i, 1);
                        }
                    });
                }

                if (searchTerm != '') {
                    filter.filters.push({ "field": "Details", "operator": "contains", "value": searchTerm });
                }

            }
            if (filter.filters.length == 0) {
                filter = '';
            }
            grid.dataSource.query({ "page": 1, "pageSize": dataSource.pageSize(), filter: filter });
        }


    });

    $('.jq-search-kendo-grid-btn').click(function (ev) {
        ev.preventDefault();

        var e = jQuery.Event("keyup");
        e.which = 13;
        $('.jq-search-kendo-grid').trigger(e);
    });

});

function persistGridSettings(clearFilter) {
    var grid = this;
    var gridId = grid._cellId;

    if (grid && grid.dataSource) {
        var dataSource = grid.dataSource;

        var state = kendo.stringify({
            page: 1,
            pageSize: dataSource.pageSize(),
            sort: dataSource.sort(),
            filter: typeof clearFilter == "boolean" && clearFilter == true ? '' : dataSource.filter(),
        });

        var hiddenColumns = [];


        for (var i = 0; i < grid.columns.length; i++) {

            // if (typeof clearFilter != "boolean" && clearFilter != true) {
            if (grid.columns[i].hidden) {
                hiddenColumns.push(grid.columns[i].field);
            }
            // } else {
            //     grid.showColumn(i);
            // }
        }


        setCookie(gridId + "gridHiddenColumns", kendo.stringify(hiddenColumns), 3);
        setCookie(gridId + "gridState", state, 3);

        setFilterString(dataSource.filter(), gridId);
    }

}


function setFilterString(filterObj, gridId) {
    var filterString = '';

    if (filterObj != undefined && typeof filterObj != "undefined" && typeof filterObj.filters != "undefined") {

        for (var i = 0; i < filterObj.filters.length; i++) {
            var filter = filterObj.filters[i];

            if (filter.filters == undefined) {
                //set user friendly filer names
                if (filterString == '') {
                    filterString = '[' + getUserFreindlyFilterName(filter.field) + '] ' + getOperatorSymbol(filter.operator) + ' ' + getUserFriendlyFilterValue(filter.field, filter.value);
                } else {
                    filterString += ' (' + filterObj.logic + ') ' + '[' + getUserFreindlyFilterName(filter.field) + '] ' + getOperatorSymbol(filter.operator) + ' ' + getUserFriendlyFilterValue(filter.field, filter.value);
                }
            } else {
                var nestedFilters = filter.filters;
                var nestedFilterString = '';

                for (var j = 0; j < nestedFilters.length; j++) {

                    var nestedfilter = nestedFilters[j];

                    //set user friendly filer names
                    if (nestedFilterString == '') {

                        if (filterString != '') {
                            nestedFilterString = ' (' + filterObj.logic + ') ' + '[' + getUserFreindlyFilterName(nestedfilter.field) + '] ' + getOperatorSymbol(nestedfilter.operator) + ' ' + getUserFriendlyFilterValue(nestedfilter.field, nestedfilter.value);
                        }
                        else {
                            nestedFilterString = '[' + getUserFreindlyFilterName(nestedfilter.field) + '] ' + getOperatorSymbol(nestedfilter.operator) + ' ' + getUserFriendlyFilterValue(nestedfilter.field, nestedfilter.value);
                        }
                    } else {
                        nestedFilterString += ' (' + filter.logic + ') ' + '[' + getUserFreindlyFilterName(nestedfilter.field) + '] ' + getOperatorSymbol(nestedfilter.operator) + ' ' + getUserFriendlyFilterValue(nestedfilter.field, nestedfilter.value);
                    }
                }

                filterString += nestedFilterString;
            }
        }

        var result = $.grep(filterObj.filters, function (e) { return e.field == "Details" && e.operator == "contains"; });
        if (result.length != 0) {
            $('.jq-search-kendo-grid').val(result[0].value);
        }

    }

    $('#' + gridId.split('_active_')[0]).parents('.grid-parent').find('.jq-kendo-filters').text(filterString);

    if (filterString != '') {
        $('#' + gridId.split('_active_')[0]).parents('.grid-parent').find('.jq-kendo-filters-wrapper').removeClass('hide');
    } else {
        $('#' + gridId.split('_active_')[0]).parents('.grid-parent').find('.jq-kendo-filters-wrapper').addClass('hide');
    }
}


function getUserFriendlyFilterValue(filterName, filterValue) {

    if (filterName == 'Priority') {
        switch (filterValue) {
            case 1:
                return 'High';
            case 2:
                return 'Normal';
            case 3:
                return 'Low';
            default:
                return '';
        }
    }
    return filterValue;
}

function getOperatorSymbol(name) {

    if (name == 'eq') {
        return "=";

    } else if (name == 'neq') {
        return "!=";
    }
    else if (name == 'contains') {
        return "(contains)";
    }
    else if (name == 'doesnotcontain') {
        return "(does not contains)";
    }
    else if (name == 'startswith') {
        return "^=";
    }
    else if (name == 'endswith') {
        return "(ends with)";
    }
}
function getUserFreindlyFilterName(raw) {

    if (raw == 'workOrderType') {
        return 'Status';
    } else if (raw == 'DepartmentAreaName') {
        return 'Area';
    }
    else if (raw == 'DepartmentName') {
        return 'Department';
    }
    else if (raw == 'Priority') {
        return 'Priority';
    }
    else if (raw == 'workOrderDueType') {
        return 'Due Status';
    }
    else if (raw == 'Closer') {
        return 'Closer';
    } else {
        return raw;
    }
}


// set cooke
function setCookie(cookieName, cookieValue, nDays) {

    var today = new Date();
    var expire = new Date();
    if (nDays == null || nDays == 0) nDays = 1;
    expire.setTime(today.getTime() + 3600000 * 24 * nDays);
    document.cookie = cookieName + "=" + escape(cookieValue)
        + ";expires=" + expire.toGMTString();
};


// get cookie
function getCookie(cName) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == cName) {
            return unescape(y);
        }
    }
};

function loadGridStateFromCookie(grid) {
    var gridId = grid._cellId;

    var state = getCookie(gridId + "gridState");
    var gridHiddenColumns = getCookie(gridId + "gridHiddenColumns");

    if (state != undefined && typeof state != "undefined") {

        state = JSON.parse(state);

        if (state.filter) {
            parseFilterDates(state.filter, grid.dataSource.options.schema.model.fields);
        }
        grid.dataSource.query(state);
    }
    else {
        grid.dataSource.read();
    }

    if (gridHiddenColumns != undefined && typeof gridHiddenColumns != "undefined") {
        gridHiddenColumns = JSON.parse(gridHiddenColumns);
        for (var i = 0; i < gridHiddenColumns.length; i++) {
            grid.hideColumn(gridHiddenColumns[i]);

        }
    }
}
function parseFilterDates(filter, fields) {
    if (filter.filters) {
        for (var i = 0; i < filter.filters.length; i++) {
            parseFilterDates(filter.filters[i], fields);
        }
    }
    else {
        if (fields[filter.field].type == "date") {
            filter.value = kendo.parseDate(filter.value);
        }
    }
}
