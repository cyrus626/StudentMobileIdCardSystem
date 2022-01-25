const $axios = axios;
$axios.defaults.headers.common["Access-Control-Allow-Origin"] = "*";

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
var username = document.getElementsByName("username").innerHTML;
var password = document.getElementsByName("password").innerHTML;
function init() {
    axios.post("https://localhost:7252/api/auth/login", {
        username: this.username,
        password: this.password // real password: admin@321
    })
    .then(res => {
        console.log("Login successful", res.data);
    }).catch(err => {
        console.log(err.response);
        console.log(err.response.data);

        const data = err.response.data;
        if (data) {
            const errorMessage = data[0].description;
            console.log(errorMessage);
        }
    });
}