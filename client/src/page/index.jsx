import {
  Box,
  Button,
  CssBaseline,
  Grid,
  Paper,
  Typography,
} from "@mui/material";
import { useEffect, useState } from "react";
import { downloadFile } from "./controllers/MediaUploadHandlers";
import { quickGuide } from "./components/Content";
import axios from "axios";
import ImagePreview from "./components/ImagePreview";
import RandomImageModal from "./modals/RandomImageSelection";
import FileSelectionModal from "./modals/FileSelection";

const coveringBoxStyle = {
  mx: 4,
  my: 2,
  display: "flex",
  flexDirection: "column",
  alignItems: "center",
};

export default function Page() {
  const [previewUrl, setPreviewUrl] = useState();
  const [selectedRandomImageUrl, setSelectedRandomImageUrl] = useState();
  const [fileAnalyzed, setFileAnalyzed] = useState(false);
  const [fileToDownload, setFileToDownload] = useState(null);

  useEffect(() => {
    axios
      .get("https://source.unsplash.com/random")
      .then(async (response) => {
        const url = response.request.responseURL;
        setSelectedRandomImageUrl(url);
        setPreviewUrl(url);
      })
      .catch((error) => {
        toast.error("Oops, we can't fetch a random image now!");
        console.error(error);
      });
  }, []);

  return (
    <Grid container component="main" sx={{ height: "100vh" }}>
      <CssBaseline />
      <ImagePreview previewUrl={previewUrl} />
      <Grid item md={5} component={Paper}>
        <Box sx={{ ...coveringBoxStyle, my: 8, mx: 4 }}>
          <Typography variant="h3">Search</Typography>
        </Box>
        <Box style={coveringBoxStyle}>
          <FileSelectionModal
            setPreviewUrl={setPreviewUrl}
            fileToDownload={fileToDownload}
            setFileToDownload={setFileToDownload}
            setFileAnalyzed={setFileAnalyzed}
          />
        </Box>
        <Box style={coveringBoxStyle}>
          <RandomImageModal
            selectedRandomImageUrl={selectedRandomImageUrl}
            setPreviewUrl={setPreviewUrl}
            setFileToDownload={setFileToDownload}
            setFileAnalyzed={setFileAnalyzed}
          />
        </Box>
        {fileAnalyzed && (
          <Button
            variant="contained"
            onClick={() => downloadFile(fileToDownload)}
          >
            Download result
          </Button>
        )}
        <Grid
          container
          spacing={4}
          justify="space-evenly"
          textAlign="center"
          sx={{ position: "static" }}
        >
          {quickGuide.map((footer) => (
            <Grid item xs key={footer.title}>
              <Typography variant="h5" color="textPrimary" gutterBottom>
                {footer.title}
              </Typography>
              <Typography variant="subtitle1" color="textSecondary">
                {footer.description}
              </Typography>
            </Grid>
          ))}
        </Grid>
      </Grid>
    </Grid>
  );
}
