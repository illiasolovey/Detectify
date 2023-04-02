import axios from "axios";
import { toast } from "react-toastify";
import { API_ENDPOINT } from "../../../../appsettings.json";

const endpoint = API_ENDPOINT + "s3/";

export function onFileInput(event, setCurrentFile, setPreviewUrl) {
  const file = event.target.files[0];
  if (!file) return;
  setCurrentFile(file);
  setPreviewUrl(URL.createObjectURL(file));
  toast.success(`${file.name} selected`);
}

export async function onFileSubmit(currentFile, setFileUploaded) {
  const formData = new FormData();
  formData.append("formFile", currentFile);
  const url = endpoint + "upload/" + currentFile.name;
  const requestConfig = {
    headers: {
      "content-type": "multipart/form-data",
    },
  };
  try {
    await axios.post(url, formData, requestConfig);
    setFileUploaded(true);
    toast.success(`${currentFile.name} uploaded`);
  } catch (err) {
    toast.error("Upload error");
    console.log(err);
  }
}
