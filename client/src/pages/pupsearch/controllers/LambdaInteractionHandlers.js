import axios from "axios";
import { toast } from "react-toastify";
import { API_ENDPOINT } from "../../../../appsettings.json";

const endpoint = API_ENDPOINT + "lambda/";

export async function invokeObjectAnalysis(currentFile, setPreviewUrl, setFileAnalyzed) {
  const url = endpoint + "object-analysis";
  const requestConfig = {
    params: { filename: currentFile.name },
    headers: {
      "Content-Type": "application/json",
    },
  };
  try {
    const response = await axios.get(url, requestConfig);
    setPreviewUrl(response.data);
    setFileAnalyzed(true);
    toast.success("Lambda");
  } catch (err) {
    console.log(err);
    toast.error("Lamba error");
  }
}
