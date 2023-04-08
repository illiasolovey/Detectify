import { Grid } from "@mui/material";

export default function ImagePreview(props) {
  return (
    <Grid
      item
      xs={false}
      md={7}
      sx={{
        backgroundImage: `url(${props.previewUrl})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    ></Grid>
  );
}
