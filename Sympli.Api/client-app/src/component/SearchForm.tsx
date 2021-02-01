import React, { useState } from "react";
import { Button, Form, Header, Segment } from "semantic-ui-react";
import { ISearchRequestModel } from "../models/SearchRequestModel";

interface IProps {
  onSearch: (request: ISearchRequestModel) => void;
}

export const SearchForm: React.FC<IProps> = ({ onSearch }) => {
  const initialSeachRequestModalState: ISearchRequestModel = {
    keyword: "",
    targetUrl: "",
    searchEngines: ["Google", "Bing"], 
  };

  const [searchRequestModel, setSearchRequestModel] = useState<ISearchRequestModel>(initialSeachRequestModalState);

  const handleChange = (event: any) => {
    event.persist();
    setSearchRequestModel((requestModel) => ({
      ...requestModel,
      [event.target.name]: event.target.value,
    }));
  };

  const handleFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    onSearch(searchRequestModel);
  };

  const onClear = () => {
    setSearchRequestModel(initialSeachRequestModalState);
  };

  return (
    <Segment>
      <Header>Search</Header>
      <Form onSubmit={handleFormSubmit}>
        <Form.Field>
          <label>Keywords</label>
          <input
            placeholder="Keywords" required
            type="text"
            name="keyword"
            value={searchRequestModel.keyword}
            onChange={handleChange}
          />
        </Form.Field>
        <Form.Field>
          <label>Target Url</label>
          <input
            placeholder="Target Url"
            name="targetUrl" required
            value={searchRequestModel.targetUrl}
            type="text"
            onChange={handleChange}
          />
        </Form.Field>
        <Button.Group>
          <Button positive type="submit">
            View Report
          </Button>
          <Button wa type="button" onClick={onClear}>
            Clear
          </Button>
        </Button.Group>
      </Form>
    </Segment>
  );
};
