import { Button } from "@mui/material";
import axios from "axios";

export function DownloadButton({ filename }) {
  const handleDownload = async () => {
    const url2 = `http://localhost:5171/api/s3/download/${filename}`;
    try {
      const response = await axios({
        url: url2,
        method: "GET",
        responseType: "blob"
      });
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", "pupsearch-result.png");
      document.body.appendChild(link);
      link.click();
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <Button variant="outlined" onClick={handleDownload}>
      Download Image
    </Button>
  );
};
