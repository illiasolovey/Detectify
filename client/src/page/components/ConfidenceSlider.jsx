import { Box, Slider, Typography } from "@mui/material";

export default function ConfidenceSlider(props) {
  return (
    <Box
      sx={{
        mx: 4,
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
      }}
    >
      <Slider
        aria-label="Confidence"
        value={props.confidence}
        onChange={(event, newValue) => props.setConfidence(newValue)}
        valueLabelDisplay="auto"
        step={10}
        marks
        min={0}
        max={99}
        sx={{ maxWidth: "50%" }}
      />
      <Typography>Confidence</Typography>
    </Box>
  );
}
