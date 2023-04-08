import { Box, Button, Typography } from "@mui/material";
import ConfidenceSlider from "../components/ConfidenceSlider";
import { useState } from "react";
import { useDefault } from "../controllers/MediaUploadHandlers";
import ModalLayout from "./ModalLayout";
import { toast } from "react-toastify";

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
          <Box sx={{ marginTop: 6 }}>
            <ConfidenceSlider
              confidence={analysisConfidenceLevel}
              setConfidence={setAnalysisConfidenceLevel}
            />
          </Box>
          <Button
            variant="outlined"
            onClick={async () => {
              const imageUrl = await useDefault(
                selectedRandomImageUrl,
                analysisConfidenceLevel,
                setFileToDownload
              );
              setPreviewUrl(imageUrl);
              setFileAnalyzed(true);
              toast.success("Lambda success");
            }}
            sx={{ my: 4 }}
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
