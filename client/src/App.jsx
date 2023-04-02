import { Route, Routes } from "react-router";
import HomePage from "./pages/home/Page";
import PupSearchPage from "./pages/pupsearch/Page";

export default function App() {
  return (
    <div>
      <main>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/search" element={<PupSearchPage />} />
        </Routes>
      </main>
    </div>
  );
}
