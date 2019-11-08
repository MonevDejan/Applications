import React, { Component } from "react";

export class ListGroup extends Component {
  render() {
    const {
      item,
      selectedItem,
      onItemSelect,
      idProperty,
      valueProperty
    } = this.props;
    return (
      <ul className="list-group">
        {item.map(g => (
          <li
            key={g[idProperty]}
            className={
              g === selectedItem ? "list-group-item active" : "list-group-item"
            }
            onClick={() => onItemSelect(g)}
          >
            {g[valueProperty]}
          </li>
        ))}
      </ul>
    );
  }
}

ListGroup.defaultProps = {
  idProperty: "_id",
  valueProperty: "name"
};
export default ListGroup;
