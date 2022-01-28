function init() {
    if (!isLoggedIn()) {
        window.location.replace("/");
    } 

    if (!account) {
        processAccount();
    }
}

function processAccount() {
    getAccount()
        .then(x => {
            if (x.kind == 0) {
                window.location.replace("/home.html");
            } else if (x.kind == 1) {
                getStudents();
                getApplications();
            }
        });
}

function approve(id) {
    const ans = confirm("Are you sure you would like to approve this application?");

    if (ans) {
        approveEnrollment(id)
            .then(removeApplication(id))
            .then(getStudents());
    }
}

function reject(id) {
    const ans = confirm("Are you sure you would like to reject this application?");

    if (ans) {
        rejectEnrollment(id)
            .then(removeApplication(id));
    }
}

function removeApplication(id) {
    const index = allApplications.findIndex(x => x.id == id);
        if (index >= 0) {
            allApplications.splice(index, 1);
        }
}