//init Metis Menu
$(function () {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function () {
    $(window).bind("load resize", function () {
        var topOffset = 50;
        var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        var height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
        }
    });

    var url = window.location;
    // var element = $('ul.nav a').filter(function() {
    //     return this.href == url;
    // }).addClass('active').parent().parent().addClass('in').parent();
    var element = $('ul.nav a').filter(function () {
        return this.href === url;
    }).addClass('active').parent();

    while (true) {
        if (element.is('li')) {
            element = element.parent().addClass('in').parent();
        } else {
            break;
        }
    }
});

$(document).ready(function () {
    $('.btnSelectFile').on('click', selectFileForUpload)
    $('input[type=file]').on('change', fileInputChanged)

})


var saveRuleConfiguration = function (url) {

    swal({
        title: "Checking...",
        text: "Please wait",
        type: "info",
        showConfirmButton: false,
        allowOutsideClick: false
    });

    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            swal("Success!", data + "", "success")
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            swal("Error!", XMLHttpRequest.responseText, "error");
        }
    });

}

var selectFileForUpload = function () {
    $(this).parent().find('input[type=file]').click();
}

var fileInputChanged = function (eventdata) {
    console.log('Submit Btn : ', $(this).parent().find('input[type=submit]'))
    //$(this).parent().parent().find('.btnSelectFile').hide(250);
    //$(this).parent().find('input[type=submit]').show(250);

    $(this).parent().submit();
}