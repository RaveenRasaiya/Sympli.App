import React, { useState } from "react";
import { SearchList } from "../component/SearchList";
import { SearchForm } from "../component/SearchForm";
import { ISearchResponseModel } from "../models/SearchResponseModel";
import { ISearchRequestModel } from "../models/SearchRequestModel";
import Axios from "axios";
import { Container, Dimmer, Loader } from "semantic-ui-react";

function App() {
  const [searchResults, setSearchResults] = useState<ISearchResponseModel | null>(null);

  const [loading, setLoading] = useState<boolean>(false);

  const handleOnSearch = (searchRequestModel: ISearchRequestModel) => {
    setLoading(true);
    Axios.post<ISearchResponseModel>("http://localhost:30367/api/search", searchRequestModel)
      .then((response) => {
        setSearchResults(response.data);
      })
      .catch((error) => {
        console.log(error);
      })
      .finally(() => {
        setLoading(false);
      });
  };

  return (
    <Container style={{ margin: "15px" }}>
      <SearchForm onSearch={handleOnSearch}></SearchForm>
      <SearchList searchResults={searchResults}></SearchList>
      {loading && (
        <Dimmer active inverted>
          <Loader inverted content="Loading" />
        </Dimmer>
      )}
    </Container>
  );
}

export default App;
