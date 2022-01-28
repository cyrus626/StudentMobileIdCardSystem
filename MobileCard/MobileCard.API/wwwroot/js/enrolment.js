let currentPhoto;
let currentPhotoPath;
let photoId;

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

    return ploadEnrollmentPhoto(data)
        .then(res => {
            photoId = res.data.id;
        });
}

function handleSubmit(e) {
    e.preventDefault();

    handleUpload(undefined)
        .then(() => {
            // TODO: Create submit object and add photoId
            const model = {};
            enrollUser(model);
            // TODO: Clear the form fields...
        });
}