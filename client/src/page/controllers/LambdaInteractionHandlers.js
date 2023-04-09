import axios from "axios";
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
  const response = await axios.get(url, requestConfig);
  const data = response.data;
  if (data.errorType)
    throw data.errorType + ": " + data.errorMessage;
  return data;
}
