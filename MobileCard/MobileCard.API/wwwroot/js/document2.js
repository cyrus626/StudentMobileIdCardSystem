//adding date
var today = new Date();
var thisYear = today.getFullYear();
document.getElementById("showDate").innerHTML = " " + thisYear ;

//Let's do a quick introduction
function doIntro(){
    window.alert(`Welcome to Student Mobile Id,\n 
     Designed by Momoh Cyrus Adinoyi.\n
     Using University of Abuja as case study.`)
}