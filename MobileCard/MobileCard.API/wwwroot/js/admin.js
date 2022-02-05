function init() {
    if (!isLoggedIn()) {
        window.location.replace("/");
    } 

    if (!account) {
        processAccount();
    }
}
const allTableHeader = [
    "Passport photo",
    "Full name",
    "Matric number",
    "Gender",
    "Phone number",
    "Email",
    "Department",
    "Faculty",
    "Year of entry",
    "Kin's name",
    "Actions"
];

function processAccount() {
    getAccount()
        .then(x => {
            if (x.kind == 0) {
                window.location.replace("/");
            } else if (x.kind == 1) {
                getStudents()
                    .then(() => {
                        showAllStudents(allStudents);  
                    });
                getApplications()
                    .then(() => {
                        showEnrolledStudent(allApplications);
                    });
            }
        });
}

function showEnrolledStudent(allApplications){
    let getEnrolledStudent = "<h3>Yet to be approved applicants</h3>";
    getEnrolledStudent += "<table><tr>";
    for (let tableHeader of allTableHeader){
        getEnrolledStudent += "<th>" + tableHeader + "</th>";
    }

    getEnrolledStudent += "</tr>";

    for (let student of allApplications) {
        console.log(student);
        getEnrolledStudent += "<tr>";
        getEnrolledStudent += addColumn(`<img src="${student.photoUrl}" class="userPassport">`, getEnrolledStudent);
        getEnrolledStudent += addColumn(student.fullName);
        getEnrolledStudent += addColumn(student.matricNumber);
        getEnrolledStudent += addColumn(student.gender);
        getEnrolledStudent += addColumn(student.phoneNumber);
        getEnrolledStudent += addColumn(student.email);
        getEnrolledStudent += addColumn(student.department);
        getEnrolledStudent += addColumn(student.faculty);
        getEnrolledStudent += addColumn(student.yearOfEntry);
        getEnrolledStudent += addColumn(student.nextOfKin);
        getEnrolledStudent += addColumn(`<button onclick="approve('${student.id}')"class="rectInput">
            Approve?</button><br/><button onclick="reject('${student.id}')"  class="rectInput"
            >Disapprove?</button>`);
        getEnrolledStudent += "</tr>";
    }

    console.log(getEnrolledStudent);
    getEnrolledStudent += "</table>";
    document.getElementById("showEnrolledStudents").innerHTML = getEnrolledStudent;
}

function showAllStudents(allStudents){
    let getAllStudentTable = "<h3>All Registered Studens </h3>";
    getAllStudentTable += "<table><tr>";
    for (let tableHeader of allTableHeader) {
        getAllStudentTable += "<th>" + tableHeader + "</th>";
    }

    getAllStudentTable += "</tr>";

    for (let student of allStudents){
        getAllStudentTable += "<tr>";
        getAllStudentTable += addColumn(`<img src="${student.photoUrl}" class="userPassport">`, getAllStudentTable);
        getAllStudentTable += addColumn(student.fullName, getAllStudentTable);
        getAllStudentTable += addColumn(student.matricNumber, getAllStudentTable);
        getAllStudentTable += addColumn(student.gender, getAllStudentTable);
        getAllStudentTable += addColumn(student.phoneNumber, getAllStudentTable);
        getAllStudentTable += addColumn(student.email, getAllStudentTable);
        getAllStudentTable += addColumn(student.department, getAllStudentTable);
        getAllStudentTable += addColumn(student.faculty, getAllStudentTable);
        getAllStudentTable += addColumn(student.yearOfEntry, getAllStudentTable);
        getAllStudentTable += addColumn(student.nextOfKin, getAllStudentTable);
        getAllStudentTable += "</tr>";
    }
    getAllStudentTable += "</table>"
    document.getElementById("showAllStudents").innerHTML = getAllStudentTable;
}

function addColumn(value) {
    return "<td>" + value + "</td>";
}

function approve(id) {
    console.log(id);
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