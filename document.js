function myFunctn(){
    var pword0 = document.getElementById("pword0").value;
    var pword1 = document.getElementById("pword1").value;

    if(pword0 != pword1){
        document.getElementById("reslt").innerHTML = "Password not matched";
    }
    else{
        document.getElementById("reslt").innerHTML = "Password matched";
    }
}