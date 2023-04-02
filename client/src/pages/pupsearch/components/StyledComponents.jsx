import { Paper } from "@mui/material";
import { styled } from "@mui/system";

export const PaperNote = styled(Paper)({
  backdropFilter: "blur(50px)",
  backgroundColor: "transparent",
  backgroundImage: 'url("/path/to/image.jpg")',
  backgroundSize: "cover",
  backgroundPosition: "center",
});
