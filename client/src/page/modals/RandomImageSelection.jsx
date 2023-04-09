import { Box, Button, Typography } from "@mui/material";
import ConfidenceSlider from "../components/ConfidenceSlider";
import { useState } from "react";
import { useDefault } from "../controllers/MediaUploadHandlers";
import ModalLayout from "./ModalLayout";

export default function RandomImageModal(props) {
  const {
    selectedRandomImageUrl,
    setPreviewUrl,
    setFileToDownload,
    setFileAnalyzed,
  } = props;
  const [analysisConfidenceLevel, setAnalysisConfidenceLevel] = useState(80);

  return (
    <ModalLayout
      buttonTitle="Use this image"
      Child={
        <div>
          <Typography
            id="transition-modal-title"
            variant="h6"
            component="h2"
            sx={{ m: 1 }}
          >
            Random image analysis
          </Typography>
          <Box sx={{ mt: 4 }}>
            <ConfidenceSlider
              confidence={analysisConfidenceLevel}
              setConfidence={setAnalysisConfidenceLevel}
            />
          </Box>
          <Button
            variant="contained"
            onClick={async () => {
              const imageUrl = await useDefault(
                selectedRandomImageUrl,
                analysisConfidenceLevel,
                setFileToDownload
              );
              setPreviewUrl(imageUrl);
              setFileAnalyzed(true);
            }}
            sx={{ mt: 2 }}
          >
            Use this image
          </Button>
          <Typography
            id="transition-modal-description"
            sx={{ mt: 2 }}
          ></Typography>
        </div>
      }
    />
  );
}
