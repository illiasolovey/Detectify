import { Button, Typography, Fade } from "@mui/material";
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
          <Button variant="contained" component="label" sx={{ my: 6 }}>
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
          <ConfidenceSlider
            confidence={analysisConfidenceLevel}
            setConfidence={setAnalysisConfidenceLevel}
          />
          {fileSelected && (
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
              sx={{ my: 6 }}
            >
              Submit
            </Button>
          )}
          {fileSubmitted && (
            <Button
              onClick={async () => {
                const analyzedImageUrl = await invokeObjectAnalysis(
                  fileToDownload,
                  analysisConfidenceLevel
                );
                setPreviewUrl(analyzedImageUrl);
                setFileAnalyzed(true);
                toast.success("Lambda success");
              }}
            >
              Analyze
            </Button>
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
