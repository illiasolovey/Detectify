import {
  Box,
  Button,
  CardContent,
  CssBaseline,
  Grid,
  Paper,
  Typography,
} from "@mui/material";
import { useState } from "react";
import { onFileInput, onFileSubmit } from "./controllers/MediaUploadHandlers";
import { invokeObjectAnalysis } from "./controllers/LambdaInteractionHandlers";
import { PaperNote } from "./components/StyledComponents";
import { quickGuide } from "./components/Content";
import { DownloadButton } from "./components/DownloadButton";

export default function HomePage() {
  const [currentFile, setCurrentFile] = useState(null);
  const [renderedFileName, setRenderedFileName] = useState();
  const [previewUrl, setPreviewUrl] = useState();
  const [fileUploaded, setFileUploaded] = useState(false);
  const [fileAnalyzed, setFileAnalyzed] = useState(false);

  return (
    <Grid container component="main" sx={{ height: "100vh" }}>
      <CssBaseline />
      <Grid
        item
        xs={false}
        md={7}
        sx={{
          backgroundImage: `url(${
            previewUrl || "https://source.unsplash.com/random"
          })`,
          backgroundSize: "cover",
          backgroundPosition: "center",
        }}
      >
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            height: "100%",
          }}
        >
          <PaperNote
            sx={{
              position: "absolute",
              bottom: 0,
              left: 0,
              margin: "12vh",
              width: "30%",
            }}
          >
            <CardContent>
              PupSearch is pawsome object detection tool to identify objects in
              images and videos.
            </CardContent>
          </PaperNote>
        </Box>
      </Grid>
      <Grid item xs={12} sm={8} md={5} component={Paper}>
        <Box
          sx={{
            my: 8,
            mx: 4,
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
          }}
        >
          <Typography variant="h3">Search</Typography>
          {!currentFile ? (
            <Button variant="contained" component="label" sx={{ my: 6 }}>
              Upload File
              <input
                type="file"
                onChange={(event) =>
                  onFileInput(event, setCurrentFile, setPreviewUrl)
                }
                hidden
              />
            </Button>
          ) : (
            <Button
              variant="outlined"
              onClick={() =>
                onFileSubmit(currentFile, setFileUploaded, setRenderedFileName)
              }
              sx={{ my: 6 }}
            >
              Submit
            </Button>
          )}
          {previewUrl && (
            <Button
              disabled={!fileUploaded}
              onClick={() =>
                invokeObjectAnalysis(
                  currentFile,
                  setPreviewUrl,
                  setFileAnalyzed
                )
              }
              sx={{ my: 6 }}
            >
              <img src={previewUrl} alt="Preview" width="100%" height="100%" />
            </Button>
          )}
          {fileAnalyzed && <DownloadButton filename={renderedFileName} />}
          <Grid container spacing={4} justify="space-evenly" textAlign="center">
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
        </Box>
      </Grid>
    </Grid>
  );
}
