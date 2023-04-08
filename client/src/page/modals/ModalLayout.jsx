import { Backdrop, Box, Modal, Fade, Button } from "@mui/material";
import { useState } from "react";
import CloseIcon from "@mui/icons-material/Close";

const modalStyle = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  boxShadow: 24,
  p: 4,
  borderRadius: "12px",
  textAlign: "center",
};

const closeButtonStyle = {
  position: "absolute",
  top: 0,
  right: 0,
  m: 1,
  "&:hover": {
    bgcolor: "transparent",
  },
};

export default function RandomImageModal(props) {
  const { buttonTitle, Child } = props;
  const [open, setOpen] = useState(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>
      <Button onClick={handleOpen}>{buttonTitle}</Button>
      <Modal
        aria-labelledby="transition-modal-title"
        aria-describedby="transition-modal-description"
        open={open}
        onClose={handleClose}
        closeAfterTransition
        slots={{ backdrop: Backdrop }}
        sx={{
          backdropFilter: "blur(3px)",
          backgroundColor: "rgba(255, 255, 255, 0.1)",
        }}
        slotProps={{
          backdrop: {
            timeout: 500,
          },
        }}
      >
        <Fade in={open}>
          <Box sx={modalStyle}>
            <Button sx={closeButtonStyle} onClick={handleClose} disableRipple>
              <CloseIcon />
            </Button>
            {Child}
          </Box>
        </Fade>
      </Modal>
    </div>
  );
}
