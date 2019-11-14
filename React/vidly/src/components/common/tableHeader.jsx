import React, { Component } from "react";

export class TableHeader extends Component {
  raiseSort = path => {
    const sortColumn = { ...this.props.sortColumn };
    if (sortColumn.path === path) {
      sortColumn.order = sortColumn.order === "asc" ? "desc" : "asc";
    } else {
      sortColumn.path = path;
      sortColumn.order = "asc";
    }
    this.props.onSort(sortColumn);
  };

  renderSortIcon = (sortColumn, column) => {
    if (sortColumn.path === column.path) {
      return (
        <i
          className={"m-1 fa fa-sort-" + sortColumn.order}
          aria-hidden="true"
        ></i>
      );
    }
    return null;
  };

  render() {
    const { sortColumn, columns } = this.props;

    return (
      <thead>
        <tr>
          {columns.map(column => (
            <th
              key={column.path || column.key}
              onClick={column.path ? () => this.raiseSort(column.path) : null}
              className={column.path ? "pointer" : null}
            >
              {column.label}
              {this.renderSortIcon(sortColumn, column)}
            </th>
          ))}
        </tr>
      </thead>
    );
  }
}

export default TableHeader;
