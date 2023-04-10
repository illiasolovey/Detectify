import { Box, Button, Grid, Typography } from "@mui/material";
import { useState } from "react";
import { BlockPicker } from "react-color";

const defaultColors = [
  "#ffffff", // white
  "#000000", // black
  "#ff0000", // red
  "#00ff00", // green
  "#ffff00", // yellow
  "#ffa500", // orange
  "#a020f0", // purple
  "#0000ff", // blue
  "#00ffff", // cyan
];

const defaultPickerStyle = {
  display: "flex",
  flexDirection: "column",
  alignItems: "center",
  justifyContent: "center",
  my: 2,
};
const defaultMenuStyle = {
  mt: 4,
  display: "flex",
  flexDirection: "column",
  alignItems: "center",
};

export default function AdvancedConfigurationMenu(props) {
  const {
    boundingBoxColor,
    setBoundingBoxColor,
    labelColor,
    setLabelColor,
    menuStyle,
    colorPickerStyle,
  } = props;
  const [isAdvancedVisible, setIsAdvancedVisible] = useState(false);
  const [selectedElement, setSelectedElement] = useState("boundingBox");

  const handleToggleVisibility = () => {
    setIsAdvancedVisible(!isAdvancedVisible);
    setSelectedElement("boundingBox"); 
  };

  return (
    <Box sx={menuStyle}>
      <Button variant="outlined" onClick={handleToggleVisibility}>
        {isAdvancedVisible ? "Hide" : "Show"} advanced
      </Button>
      {isAdvancedVisible && (
        <Box sx={colorPickerStyle}>
          <Grid container spacing={2} justifyContent="center">
            <Grid item>
              <Button
                variant="outlined"
                onClick={() => setSelectedElement("boundingBox")}
              >
                Boxes
              </Button>
            </Grid>
            <Grid item>
              <Button
                variant="outlined"
                onClick={() => setSelectedElement("label")}
              >
                Labels
              </Button>
            </Grid>
          </Grid>
          <Typography variant="h6" component="h2" my={1}>
            {selectedElement === "boundingBox" ? "Boxes" : "Labels"} color
          </Typography>
          <BlockPicker
            triangle="hide"
            color={selectedElement === "boundingBox" ? boundingBoxColor : labelColor}
            onChange={(color) => {
              selectedElement === "boundingBox" ? setBoundingBoxColor(color.hex) : setLabelColor(color.hex)
            }}
            colors={defaultColors}
          />
        </Box>
      )}
    </Box>
  );
}

AdvancedConfigurationMenu.defaultProps = {
  colorPickerStyle: defaultPickerStyle,
  menuStyle: defaultMenuStyle,
};
