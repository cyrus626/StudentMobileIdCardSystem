const GET_REGISTERED_STUDENTS = "manage/students/all";
const GET_ENROLLMENT_APPLICATIONS = "manage/enrollment/all";
const APPROVE_ENROLLMENT = "manage/enrollment/{enrollmentId}/approve";
const REJECT_ENROLLMENT = "manage/enrollment/{enrollmentId}/reject";

let allStudents, allApplications;

function getStudents() {
    return $axios.get(GET_REGISTERED_STUDENTS)
        .then(res => {
            allStudents = res.data;
            console.log(allStudents);
            return res.data;
        }).catch(err => {
            getError(err);
        });
}

function getApplications() {
    return $axios.get(GET_ENROLLMENT_APPLICATIONS)
        .then(res => {
            allApplications = res.data;
            console.log(allApplications);
            return res.data;
        }).catch(err => {
            getError(err);
        });
}

function approveEnrollment(enrollmentId) {
    const url = APPROVE_ENROLLMENT.replace("{enrollmentId}", enrollmentId);
    return $axios.post(url)
        .then(res => {
            // TODO: Notify that enrollment has been approved
        }).catch(err => {
            getError(err);
        });
}

function rejectEnrollment(enrollmentId) {
    const url = REJECT_ENROLLMENT.replace("{enrollmentId}", enrollmentId);
    return $axios.post(url)
        .then(res => {

        }).catch(err => {
            getError(err);
        });
}