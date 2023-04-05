import axios from "axios";
import { toast } from "react-toastify";
import { API_ENDPOINT } from "../../../../appsettings.json";

const endpoint = API_ENDPOINT + "lambda/";

export async function invokeObjectAnalysis(filename, confidence, setPreviewUrl, setFileAnalyzed) {
  const url = endpoint + "object-analysis";
  const requestConfig = {
    params: { filename: filename, confidencePercentage: confidence },
    headers: {
      "Content-Type": "application/json",
    },
  };
  try {
    const response = await axios.get(url, requestConfig);
    if (response.status !== 200) {
      console.log(response.data);
      return;
    }
    setPreviewUrl(response.data);
    setFileAnalyzed(true);
    toast.success("Lambda");
  } catch (err) {
    console.log(err);
    toast.error("Lamba error");
  }
}
