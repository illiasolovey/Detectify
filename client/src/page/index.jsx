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
import { toast } from "react-toastify";

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
        toast.error(
          "Oops, we can't fetch a random image now! Cause: " + error.message
        );
        console.error(error);
      });
  }, []);

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
                setFileAnalyzed={setFileAnalyzed}
              />
            </Box>
          </Grid>
          <Grid item>
            <Box style={coveringBoxStyle}>
              <RandomImageModal
                selectedRandomImageUrl={selectedRandomImageUrl}
                setPreviewUrl={setPreviewUrl}
                setFileToDownload={setFileToDownload}
                setFileAnalyzed={setFileAnalyzed}
              />
            </Box>
          </Grid>
        </Grid>
        {fileAnalyzed && (
          <Box
            sx={{
              ...coveringBoxStyle,
              display: "flex",
              justifyContent: "center",
            }}
          >
            <Button
              variant="contained"
              onClick={() => downloadFile(fileToDownload)}
            >
              Download result
            </Button>
          </Box>
        )}
        <Grid
          container
          textAlign="center"
          justifyContent="center"
          sx={{
            position: "absolute",
            bottom: 0,
            left: 0,
            right: 0,
            mx: "auto",
            mb: 4,
            maxWidth: "90%",
          }}
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
