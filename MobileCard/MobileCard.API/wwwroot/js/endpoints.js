const baseUrl = "https://localhost:7252/api/";
const AxiosConfig = {
    baseURL: baseUrl,
    timeout: 2 * 60 * 1000
};

const $axios = axios.create(AxiosConfig);

$axios.defaults.headers.common["Access-Control-Allow-Origin"] = "*";

const getAuthToken = () => localStorage.getItem("accessToken");

const authInterceptor = (config) => {
  config.headers["Authorization"] = "Bearer " + getAuthToken();

  return config;
};

$axios.interceptors.request.use(authInterceptor);


const GET_ACCOUNT = "account";

const LOGIN = "auth/login";
const ENROLL = "auth/enroll";

const GET_RESOURCE = "res/{resourceId}";
const UPLOAD_ENROLLMENT = "res/upload/enrollment/photo";


const UnknownApiError = {
    code: "UnknownError",
    description: "An unknown error has occurred",
    status: "500"
};



function getError(error, report = false) {
    let data = error.response.data;
    let err = UnknownApiError;

    if (data) {
        err = data[0];
    }

    if (report) {
        alert(err.description);
    }

    return err;
}

// #region State
let isBusy = false;
let account;
// #endregion



function isLoggedIn() {
    const token = localStorage.getItem("accessToken");
    return !!token;
}

function login(model) {
    isBusy = true;
    return $axios.post(LOGIN, model)
        .then(res => {
            localStorage.setItem("accessToken", res.data.accessToken);
        }).catch(err => {
            getError(err, true);
        }).finally(() => isBusy = false);
}

function logout() {
    localStorage.removeItem("accessToken");
    window.location.replace("/");
}

function getAccount() {
    return $axios.get(GET_ACCOUNT)
        .then(res => {
            account = res.data;
            return account;
        }).catch(err => {
            getError(err, true);
        }).finally(() => isBusy = false);
}
function enrollUser(model) {
    return $axios.post(ENROLL, model)
        .then(res => {
            // Report success to user
            alert(`Successful!, \n Data collected\n userId: matric number\n password: firstname and lastname`);
        }).catch(err => {
            getError(err, true);
        });
}

function uploadEnrollmentPhoto(data) {
    return $axios.post(UPLOAD_ENROLLMENT, data).catch(err => {
        getError(err, true);
    });
}

function getResource(resourceId) {
    const url = GET_RESOURCE.replace("{resourceId}", resourceId);
    return $axios.get(url).catch(err => {
        getError(err, true);
    });
}
