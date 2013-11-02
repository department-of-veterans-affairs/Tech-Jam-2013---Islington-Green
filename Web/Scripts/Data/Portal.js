window.onload = function () {
    $(".column").sortable({
        connectWith: ".column"
    });

    $(".portlet").addClass("ui-widget ui-widget-content ui-helper-clearfix ui-corner-all")
        .find(".portlet-header")
        .addClass("ui-widget-header ui-corner-all")
        .prepend("<span class='ui-icon ui-icon-minusthick'></span>")
        .end()
        .find(".portlet-content");

    $(".portlet-header .ui-icon").click(function () {
        $(this).toggleClass("ui-icon-minusthick").toggleClass("ui-icon-plusthick");
        $(this).parents(".portlet:first").find(".portlet-content").toggle();
    });

    $(".column").disableSelection();

    $("#dialog").dialog({
        autoOpen: false,
        show: {
            effect: "blind",
            duration: 250
        },
        hide: {
            effect: "fade",
            duration: 600
        }
    });

    $($("[data-bind*='Drugs.DoctorPrescribed']")).click(function (e) {
        var el = $(e.target);
        var details = el.siblings('.details').eq(0);
        var title = details.prev('.title').eq(0).text();
        $("#dialog").attr("title", el.closest(".title").text()).find("p").html(details.html()).end().dialog({
            modal: true,
            width: 640
        }).dialog("open");
        $("#dialog").prev(".ui-dialog-titlebar").find(".ui-dialog-title").text(title);
    });
};