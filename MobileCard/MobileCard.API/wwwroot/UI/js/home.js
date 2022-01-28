function init() {
    if (!isLoggedIn()) {
        window.location.replace("/");
    } else {
        getAccount()
            .then(() => {
                console.log(account);
            });
    }
}