import { Button, Typography, Fade, Grid, Box } from "@mui/material";
import ConfidenceSlider from "../components/ConfidenceSlider";
import { useState } from "react";
import { selectFile, uploadFile } from "../controllers/MediaUploadHandlers";
import ModalLayout from "./ModalLayout";
import { toast } from "react-toastify";
import { invokeObjectAnalysis } from "../controllers/LambdaInteractionHandlers";
import AdvancedConfigurationMenu from "../components/AdvancedConfigurationMenu";

async function onFileInput(
  event,
  setCurrentFile,
  setPreviewUrl,
  setFileSelected
) {
  setPreviewUrl(selectFile(event, setCurrentFile));
  setFileSelected(true);
}
async function onFileSubmition(file, setFileToDownload, setFileSubmitted) {
  const filename = uploadFile(file);
  toast.promise(filename, {
    pending: "Uploading..",
    success: "Uploaded successfully!",
    error: "Error occurred while uploading the file",
  });
  setFileToDownload(await filename);
  setFileSubmitted(true);
}
async function handleImageAnalysis(
  fileToDownload,
  analysisConfidenceLevel,
  boundingBoxColor,
  labelColor,
  setPreviewUrl,
  setFileIsAnalyzed
) {
  const response = invokeObjectAnalysis(
    fileToDownload,
    analysisConfidenceLevel,
    boundingBoxColor,
    labelColor
  );
  toast.promise(response, {
    pending: "Processing..",
    success: "Analysis completed successfully!",
    error: "Error occurred while analyzing the file",
  });
  setPreviewUrl(await response);
  setFileIsAnalyzed(true);
}

export default function RandomImageModal(props) {
  const {
    setPreviewUrl,
    fileToDownload,
    setFileToDownload,
    setFileIsAnalyzed,
  } = props;
  const [currentFile, setCurrentFile] = useState(null);
  const [fileSelected, setFileSelected] = useState(null);
  const [fileSubmitted, setFileSubmitted] = useState(null);
  const [analysisConfidenceLevel, setAnalysisConfidenceLevel] = useState(80);
  const [boundingBoxColor, setBoundingBoxColor] = useState("#ff0000");
  const [labelColor, setLabelColor] = useState("#ffffff");

  return (
    <ModalLayout
      buttonTitle="Select File"
      Child={
        <div>
          <Typography
            id="transition-modal-title"
            variant="h6"
            component="h2"
            sx={{ m: 1 }}
          >
            Analyze Local File
          </Typography>
          <Grid container spacing={2} justifyContent="center">
            <Grid item>
              <Button
                variant="contained"
                component="label"
                sx={{ mt: 4, mb: 1 }}
              >
                Select File
                <input
                  type="file"
                  onChange={(event) =>
                    onFileInput(
                      event,
                      setCurrentFile,
                      setPreviewUrl,
                      setFileSelected
                    )
                  }
                  hidden
                />
              </Button>
            </Grid>
            <Grid item>
              <Button
                variant="outlined"
                disabled={!fileSelected}
                onClick={() =>
                  onFileSubmition(
                    currentFile,
                    setFileToDownload,
                    setFileSubmitted
                  )
                }
                sx={{ mt: 4, mb: 1 }}
              >
                Submit
              </Button>
            </Grid>
          </Grid>

          {fileSubmitted && (
            <Box sx={{ mt: 4 }}>
              <ConfidenceSlider
                confidence={analysisConfidenceLevel}
                setConfidence={setAnalysisConfidenceLevel}
              />
              <AdvancedConfigurationMenu
                boundingBoxColor={boundingBoxColor}
                setBoundingBoxColor={setBoundingBoxColor}
                labelColor={labelColor}
                setLabelColor={setLabelColor}
              />
              <Button
                variant="contained"
                onClick={() =>
                  handleImageAnalysis(
                    fileToDownload,
                    analysisConfidenceLevel,
                    boundingBoxColor,
                    labelColor,
                    setPreviewUrl,
                    setFileIsAnalyzed
                  )
                }
                sx={{ mt: 2 }}
              >
                Analyze
              </Button>
            </Box>
          )}
          <Typography
            id="transition-modal-description"
            sx={{ mt: 2 }}
          ></Typography>
        </div>
      }
    />
  );
}
