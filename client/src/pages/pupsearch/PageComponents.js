import axios from "axios";
import { API_ENDPOINT as endpoint } from "../../../appsettings.json";

const s3Endpoint = endpoint + "s3/";
const lambdaEndpoint = endpoint + "lambda/";

export function onFileInput(event, setCurrentFile, setPreviewUrl) {
  const file = event.target.files[0];
  if (!file) return;
  setCurrentFile(file);
  setPreviewUrl(URL.createObjectURL(file));
}

export async function onFileSubmit(currentFile, setFileUploaded) {
  const formData = new FormData();
  formData.append("formFile", currentFile);
  const url = s3Endpoint + "upload/" + currentFile.name;
  const requestConfig = {
    headers: {
      "content-type": "multipart/form-data",
    },
  };
  try {
    const response = await axios.post(url, formData, requestConfig);
    setFileUploaded(true);
  } catch (err) {
    console.log(err);
  }
}

export async function getUrl(currentFile, setPreviewUrl) {
  const url = lambdaEndpoint + "object-analysis";
  const requestConfig = {
    params: { filename: currentFile.name },
    headers: {
      "Content-Type": "application/json",
    },
  };
  try {
    const response = await axios.get(url, requestConfig);
    setPreviewUrl(response.data);
  } catch (err) {
    console.log(err);
  }
}
