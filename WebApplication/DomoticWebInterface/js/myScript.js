
$(window).load(function () {

    $("#accordion").accordion({ header: "h3", collapsible: true, active: false });

});

function permanentScript() {

    $("select[id$='_DropDownList1']").change(function () {
        $("img[id$='_LoadingGif2']").removeClass("hidden").addClass("visible");
    });


}


function ShowLoading() {
    $("img[id$='_LoadingGif']").removeClass("hidden").addClass("visible");
}

