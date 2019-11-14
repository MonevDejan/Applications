import React, { Component } from "react";
import "./../App.css";

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
              g === selectedItem
                ? "list-group-item pointer active"
                : "list-group-item pointer"
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
