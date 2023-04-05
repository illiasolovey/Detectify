import axios from "axios";
import { toast } from "react-toastify";
import { API_ENDPOINT } from "../../../../appsettings.json";
import { v4 as uuidv4 } from 'uuid';

const endpoint = API_ENDPOINT + "s3/";

export function onFileInput(event, setCurrentFile, setPreviewUrl) {
  const file = event.target.files[0];
  if (!file) return;
  setCurrentFile(file);
  setPreviewUrl(URL.createObjectURL(file));
  toast.success(`${file.name} selected`);
}

export async function onFileSubmit(
  currentFile,
  setFileUploaded,
  setRenderedFileName
) {
  const formData = new FormData();
  formData.append("formFile", currentFile);
  const url = endpoint + "upload/" + currentFile.name;
  const requestConfig = {
    headers: {
      "content-type": "multipart/form-data",
    },
  };
  try {
    const response = await axios.post(url, formData, requestConfig);
    setFileUploaded(true);
    setRenderedFileName(response.data);
    toast.success(`${currentFile.name} uploaded`);
  } catch (err) {
    toast.error("Upload error");
    console.log(err);
  }
}

// ticket!

export async function useDefault(fileUrl) {
  const file = await fetch(fileUrl).then((r) => r.blob());
  const extension = file.type.split("/")[1];
  const url = `${endpoint}upload/default-${uuidv4()}.${extension}`;
  const formData = new FormData();
  formData.append("formFile", file);
  const response = await axios.post(url, formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
  if (response.status === 200) {
    return response.data;
  } else {
    console.error("Error uploading image:", response.statusText);
  }
}
