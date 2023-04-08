import axios from "axios";
import { toast } from "react-toastify";
import { API_ENDPOINT } from "../../../appsettings.json";

const endpoint = API_ENDPOINT + "lambda/";

export async function invokeObjectAnalysis(filename, confidence) {
  const url = endpoint + "object-analysis";
  const requestConfig = {
    params: { filename: filename, confidencePercentage: confidence },
    headers: {
      "Content-Type": "application/json",
    },
  };
  try {
    const response = await axios.get(url, requestConfig);
    if (response.status !== 200) return;
    return response.data;
  } catch (err) {
    toast.error("Lamba error");
  }
}
