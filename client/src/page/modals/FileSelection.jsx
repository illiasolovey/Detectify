import { Button, Typography, Fade, Grid, Box } from "@mui/material";
import ConfidenceSlider from "../components/ConfidenceSlider";
import { useState } from "react";
import { selectFile, uploadFile } from "../controllers/MediaUploadHandlers";
import ModalLayout from "./ModalLayout";
import { toast } from "react-toastify";
import { invokeObjectAnalysis } from "../controllers/LambdaInteractionHandlers";

async function onFileInput(
  event,
  setCurrentFile,
  setPreviewUrl,
  setFileSelected
) {
  setPreviewUrl(selectFile(event, setCurrentFile));
  setFileSelected(true);
  toast.success("File selected");
}
async function onFileSubmition(file, setFileToDownload, setFileSubmitted) {
  const filename = await uploadFile(file);
  setFileToDownload(filename);
  setFileSubmitted(true);
  toast.success("File uploaded");
}

export default function RandomImageModal(props) {
  const { setPreviewUrl, fileToDownload, setFileToDownload, setFileAnalyzed } =
    props;
  const [currentFile, setCurrentFile] = useState(null);
  const [fileSelected, setFileSelected] = useState(null);
  const [fileSubmitted, setFileSubmitted] = useState(null);
  const [analysisConfidenceLevel, setAnalysisConfidenceLevel] = useState(80);

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
              <Button variant="contained" component="label" sx={{ mt: 4, mb: 1 }}>
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
            <Box sx={{mt: 4}}>
              <ConfidenceSlider
                confidence={analysisConfidenceLevel}
                setConfidence={setAnalysisConfidenceLevel}
              />
              <Button
                variant="contained"
                onClick={async () => {
                  const analyzedImageUrl = await invokeObjectAnalysis(
                    fileToDownload,
                    analysisConfidenceLevel
                  );
                  setPreviewUrl(analyzedImageUrl);
                  setFileAnalyzed(true);
                  toast.success("Lambda success");
                }}
                sx={{mt: 2}}
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
