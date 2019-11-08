import PropTypes from "prop-types";

export function paginate(items, pageNumber, pageSize) {
  let startCycle = pageSize * (pageNumber - 1);
  let stopCycle = pageSize * pageNumber;

  const arrayToRender = items.filter((movie, index) => {
    if (index >= startCycle && index < stopCycle) {
      return movie;
    }
    return null;
  });

  return arrayToRender;
}

paginate.PropTypes = {
  items: PropTypes.array.isRequired,
  pageNumber: PropTypes.number.isRequired,
  pageSize: PropTypes.number.isRequired
};

export default paginate;
