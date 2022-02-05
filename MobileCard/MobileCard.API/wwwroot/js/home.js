function init() {
    if (!isLoggedIn()) {
        window.location.replace("/");
    } else {
        getAccount()
            .then(() => {
                document.getElementById("passport").innerHTML = `<img alt="passport" src="${account.photoUrl}" class="userPassport"/>`;
                document.getElementById("matricNo").innerHTML = account.matricNumber;
                document.getElementById("fullName").innerHTML = account.fullName;
                document.getElementById("courseOffered").innerHTML = account.department;
                document.getElementById("faculty").innerHTML = account.faculty;
                let thisYear = new Date().getFullYear();
                const level = thisYear - account.yearOfEntry;
                document.getElementById("level").innerHTML = (level *100) + "l";
                document.getElementById("yearOfEntry").innerHTML = account.yearOfEntry;
                document.getElementById("email").innerHTML = account.email;
                document.getElementById("phoneNumber").innerHTML = account.phoneNumber;
                console.log(account);
            });
    }
}