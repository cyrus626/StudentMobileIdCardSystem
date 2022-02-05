let currentPhoto;
let currentPhotoPath;

function init() {
    const form = document.getElementById("uploadForm");


    form.addEventListener("change", event => {
        const files = event.target.files;
        currentPhoto = files[0];

        currentPhotoPath = URL.createObjectURL(currentPhoto);
        document.getElementById("passportPreview").src = currentPhotoPath;
      });
}

function handleUpload(e) {
    if (e) {
        e.preventDefault();
    }

    if (!currentPhoto) {
        alert("Please select your passport.");
        return;
    }

    const data = new FormData();
    data.append("file", currentPhoto);

    return uploadEnrollmentPhoto(data)
        .then(res => {
            return res.data;
        });
}

function handleSubmit(e) {
    e.preventDefault();
    
    const form = document.querySelector('#enrollForm');
    const model = compileData(form);
    
    handleUpload(undefined)
        .then((res) => {
            model.photoId = res.id;
            enrollUser(model).then(() => {
                form.reset();
                window.location.replace("/");
            });
        });
}

function compileData(form) {
    const data = new FormData(form);

    const model = {};

    for (var [key, value] of data.entries()) {
        model[key] = value;
    }
    return model;
}