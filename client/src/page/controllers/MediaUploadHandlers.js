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
  const response = await axios.post(url, formData, requestConfig);
  const data = response.data;
  if (data.errorType) throw data.errorType + ": " + data.errorMessage;
  return response.data;
}

export async function downloadFile(filename) {
  const url = `${endpoint}download/${filename}`;
  const response = await axios.get(url, {
    responseType: "blob",
  });
  const responseUrl = window.URL.createObjectURL(new Blob([response.data]));
  const link = document.createElement("a");
  link.href = responseUrl;
  link.setAttribute("download", "pupsearch-result.png");
  document.body.appendChild(link);
  link.click();
}

export async function useDefault(
  fileUrl,
  analysisConfidenceLevel,
  setFileToDownload
) {
  const file = await fetch(fileUrl).then((r) => r.blob());
  const extension = file.type.split("/")[1];
  file.name = `random-image-request.${extension}`;
  const filename = uploadFile(file);
  toast.promise(filename, {
    pending: "Uploading..",
    success: "Uploaded successfully!",
    error: "Error occurred while uploading the file",
  });
  setFileToDownload(await filename);
  const response = invokeObjectAnalysis(
    await filename,
    analysisConfidenceLevel
  );
  toast.promise(response, {
    pending: "Processing..",
    success: "Analysis completed successfully!",
    error: "Error occurred while analyzing the file",
  });
  return response;
}
