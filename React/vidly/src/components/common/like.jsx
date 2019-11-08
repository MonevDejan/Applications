import React from "react";
import PropTypes from "prop-types";

export const Like = props => {
  const { isLiked, onLike } = props;

  // to render full-heart or empty-heart icon dynamically
  let classes = "fa fa-heart";
  if (!isLiked) {
    classes += "-o";
  }

  return (
    <i
      className={classes}
      style={{ cursor: "pointer" }}
      aria-hidden="true"
      onClick={onLike}
    ></i>
  );
};

Like.propTypes = {
  isLiked: PropTypes.bool,
  onLike: PropTypes.func.isRequired
};

export default Like;
