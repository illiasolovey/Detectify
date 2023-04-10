import { Box, Button } from "@mui/material";
import { coveringBoxStyle } from "./DefaultStyles";
import { downloadFile } from "../controllers/MediaUploadHandlers";

export default function FileDownloadButton(props) {
  const { boxStyle } = props;

  return (
    <Box
      sx={{
        ...boxStyle,
        display: "flex",
        justifyContent: "center",
      }}
    >
      <Button variant="contained" onClick={() => downloadFile(fileToDownload)}>
        Download result
      </Button>
    </Box>
  );
}

FileDownloadButton.defaultProps = {
  boxStyle: coveringBoxStyle,
};
