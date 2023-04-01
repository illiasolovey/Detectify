import { Box, Button, Container, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { getUrl, onFileInput, onFileSubmit } from "./PageComponents";

export default function HomePage() {
  const [currentFile, setCurrentFile] = useState(null);
  const [previewUrl, setPreviewUrl] = useState();
  const [fileUploaded, setFileUploaded] = useState(false);
  const title = "Search";

  useEffect(() => {
    document.title = title;
  }, []);

  return (
    <Box textAlign="center" sx={{ py: 12 }}>
      <Container maxWidth="sm">
        <Typography variant="h3" gutterBottom>
          {title}
        </Typography>
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
            onClick={() => onFileSubmit(currentFile, setFileUploaded)}
            sx={{ my: 6 }}
          >
            Submit
          </Button>
        )}
        {previewUrl && (
          <Button
            disabled={!fileUploaded}
            onClick={() => getUrl(currentFile, setPreviewUrl)}
            sx={{ my: 6 }}
          >
            <img src={previewUrl} alt="Preview" width="100%" height="100%" />
          </Button>
        )}
      </Container>
    </Box>
  );
}
