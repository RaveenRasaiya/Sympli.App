import React from "react";
import { Header, Segment, Table } from "semantic-ui-react";
import { ISearchResponseModel } from "../models/SearchResponseModel";

interface IProps {
  searchResults: ISearchResponseModel | null;
}

export const SearchList: React.FC<IProps> = ({ searchResults }) => {
  if (searchResults) {
    return (
      <Segment>
        <Header>Search Results</Header>
        <Table basic="very" celled collapsing>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell key="header">Engine</Table.HeaderCell>
              {searchResults?.result.map((result) => (
                <Table.HeaderCell key={result.engine}>{result.engine}</Table.HeaderCell>
              ))}
            </Table.Row>
          </Table.Header>

          <Table.Body>
            <Table.Row>
              <Table.Cell key="line">Page Position(s)</Table.Cell>
              {searchResults?.result.map((result) => (
                <Table.Cell key={result.engine}>{result.pagePositions}</Table.Cell>
              ))}
            </Table.Row>
          </Table.Body>
        </Table>
      </Segment>
    );
  } else {
    return  <Header as='h5'>There is no content from api</Header>
  }
};
