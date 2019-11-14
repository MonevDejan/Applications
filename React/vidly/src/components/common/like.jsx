import React from "react";
import PropTypes from "prop-types";

export const Like = ({ isLiked, onLike }) => {
  // to render full-heart or empty-heart icon dynamically
  let classes = "pointer fa fa-heart";
  if (!isLiked) {
    classes += "-o";
  }

  return <i className={classes} aria-hidden="true" onClick={onLike}></i>;
};

Like.propTypes = {
  isLiked: PropTypes.bool,
  onLike: PropTypes.func.isRequired
};

export default Like;
