import { Box, CssBaseline, Grid, Paper, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import QuickGuide from "./components/QuickGuide";
import axios from "axios";
import ImagePreview from "./components/ImagePreview";
import RandomImageModal from "./modals/RandomImageSelection";
import FileSelectionModal from "./modals/FileSelection";
import { toast } from "react-toastify";
import { coveringBoxStyle } from "./components/DefaultStyles";
import FileDownloadButton from "./components/FileDownloadButton";

function onDocumentMounted(setSelectedRandomImageUrl, setPreviewUrl) {
  axios
    .get("https://source.unsplash.com/random")
    .then(async (response) => {
      const url = response.request.responseURL;
      setSelectedRandomImageUrl(url);
      setPreviewUrl(url);
    })
    .catch((error) => {
      toast.error(
        "Oops, we can't fetch a random image now! Cause: " + error.message
      );
      console.error(error);
    });
}

export default function Page() {
  const [previewUrl, setPreviewUrl] = useState();
  const [selectedRandomImageUrl, setSelectedRandomImageUrl] = useState();
  const [fileIsAnalyzed, setFileIsAnalyzed] = useState(false);
  const [fileToDownload, setFileToDownload] = useState(null);

  useEffect(
    () => onDocumentMounted(setSelectedRandomImageUrl, setPreviewUrl),
    []
  );

  return (
    <Grid container component="main" sx={{ height: "100vh" }}>
      <CssBaseline />
      <ImagePreview previewUrl={previewUrl} />
      <Grid item md={5} component={Paper} position="relative">
        <Box sx={{ ...coveringBoxStyle, my: 12, mx: 4 }}>
          <Typography variant="h3">Search</Typography>
        </Box>
        <Grid container spacing={2} justifyContent="center">
          <Grid item>
            <Box style={coveringBoxStyle}>
              <FileSelectionModal
                setPreviewUrl={setPreviewUrl}
                fileToDownload={fileToDownload}
                setFileToDownload={setFileToDownload}
                setFileIsAnalyzed={setFileIsAnalyzed}
              />
            </Box>
          </Grid>
          <Grid item>
            <Box style={coveringBoxStyle}>
              <RandomImageModal
                selectedRandomImageUrl={selectedRandomImageUrl}
                setPreviewUrl={setPreviewUrl}
                setFileToDownload={setFileToDownload}
                setFileIsAnalyzed={setFileIsAnalyzed}
              />
            </Box>
          </Grid>
        </Grid>
        {fileIsAnalyzed && (
          <FileDownloadButton
            fileToDownload={fileToDownload}
            boxStyle={{
              ...coveringBoxStyle,
              display: "flex",
              justifyContent: "center",
            }}
          />
        )}
        <QuickGuide />
      </Grid>
    </Grid>
  );
}
