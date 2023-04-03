import { Box, Button, Container, Stack, Typography } from "@mui/material";
import { Link } from "react-router-dom";

export default function HomePage() {
  return (
    <Box sx={{ py: 6 }}>
      <Container maxWidth="sm">
        <Typography variant="h3" align="center" gutterBottom>
          Image PupSearch
        </Typography>
        <Typography variant="h5" align="center">
          Analyze image content using AWS Rekognition machine learning models.
          Detect objects, scenes, activities, landmarks, dominant colors, and
          image quality.
        </Typography>
      </Container>
      <Stack direction="row" justifyContent="center" spacing={2} sx={{ py: 4 }}>
        <Link to="/search" style={{ textDecoration: "none" }}>
          <Button variant="contained">Recognize</Button>
        </Link>
        <Button variant="outlined" href="https://github.com/illiasolovey/pupsearch" target="_blank">
          Github
        </Button>
      </Stack>
    </Box>
  );
}
