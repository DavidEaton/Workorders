$(function () {
    
    //mobile grid object
    var mobileGrid = mobileGrid || {};

    //defaults
    mobileGrid.page = 1; //page number
    mobileGrid.pageSize = 10; //page size
    mobileGrid.isFilterOn = true; //boolean to switch filter
    mobileGrid.filter = ''; //filter string
    mobileGrid.searchString = ''; //search string
    mobileGrid.workOrderType = ''; //workorder type . e.g Open or Close
    mobileGrid.workOrderDueType = ''; //workorder due typee.g Due or Overdue
    mobileGrid.filterString = ''; //concatination of mobileGrid.filter + mobileGrid.searchString
    mobileGrid.sort = ''; //sort field
    mobileGrid.$gridElement = $('.wo-jquery-mobile-grid'); //main grid html element
    mobileGrid.$gridContentElement = $('.wo-jquery-mobile-grid .wo-mobile-grid-contents'); //content wrapper, new rows will append in this html element
    mobileGrid.dataSourceUrl = mobileGrid.$gridElement.data('url'); //data source url (attached to data attribute of .wo-jquery-mobile-grid html element)
    mobileGrid.itemDetailUrl = mobileGrid.$gridElement.data('itemurl'); //workorder detail url
    mobileGrid.gridData = null;  //variable will hold json data
    mobileGrid.blockRequest = true; //variable to avoid multiple requests at one time. only process next request if prev has been completed.

    //method will return formated date from json string
   
    mobileGrid.formatDate = function (jsonString) {
        var date = new Date(parseInt(jsonString.substr(6)));

        return (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear();
    };

    //method will return priority class based on priority number
    mobileGrid.getPriorityClass = function (priorityNumber) {

        if (priorityNumber == 1) { //high
            return 'high-priority';
        } else if (priorityNumber == 2) { // normal
            return 'normal-priority';
        } else if (priorityNumber == 3) { //low
            return 'low-priority';
        }

        return '';
    };

    //set applied filters string
    mobileGrid.setFilterString = function () {
        
        mobileGrid.filterString = '';
        //make sure toogle filter button is on 
        if ($('.wo-jquery-mobile-grid-toggle-filter:first').parents('.make-switch').bootstrapSwitch('status')) {

            $('#filterModal').find('select').each(function () {
                var $control = $(this);

                if ($control.val() != '' && $control.attr('name') != undefined) {

                    //set user friendly filer names
                    if (mobileGrid.filterString == '') {
                        mobileGrid.filterString = '[' + mobileGrid.getUserFreindlyFilterName($control.attr('name')) + ']' + "=" + $control.find('option:selected').text();
                    } else {
                        mobileGrid.filterString += ', ' + '[' + mobileGrid.getUserFreindlyFilterName($control.attr('name')) + ']' + "=" + $control.find('option:selected').text();
                    }
                }
            });
        }

        if (mobileGrid.filterString == '') {
            $('.wo-jquery-mobile-grid-filter-link').addClass('btn-up');
            $('.wo-jquery-active-filter-row').hide();
        } else {
            $('.wo-jquery-mobile-grid-filter-link').removeClass('btn-up');
            $('.wo-jquery-active-filter-row').show();
        }


        //display filters to user
        $('.wo-jquery-active-filters').text(mobileGrid.filterString);
    };

    //filter dropdowns name attributes match with properties name of WorkOrdersGridViewModel. 
    //but when we display applied filters to end user we need user friendly names. Method will return user friendly names
    mobileGrid.getUserFreindlyFilterName = function (raw) {

        if (raw == 'AreaId') {
            return 'Area';
        }
    };

    //this function will make ajax request to get json data from server and then append records to grid
    mobileGrid.loadData = function (emptyGrid) {

        var filter = '';
        var sort = '';

        //we need to pass filter params to server side codes.
        //if mobileGrid.filter and mobileGrid.searchString is not empty, we will concat both strings 
        if (mobileGrid.filter != '' && mobileGrid.searchString != '') {
            filter = mobileGrid.filter + '~and~' + mobileGrid.searchString;

        } else if (mobileGrid.filter == '' && mobileGrid.searchString != '') { //only mobileGrid.searchString has value
            filter = mobileGrid.searchString;
        } else if (mobileGrid.filter != '' && mobileGrid.searchString == '') { //mobileGrid.filter has value 
            filter = mobileGrid.filter;
        }

        //if field is selected from sort dropdown, pass order direction (asc or desc) as {fieldName}- {orderDirection} e.g Priority-desc
        if (mobileGrid.sort != '') {
            sort = mobileGrid.sort + '-' + ($('.wo-jquery-mobile-grid-sort-direction').parents('.make-switch').bootstrapSwitch('status') ? 'desc' : 'asc');
        }

        //we call this method in multiple scenarios (when we apply filter, when we do sort, when search and when we load next records).
        //except load more scenario, we need to rest page number to first page 
        if (emptyGrid) {
            mobileGrid.page = 1;
        }

        //display grid loader image
        $('.wo-jquery-mobile-grid-loader').fadeIn();

        //make an ajax request to load data from server (from mobileGrid.dataSourceUrl) 
        $.ajax({
            url: mobileGrid.dataSourceUrl,
            dataType: 'Json',
            type: 'Post',
            data: { //pass params page, pageSize, filter, sort, workOrderType and workOrderDueType to request
                page: mobileGrid.page, pageSize: mobileGrid.pageSize, filter: filter, sort: sort,
              
            },
            success: function (response) { //when ajax call will complete, this method will get executed

                //we call this method in multiple scenarios (when we apply filter, when we do sort, when search and when we load next records).
                //except load more scenario, we need to empty grid data. In load more we'll append new data rows below existing data rows 
                if (emptyGrid) {
                    mobileGrid.$gridContentElement.empty();
                }

                //assign json data returned from server to mobileGrid.gridData
                mobileGrid.gridData = response.Data.Data;

                //set total records count at top of grid
                $('.wo-mobile-grid-count').html(' (<span>' + response.TotalRecords + '</span>)');

                //iterate over each data record and insert row to grid
                for (var i = 0; i < mobileGrid.gridData.length; i++) {

                    mobileGrid.createDataRow(mobileGrid.gridData[i]);
                }

                //all processing is completed. So hide loaders
                $('.wo-jquery-mobile-grid-loader').fadeOut(); //header loader
                $('.wo-mobile-grid-bottom-loader').fadeOut(); //bottom loader (display when we load next records)

                //set mobileGrid.blockRequest to false to allow to process next request
                mobileGrid.blockRequest = false;
                
            },

            error: function () { //got error
                //but hide loaders and set blockRequest to false
                $('.wo-jquery-mobile-grid-loader').fadeOut();
                $('.wo-mobile-grid-bottom-loader').fadeOut();
                mobileGrid.blockRequest = false;
            }
        });
    };

    //method will create data row and append it to grid
    mobileGrid.createDataRow = function (alertrecipient) {

        mobileGrid.createOpenWorkorderRow(alertrecipient)
        //add detail url and priority class to <a> tag
        mobileGrid.$gridContentElement.find('.wo-jquery-mobile-grid-content-row:last').attr('href', mobileGrid.itemDetailUrl + '/' + alertrecipient.Id);
                                   
    };

    //method will use open workorder template and will create new data row to append to grid
    mobileGrid.createOpenWorkorderRow = function (alertrecipient) {

        //open workorder template object
        var $template = $($('#wo-mobile-grid-open-template').html());

        //set params
        $template.find('.wo-jquery-mobile-grid-area').text(alertrecipient.AreaName).end()
                .find('.wo-jquery-mobile-grid-email').text(alertrecipient.Emails).end()
              

        //append to grid
        mobileGrid.$gridContentElement.append($template);
    };

    //click event on apply filter button
    $('.wo-mobile-grid-apply-filter').click(function (ev) {
        
        //reset filters
        mobileGrid.filter = '';

        ev.preventDefault();
        //make sure toogle filter button is on 
        if ($('.wo-jquery-mobile-grid-toggle-filter:first').parents('.make-switch').bootstrapSwitch('status')) {

            //iterate over each dropdown
            $('#filterModal').find('select').each(function () {


                var $control = $(this);

                //if has selected value and has name attribute (exclude workOrderType and workOrderDueType). 
                //Because we pass workOrderType and workOrderDueType as a two different params (don't add to mobileGrid.filter)
                if ($control.val() != '' && $control.attr('name') != undefined ) {

                    //create filter string
                    if (mobileGrid.filter == '') {
                        mobileGrid.filter = $control.attr('name') + '~eq~' + "'" + $control.find('option:selected').val() + "'";
                    } else {
                        mobileGrid.filter += '~and~' + $control.attr('name') + '~eq~' + "'" + $control.find('option:selected').val() + "'";
                    }
                }
            });
        };

        //hide modal
        $('#filterModal').modal('hide');

        //load data based on selected filters
        mobileGrid.loadData(true);

        //set filters string (to display applied filters to end user)
        mobileGrid.setFilterString();
    });

    //click event on search button
    $('.wo-mobile-grid-search').click(function () {

        //reset search variable 
        mobileGrid.searchString = '';

        //iterate over each input (we have two Details and Resolution)
        $('#searchModal').find('input').each(function () {

            var $control = $(this);
            var controlValue = $.trim($(this).val());

            if (controlValue != '') { //if has value

                if ($control.hasClass('wo-mobile-grid-detail-field-search')) { //if Details search field
                    mobileGrid.searchString = 'Emails~contains~' + "'" + controlValue + "'"; //set Details search string

                }
            }
        });
        //new join the line
        mobileGrid.userSearchString = '';

        //iterate over each input (we have two Details and Resolution)
        $('#searchModal').find('input').each(function () {
            
            var $control = $(this);
            var controlValue = $.trim($(this).val());

            if (controlValue != '') { //if has value

                if ($control.hasClass('wo-mobile-grid-detail-field-search')) { //if Details search field
                    mobileGrid.userSearchString = '[Emails](contains)' + controlValue; //set Details search string

                }
            }

        });



        if (mobileGrid.userSearchString == '') {
            $('.wo-jquery-mobile-grid-search-link').addClass('btn-up');
            $('.wo-jquery-active-search-row').hide();
        } else {
            $('.wo-jquery-mobile-grid-search-link').removeClass('btn-up');
            $('.wo-jquery-active-search-row').show();
        }

        $('.wo-jquery-active-search').text(mobileGrid.userSearchString);
        //hide search modal
        $('#searchModal').modal('hide');

        //load data based on search terms
        mobileGrid.loadData(true);
    });

    //click event on sort button
    $('.wo-mobile-grid-sort').click(function () {

        mobileGrid.sort = '';
        var $control = $('#sortModal select');

        if ($control.val() != '') { //if has selected field from dropdown
            mobileGrid.sort = $control.val(); //set sort
        }

        if (mobileGrid.sort == '') {
            $('.wo-jquery-mobile-grid-sort-link').addClass('btn-up');
        } else {
            $('.wo-jquery-mobile-grid-sort-link').removeClass('btn-up');
        }

        //hide sort modal
        $('#sortModal').modal('hide');

        //load data sorted by selected sort field
        mobileGrid.loadData(true);
    });

    //click event on clear all filter button
    $('.wo-mobile-grid-clear-filter').click(function () {

        //set all dropdowns to default
        $('#filterModal').find('select').each(function () {
            $(this).val('');
        });
        //trigger click of filter apply button (to load data again from server)
        $('.wo-mobile-grid-apply-filter').trigger('click');

    });

    //click event on clear search button
    $('.wo-mobile-grid-clear-search').click(function () {

        //set all dropdowns to default
        $('#searchModal').find('input').each(function () {
            $(this).val('');
        });
        $('.wo-jquery-active-search-row').hide();
        //trigger click of search apply button (to load data again from server)
        $('.wo-mobile-grid-search').trigger('click');
    });

    //we have two toggle filter buttons for mobile grid. One at top of grid and other is in filter modal
    //make sure when one is switched, we set same state for other one 
    $('.make-switch').on('switch-change', function (e, data) {

        if ($(this).find('.wo-jquery-mobile-grid-toggle-filter').length > 0) {
            $('.wo-jquery-mobile-grid-toggle-filter').parents('.make-switch').not($(this)).bootstrapSwitch('setState', data.value);
            $('.wo-mobile-grid-apply-filter').trigger('click');
        }
    });

    //key press event on search fields
    $('#searchModal input').keypress(function (e) {

        var code = e.keyCode || e.which;

        //if enter key was pressed
        if (code == 13) {
            //trigger click of search button
            $('.wo-mobile-grid-search').trigger('click');
        }
    });


    //set default filter Status = Open
    $('#filtermodal').find('select[name="areaname"]').val('open');
    $('.wo-mobile-grid-apply-filter').trigger('click');


    //load more data on scroll
    $(window).scroll(function () {

        //if scroll is 150px before bottom.
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 150) {

            //get total records from top count element
            var totalRecords = $.trim($('.wo-mobile-grid-count span').text());

            //count no of rows we have in grid
            var displayedRecords = $('.wo-jquery-mobile-grid-content-row').length;

            if (totalRecords > displayedRecords && mobileGrid.blockRequest == false) { //if total records is greater than displayedRecords and request is not blocked

                //increment page number
                ++mobileGrid.page;

                //block next request 
                mobileGrid.blockRequest = true;

                //display bottom loader
                $('.wo-mobile-grid-bottom-loader').fadeIn();

                //load next set of data from server
                mobileGrid.loadData(false);
            }

        }
    });
});