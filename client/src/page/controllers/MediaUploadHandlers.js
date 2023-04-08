import axios from "axios";
import { toast } from "react-toastify";
import { API_ENDPOINT } from "../../../appsettings.json";
import { invokeObjectAnalysis } from "./LambdaInteractionHandlers";

const endpoint = API_ENDPOINT + "s3/";

export function selectFile(event, setCurrentFile) {
  const file = event.target.files[0];
  if (!file) return;
  setCurrentFile(file);
  return URL.createObjectURL(file);
}

export async function uploadFile(file) {
  const formData = new FormData();
  formData.append("formFile", file);
  const url = endpoint + "upload/" + file.name;
  const requestConfig = {
    headers: {
      "content-type": "multipart/form-data",
    },
  };
  try {
    const response = await axios.post(url, formData, requestConfig);
    return response.data;
  } catch (err) {
    toast.error("Upload error");
  }
}

export async function downloadFile(filename) {
  const url = `${endpoint}download/${filename}`;
  try {
    const response = await axios.get(url, {
      responseType: "blob",
    });
    const responseUrl = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement("a");
    link.href = responseUrl;
    link.setAttribute("download", "pupsearch-result.png");
    document.body.appendChild(link);
    link.click();
  } catch (error) {
    console.log(error);
  }
}

export async function useDefault(
  fileUrl,
  analysisConfidenceLevel,
  setFileToDownload
) {
  const file = await fetch(fileUrl).then((r) => r.blob());
  const extension = file.type.split("/")[1];
  file.name = `random-image-request.${extension}`;
  const filename = await uploadFile(file);
  setFileToDownload(filename);
  const response = await invokeObjectAnalysis(filename, analysisConfidenceLevel);
  return response;
}
