import React from "react";
import PropTypes from "prop-types";

export const Pagination = ({
  itemsNumber,
  itemsPerPage,
  onPageSelect,
  activePage
}) => {
  const numberOfPages = Math.ceil(itemsNumber / itemsPerPage);

  // no render if it is only one page
  if (numberOfPages === 1) {
    return null;
  }

  // create array so we can iterate it in the render method
  const pages = [];
  for (let i = 0; i < numberOfPages; i++) {
    pages.push(i + 1);
  }

  //
  return (
    <nav aria-label="Page navigation example">
      <ul className="pagination">
        {pages.map(n => {
          // render active class dynamically
          return (
            <li
              key={n}
              className={
                n === activePage
                  ? "page-item pointer active"
                  : "page-item pointer"
              }
            >
              <button className="page-link" onClick={() => onPageSelect(n)}>
                {n}
              </button>
            </li>
          );
        })}
      </ul>
    </nav>
  );
};

Pagination.propTypes = {
  itemsNumber: PropTypes.number.isRequired,
  itemsPerPage: PropTypes.number.isRequired,
  activePage: PropTypes.number.isRequired,
  onPageSelect: PropTypes.func.isRequired
};

export default Pagination;
