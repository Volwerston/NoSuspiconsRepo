$("#showHideGenres").on("click", function(){
    var element = $("#categories");
    if(element.css('display') === 'none') {
        element.show();
        $("#showHideGenres").html('Приховати');
    } else {
        element.hide();
        $("#showHideGenres").html('Показати');
    }
});

function openCity(cityName) {
    var i;
    var x = document.getElementsByClassName("city");
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none"; 
    }
    document.getElementById(cityName).style.display = "block"; 
}