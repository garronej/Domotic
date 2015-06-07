$(document).ready(function () {

    $("#wrapper").hide();
    $("#realTimeValues").hide();
    $("#accordion").hide();

});

$(window).load(function () {

    

    $("#wrapper").show("drop", { direction: "horizontal" }, 2000, function () {


        $("#accordion").show();

        $("#accordion").accordion({ header: "h3", collapsible: true, active: false });


        $("#realTimeValues").show("clip", null, 1000, function () {
      
            
        });

    });




});

function permanentScript() {

    $("select[id$='_DropDownList1']").change(function () {
        $("img[id$='_LoadingGif2']").removeClass("hidden").addClass("visible");
    });

    $("select[id$='_DropDownList2']").change(function () {
        $("img[id$='_LoadingGif3']").removeClass("hidden").addClass("visible");
    });

}


function ShowLoading() {
    $("img[id$='_LoadingGif']").removeClass("hidden").addClass("visible");
}

