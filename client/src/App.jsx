import { Container } from "@mui/material";
import { Route, Routes } from "react-router";
import HomePage from "./pages/home/Page"
import PupSearchPage from "./pages/pupsearch/Page"

export default function App() {
  return (
    <div>
      <main>
        <Container>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/recognize" element={<PupSearchPage />} />
          </Routes>
        </Container>
      </main>
    </div>
  );
}
