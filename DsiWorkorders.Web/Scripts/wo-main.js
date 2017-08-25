$(function () {
    
    //click event on all anchor tags to fix iOS links open in new window 
    //source: http://stackoverflow.com/questions/2898740/iphone-safari-web-app-opens-links-in-new-window
    $('body').delegate('a:not(".k-grid-header a,#deleteModal a, .jq-delete-link,.jq-approve-link,.jq-reject-link,#approveModal a, .wo-nav-tabs a")', 'click', function (event) {

        var url = $.trim($(this).attr("href"));
        
        if (url != '#' && url != '') {
            //prevent browser default behaviour
            event.preventDefault();

            //open url in same winodw
            window.location = url;
        }
    });

    $('body').delegate('.k-grid tbody tr', 'click', function (ev) {

        if ($(ev.target).hasClass('clickable-link')) {
            // window.open($(ev.target).attr("href"));
        } else {
            if ($(this).find('td').eq(0).find('span').length == 0) {
              //  window.location.href = $(this).find('td span').eq(0).data('url');

            } else {
                window.location.href = $(this).find('td').eq(0).find('span').data('url');
            }
        }
    });



});