
function myFunctn() {
    var pword0 = document.getElementById("pword0").value;
    var pword1 = document.getElementById("pword1").value;

    if(pword0 != pword1){
        document.getElementById("reslt").innerHTML = "Password not matched";
    }
    else{
        document.getElementById("reslt").innerHTML = "Password matched";
    }
}

function viewCard(){
	window.replace("./viewer.html");
}

function enrollNow(){
    window.location.replace("./enrolment.html");
}

function handleLogin(e) {
    e.preventDefault();

    const username = document.getElementById("usernameField").value;
    const password = document.getElementById("passwordField").value;

    login({ username, password })
        .then(processAccount());
}

function processAccount() {
    getAccount()
        .then(x => {
            if (x.kind == 0) {
                window.location.replace("/home.html");
            } else if (x.kind == 1) {
                window.location.replace("/admin.html");
            }
        });
}
