import { Grid, Typography } from "@mui/material";
import { defaultGridStyle } from "./DefaultStyles";

const quickGuide = [
  {
    title: "Select File",
    description: `Choose your local file or select current random image to analyze`,
  },
  {
    title: "Upload File",
    description: "Upload the selected file to begin analysis",
  },
  {
    title: "Analyze",
    description: "Wait for the file to be analyzed and view the results",
  },
];

export default function QuickGuide(props) {
  const { gridStyle } = props;

  return (
    <Grid container textAlign="center" justifyContent="center" sx={gridStyle}>
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
  );
}

QuickGuide.defaultProps = {
  gridStyle: defaultGridStyle,
};
