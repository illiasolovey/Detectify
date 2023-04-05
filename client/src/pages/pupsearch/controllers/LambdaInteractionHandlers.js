import axios from "axios";
import { toast } from "react-toastify";
import { API_ENDPOINT } from "../../../../appsettings.json";

const endpoint = API_ENDPOINT + "lambda/";

export async function invokeObjectAnalysis(filename, setPreviewUrl, setFileAnalyzed) {
  const url = endpoint + "object-analysis";
  console.log("filename: " + filename)
  const requestConfig = {
    params: { filename: filename },
    headers: {
      "Content-Type": "application/json",
    },
  };
  try {
    const response = await axios.get(url, requestConfig);
    const url2 = response.data;
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
