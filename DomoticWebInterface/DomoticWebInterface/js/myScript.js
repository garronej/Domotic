$(document).ready(function () {



    $("#wrapper").hide();
    $("#realTimeValues").hide();

    $("#accordion").hide();


});

$(window).load(function () {



    $("#wrapper").show("clip", { direction: "horizontal" }, 2000, function () {


        $("#accordion").show();

        $("#accordion").accordion({ header: "h3", collapsible: true, active: false });


        $("#realTimeValues").show("clip", null, 1000, function () {

           



        });


    });

});